using System;
using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    public class PlaneEngine: MonoBehaviour
    {
        private float _speed;
        private Vector2 _velocity;
        private Rigidbody2D _rigidbody2D;
        
        private void FixedUpdate()
        {
            WorkingEngine();
        }

        private void WorkingEngine()
        {
            _velocity = transform.parent.right * _speed;
            _rigidbody2D.AddForce(_velocity, ForceMode2D.Impulse);
            
            float thrustForce = Vector2.Dot(_rigidbody2D.velocity, _rigidbody2D.GetRelativeVector(Vector2.down) * 50.0f);

            Vector2 relForce = Vector2.up * thrustForce;

            _rigidbody2D.AddForce(_rigidbody2D.GetRelativeVector(relForce));

            if (_rigidbody2D.velocity.magnitude > _speed)
            {
                _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _speed;
            }
        }
    }
}