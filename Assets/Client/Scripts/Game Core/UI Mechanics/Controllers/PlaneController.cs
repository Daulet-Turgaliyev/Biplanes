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

        public PlaneController(Joystick joystick, Slider slider)
        {
            GetJoystick = joystick;
            GetSlider = slider;
        }
    }
}