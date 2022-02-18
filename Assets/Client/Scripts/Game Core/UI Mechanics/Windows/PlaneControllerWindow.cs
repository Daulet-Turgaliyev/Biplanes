using UnityEngine;
using UnityEngine.UI;


public sealed class PlaneControllerWindow : BaseWindow
{
    [field:SerializeField] 
    public Joystick Joystick { get; private set; }
    
    [field:SerializeField] 
    public Slider SpeedSlider { get; private set; }
    
    [field:SerializeField] 
    public Button FireButton { get; private set; }
    
    [field:SerializeField] 
    public Button JumpButton { get; private set; }
}