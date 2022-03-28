
    using System;
    using UnityEngine;

    public class PilotController: AController
    {
        private Action OnOpenParachute = () => {};
        
        private readonly PilotControllerWindow _pilotControllerWindow;
        private readonly PilotBase _pilotBase;

        public PilotController(PilotControllerWindow pilotControllerWindow, PilotBase pilotBase)
        {
            _pilotControllerWindow = pilotControllerWindow;
            _pilotBase = pilotBase;
            base.Initialize();
        }

        ~PilotController()
        {
            OnPositionUpdated = null;
            OnOpenParachute = null;
        }

        protected override void SubscriptionToAction()
        {
            OnPositionUpdated += _pilotBase.PilotMovement.ChangeJoystickVector;
            OnOpenParachute += _pilotBase.PilotParachute.OpenParachute;
        }

        protected override void SubscriptionToControls()
        {
            _pilotControllerWindow.Joystick.OnDragAction += delegate(Vector2 newJoystickPosition)
            {
                OnPositionUpdated?.Invoke(newJoystickPosition);
            };
            
            _pilotControllerWindow.OpenParachuteButton.onClick.AddListener(delegate
            {
                _pilotControllerWindow.OpenParachuteButton.interactable = false;
                OnOpenParachute?.Invoke();
            });

            _pilotBase.OnCloseParachute += () => {
                _pilotControllerWindow.OpenParachuteButton.interactable = false;
            };
        }

        protected override void SetValuesControls()
        {
        }
    }
