using Mirror;
using UnityEngine;
using Zenject;


public class LevelInitializer : NetworkBehaviour
{
    private NetworkIdentity _networkIdentity;
    
    public PlayerHandler PlayerHandler;
    
    [Inject] 
    private WindowsManager _windowsManager;

    [Inject] 
    private NetworkSystem _networkSystem;

    private  PlaneControllerWindow _planeControllerWindow;
    
    
    public PlaneController PlaneController { get; private set; }
    
    public static LevelInitializer Instance;
    
    private void Awake()
    {
        // Singlton как временное решение
        Instance = this;
        _networkIdentity = GetComponent<NetworkIdentity>();
    }
    
    public void SetPlayerHandler(PlayerHandler playerHandler)
    {
        Debug.Log(playerHandler);
        PlayerHandler = playerHandler;
    }
    
    public void StartPlane(PlaneBase planeBase)
    {
        if(ReferenceEquals(planeBase, null) == true)
        {
            Debug.LogError("PlaneBase not found");
            return;
        }
        
        _planeControllerWindow = GetPlaneControllerWindow();
        PlaneController = new PlaneController(_planeControllerWindow, planeBase);
    }

    public int GetSkinId()
    {
        return PlayerHandler.Instance.PlayerId;
    }
    
    private PlaneControllerWindow GetPlaneControllerWindow()
    {
        return _windowsManager.OpenWindow<PlaneControllerWindow>();
    }

    public void DestroyActiveWindow()
    {
        if (ReferenceEquals(_planeControllerWindow, null) == true)
        {
            Debug.LogWarning("_planeControllerWindow not found");
            return;
        }
        
        Destroy(_planeControllerWindow.gameObject);
    }
}
