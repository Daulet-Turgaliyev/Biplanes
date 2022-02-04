﻿using UnityEngine;


public class PlaneBase
{
    private Rigidbody2D PlaneRigidbodyPlane { get; }
    public PlaneEngine PlaneEngine { get; private set; }
    public PlaneElevator PlaneElevator { get; private set; }
    public PlaneWeapon PlaneWeapon { get; }
    public PlaneCabin PlaneCabin { get; }
    public PlaneData PlaneData { get; }

    public PlaneBase(Rigidbody2D planeRigidbody2D, PlaneWeapon planeWeapon, PlaneData planeData)
    {
        PlaneData = planeData;
        
        PlaneWeapon = planeWeapon;
        
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
}
