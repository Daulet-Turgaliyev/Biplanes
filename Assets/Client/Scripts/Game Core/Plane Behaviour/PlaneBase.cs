using System;
using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlaneBase : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        private PlaneEngine planeEngine;

        [SerializeField]
        private PlaneData planeData;

        public Action<float> onSpeedUpdated;
            
        private void Awake()
        {
            PlaneBaseInit();
            PlaneElementsInit();
        }

        private void OnEnable()
        {
                if (planeEngine != null)
                    onSpeedUpdated += planeEngine.ChangeSpeed;
        }

        private void OnDisable()
        {
            onSpeedUpdated = null;
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
        }
        
        private void PlaneEngineInit() 
        {
            var planeEngineSettings = new PlaneEngineSettings(planeData.MinSpeed, planeData.MaxSpeed); 
            planeEngine = new PlaneEngine(planeEngineSettings, _rigidbody2D);
        }
    }
}