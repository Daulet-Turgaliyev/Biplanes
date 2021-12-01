using UnityEngine;
using UnityEngine.UI;

namespace AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings
{
    public readonly struct PlaneEngineSettings
    {
        public float MinSpeed { get; }
        public float MaxSpeed { get; }
        
        public Transform TransformPlane { get; }
        public Rigidbody2D Rigidbody { get; }
        
        public PlaneEngineSettings(PlaneData planeData, ref Slider planeSlierController, ref Rigidbody2D rigidbody, bool flip = false)
        {
            MinSpeed = planeData.MinSpeed;
            MaxSpeed = planeData.MaxSpeed;
            TransformPlane = rigidbody.transform;
            Rigidbody = rigidbody;

            planeSlierController.minValue = MinSpeed;
            planeSlierController.maxValue = MaxSpeed;
        }
    }
}