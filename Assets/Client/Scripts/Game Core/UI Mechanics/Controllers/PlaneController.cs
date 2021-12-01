using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Game_Core.UI_Mechanics.Controllers
{
    public class PlaneController: AController
    {
        [field:SerializeField]
        public Joystick GetJoystick {  get; }
        
        [field:SerializeField]
        public Slider GetSlider {  get; }
        
        public Action<Vector2> onPositionUpdated;
        public Action<float> onSpeedUpdated;

       
        public PlaneController(ref Joystick joystick, Slider slider)
        {
            GetJoystick = joystick;
            GetSlider = slider;

            GetJoystick.OnDragAction += delegate(Vector2 newJoystickPosition)
            {
                onPositionUpdated?.Invoke(newJoystickPosition);
            };
            
            GetSlider.onValueChanged.AddListener(delegate(float newSpeed)
            {
                onSpeedUpdated?.Invoke(newSpeed);
            });
        }

        ~PlaneController()
        {
            onSpeedUpdated = null;
            onPositionUpdated = null;
        }
    }
}