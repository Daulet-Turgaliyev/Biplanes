
namespace AcinusProject.Game_Core.Plane_Behaviour.PlaneComponentSettings
{
    public readonly struct PlaneElevatorSettings
    { 
        public bool Flip { get; }
        public float SpeedRotation { get; }
        
        public PlaneElevatorSettings(bool flip, float speedRotation)
        {
            Flip = flip;
            SpeedRotation = speedRotation;
        }
    }
}