using System;
using System.Collections;
using Mirror;
using UnityEngine;
using Zenject;


[AddComponentMenu("")]
public class NetworkSystem : NetworkManager
{
    [SerializeField]
    private SpawnPlanePoint[] spawnPosition;
    
    [Inject]
    private LevelInitializer _levelInitializer;

    [Inject]
    private WindowsManager _windowsManager;
    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        var num = numPlayers;
        PlaneBehaviour playerBase = _levelInitializer.
            PlayerInstantiate(spawnPosition[num]);
        
        NetworkServer.AddPlayerForConnection(conn, playerBase.gameObject);

        if (numPlayers == 2)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);
        _levelInitializer.StartLevel();
        Debug.Log("Start Game");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        _windowsManager.CloseLast();
        Debug.Log($"Disconnected: {conn.identity}");
        base.OnServerDisconnect(conn);
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