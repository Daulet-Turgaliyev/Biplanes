using System;
using Mirror;
using UnityEngine.SceneManagement;
using Zenject;

public class LocalModeWindow : BaseWindow, IAutoInjectable
{
    [Inject] private NetworkManager _networkManager;

    public void RequestStartLocal()
    {
        _networkManager.StartHost();
    }


    public void RequestConnectLocal()
    {
        _networkManager.StartClient();
    }
}
