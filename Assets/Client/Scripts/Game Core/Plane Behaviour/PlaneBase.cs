using System;
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

        private UserInterfaceHandler _userInterfaceHandler;
        
        [SerializeField] 
        private PlaneData planeData;

        public UnityEvent<float> onSpeedUpdated;

        public void GlobalInit(UserInterfaceHandler userInterfaceHandler)
        {
            _userInterfaceHandler = userInterfaceHandler;
            
            PlaneBaseInit();
            PlaneElementsInit();
        }

        private void OnEnable()
        {
            if (planeEngine != null)
                onSpeedUpdated.AddListener(planeEngine.ChangeSpeed);
        }

        private void OnDisable()
        {
            onSpeedUpdated.AddListener(planeEngine.ChangeSpeed);
        }

        private void FixedUpdate()
        {
            planeEngine.WorkingEngine();
        }

        private void PlaneBaseInit()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void PlaneElementsInit()
        {
            PlaneEngineInit();
            PlaneController planeController = _userInterfaceHandler.currentController as PlaneController;
            if (ReferenceEquals(planeController, null) == false) 
                onSpeedUpdated.Invoke(planeController.GetSlider.value);
        }

        private void PlaneEngineInit()
        {
            var planeEngineSettings = new PlaneEngineSettings(planeData.MinSpeed, planeData.MaxSpeed);
            planeEngine = new PlaneEngine(planeEngineSettings, ref _rigidbody2D);
        }
    }
}