using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ErrorViewWindow : BaseWindow
{

    [Inject] private WindowsManager _windowsManager;
    [Inject] private NetworkHandler _networkHandler;
    
    [SerializeField] 
    private TextMeshProUGUI _errorText;

    public void ViewError(string textError)
    {
        _errorText.SetText(textError);
    }

    public void OnOpenStartMenu()
    {
        _networkHandler.AllDisconnect();
        _windowsManager.OpenWindow<StartMenuWindow>();
    }

}
