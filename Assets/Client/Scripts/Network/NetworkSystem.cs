using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

[AddComponentMenu("")]
public sealed class NetworkSystem : NetworkManager
{
    [SerializeField]
    private SpawnPlanePoint[] spawnPosition;
    
    [Inject]
    private LevelInitializer _levelInitializer;

    [Inject]
    private WindowsManager _windowsManager;

    [Inject]
    private GameManager _gameManager;
    
    [field: SerializeField] 
    public PlaneData planeData { get; private set; }
    
    private List<NetworkConnection> _players = new List<NetworkConnection>();
    
    
    private int _spawnPoint;
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<PlayerData>(RegisterMessage);
    }
    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        _players.Add(conn);

        var playerHandler = conn.identity.GetComponent<PlayerHandler>();

        if (ReferenceEquals(playerHandler, null))
        {
            Debug.LogError("Nor Found PlayerHandler");
            NetworkClient.Disconnect();
            return;
        }

        playerHandler.PlayerId = numPlayers - 1;
        
        if (numPlayers == 2)
        {
            StartGame();
        }
    }
    

    public override void OnClientConnect(NetworkConnection conn)
    {
        
      /*  if (ReferenceEquals(conn, null))
        {
            Debug.LogWarning("Connection not found ");
            return;
        }
        
        conn.Send(new PlayerData { PlayerName = $"Player 00" });*/
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        _windowsManager.CloseAll();
        _players.Remove(conn);
        Debug.Log($"Disconnected: {conn.identity}");
        base.OnServerDisconnect(conn);
    }

    private async void StartGame()
    {
        await Task.Delay(1000);

        foreach (var connect in NetworkServer.connections.Values)
        {
            var newPlaneBehaviour = PlaneInstantiate(connect, planeData, GetSpawnPosition());
            newPlaneBehaviour.OnDestroyedPlane += RespawnPlane;
        }
        
        Debug.Log("Start Game");
    }

    [Server]
    private async void RespawnPlane(PlaneBehaviour planeObject)
    {
        var connectionToClient = planeObject.GetComponent<NetworkIdentity>().connectionToClient;
        
        NetworkServer.Destroy(planeObject.gameObject);

        await Task.Delay(2500);

        var newPlaneBehaviour = PlaneInstantiate(connectionToClient, planeData, GetSpawnPosition());
        newPlaneBehaviour.OnDestroyedPlane += RespawnPlane;
    }
    
    public PlaneBehaviour PlaneInstantiate(NetworkConnection conn, PlaneData planeData, SpawnPlanePoint playerSpawnTransform)
    {
        PlaneBehaviour planeBehaviour = Instantiate(planeData.PlanePrefab, 
            playerSpawnTransform.position, playerSpawnTransform.rotation);
        
        NetworkServer.Spawn(planeBehaviour.gameObject, conn);
        
        return planeBehaviour;
    }
    
    private SpawnPlanePoint GetSpawnPosition()
    {
        int spawnPoint = _spawnPoint == 1 ? 0 : 1;
        _spawnPoint = spawnPoint;
        return spawnPosition[spawnPoint];
    }

    private void RegisterMessage(NetworkConnection conn, PlayerData player)
    {
        var ident = conn.identity.GetComponent<PlayerHandler>();
        PlayerHandler.Instance.PlayerName = player.PlayerName;
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
