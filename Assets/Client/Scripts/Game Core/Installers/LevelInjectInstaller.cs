using UnityEngine;
using Zenject;

public class LevelInjectInstaller : MonoInstaller
{
    [SerializeField] 
    private WindowsManager windowsManager;
    
    [SerializeField] 
    private LevelInitializer levelInitializer;
    public override void InstallBindings()
    {
        Container.Bind<GlobalSettings.Screen>().AsSingle().Lazy();
        Container.Bind<BackButton>().AsSingle().Lazy();
        Container.Bind<WindowsManager>().FromInstance(windowsManager).NonLazy();
        Container.Bind<LevelInitializer>().FromInstance(levelInitializer).NonLazy();
    }
}