using UnityEngine;


public class PlaneBase
{

    public Rigidbody2D PlaneRigidbodyPlane { get; }

    public PlaneEngine PlaneEngine { get; private set; }
    public PlaneElevator PlaneElevator { get; private set; }

    private PlaneData _planeData;

    public PlaneBase(ref Rigidbody2D planeRigidbody2D)
    {
        _planeData = Resources.Load<PlaneData>("Data/Planes/SimplePlane");
        
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
    }

    private void PlaneEngineInit()
    {
        var planeEngineSettings = new PlaneEngineSettings(_planeData);
        var rigidBody2D = PlaneRigidbodyPlane;
        PlaneEngine = new PlaneEngine(planeEngineSettings, ref rigidBody2D);
    }

    private void PlaneElevatorInit()
    {
        var planeElevatorSettings = new PlaneElevatorSettings(_planeData.SpeedRotation);
        var planeBase = this;
        PlaneElevator = new PlaneElevator(planeElevatorSettings, ref planeBase);
    }
}
