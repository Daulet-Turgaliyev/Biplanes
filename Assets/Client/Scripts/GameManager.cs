using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Inject] 
    private WindowsManager _windowsManager;
    
    [SerializeField]
    private GameObject _currentController;

    
    [field:SerializeField] 
    public Text ScoreText { get; private set; }
    
    private void Awake()
    {
        // Singlton как временное решение
        Instance = this;
    }

    
    public void OpenGameWindow(IBaseObject BaseEntity)
    {
        if(ReferenceEquals(BaseEntity, null) == true)
            throw new NullReferenceException($"{nameof(BaseEntity)} not found");

        GameEntity gameEntity;

        if(ReferenceEquals(_currentController, null) == false)
            Destroy(_currentController);
        
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
    
    public void CloseCurrentWindow()
    {
        if (ReferenceEquals(_currentController, null)) return;

        Debug.Log(_currentController);
        Destroy(_currentController);
    }
}