using Mirror;
using UnityEngine;
using Zenject;


public class LevelInitializer : NetworkBehaviour
{
    [Inject] 
    private WindowsManager _windowsManager;

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

        var planeControllerWindow = OpenPlaneControllerWindow();
        
        var planeController = new PlaneController(planeControllerWindow, planeBase, PlaneData);
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
