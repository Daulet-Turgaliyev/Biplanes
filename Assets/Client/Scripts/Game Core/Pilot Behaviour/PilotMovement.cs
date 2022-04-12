
    using UnityEngine;

    public sealed class PilotMovement : IJoystickVector
    {
        private Vector2 _joystickVector;

        private readonly Rigidbody2D _rigidbody2D;
        private readonly PilotParachute _pilotParachute;

        private bool _isGround;

        private readonly float _speedRun;
        
        private readonly float _flightControlSpeed;
        private readonly Vector2 _fallSpeed;

        public PilotMovement(PilotSettings pilotSettings, Rigidbody2D rigidbody2D, PilotParachute pilotParachute)
        {
            _rigidbody2D = rigidbody2D;
            _pilotParachute = pilotParachute;
            _speedRun = pilotSettings.SpeedRun;
            _flightControlSpeed = pilotSettings.FlightControlSpeed;
            _fallSpeed = pilotSettings.FallSpeed;
        }

        public void Movement()
        {
            if (_isGround)
            {
                _rigidbody2D.velocity = new Vector2(_joystickVector.x * _speedRun, _rigidbody2D.velocity.y);
            }
            else
            {
                _rigidbody2D.AddForce(_joystickVector * _flightControlSpeed);
                _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _fallSpeed.x);

                if (_pilotParachute.IsParachuteActive)
                {
                    Vector2 currentVelocity = _rigidbody2D.velocity;
                    float newVelocityY = Mathf.Clamp(currentVelocity.y, _fallSpeed.y, 10);
                    Vector2 newVelocity = new Vector2(currentVelocity.x, newVelocityY);
                    _rigidbody2D.velocity = newVelocity;
                }
            }
        }

        public void ChangeJoystickVector(Vector2 newJoystickVector) => _joystickVector = newJoystickVector;
        public void ChangeGroundState(bool isGround) => _isGround = isGround;
    }