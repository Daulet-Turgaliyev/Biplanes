using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Networking.Types;
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

    private int numberPlayer;
    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        numberPlayer = numPlayers;
        
        PlaneBehaviour playerBase = _levelInitializer.PlayerInstantiate(GetSpawnPosition());

        NetworkServer.AddPlayerForConnection(conn, playerBase.gameObject);

        if (numPlayers == 2)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(.2f);
        _levelInitializer.StartLevel();
        Debug.Log("Start Game");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        _windowsManager.CloseAll();
        Debug.Log($"Disconnected: {conn.identity}");
        base.OnServerDisconnect(conn);
    }

    public SpawnPlanePoint GetSpawnPosition()
    {
        return spawnPosition[numberPlayer];
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