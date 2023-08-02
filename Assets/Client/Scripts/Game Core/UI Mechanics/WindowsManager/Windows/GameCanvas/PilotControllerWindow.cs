
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class PilotControllerWindow : MonoBehaviour
    {
        [field:SerializeField] 
        public Joystick Joystick { get; private set; }
        
        [field:SerializeField] 
        public Button OpenParachuteButton { get; private set; }
        
        public Action OnDestroyController = delegate {  };
        
        private void OnDestroy()
        {
            OnDestroyController();
            
            OnDestroyController = null;
        }
    }
