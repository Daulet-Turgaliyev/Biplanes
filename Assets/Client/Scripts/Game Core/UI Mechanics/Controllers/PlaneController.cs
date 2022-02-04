using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlaneController : AController
{
    public Action<Vector2> OnPositionUpdated;
    public Action<float> OnSpeedUpdated;
    
    public Action OnShoot;
    public Action OnJump;
    public Action<float> OnReload;

    private PlaneControllerWindow _planeControllerWindow;
    private PlaneBase _planeBase;
    private PlaneData _planeData;
    
    public PlaneController(PlaneControllerWindow planeControllerWindow, PlaneBase planeBase)
    {
        _planeControllerWindow = planeControllerWindow;
        _planeBase = planeBase;
        _planeData = planeBase.PlaneData;

        Initialize();
    }

    private void Initialize()
    {
        SubscriptionToAction();
        SubscriptionToControls();
        SetValuesControls();
    }
    
    private void SubscriptionToAction()
    {
        OnSpeedUpdated += _planeBase.PlaneEngine.ChangeSpeed;
        OnPositionUpdated += _planeBase.PlaneElevator.ChangeJoystickVector;
        OnShoot += _planeBase.PlaneWeapon.OnFire;
        OnShoot += ReloadWeapon;
    }
    
    private void SubscriptionToControls()
    {
        _planeControllerWindow.Joystick.OnDragAction += delegate(Vector2 newJoystickPosition)
        {
            OnPositionUpdated?.Invoke(newJoystickPosition);
        };

        _planeControllerWindow.SpeedSlider.onValueChanged.AddListener(delegate(float newSpeed)
        {
            OnSpeedUpdated?.Invoke(newSpeed);
        });

        _planeControllerWindow.FireButton.onClick.AddListener(delegate
        {
            OnShoot?.Invoke();
        });
        
        _planeControllerWindow.JumpButton.onClick.AddListener(delegate
        {
            OnJump?.Invoke();
        });
    }

    private void SetValuesControls()
    {
        _planeControllerWindow.SpeedSlider.minValue = _planeData.MinSpeed;
        _planeControllerWindow.SpeedSlider.maxValue = _planeData.MaxSpeed;
    }
    
    private async void ReloadWeapon()
    {
        if (_planeControllerWindow is { })
        {
            _planeControllerWindow.FireButton.interactable = false;
            await Task.Delay(_planeData.CoolDown);
            _planeControllerWindow.FireButton.interactable = true;
        }
    }

    ~PlaneController()
    {
        OnSpeedUpdated = null;
        OnPositionUpdated = null;
        OnShoot = null;
    }
}