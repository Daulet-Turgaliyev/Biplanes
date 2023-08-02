using Zenject;

public class ConnectingViewWindow : BaseWindow
{
    [Inject] private WindowsManager _windowsManager;
    [Inject] private NetworkHandler _networkHandler;

    public void OnStopConnect()
    {
        _networkHandler.AllDisconnect();
        _windowsManager.OpenWindow<StartMenuWindow>();
    }
}
