using UnityEngine;
using Zenject;

public class LevelInjectInstaller : MonoInstaller
{
    [SerializeField] 
    private WindowsManager windowsManager;
    public override void InstallBindings()
    {
        Container.Bind<GlobalSettings.Screen>().AsSingle().Lazy();
        Container.Bind<BackButton>().AsSingle().Lazy();
        Container.Bind<WindowsManager>().FromInstance(windowsManager).NonLazy();
    }
}