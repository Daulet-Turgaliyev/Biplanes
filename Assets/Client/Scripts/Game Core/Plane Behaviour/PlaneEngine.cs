﻿using System;
using AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings;
using Client.Scripts.Game_Core.UI_Mechanics.Controllers;
using UnityEngine;
using UnityEngine.UI;


namespace AcinusProject.Game_Core.Plane_Behaviour
{
    public class PlaneEngine
    {
        private float _speed;
        private Vector2 _velocity;
        private readonly Transform _planeTransform;
        private readonly Rigidbody2D _rigidbody2D;
        private readonly PlaneEngineSettings _planeEngineSettings;
        
        public PlaneEngine(PlaneEngineSettings planeEngineSettings)
        {
            _rigidbody2D = planeEngineSettings.Rigidbody;
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
            if (_planeEngineSettings.MinSpeed > newSpeed)
            {
                Debug.Log("It's impossible to set speed LESS than MinSpeed");
                return;
            }
            
            if (_planeEngineSettings.MaxSpeed < newSpeed)
            {
                Debug.Log("It's impossible to set speed HIGHER than MaxSpeed");
                return;
            }
            
            _speed = newSpeed;
        }
    }
}