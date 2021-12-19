using System.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;


public class LevelInitializer : NetworkBehaviour
{
    [Inject] 
    private WindowsManager _windowsManager;

    [Inject] 
    private NetworkSystem _networkSystem;

    private  PlaneControllerWindow _planeControllerWindow;

    [field: SerializeField] 
    public PlaneData PlaneData { get; private set; }

    public static LevelInitializer Instance;

    public PlaneBase planeBase;
    
    private void Awake()
    {
        // Singlton как временное решение
        Instance = this;
    }

    [ClientRpc]
    public void StartLevel()
    {
        if (ReferenceEquals(planeBase, null) == true)
        {
            Debug.Log("Plane Base не найден");
            return;
        }

        _planeControllerWindow = OpenPlaneControllerWindow();
        
        var planeController = new PlaneController(_planeControllerWindow, planeBase, PlaneData);
    }

    public async void ResetPlane()
    {
        //restart plane
    }

    public PlaneBehaviour PlayerInstantiate(SpawnPlanePoint playerSpawnTransform)
    {
        PlaneBehaviour planeBase = Instantiate(PlaneData.PlanePrefab, 
            playerSpawnTransform.position, playerSpawnTransform.rotation);
        
        return planeBase;
    }

    private PlaneControllerWindow OpenPlaneControllerWindow()
    {
        return _windowsManager.OpenWindow<PlaneControllerWindow>();
    }
}
