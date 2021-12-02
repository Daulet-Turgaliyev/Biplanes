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

        [SerializeField] 
        private PlaneData planeData;

        private void Awake()
        {
            OpenPlayerControllerWindow();
        }

        private void OpenPlayerControllerWindow()
        {
            var planeControllerWindow = OpenPlaneControllerWindow();
            var player = PlayerInstantiate();
            player.GlobalInit();

            var planeController = new PlaneController(ref planeControllerWindow, ref player, planeData);
        }

        
        private PlaneBase PlayerInstantiate()
        {
            PlaneBase planeBase = Instantiate(planeData.PlanePrefab);
            return planeBase;
        }

        private PlaneControllerWindow OpenPlaneControllerWindow()
        {
           return _windowsManager.OpenWindow<PlaneControllerWindow>();
        }
    }
}