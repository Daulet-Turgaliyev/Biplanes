using Mirror;
using UnityEngine;
using Zenject;

public sealed class LevelInjectInstaller : MonoInstaller
{
    [SerializeField] 
    private WindowsManager _windowsManager;
    
    [SerializeField] 
    private NetworkManager _networkManager;
    
    [SerializeField] 
    private GameManager _gameManager;
    
    
    
    public override void InstallBindings()
    {
        Container.Bind<WindowsManager>().FromInstance(_windowsManager).NonLazy();
        Container.Bind<GameManager>().FromInstance(_gameManager).NonLazy();
        Container.Bind<NetworkManager>().FromInstance(_networkManager).NonLazy();
    }
}