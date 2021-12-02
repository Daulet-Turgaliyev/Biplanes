using UnityEngine;
using UnityEngine.UI;


public class PlaneControllerWindow : BaseWindow
{
    [field:SerializeField] 
    public Joystick Joystick { get; private set; }
    
    [field:SerializeField] 
    public Slider SpeedSlider { get; private set; }
}