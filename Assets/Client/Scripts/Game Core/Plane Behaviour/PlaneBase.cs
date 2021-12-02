using AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings;
using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlaneBase : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        public PlaneEngine PlaneEngine { get; private set; }
        public PlaneElevator PlaneElevator { get; private set; }
        
        
        [SerializeField] 
        private PlaneData planeData;
        
        public void GlobalInit()
        {
            PlaneBaseInit();
            PlaneElementsInit();
        }

        private void FixedUpdate()
        {
            PlaneEngine.WorkingEngine();
            PlaneElevator.RotationPlane();
        }

        private void PlaneBaseInit()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void PlaneElementsInit()
        {
            PlaneEngineInit();
            PlaneElevatorInit();
        }

        private void PlaneEngineInit()
        {
            var planeEngineSettings = new PlaneEngineSettings(planeData);
            PlaneEngine = new PlaneEngine(planeEngineSettings, ref _rigidbody2D);
        }
        
        private void PlaneElevatorInit()
        {
            var planeElevatorSettings = new PlaneElevatorSettings(false, planeData.SpeedRotation);
            PlaneElevator = new PlaneElevator(planeElevatorSettings, ref _rigidbody2D);
        }
    }
}