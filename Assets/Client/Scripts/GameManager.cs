using System;
using Mirror;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field:SerializeField]
    public PlaneBehaviour LocalPlaneBehaviour { get; private set; }
    
    [Inject] 
    private WindowsManager _windowsManager;

    [Inject] 
    private NetworkSystem _networkSystem;
    
    private PlaneControllerWindow _planeControllerWindow;
    private PilotControllerWindow _pilotControllerWindow;

    private void Awake()
    {
        // Singlton как временное решение
        Instance = this;
    }
    
    public PlaneBehaviour SetPlaneBehaviour(PlaneBehaviour planeBehaviour, bool isCurrentDestroy = false)
    {
        if (isCurrentDestroy)
        {
            if(ReferenceEquals(planeBehaviour, null) == true)
                throw new NullReferenceException($"{nameof(planeBehaviour)} not found");
            
            Destroy(LocalPlaneBehaviour.gameObject);
        }
        
        LocalPlaneBehaviour = planeBehaviour;
        return LocalPlaneBehaviour;
    }
    
//TODO: Let see it later
    public void OpenGameWindow(PlaneBase planeBase)
    {
        if(ReferenceEquals(planeBase, null) == true)
            throw new NullReferenceException($"{nameof(planeBase)} not found");
        
        _planeControllerWindow = _windowsManager.OpenWindow<PlaneControllerWindow>();
        var planeController = new PlaneController(_planeControllerWindow, planeBase);
    }

    public void OpenGameWindow(PilotBase pilotBase)
    {
        _pilotControllerWindow = _windowsManager.OpenWindow<PilotControllerWindow>();
        var planeController = new PilotController(_pilotControllerWindow, pilotBase);
    }

    public void CloseCurrentWindow() => _windowsManager.CloseLast();


    public void DestroyPilot(PilotBehaviour pilotBehaviour)
    {
        NetworkSystem.DestroyPilot(pilotBehaviour);
    }

    public void RespawnPlaneFromHuman(NetworkConnection networkConnection)
    {
        _networkSystem.RespawnPlane(networkConnection);
    }
}