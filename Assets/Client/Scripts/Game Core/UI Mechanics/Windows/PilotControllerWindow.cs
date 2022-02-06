﻿
    using UnityEngine;
    using UnityEngine.UI;

    public class PilotControllerWindow : BaseWindow
    {
        [field:SerializeField] 
        public Joystick Joystick { get; private set; }
        
        [field:SerializeField] 
        public Button OpenParachuteButton { get; private set; }
    }
