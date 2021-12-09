using UnityEngine;


public class PlaneBase
{

    public Rigidbody2D PlaneRigidbodyPlane { get; }

    public PlaneEngine PlaneEngine { get; private set; }
    public PlaneElevator PlaneElevator { get; private set; }
    public PlaneWeapon PlaneWeapon { get; private set; }

    private PlaneData _planeData;

    public PlaneBase(Rigidbody2D planeRigidbody2D, PlaneWeapon planeWeapon)
    {
        _planeData = Resources.Load<PlaneData>("Data/Planes/SimplePlane");
        
        PlaneWeapon = planeWeapon;
        
        this.PlaneRigidbodyPlane = planeRigidbody2D;
        
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
    }

    private void PlaneEngineInit()
    {
        var planeEngineSettings = new PlaneEngineSettings(_planeData);
        PlaneEngine = new PlaneEngine(planeEngineSettings, PlaneRigidbodyPlane);
    }

    private void PlaneElevatorInit()
    {
        var planeElevatorSettings = new PlaneElevatorSettings(_planeData.SpeedRotation);
        PlaneElevator = new PlaneElevator(planeElevatorSettings, PlaneRigidbodyPlane);
    }
    
    private void PlaneWeaponInit()
    {
        var planeWeaponSettings = new PlaneWeaponSettings(_planeData.CoolDown, _planeData.BulletAcceleration);
        PlaneWeapon.Init(planeWeaponSettings); 
    }
}
