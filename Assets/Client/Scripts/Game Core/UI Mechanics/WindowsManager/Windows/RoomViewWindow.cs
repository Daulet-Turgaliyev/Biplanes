using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;


    public class RoomViewWindow : BaseWindow
    {
        [SerializeField]
        private GameObject _playerList;
        
        [SerializeField]
        private GameObject _playerPrefab;
        
        [SerializeField]
        private GameObject _cancelButton;
        
        [SerializeField]
        private GameObject _leaveButton;
        
        [SerializeField]
        private Button _startButton;
        
        private bool _owner;
        
        public Guid LocalPlayerMatch = Guid.Empty;
        public Guid LocalJoinedMatch = Guid.Empty;

        public void RequestStartMatch()
        {
            if (!NetworkClient.active || LocalPlayerMatch == Guid.Empty) return;

            NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Start });
        }
        
        public void RequestLeaveMatch()
        {
            if (!NetworkClient.active || LocalJoinedMatch == Guid.Empty) return;

            NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Leave, matchId = LocalJoinedMatch });
        }

        public void RequestCancelMatch()
        {
            if (!NetworkClient.active || LocalPlayerMatch == Guid.Empty) return;

            NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Cancel });
        }

        public void RequestReadyChange()
        {
            if (!NetworkClient.active || (LocalPlayerMatch == Guid.Empty && LocalJoinedMatch == Guid.Empty)) return;

            Guid matchId = LocalPlayerMatch == Guid.Empty ? LocalJoinedMatch : LocalPlayerMatch;

            NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Ready, matchId = matchId });
        }
        
        public void RefreshRoomPlayers(PlayerInfo[] playerInfos)
        {
            // Debug.Log($"RefreshRoomPlayers: {playerInfos.Length} playerInfos");

            foreach (Transform child in _playerList.transform)
            {
                Destroy(child.gameObject);
            }

            _startButton.interactable = false;
            bool everyoneReady = true;

            foreach (PlayerInfo playerInfo in playerInfos)
            {
                GameObject newPlayer = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
                newPlayer.transform.SetParent(_playerList.transform, false);
                newPlayer.GetComponent<PlayerGUI>().SetPlayerInfo(playerInfo);
                if (!playerInfo.ready)
                {
                    everyoneReady = false;
                }
            }

            _startButton.interactable = everyoneReady && _owner && (playerInfos.Length > 1);
        }

        public void SetOwner(bool owner)
        {
            this._owner = owner;
            _cancelButton.SetActive(owner);
            _leaveButton.SetActive(!owner);
        }
    }

