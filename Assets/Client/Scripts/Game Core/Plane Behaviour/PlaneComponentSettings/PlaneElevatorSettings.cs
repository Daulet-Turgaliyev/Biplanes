using System;
using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings
{
    public readonly struct PlaneElevatorSettings
    {
        public float RotationSpeed { get; }
        public bool Flip { get; }
        public Rigidbody2D Rigidbody { get; }
        
        public PlaneElevatorSettings(Rigidbody2D rigidbody, float rotationSpeed, bool flip = false)
        {
            Rigidbody = rigidbody;
            Flip = flip;
            RotationSpeed = rotationSpeed;
        }
    }
}