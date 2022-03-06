
    using System;
    using UnityEngine;

    public sealed class PilotBase: IBaseObject
    {
        private readonly Rigidbody2D _planeRigidbodyPlane;
        
        private readonly PilotData _pilotData;

        public readonly PilotParachute PilotParachute;
        
        public PilotMovement PilotMovement { get; private set; }

        public Action OnCloseParachute = () => {};

        public PilotBase(Rigidbody2D planeRigidbody2D, PilotData pilotData, PilotParachute pilotParachute)
        {
            _planeRigidbodyPlane = planeRigidbody2D;
            _pilotData = pilotData;
            PilotParachute = pilotParachute;
            Initialize();
        }

        ~PilotBase()
        {
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
        
        //TODO: Возможна ошибка последовательности подключени с Pilot Controller
        private void PilotMovementInit()
        {
            var pilotSettings = new PilotSettings(_pilotData);
            PilotMovement = new PilotMovement(pilotSettings, _planeRigidbodyPlane);
            OnCloseParachute += PilotParachute.CloseParachute;
            GameManager.Instance.OpenGameWindow(this);
        }
    }