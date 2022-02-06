
    using UnityEngine;

    public class PilotMovement
    {
        private float _speed;
        private Vector2 _velocity;

        private readonly Transform _planeTransform;
        private readonly Rigidbody2D rigidbody2D;
        private readonly PilotSettings _pilotSettings;

        public PilotMovement(PilotSettings pilotSettings, Rigidbody2D rigidbody2D)
        {
            this.rigidbody2D = rigidbody2D;
            _pilotSettings = pilotSettings;
            _planeTransform = this.rigidbody2D.transform;
        }
        
        
    }