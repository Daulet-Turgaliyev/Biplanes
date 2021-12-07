using System;
using Mirror;
using UnityEngine;
using Zenject;


public class LevelInitializer : MonoBehaviour
{
    [Inject] 
    private WindowsManager _windowsManager;

    [field: SerializeField] 
    public PlaneData PlaneData { get; private set; }
    
    private PlaneBehaviour _currentPlane;

    public void LocalPlayerInit()
    {
        if(ReferenceEquals(_currentPlane, null)) return;
        
        var planeControllerWindow = OpenPlaneControllerWindow();

        _currentPlane.StartLocalPlayerInit();

        var playerPlaneBase = _currentPlane.PlaneBase;

        var planeController = new PlaneController(ref planeControllerWindow, ref playerPlaneBase, PlaneData);
    }


    public PlaneBehaviour PlayerInstantiate(Transform playerSpawnTransform)
    {
        PlaneBehaviour planeBase = Instantiate(PlaneData.PlanePrefab, 
            playerSpawnTransform.position, playerSpawnTransform.rotation);
        
        _currentPlane = planeBase;
        return planeBase;
    }

    private PlaneControllerWindow OpenPlaneControllerWindow()
    {
        return _windowsManager.OpenWindow<PlaneControllerWindow>();
    }
}
