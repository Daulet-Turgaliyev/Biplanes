using System;
using UnityEngine;


public sealed class PlaneBase: IBaseObject
{
    private Rigidbody2D PlaneRigidbody { get; }

    private PlaneEngine _planeEngine;
    public PlaneEngine PlaneEngine
    {
        get
        {
            if (ReferenceEquals(_planeEngine, null))
                throw new NullReferenceException($"{_planeEngine} is null");

            return _planeEngine;
        }
    }

    
    private PlaneElevator _planeElevator;
    public PlaneElevator PlaneElevator
    {
        get
        {
            if (ReferenceEquals(_planeElevator, null))
                throw new NullReferenceException($"{_planeElevator} is null");

            return _planeElevator;
        }
    }

    public PlaneWeapon PlaneWeapon { get; }
    
    public PlaneCabin PlaneCabin { get; }
    public PlaneData PlaneData { get; }

    public Action OnFastDestroyPlane = () => {};
    
    public PlaneBase(Rigidbody2D planeRigidbody2D, PlaneWeapon planeWeapon, PlaneCabin planeCabin, PlaneData planeData)
    {
        PlaneData = planeData;
        
        PlaneWeapon = planeWeapon;
        
        PlaneCabin = planeCabin;
        
        PlaneRigidbody = planeRigidbody2D;
        
        PlaneElementsInit();
    }

    private void PlaneElementsInit()
    {
        PlaneEngineInit();
        PlaneElevatorInit();
        PlaneWeaponInit();
    }
    
    ~PlaneBase()
    {
        OnFastDestroyPlane = null;
    }

    public void CustomFixedUpdate()
    {
        PlaneEngine.WorkingEngine();
        PlaneElevator.RotationPlane();
    }
    

    private void PlaneEngineInit()
    {
        var planeEngineSettings = new PlaneEngineSettings(PlaneData);
        _planeEngine = new PlaneEngine(planeEngineSettings, PlaneRigidbody);
    }

    private void PlaneElevatorInit()
    {
        var planeElevatorSettings = new PlaneElevatorSettings(PlaneData.SpeedRotation);
        _planeElevator = new PlaneElevator(planeElevatorSettings, PlaneRigidbody);
    }
    
    private void PlaneWeaponInit()
    {
        var planeWeaponSettings = new PlaneWeaponSettings(PlaneData.CoolDown, PlaneData.BulletAcceleration);
        PlaneWeapon.Init(planeWeaponSettings); 
    }
}
