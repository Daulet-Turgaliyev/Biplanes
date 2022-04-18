
    using System;
    using UnityEngine;

    public sealed class PilotBase: IBaseObject
    {
        private readonly Rigidbody2D _planeRigidbodyPlane;
        
        private readonly PilotData _pilotData;

        public readonly PilotParachute PilotParachute;

        private bool _isFlipX;
        
        public PilotMovement PilotMovement { get; private set; }

        public Action OnCloseParachute = () => {};

        public Action<bool> OnUpdateFlipX = (bool b) => { };
        
        public PilotBase(Rigidbody2D planeRigidbody2D, PilotData pilotData, PilotParachute pilotParachute)
        {
            _planeRigidbodyPlane = planeRigidbody2D;
            _pilotData = pilotData;
            PilotParachute = pilotParachute;
            Initialize();
        }

        ~PilotBase()
        {
            OnUpdateFlipX = null;
            OnCloseParachute = null;
        }
        
        public void CustomFixedUpdate()
        {
            PilotMovement.Movement();
        }

        private void Initialize()
        {
            PilotMovementInit();
        }

        public void DirectionSpriteUpdate(Vector2 direction)
        {
            var oldFlip = _isFlipX;
            _isFlipX = direction.x > .1f;
            if(oldFlip != _isFlipX) OnUpdateFlipX?.Invoke(_isFlipX);
        }
        
        //TODO: Возможна ошибка последовательности подключени с Pilot Controller
        private void PilotMovementInit()
        {
            var pilotSettings = new PilotSettings(_pilotData);
            PilotMovement = new PilotMovement(pilotSettings, _planeRigidbodyPlane, PilotParachute);
            OnCloseParachute += PilotParachute.CloseParachute;
            GameManager.Instance.OpenGameWindow(this);
        }
    }