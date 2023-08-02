using Mirror;
using Zenject;

public class StartMenuWindow : BaseWindow, IAutoInjectable
{
    [Inject] private WindowsManager _windowsManager;
    [Inject] private NetworkManager _networkManager;

    public void OnOpenNetworkMode()
    {
        _networkManager.StartClient();
    }

    public void OnOpenLocalMode()
    {
        _windowsManager.OpenWindow<LocalModeWindow>();
    }
}
