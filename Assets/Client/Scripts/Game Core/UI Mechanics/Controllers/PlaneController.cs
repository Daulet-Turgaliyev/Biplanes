using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlaneController : AController
{
    private Action<float> OnSpeedUpdated;
    
    private Action OnShoot;
    private Action OnJump;
    public Action<float> OnReload;

    private readonly PlaneControllerWindow _planeControllerWindow;
    private readonly PlaneBase _planeBase;
    private readonly PlaneData _planeData;
    
    public PlaneController(PlaneControllerWindow planeControllerWindow, PlaneBase planeBase)
    {
        _planeControllerWindow = planeControllerWindow;
        _planeBase = planeBase;
        _planeData = planeBase.PlaneData;
        
        base.Initialize();
    }

    protected override void SubscriptionToAction()
    {
        OnSpeedUpdated += _planeBase.PlaneEngine.ChangeSpeed;
        OnPositionUpdated += _planeBase.PlaneElevator.ChangeJoystickVector;
        OnJump += _planeBase.PlaneCabin.OnJumpUp;
        OnShoot += _planeBase.PlaneWeapon.OnFire;
        OnShoot += ReloadWeapon;
    }

    protected override void SubscriptionToControls()
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
        
        _planeControllerWindow.JumpButton.onClick.AddListener(delegate {
            _planeBase?.OnFastDestroyPlane();
            OnJump?.Invoke();
        });
    }

    protected override void SetValuesControls()
    {
        _planeControllerWindow.SpeedSlider.minValue = _planeData.MinSpeed;
        _planeControllerWindow.SpeedSlider.maxValue = _planeData.MaxSpeed;
    }
    
    private async void ReloadWeapon()
    {
        if (_planeControllerWindow is null) return;
        
        _planeControllerWindow.FireButton.interactable = false;
        await Task.Delay(_planeData.CoolDown);
        if (_planeControllerWindow.FireButton == null) return;
        _planeControllerWindow.FireButton.interactable = true;
    }

    ~PlaneController()
    {
        OnSpeedUpdated = null;
        OnPositionUpdated = null;
        OnShoot = null;
        OnJump = null;
    }
}