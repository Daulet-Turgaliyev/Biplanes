using System;
using AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlaneBase : MonoBehaviour
    {
        [field:SerializeField] 
        public SpriteRenderer SpriteRenderer { get; private set; } 
        
        [field:SerializeField] 
        public Transform CollidersTransform  { get; private set; }
        
        public Rigidbody2D RigidbodyPlane  { get; private set; }

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
            RigidbodyPlane = GetComponent<Rigidbody2D>();
        }

        private void PlaneElementsInit()
        {
            PlaneEngineInit();
            PlaneElevatorInit();
        }

        private void PlaneEngineInit()
        {
            var planeEngineSettings = new PlaneEngineSettings(planeData);
            var rigidBody2D = RigidbodyPlane;
            PlaneEngine = new PlaneEngine(planeEngineSettings, ref rigidBody2D);
        }
        
        private void PlaneElevatorInit()
        {
            var planeElevatorSettings = new PlaneElevatorSettings(planeData.SpeedRotation);
            var planeBase = this;
            PlaneElevator = new PlaneElevator(planeElevatorSettings, ref planeBase);
        }
    }
}