using System;
using Mirror;
using UnityEngine;
using Zenject;


[AddComponentMenu("")]
public class NetworkSystem : NetworkManager
{
    [field: SerializeField] 
    private Transform leftPlaneSpawn;

    [field: SerializeField] 
    private Transform rightPlaneSpawn;

    [Inject]
    private LevelInitializer _levelInitializer;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startTransform = numPlayers == 0 ? leftPlaneSpawn : rightPlaneSpawn;
        PlaneBehaviour playerBase = _levelInitializer.PlayerInstantiate(startTransform);
        NetworkServer.AddPlayerForConnection(conn, playerBase.gameObject);
        
        _levelInitializer.LocalPlayerInit();

        if (numPlayers == 2)
        {
            Debug.Log("Start Game");
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log($"Disconnected: {conn.identity}");
        base.OnServerDisconnect(conn);
    }
}
