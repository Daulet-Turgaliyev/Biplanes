using AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings;
using Client.Scripts.Game_Core.UI_Mechanics;
using Client.Scripts.Game_Core.UI_Mechanics.Controllers;
using UnityEngine;
using Zenject;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    public class PlaneElevator
    {
        private float _speedRotation;
        private bool _flip;

        private readonly Transform _transformPlane;
        private readonly Rigidbody2D _rigidbody2D;

        private Vector3 joystickVector;
        
        public PlaneElevator(PlaneElevatorSettings planeElevatorSettings, ref Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
            _speedRotation = planeElevatorSettings.SpeedRotation;
            _flip = planeElevatorSettings.Flip;
            _transformPlane = _rigidbody2D.transform;
        }


        public void RotationPlane()
        {
            float dir = Vector2.Dot(_rigidbody2D.velocity, _rigidbody2D.GetRelativeVector(Vector2.right));
            if (dir > 0)
            {
                _rigidbody2D.rotation += joystickVector.y *  _speedRotation;
                _rigidbody2D.angularVelocity = 0;
                TurnChecker();
            }
        }

        public void ChangeJoystickVector(Vector2 newJoystickVector) => joystickVector = newJoystickVector;

        private void TurnChecker()
        {
            var rotation = _transformPlane.rotation;
            _flip = rotation.eulerAngles.z > 90 && rotation.eulerAngles.z < 280;
        }

    }
}