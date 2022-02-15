using System;
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
    public AController CurrentController { get; private set; }

    
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
        CurrentController = new PlaneController(_planeControllerWindow, planeBase);
    }

    public void OpenGameWindow(PilotBase pilotBase)
    {
        _pilotControllerWindow = _windowsManager.OpenWindow<PilotControllerWindow>();
        CurrentController = new PilotController(_pilotControllerWindow, pilotBase);
    }

    public void CloseCurrentWindow()
    {
        if (ReferenceEquals(CurrentController, null) == true)
        {
            Debug.LogWarning($"{nameof(CurrentController)} not found");
            return;
        }
           // throw new NullReferenceException($"{nameof(CurrentController)} not found");
        Destroy(CurrentController.PlaneControllerWindowGameObject.gameObject);
    }
}