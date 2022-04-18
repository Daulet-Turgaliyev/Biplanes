
    using UnityEngine;

    public sealed class PilotSettings
    {
        public readonly float SpeedRun;
        public readonly Vector2 FallSpeed;
        public readonly float FlightControlSpeed;
        
        public PilotSettings(PilotData planeData)
        {
            FlightControlSpeed = planeData.FlightControlSpeed;
            FallSpeed = planeData.FallSpeedLimit;
            SpeedRun = planeData.SpeedRun;
        }
    }