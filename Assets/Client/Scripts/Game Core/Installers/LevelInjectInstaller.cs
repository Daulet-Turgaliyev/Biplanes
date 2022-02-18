using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public sealed class LevelInjectInstaller : MonoInstaller
{
    [FormerlySerializedAs("WindowsManager"),SerializeField] 
    private WindowsManager _windowsManager;
    
    [FormerlySerializedAs("NetworkSystem"),SerializeField] 
    private NetworkSystem _networkSystem;
    
    [FormerlySerializedAs("GameManager"),SerializeField] 
    private GameManager _gameManager;
    
    [FormerlySerializedAs("LevelInitializer"),SerializeField] 
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