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
        
        [Inject]
        private SpriteRenderer _planeSkin;

        private readonly Transform _transformPlane;
        
        [Inject]
        private readonly Transform collidersPlane;
        
        [Inject]
        private readonly Rigidbody2D _rigidbody2D;

        private Vector3 joystickVector;
        
        public PlaneElevator(PlaneElevatorSettings planeElevatorSettings)
        {
            _speedRotation = planeElevatorSettings.SpeedRotation;
           // _transformPlane = _rigidbody2D.transform;
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
            // Времнная затычка, нужно будет пересмотреть
            var rotation = _transformPlane.rotation;
            if (rotation.eulerAngles.z > 90 && rotation.eulerAngles.z < 280)
            {
                _planeSkin.flipY = true;
                collidersPlane.rotation = Quaternion.Euler(180,0,0);
            }
            else
            {
                _planeSkin.flipY = false;
                collidersPlane.rotation = Quaternion.identity;
            }
            
        }

    }
}