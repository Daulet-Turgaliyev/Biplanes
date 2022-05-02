using Mirror;
using UnityEngine;

    [AddComponentMenu("")]
    public class MatchNetworkManager : NetworkManager
    {
        [Header("Match GUI")]
        public NetworkHandler _networkHandler;
        
        #region Unity Callbacks
        
        public override void Awake()
        {
            base.Awake();
            _networkHandler.InitializeData();
        }

        #endregion

        #region Server System Callbacks
        
        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);
            _networkHandler.OnServerReady(conn);
        }
        
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            _networkHandler.OnServerDisconnect(conn);
            base.OnServerDisconnect(conn);
        }

        #endregion

        #region Client System Callbacks
        
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            
            _networkHandler.OnClientConnect(conn);
        }
        
        public override void OnClientDisconnect(NetworkConnection conn)
        {
            _networkHandler.OnClientDisconnect();
            base.OnClientDisconnect(conn);
        }

        #endregion

        #region Start & Stop Callbacks
        
        public override void OnStartServer()
        {
            _networkHandler.OnStartServer();
        }
        
        public override void OnStartClient()
        {
            _networkHandler.OnStartClient();
        }
        
        public override void OnStopServer()
        {
            _networkHandler.OnStopServer();
        }
        
        public override void OnStopClient()
        {
            _networkHandler.OnStopClient();
        }

        #endregion
    }
