using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client.Scripts.Game_Core.UI_Mechanics.Controllers
{
    public class PlaneController: AController
    {
        [field:SerializeField]
        public Joystick GetJoystick {  get; }
        
        [field:SerializeField]
        public Slider GetSlider {  get; }
        
        public Action<float> onSpeedUpdated;

       
        public PlaneController(Joystick joystick, Slider slider)
        {
            GetJoystick = joystick;
            GetSlider = slider;
            
            GetSlider.onValueChanged.AddListener(delegate(float newSpeed)
            {
                onSpeedUpdated?.Invoke(newSpeed);
            });
        }

        public float GetJoystickVerticalPosition()
        {
            return GetJoystick.Vertical;
        }
    }
}