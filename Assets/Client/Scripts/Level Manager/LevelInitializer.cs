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
        private LevelData levelData;
        
        [SerializeField] 
        private PlaneData planeData;

        private void Awake()
        {
            LocalPlayerInit();
        }

        private void LocalPlayerInit()
        {
            var planeControllerWindow = OpenPlaneControllerWindow();
            var player = PlayerInstantiate(levelData.SpawnPoints[0]);
            player.GlobalInit();

            var planeController = new PlaneController(ref planeControllerWindow, ref player, planeData);
        }

        
        private PlaneBase PlayerInstantiate(Vector2 spawnPosition)
        {
            var planeBase = Instantiate(planeData.PlanePrefab);
            planeBase.transform.position = spawnPosition;
            return planeBase;
        }

        private PlaneControllerWindow OpenPlaneControllerWindow()
        {
           return _windowsManager.OpenWindow<PlaneControllerWindow>();
        }
    }
}