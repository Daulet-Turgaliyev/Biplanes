using System;
using AcinusProject.Game_Core.Plane_Behaviour;
using Client.Scripts.Game_Core.UI_Mechanics;
using Client.Scripts.Game_Core.UI_Mechanics.Controllers;
using Photon.Pun;
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

            Vector2 spawnPos = PhotonNetwork.IsMasterClient ? levelData.SpawnPoints[0] : levelData.SpawnPoints[1];
            
            var player = PlayerInstantiate(spawnPos);
            player.GlobalInit();

            var planeController = new PlaneController(ref planeControllerWindow, ref player, planeData);
        }

        private void GlobalPlayerInit()
        {
            var planeBaseGameObject = PhotonNetwork.Instantiate("SimplePlane_Global", 
                levelData.SpawnPoints[1], Quaternion.identity, 0);

        }

        
        private PlaneBase PlayerInstantiate(Vector2 spawnPosition)
        {
            var planeBaseGameObject = PhotonNetwork.Instantiate("SimplePlane", spawnPosition, Quaternion.identity, 0);
            var planeBase = planeBaseGameObject.GetComponent<PlaneBase>();
            return planeBase;
        }

        private PlaneControllerWindow OpenPlaneControllerWindow()
        {
           return _windowsManager.OpenWindow<PlaneControllerWindow>();
        }
    }
}