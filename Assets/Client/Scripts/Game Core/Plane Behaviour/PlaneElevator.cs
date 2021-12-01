using AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings;
using Client.Scripts.Game_Core.UI_Mechanics;
using Client.Scripts.Game_Core.UI_Mechanics.Controllers;
using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    public class PlaneElevator
    {
        private float _speedRotation;
        private bool _flip;

        private readonly Transform _transformPlane;
        private readonly Rigidbody2D _rigidbody2D;

        private Vector2 joystickVector;
        
        public PlaneElevator(ref PlaneElevatorSettings planeElevatorSettings, ref PlaneController planeController)
        {
            _flip = planeElevatorSettings.Flip;
            _rigidbody2D = planeElevatorSettings.Rigidbody;
            _speedRotation = planeElevatorSettings.RotationSpeed;
            _transformPlane = _rigidbody2D.transform;

            planeController.onPositionUpdated += SetJoystickVector;
        }


        public void RotationPlane()
        {
            float Dir = Vector2.Dot(_rigidbody2D.velocity, _rigidbody2D.GetRelativeVector(Vector2.right));
            if (Dir > 0)
            {
                _rigidbody2D.rotation +=  joystickVector.y *  _speedRotation;
                _rigidbody2D.angularVelocity = 0;
                TurnChecker();
            }
        }

        private void SetJoystickVector(Vector2 newJoystickVector) => joystickVector = newJoystickVector;
 
        
        private void TurnChecker()
        {
            var rotation = _transformPlane.rotation;
            _flip = rotation.eulerAngles.z > 90 && rotation.eulerAngles.z < 280;
        }

    }
}