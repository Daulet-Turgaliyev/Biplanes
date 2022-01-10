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

    private PlaneController _planeController;
    
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
        _planeController = new PlaneController(_planeControllerWindow, planeBase);
    }

    public int GetSkinId()
    {
        return PlayerHandler.Instance.PlayerId;
    }
    
    private PlaneControllerWindow GetPlaneControllerWindow()
    {
        return _windowsManager.OpenWindow<PlaneControllerWindow>();
    }

    public void ClosePlanePanel()
    {
        _windowsManager.CloseAll();
    }
}
