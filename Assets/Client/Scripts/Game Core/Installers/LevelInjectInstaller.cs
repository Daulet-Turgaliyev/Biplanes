using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public sealed class LevelInjectInstaller : MonoInstaller
{
    [SerializeField] 
    private WindowsManager _windowsManager;
    
    [SerializeField] 
    private NetworkSystem _networkSystem;
    
    [SerializeField] 
    private GameManager _gameManager;
    
    [SerializeField] 
    private LevelInitializer levelInitializer;
    
    
    public override void InstallBindings()
    {
        Container.Bind<GlobalSettings.Screen>().AsSingle().Lazy();
        Container.Bind<BackButton>().AsSingle().Lazy();
        Container.Bind<WindowsManager>().FromInstance(_windowsManager).NonLazy();
        Container.Bind<LevelInitializer>().FromInstance(levelInitializer).NonLazy();
        Container.Bind<GameManager>().FromInstance(_gameManager).NonLazy();
        Container.Bind<NetworkSystem>().FromInstance(_networkSystem).NonLazy();
    }
}