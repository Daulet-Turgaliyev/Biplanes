
    using System;
    using UnityEngine;

    public class PilotBase: IBaseObject
    {
        private Rigidbody2D PlaneRigidbodyPlane { get; }
        private SpriteRenderer SpriteParashute { get; }
        public PilotData PilotData { get; }

        public PilotMovement PilotMovement { get; private set; }
        public PilotParachute PilotParachute { get; private set; }

        public PilotBase(Rigidbody2D planeRigidbody2D, PilotData pilotData, SpriteRenderer spriteParashute)
        {
            PlaneRigidbodyPlane = planeRigidbody2D;
            PilotData = pilotData;
            SpriteParashute = spriteParashute;
            Initialize();
        }
        
        public void CustomFixedUpdate()
        {
            PilotMovement.Movement();
        }

        private void Initialize()
        {
            PilotMovementInit();
            PilotParachuteInit();
        }
        
        private void PilotMovementInit()
        {
            var pilotSettings = new PilotSettings(PilotData);
            PilotMovement = new PilotMovement(pilotSettings, PlaneRigidbodyPlane);
            GameManager.Instance.OpenGameWindow(this);
        }
        
        private void PilotParachuteInit()
        {
            if(ReferenceEquals(SpriteParashute, null) == true)
                throw new NullReferenceException($"{nameof(SpriteParashute)} is null");
            
            PilotParachute = new PilotParachute(PlaneRigidbodyPlane, SpriteParashute);
        }
    }