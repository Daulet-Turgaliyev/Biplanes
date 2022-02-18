
    using System;
    using UnityEngine;

    public sealed class PilotBase: IBaseObject
    {
        private Rigidbody2D PlaneRigidbodyPlane { get; }
        public PilotData PilotData { get; }

        public PilotMovement PilotMovement { get; private set; }
        public PilotParachute PilotParachute { get; private set; }
        
        public Action OnCloseParachute = () => {};

        public PilotBase(Rigidbody2D planeRigidbody2D, PilotData pilotData, PilotParachute pilotParachute)
        {
            PlaneRigidbodyPlane = planeRigidbody2D;
            PilotData = pilotData;
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

        public void Initialize()
        {
            PilotMovementInit();
        }
        //TODO: Возможна ошибка последовательности подключени с Pilot Controller
        private void PilotMovementInit()
        {
            var pilotSettings = new PilotSettings(PilotData);
            PilotMovement = new PilotMovement(pilotSettings, PlaneRigidbodyPlane);
            OnCloseParachute += PilotParachute.CloseParachute;
            GameManager.Instance.OpenGameWindow(this);
        }
    }