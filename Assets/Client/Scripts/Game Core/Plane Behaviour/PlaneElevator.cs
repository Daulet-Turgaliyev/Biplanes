using UnityEngine;


public sealed class PlaneElevator: IJoystickVector
{
    private float _speedRotation;

    private readonly Rigidbody2D _rigidbody2D;

    private Vector3 _joystickVector;

    public PlaneElevator(PlaneElevatorSettings planeElevatorSettings, Rigidbody2D planeRigidbody)
    {
        _speedRotation = planeElevatorSettings.SpeedRotation;
        _rigidbody2D = planeRigidbody;
    }


    public void RotationPlane()
    {
        float dir = Vector2.Dot(_rigidbody2D.velocity, _rigidbody2D.GetRelativeVector(Vector2.right));
        if (dir > 0)
        {
            _rigidbody2D.rotation += _joystickVector.y * _speedRotation;
            _rigidbody2D.angularVelocity = 0;
        }
    }

    public void ChangeJoystickVector(Vector2 newJoystickVector) => _joystickVector = newJoystickVector;

}