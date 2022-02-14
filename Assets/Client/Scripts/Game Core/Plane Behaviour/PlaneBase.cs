using UnityEngine;


public sealed class PlaneBase: IBaseObject
{
    private Rigidbody2D PlaneRigidbodyPlane { get; }
    public PlaneEngine PlaneEngine { get; private set; }
    public PlaneElevator PlaneElevator { get; private set; }
    public PlaneWeapon PlaneWeapon { get; }
    
    public VelocityLimitChecker VelocityLimitChecker { get; private set; }
    
    public PlaneCabin PlaneCabin { get; }
    public PlaneData PlaneData { get; }

    public PlaneBase(Rigidbody2D planeRigidbody2D, PlaneWeapon planeWeapon, PlaneCabin planeCabin, PlaneData planeData)
    {
        PlaneData = planeData;
        
        PlaneWeapon = planeWeapon;
        
        PlaneCabin = planeCabin;
        
        PlaneRigidbodyPlane = planeRigidbody2D;
        
        PlaneElementsInit();
    }

    public void CustomFixedUpdate()
    {
        PlaneEngine.WorkingEngine();
        PlaneElevator.RotationPlane();
    }


    private void PlaneElementsInit()
    {
        PlaneEngineInit();
        PlaneElevatorInit();
        PlaneWeaponInit();
        PlaneVelocityLimiter();
    }

    private void PlaneEngineInit()
    {
        var planeEngineSettings = new PlaneEngineSettings(PlaneData);
        PlaneEngine = new PlaneEngine(planeEngineSettings, PlaneRigidbodyPlane);
    }

    private void PlaneElevatorInit()
    {
        var planeElevatorSettings = new PlaneElevatorSettings(PlaneData.SpeedRotation);
        PlaneElevator = new PlaneElevator(planeElevatorSettings, PlaneRigidbodyPlane);
    }
    
    private void PlaneWeaponInit()
    {
        var planeWeaponSettings = new PlaneWeaponSettings(PlaneData.CoolDown, PlaneData.BulletAcceleration);
        PlaneWeapon.Init(planeWeaponSettings); 
    }

    private void PlaneVelocityLimiter()
    {
        VelocityLimitChecker = new VelocityLimitChecker(PlaneRigidbodyPlane, PlaneData.VelocityLimit);
    }
}
