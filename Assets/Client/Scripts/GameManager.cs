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
    
    [SerializeField]
    private GameObject _currentController;

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
    public void OpenGameWindow(IBaseObject BaseEntity)
    {
        if(ReferenceEquals(BaseEntity, null) == true)
            throw new NullReferenceException($"{nameof(BaseEntity)} not found");

        GameEntity gameEntity;

        switch (BaseEntity)
        {
            case PlaneBase planeBase:
                gameEntity = GameEntity.Plane;
                _currentController = _windowsManager.OpenWindow(gameEntity);
                var planeControllerWindow = _currentController.GetComponent<PlaneControllerWindow>();
                var planeController = new PlaneController(planeControllerWindow, planeBase);
                break;
            case PilotBase pilotBase:
                gameEntity = GameEntity.Pilot;
                _currentController = _windowsManager.OpenWindow(gameEntity);
                var pilotControllerWindow = _currentController.GetComponent<PilotControllerWindow>();
                var pilotController = new PilotController(pilotControllerWindow, pilotBase);
                break;
            default:
                throw new ArgumentOutOfRangeException($"{nameof(BaseEntity)} not found");
        }
    }

    [Client]
    public void CloseCurrentWindow()
    {
        if (ReferenceEquals(_currentController, null))
            throw new NullReferenceException($"{nameof(_currentController)} is null");
        
        Debug.Log(_currentController);
        Destroy(_currentController);
    }
}