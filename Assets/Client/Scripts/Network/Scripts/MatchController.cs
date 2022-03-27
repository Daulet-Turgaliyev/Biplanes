using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;


    [RequireComponent(typeof(NetworkMatch))]
    public class MatchController : NetworkBehaviour
    {
        public static MatchController Instance;

        
        private NetworkMatch _networkMatch;
        public Guid GetNetworkMath => _networkMatch.matchId;
        
        internal readonly SyncDictionary<NetworkIdentity, MatchPlayerData> matchPlayerData = new SyncDictionary<NetworkIdentity, MatchPlayerData>();
        internal readonly Dictionary<CellValue, CellGUI> MatchCells = new Dictionary<CellValue, CellGUI>();

        CellValue boardScore = CellValue.None;
        bool playAgain = false;
        
        [SerializeField]
        private SpawnPlanePoint[] spawnPosition;
        private int _spawnPoint;
        [SerializeField] 
        private PlaneData _planeData;
        
        [Header("Diagnostics - Do Not Modify")]
        public CanvasController canvasController;
        public NetworkIdentity player1;
        public NetworkIdentity player2;

        private void Awake()
        {
            Instance = this;
            _networkMatch = GetComponent<NetworkMatch>();
            canvasController = FindObjectOfType<CanvasController>();
        }

        public override void OnStartServer()
        {
            StartCoroutine(AddPlayersToMatchController());
        }

        // For the SyncDictionary to properly fire the update callback, we must
        // wait a frame before adding the players to the already spawned MatchController
        IEnumerator AddPlayersToMatchController()
        {
            yield return null;

            matchPlayerData.Add(player1, new MatchPlayerData { playerIndex = CanvasController.playerInfos[player1.connectionToClient].playerIndex });
            matchPlayerData.Add(player2, new MatchPlayerData { playerIndex = CanvasController.playerInfos[player2.connectionToClient].playerIndex });
        }

        [Server]
        public PlaneBehaviour PlaneInstantiate(NetworkIdentity conn)
        {

            var spawnPosition = GetSpawnPosition();
            
            var planeBehaviour = Instantiate(_planeData.PlanePrefab, 
                spawnPosition.position, spawnPosition.rotation);

            planeBehaviour.GetComponent<NetworkMatch>().matchId = _networkMatch.matchId;
            NetworkServer.Spawn(planeBehaviour.gameObject,conn.connectionToClient);
            return planeBehaviour;
        }


         [Server]
         public void GetDamage(PlaneBehaviour planeBehaviour, int damage)
         {
             planeBehaviour.RpcChangeCondition(damage);
         }
         
         [Server]
         public void GetFatalDamage(PlaneBehaviour planeBehaviour)
         {
             planeBehaviour.RpcFastDestroyPlane();
         }
         
        [Server]
        public async void DestroyAndRespawnPlane(NetworkIdentity conn, int respawnTime = 0, int destroyTime = 0)
        {
            var networkIdentity = conn;
            await NetworkDestroy(conn.gameObject, destroyTime);
            
            await RespawnPlane(networkIdentity, respawnTime);
        }
        
        [Server]
        public async Task RespawnPlane(NetworkIdentity conn, int respawnTimeToSecond = 0)
        {
            var respawnTimeToMillisecondsDelay = respawnTimeToSecond * 1000;
            var networkIdentity = conn;
            await Task.Delay(respawnTimeToMillisecondsDelay);
            PlaneInstantiate(networkIdentity);
        }
        
        [Server]
        public async Task NetworkDestroy(GameObject networkObject, int destroyTimeToSecond = 0)
        {
            var destroyTimeToMillisecondsDelay = destroyTimeToSecond * 1000;
            
            await Task.Delay(destroyTimeToMillisecondsDelay);
            
            NetworkServer.Destroy(networkObject);
        }
        
        private SpawnPlanePoint GetSpawnPosition()
        {
            int spawnPoint = _spawnPoint == 1 ? 0 : 1;
            _spawnPoint = spawnPoint;
            return spawnPosition[spawnPoint];
        }
        
        [Command(requiresAuthority = false)]
        public void CmdRequestExitGame(NetworkConnectionToClient sender = null)
        {
            StartCoroutine(ServerEndMatch(sender, false));
        }
        

        public void OnPlayerDisconnected(NetworkConnection conn)
        {
            // Check that the disconnecting client is a player in this match
            if (player1 == conn.identity || player2 == conn.identity)
            {
                StartCoroutine(ServerEndMatch(conn, true));
            }
        }

        public IEnumerator ServerEndMatch(NetworkConnection conn, bool disconnected)
        {
            canvasController.OnPlayerDisconnected -= OnPlayerDisconnected;

            RpcExitGame();

            // Skip a frame so the message goes out ahead of object destruction
            yield return null;

            // Mirror will clean up the disconnecting client so we only need to clean up the other remaining client.
            // If both players are just returning to the Lobby, we need to remove both connection Players

            if (!disconnected)
            {
                NetworkServer.RemovePlayerForConnection(player1.connectionToClient, true);
                CanvasController.waitingConnections.Add(player1.connectionToClient);

                NetworkServer.RemovePlayerForConnection(player2.connectionToClient, true);
                CanvasController.waitingConnections.Add(player2.connectionToClient);
            }
            else if (conn == player1.connectionToClient)
            {
                // player1 has disconnected - send player2 back to Lobby
                NetworkServer.RemovePlayerForConnection(player2.connectionToClient, true);
                CanvasController.waitingConnections.Add(player2.connectionToClient);
            }
            else if (conn == player2.connectionToClient)
            {
                // player2 has disconnected - send player1 back to Lobby
                NetworkServer.RemovePlayerForConnection(player1.connectionToClient, true);
                CanvasController.waitingConnections.Add(player1.connectionToClient);
            }

            // Skip a frame to allow the Removal(s) to complete
            yield return null;

            // Send latest match list
            canvasController.SendMatchList();

            NetworkServer.Destroy(gameObject);
        }

        [ClientRpc]
        public void RpcExitGame()
        {
            canvasController.OnMatchEnded();
        }
    }
[Serializable]
public struct SpawnPlanePoint
{
    [field:SerializeField]
    public Vector2 position { get; private set; }
    
    [field:SerializeField]
    public Quaternion rotation { get; private set; }
}
