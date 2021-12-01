using System;
using AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings;
using Client.Scripts.Game_Core.UI_Mechanics;
using Client.Scripts.Game_Core.UI_Mechanics.Controllers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlaneBase : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        private PlaneEngine planeEngine;
        private PlaneElevator planeElevator;
        
        private PlaneController planeController;
        
        private UserInterfaceHandler _userInterfaceHandler;
        
        [SerializeField] 
        private PlaneData planeData;
        
        public void GlobalInit(UserInterfaceHandler userInterfaceHandler)
        {
            _userInterfaceHandler = userInterfaceHandler;
            
            PlaneBaseInit();
            PlaneElementsInit();
        }

        private void FixedUpdate()
        {
            planeEngine.WorkingEngine();
            planeElevator.RotationPlane();
        }

        private void PlaneBaseInit()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void PlaneElementsInit()
        {
            PlaneEngineInit();
            PlaneElevatorInit();
            planeController = _userInterfaceHandler.planeController;
            if (ReferenceEquals(planeController, null) == false)
            {
                planeController.onSpeedUpdated += planeEngine.ChangeSpeed;
            }
        }

        private void PlaneEngineInit()
        {
            var speedSlider = _userInterfaceHandler.planeController.GetSlider;
            var planeEngineSettings = new PlaneEngineSettings(planeData,  ref speedSlider, ref _rigidbody2D);
            planeEngine = new PlaneEngine(planeEngineSettings);
        }
        
        private void PlaneElevatorInit()
        {
            var planeElevatorSettings = new PlaneElevatorSettings(_rigidbody2D, planeData.SpeedRotation);
            var controller = _userInterfaceHandler.planeController;
            planeElevator = new PlaneElevator(ref planeElevatorSettings, ref controller);
        }
    }
}