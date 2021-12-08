
using UnityEngine;

public class PlaneEngine
{
    private float _speed;
    private Vector2 _velocity;

    private readonly Transform _planeTransform;
    private readonly Rigidbody2D _rigidbody2D;
    private readonly PlaneEngineSettings _planeEngineSettings;

    public PlaneEngine(PlaneEngineSettings planeEngineSettings, ref Rigidbody2D rigidbody)
    {
        _rigidbody2D = rigidbody;
        _planeEngineSettings = planeEngineSettings;
        _planeTransform = _rigidbody2D.transform;
    }

    public void WorkingEngine()
    {
        _velocity = _planeTransform.right * _speed;
        _rigidbody2D.AddForce(_velocity, ForceMode2D.Impulse);
        float thrustForce = Vector2.Dot(_rigidbody2D.velocity, _rigidbody2D.GetRelativeVector(Vector2.down) * 50.0f);

        Vector2 relForce = Vector2.up * thrustForce;

        _rigidbody2D.AddForce(_rigidbody2D.GetRelativeVector(relForce));

        if (_rigidbody2D.velocity.magnitude > _speed)
        {
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _speed;
        }
    }

    public void ChangeSpeed(float newSpeed)
    {
        _speed = Mathf.Clamp(newSpeed,
            _planeEngineSettings.MinSpeed, _planeEngineSettings.MaxSpeed);
    }
}