using System;
using AcinusProject.Game_Core.Plane_Behaviour;
using Client.Scripts.Game_Core.UI_Mechanics;
using Client.Scripts.Game_Core.UI_Mechanics.Controllers;
using UnityEngine;
using Zenject;

namespace Client.Scripts.Level_Manager
{
    public class LevelInitializer : MonoBehaviour
    {
        [Inject] 
        private WindowsManager _windowsManager;

        [Inject] 
        private UserInterfaceHandler userInterfaceHandler;
        
        [SerializeField] 
        private PlaneData planeData;

        private void Awake()
        {
            OpenPlayerControllerWindow();
            PlayerInstantiate();
        }

        private void OpenPlayerControllerWindow()
        {
            var planeControllerWindow = _windowsManager.OpenWindow<PlaneControllerWindow>();
            var joystick = planeControllerWindow.Joystick;
            
            userInterfaceHandler.SetPlaneController(
                new PlaneController(ref joystick, planeControllerWindow.speedSlider));
        }

        private void PlayerInstantiate()
        {
            PlaneBase planeBase = Instantiate(planeData.PlanePrefab); 
            planeBase.GlobalInit(userInterfaceHandler);
        }
    }
}