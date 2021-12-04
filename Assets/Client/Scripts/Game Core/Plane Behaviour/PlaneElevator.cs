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
        
        private readonly SpriteRenderer _planeSkin;

        private readonly Transform _transformPlane;
        
        private readonly Transform collidersPlane;
        
        private readonly Rigidbody2D _rigidbody2D;

        private Vector3 joystickVector;
        
        public PlaneElevator(PlaneElevatorSettings planeElevatorSettings, ref PlaneBase planeBase)
        {
            _speedRotation = planeElevatorSettings.SpeedRotation;
            _rigidbody2D = planeBase.RigidbodyPlane;
            _transformPlane = _rigidbody2D.transform;
            collidersPlane = planeBase.CollidersTransform;
            _planeSkin = planeBase.SpriteRenderer;
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
            if (rotation.eulerAngles.z > 100 && rotation.eulerAngles.z < 290)
            {
                _planeSkin.flipY = true;
                collidersPlane.localEulerAngles = new Vector3(180,0,0);
            }
            else
            {
                _planeSkin.flipY = false;
                collidersPlane.localEulerAngles = Vector3.zero;
            }
            
        }

    }
}