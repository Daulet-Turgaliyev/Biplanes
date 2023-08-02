using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class NetworkHandler : MonoBehaviour
    {
        public event Action<NetworkConnection> OnPlayerDisconnected;

        private readonly Dictionary<NetworkConnection, Guid> _playerMatches = new Dictionary<NetworkConnection, Guid>();

        private readonly Dictionary<Guid, MatchInfo> _openMatches = new Dictionary<Guid, MatchInfo>();
        public IReadOnlyDictionary<Guid, MatchInfo> GetOpenMatches => _openMatches;

        private readonly Dictionary<Guid, HashSet<NetworkConnection>> _matchConnections = new Dictionary<Guid, HashSet<NetworkConnection>>();

        private readonly Dictionary<NetworkConnection, PlayerInfo> _playerInfos = new Dictionary<NetworkConnection, PlayerInfo>();
        
        public readonly List<NetworkConnection> WaitingConnections = new List<NetworkConnection>();
        
        
        private int _playerIndex = 1;
        
        [Header("GUI References")]
        public GameObject matchControllerPrefab;
        
        private RoomViewWindow _roomViewWindow;
        private LobbyViewWindow _lobbyViewWindow;

        [Inject]
        private WindowsManager _windowsManager;

        [Inject] 
        private NetworkManager _networkManager;
        
        #region UI Functions

        // Called from several places to ensure a clean reset
        //  - MatchNetworkManager.Awake
        //  - OnStartServer
        //  - OnStartClient
        //  - OnClientDisconnect
        //  - ResetCanvas
        public void InitializeData()
        {
            _playerMatches.Clear();
            _openMatches.Clear();
            _matchConnections.Clear();
            WaitingConnections.Clear();
        }

        public void AllDisconnect()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                _networkManager.StopHost();
                return;
            }

            if (NetworkClient.active)
            {
                _networkManager.StopClient();
            }
        }

        #endregion

        #region Button Calls
        
        
        public void OnMatchEnded()
        {
            if (!NetworkClient.active) return;

            ShowLobbyView();
        }


        public void SendMatchList(NetworkConnection conn = null)
        {
            if (!NetworkServer.active) return;

            if (conn != null)
            {
                conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.List, matchInfos = _openMatches.Values.ToArray() });
            }
            else
            {
                foreach (var waiter in WaitingConnections)
                {
                    waiter.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.List, matchInfos = _openMatches.Values.ToArray() });
                }
            }
        }

        #endregion

        #region Server & Client Callbacks

        // Methods in this section are called from MatchNetworkManager's corresponding methods

        public void OnStartServer()
        {
            if (!NetworkServer.active) return;

            InitializeData();
            NetworkServer.RegisterHandler<ServerMatchMessage>(OnServerMatchMessage);
        }

        public void OnServerReady(NetworkConnection conn)
        {
            if (!NetworkServer.active) return;

            WaitingConnections.Add(conn);
            _playerInfos.Add(conn, new PlayerInfo { playerIndex = this._playerIndex, ready = false });
            _playerIndex++;

            SendMatchList();
        }

        public void OnServerDisconnect(NetworkConnection conn)
        {
            if (NetworkServer.active == false) return;
            
            // Invoke OnPlayerDisconnected on all instances of MatchController
            OnPlayerDisconnected?.Invoke(conn);
            if (_playerMatches.TryGetValue(conn, out var matchId))
            {
                _playerMatches.Remove(conn);
                _openMatches.Remove(matchId);

                foreach (NetworkConnection playerConn in _matchConnections[matchId])
                {
                    PlayerInfo _playerInfo = _playerInfos[playerConn];
                    _playerInfo.ready = false;
                    _playerInfo.matchId = Guid.Empty;
                    _playerInfos[playerConn] = _playerInfo;
                    playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Departed });
                }
            }

            foreach (KeyValuePair<Guid, HashSet<NetworkConnection>> kvp in _matchConnections)
            {
                kvp.Value.Remove(conn);
            }

            PlayerInfo playerInfo = _playerInfos[conn];
            if (playerInfo.matchId != Guid.Empty)
            {
                MatchInfo matchInfo;
                if (_openMatches.TryGetValue(playerInfo.matchId, out matchInfo))
                {
                    matchInfo.players--;
                    _openMatches[playerInfo.matchId] = matchInfo;
                }

                HashSet<NetworkConnection> connections;
                if (_matchConnections.TryGetValue(playerInfo.matchId, out connections))
                {
                    PlayerInfo[] infos = connections.Select(playerConn => _playerInfos[playerConn]).ToArray();

                    foreach (NetworkConnection playerConn in _matchConnections[playerInfo.matchId])
                    {
                        if (playerConn != conn)
                        {
                            playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateRoom, playerInfos = infos });
                        }
                    }
                }
            }

            SendMatchList();
        }

        public void OnStopServer()
        {
            InitializeData();
        }

        public void OnClientConnect(NetworkConnection conn)
        {
            _playerInfos.Add(conn, new PlayerInfo { playerIndex = this._playerIndex, ready = false });
        }

        public void OnStartClient()
        {
            if (!NetworkClient.active) return;

            InitializeData();
            ShowLobbyView();
            NetworkClient.RegisterHandler<ClientMatchMessage>(OnClientMatchMessage);
        }

        public void OnClientDisconnect()
        {
            if (!NetworkClient.active) return;

            DisconnectClient();
            
            InitializeData();
        }

        private void DisconnectClient()
        {
            SceneManager.LoadScene(0);
        }

        public void OnStopClient()
        {
            InitializeData();
            OnClientDisconnect();
        }

        #endregion

        #region Server Match Message Handlers

        private void OnServerMatchMessage(NetworkConnection conn, ServerMatchMessage msg)
        {
            if (!NetworkServer.active) return;

            switch (msg.serverMatchOperation)
            {
                case ServerMatchOperation.None:
                    {
                        Debug.LogWarning("Missing ServerMatchOperation");
                        break;
                    }
                case ServerMatchOperation.Create:
                    {
                        OnServerCreateMatch(conn);
                        break;
                    }
                case ServerMatchOperation.Cancel:
                    {
                        OnServerCancelMatch(conn);
                        break;
                    }
                case ServerMatchOperation.Start:
                    {
                        OnServerStartMatch(conn);
                        break;
                    }
                case ServerMatchOperation.Join:
                    {
                        OnServerJoinMatch(conn, msg.matchId);
                        break;
                    }
                case ServerMatchOperation.Leave:
                    {
                        OnServerLeaveMatch(conn, msg.matchId);
                        break;
                    }
                case ServerMatchOperation.Ready:
                    {
                        OnServerPlayerReady(conn, msg.matchId);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnServerPlayerReady(NetworkConnection conn, Guid matchId)
        {
            if (!NetworkServer.active) return;

            PlayerInfo playerInfo = _playerInfos[conn];
            playerInfo.ready = !playerInfo.ready;
            _playerInfos[conn] = playerInfo;

            HashSet<NetworkConnection> connections = _matchConnections[matchId];
            PlayerInfo[] infos = connections.Select(playerConn => _playerInfos[playerConn]).ToArray();

            foreach (NetworkConnection playerConn in _matchConnections[matchId])
            {
                playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateRoom, playerInfos = infos });
            }
        }

        private void OnServerLeaveMatch(NetworkConnection conn, Guid matchId)
        {
            if (!NetworkServer.active) return;

            MatchInfo matchInfo = _openMatches[matchId];
            matchInfo.players--;
            _openMatches[matchId] = matchInfo;

            PlayerInfo playerInfo = _playerInfos[conn];
            playerInfo.ready = false;
            playerInfo.matchId = Guid.Empty;
            _playerInfos[conn] = playerInfo;

            foreach (KeyValuePair<Guid, HashSet<NetworkConnection>> kvp in _matchConnections)
            {
                kvp.Value.Remove(conn);
            }

            HashSet<NetworkConnection> connections = _matchConnections[matchId];
            PlayerInfo[] infos = connections.Select(playerConn => _playerInfos[playerConn]).ToArray();

            foreach (NetworkConnection playerConn in _matchConnections[matchId])
            {
                playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateRoom, playerInfos = infos });
            }

            SendMatchList();

            conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Departed });
        }

        private void OnServerCreateMatch(NetworkConnection conn)
        {
            if (!NetworkServer.active || _playerMatches.ContainsKey(conn)) return;
            
            Guid newMatchId = Guid.NewGuid();
            _matchConnections.Add(newMatchId, new HashSet<NetworkConnection>());
            _matchConnections[newMatchId].Add(conn);
            _playerMatches.Add(conn, newMatchId);
            _openMatches.Add(newMatchId, new MatchInfo { matchId = newMatchId, maxPlayers = 2, players = 1 });

            PlayerInfo playerInfo = _playerInfos[conn];
            playerInfo.ready = false;
            playerInfo.matchId = newMatchId;
            _playerInfos[conn] = playerInfo;

            PlayerInfo[] infos = _matchConnections[newMatchId].Select(playerConn => _playerInfos[playerConn]).ToArray();

            conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Created, matchId = newMatchId, playerInfos = infos });

            SendMatchList();
        }

        private void OnServerCancelMatch(NetworkConnection conn)
        {
            if (!NetworkServer.active || !_playerMatches.ContainsKey(conn)) return;

            conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Cancelled });

            Guid matchId;
            if (_playerMatches.TryGetValue(conn, out matchId))
            {
                _playerMatches.Remove(conn);
                _openMatches.Remove(matchId);

                foreach (NetworkConnection playerConn in _matchConnections[matchId])
                {
                    PlayerInfo playerInfo = _playerInfos[playerConn];
                    playerInfo.ready = false;
                    playerInfo.matchId = Guid.Empty;
                    _playerInfos[playerConn] = playerInfo;
                    playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Departed });
                }

                SendMatchList();
            }
        }

        private void OnServerStartMatch(NetworkConnection conn)
        {
            if (!NetworkServer.active || !_playerMatches.ContainsKey(conn)) return;

            Guid matchId;
            if (_playerMatches.TryGetValue(conn, out matchId))
            {
                GameObject matchControllerObject = Instantiate(matchControllerPrefab);
                matchControllerObject.GetComponent<NetworkMatch>().matchId = matchId;
                NetworkServer.Spawn(matchControllerObject);

                MatchController matchController = matchControllerObject.GetComponent<MatchController>();
                
                foreach (NetworkConnection playerConn in _matchConnections[matchId])
                {
                    playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Started });

                    GameObject player = Instantiate(NetworkManager.singleton.playerPrefab);
                    player.GetComponent<NetworkMatch>().matchId = matchId;
                    NetworkServer.AddPlayerForConnection(playerConn, player);

                    matchController.PlaneInstantiate(playerConn.identity);
                    
                    if (matchController.player1 == null)
                    {
                        matchController.player1 = playerConn.identity;
                    }
                    else
                    {
                        matchController.player2 = playerConn.identity;
                    }

                    /* Reset ready state for after the match. */
                    PlayerInfo playerInfo = _playerInfos[playerConn];
                    playerInfo.ready = false;
                    _playerInfos[playerConn] = playerInfo;
                }
                
                _playerMatches.Remove(conn);
                _openMatches.Remove(matchId);
                _matchConnections.Remove(matchId);
                SendMatchList();

                OnPlayerDisconnected += matchController.OnPlayerDisconnected;
            }
        }

        private void OnServerJoinMatch(NetworkConnection conn, Guid matchId)
        {
            if (!NetworkServer.active || !_matchConnections.ContainsKey(matchId) || !_openMatches.ContainsKey(matchId)) return;

            MatchInfo matchInfo = _openMatches[matchId];
            matchInfo.players++;
            _openMatches[matchId] = matchInfo;
            _matchConnections[matchId].Add(conn);

            PlayerInfo playerInfo = _playerInfos[conn];
            playerInfo.ready = false;
            playerInfo.matchId = matchId;
            _playerInfos[conn] = playerInfo;

            PlayerInfo[] infos = _matchConnections[matchId].Select(playerConn => _playerInfos[playerConn]).ToArray();
            SendMatchList();

            conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Joined, matchId = matchId, playerInfos = infos });

            foreach (NetworkConnection playerConn in _matchConnections[matchId])
            {
                playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateRoom, playerInfos = infos });
            }
        }

        #endregion

        #region Client Match Message Handler

        private void OnClientMatchMessage(ClientMatchMessage msg)
        {
            if (NetworkClient.active == false) return;

            switch (msg.clientMatchOperation)
            {
                case ClientMatchOperation.None:
                    {
                        Debug.LogWarning("Missing ClientMatchOperation");
                        break;
                    }
                case ClientMatchOperation.List:
                    {
                        _openMatches.Clear();
                        foreach (MatchInfo matchInfo in msg.matchInfos)
                        {
                            _openMatches.Add(matchInfo.matchId, matchInfo);
                        }
                        _lobbyViewWindow.RefreshMatchList();
                        break;
                    }
                case ClientMatchOperation.Created:
                    {
                        _roomViewWindow = _windowsManager.OpenWindow<RoomViewWindow>();
                        _roomViewWindow.LocalPlayerMatch = msg.matchId;
                        _roomViewWindow.RefreshRoomPlayers(msg.playerInfos);
                        _roomViewWindow.SetOwner(true);
                        GameManager.Instance.IsOwner = true;
                        break;
                    }
                case ClientMatchOperation.Cancelled:
                    {
                        _roomViewWindow.LocalPlayerMatch = Guid.Empty;
                        ShowLobbyView();
                        break;
                    }
                case ClientMatchOperation.Joined:
                    {
                        _roomViewWindow = _windowsManager.OpenWindow<RoomViewWindow>();
                        _roomViewWindow.LocalPlayerMatch = msg.matchId;
                        _roomViewWindow.RefreshRoomPlayers(msg.playerInfos);
                        _roomViewWindow.SetOwner(false);
                        GameManager.Instance.IsOwner = false;
                        break;
                    }
                case ClientMatchOperation.Departed:
                    {
                        ShowLobbyView();
                        break;
                    }
                case ClientMatchOperation.UpdateRoom:
                    {
                        _roomViewWindow.RefreshRoomPlayers(msg.playerInfos);
                        break;
                    }
                case ClientMatchOperation.Started:
                    {
                        NetworkClient.ready = true;
                        GameManager.Instance.OnStartGame?.Invoke();
                        _windowsManager.DestroyAll();
                        _windowsManager.OpenWindow<GameViewWindow>();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void ShowLobbyView()
        {
            _lobbyViewWindow = _windowsManager.OpenWindow<LobbyViewWindow>();
            _lobbyViewWindow.ShowLobbyView();
        }

        #endregion

    }
