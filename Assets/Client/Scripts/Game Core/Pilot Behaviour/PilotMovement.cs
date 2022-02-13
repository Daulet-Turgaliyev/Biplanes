
    using UnityEngine;

    public class PilotMovement : IJoystickVector
    {
        private Vector2 _joystickVector;

        private readonly Rigidbody2D _rigidbody2D;

        private bool _isGround;
        private float _speedRun;
        private float _speedFly;
        

        public PilotMovement(PilotSettings pilotSettings, Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
              _speedRun = pilotSettings.SpeedRun;
             _speedFly = pilotSettings.SpeedFly;
        }

        public void Movement()
        {
            if (_isGround)
            {
                _rigidbody2D.velocity = new Vector2(_joystickVector.x * _speedRun,_rigidbody2D.velocity.y);
            }
            else
            {
                _rigidbody2D.AddForce(_joystickVector * _speedFly);
                _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _speedFly);
            }
        }

        public void ChangeJoystickVector(Vector2 newJoystickVector) => _joystickVector = newJoystickVector;
        public void ChangeGroundState(bool isGround) => _isGround = isGround;
    }