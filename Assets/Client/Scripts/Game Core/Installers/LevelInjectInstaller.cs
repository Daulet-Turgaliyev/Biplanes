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

    [SerializeField] 
    private NetworkHandler _networkHandler;
    
    private BackButton _backButton;
    
    public override void InstallBindings()
    {
        Container.Bind<WindowsManager>().FromInstance(_windowsManager).NonLazy();
        Container.Bind<GameManager>().FromInstance(_gameManager).NonLazy();
        Container.Bind<NetworkManager>().FromInstance(_networkManager).NonLazy();
        Container.Bind<NetworkHandler>().FromInstance(_networkHandler).NonLazy();
        Container.Bind<BackButton>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GlobalSettings.Screen>().AsSingle().NonLazy();
    }
}