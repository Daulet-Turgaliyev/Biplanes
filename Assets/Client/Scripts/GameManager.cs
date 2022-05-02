using System;
using Tools.Singletons;
using UnityEngine;
using Zenject;
 
class GameManager: SingletoneMonoBehaviour<GameManager>
{
    [Inject] private WindowsManager _windowsManager;

    [SerializeField] 
    private Transform _gameViewCanvas;

    [SerializeField]
    private PilotControllerWindow _pilotControllerWindow;

    [SerializeField] 
    private PlaneControllerWindow _planeControllerWindow;
    
    [SerializeField] 
    private GameObject _currentController;
    
    public Action OnStartGame;
    public Action OnStopGame;
    
    public void OpenGameWindow(IBaseObject BaseEntity)
    {
        if (ReferenceEquals(BaseEntity, null) == true)
            throw new NullReferenceException($"{nameof(BaseEntity)} not found");

        CloseCurrentWindow();
        
        switch (BaseEntity)
        {
            case PlaneBase planeBase:
                var planeControllerWindow = Instantiate(_planeControllerWindow, parent: _gameViewCanvas);
                _currentController = planeControllerWindow.gameObject;
                var planeController = new PlaneController(planeControllerWindow, planeBase);
                break;
            case PilotBase pilotBase:
                var pilotControllerWindow = Instantiate(_pilotControllerWindow, parent: _gameViewCanvas);
                _currentController = pilotControllerWindow.gameObject;
                var pilotController = new PilotController(pilotControllerWindow, pilotBase);
                break;
            default:
                throw new ArgumentOutOfRangeException($"{nameof(BaseEntity)} not found");
        }
    }

    public void CloseCurrentWindow()
    {
        if (_currentController == null) return;

        Destroy(_currentController);
    }

    private void OnDestroy()
    {
        OnStartGame = null;
        OnStopGame = null;
    }
}