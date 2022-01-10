using Mirror;
using UnityEngine;


public class PlayerHandler : NetworkBehaviour
{
    [SyncVar] 
    public int PlayerId;

    [SyncVar]
    public string PlayerName;
    
    public static PlayerHandler Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartClient()
    {
        if (hasAuthority)
        {
            LevelInitializer.Instance.SetPlayerHandler(this);
        }
    }
}
