
using UnityEngine;

public sealed class PlaneEngine
{
    private float _speed;
    private Vector2 _velocity;

    private readonly Transform _planeTransform;
    private readonly Rigidbody2D rigidbody2D;
    private readonly PlaneEngineSettings _planeEngineSettings;

    public PlaneEngine(PlaneEngineSettings planeEngineSettings, Rigidbody2D rigidbody2D)
    {
        this.rigidbody2D = rigidbody2D;
        _planeEngineSettings = planeEngineSettings;
        _planeTransform = this.rigidbody2D.transform;
    }

    public void WorkingEngine()
    {
        _velocity = _planeTransform.right * _speed;
        rigidbody2D.AddForce(_velocity, ForceMode2D.Impulse);
        float thrustForce = Vector2.Dot(rigidbody2D.velocity, rigidbody2D.GetRelativeVector(Vector2.down) * 50.0f);

        Vector2 relForce = Vector2.up * thrustForce;

        rigidbody2D.AddForce(rigidbody2D.GetRelativeVector(relForce));

        if (rigidbody2D.velocity.magnitude > _speed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * _speed;
        }
    }

    public void ChangeSpeed(float newSpeed)
    {
        _speed = Mathf.Clamp(newSpeed,
            _planeEngineSettings.MinSpeed, _planeEngineSettings.MaxSpeed);
    }
}