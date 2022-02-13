
    using System;
    using UnityEngine;

    public class PilotController: AController
    {
        private Action<Vector2> OnPositionUpdated;
        private Action OnOpenParachute = () => {};

        private readonly PilotControllerWindow _pilotControllerWindow;
        private readonly PilotBase _pilotBase;


        public PilotController(PilotControllerWindow pilotControllerWindow, PilotBase pilotBase)
        {
            _pilotControllerWindow = pilotControllerWindow;
            _pilotBase = pilotBase;

            PlaneControllerWindowGameObject = pilotControllerWindow.gameObject;
            Initialize();
        }

        ~PilotController()
        {
            OnPositionUpdated = null;
            OnOpenParachute = null;
        }
        
        private void Initialize()
        {
            SubscriptionToControls();
            SubscriptionToAction();
        }
        
        private void SubscriptionToAction()
        {
            OnPositionUpdated += _pilotBase.PilotMovement.ChangeJoystickVector; 
            OnOpenParachute += _pilotBase.PilotParachute.OpenParachute;
        }
        
        private void SubscriptionToControls()
        {
            _pilotControllerWindow.Joystick.OnDragAction += delegate(Vector2 newJoystickPosition)
            {
                OnPositionUpdated?.Invoke(newJoystickPosition);
            };
            
            _pilotControllerWindow.OpenParachuteButton.onClick.AddListener(delegate
            {
                OnOpenParachute?.Invoke();
            });
        }
        
    }
