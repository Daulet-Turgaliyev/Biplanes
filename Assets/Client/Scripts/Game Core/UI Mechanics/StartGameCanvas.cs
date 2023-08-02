using System;
using UnityEngine;
using Zenject;

public class StartGameCanvas : MonoBehaviour
{

    [Inject] private WindowsManager _windowsManager;

    private void Start()
    {
        _windowsManager.OpenWindow<StartMenuWindow>();
    }

}
