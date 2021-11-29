using Client.Scripts.Game_Core.UI_Mechanics;
using UnityEngine;
using Zenject;

public class LevelInjectInstaller : MonoInstaller
{
    [SerializeField] 
    private WindowsManager windowsManager;
    
    public override void InstallBindings()
    {
        Container.Bind<UserInterfaceHandler>().AsSingle().NonLazy();
        Container.Bind<GlobalSettings.Screen>().AsSingle().Lazy();
        Container.Bind<BackButton>().AsSingle().Lazy();
        Container.Bind<WindowsManager>().FromInstance(windowsManager).NonLazy();
    }
}