
namespace AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings
{
    public readonly struct PlaneElevatorSettings
    { 
        public float SpeedRotation { get; }
        
        public PlaneElevatorSettings(float speedRotation)
        {
            SpeedRotation = speedRotation;
        }
    }
}