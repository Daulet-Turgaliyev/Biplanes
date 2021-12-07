using System;
using UnityEngine;

public class PlaneController : AController
{
    public Action<Vector2> onPositionUpdated;
    public Action<float> onSpeedUpdated;


    public PlaneController(ref PlaneControllerWindow planeControllerWindow, ref PlaneBase planeBase,
        PlaneData planeData)
    {
        planeControllerWindow.SpeedSlider.minValue = planeData.MinSpeed;
        planeControllerWindow.SpeedSlider.maxValue = planeData.MaxSpeed;

        planeControllerWindow.Joystick.OnDragAction += delegate(Vector2 newJoystickPosition)
        {
            onPositionUpdated?.Invoke(newJoystickPosition);
        };

        planeControllerWindow.SpeedSlider.onValueChanged.AddListener(delegate(float newSpeed)
        {
            onSpeedUpdated?.Invoke(newSpeed);
        });

        onSpeedUpdated += planeBase.PlaneEngine.ChangeSpeed;
        onPositionUpdated += planeBase.PlaneElevator.ChangeJoystickVector;
    }

    ~PlaneController()
    {
        onSpeedUpdated = null;
        onPositionUpdated = null;
    }
}