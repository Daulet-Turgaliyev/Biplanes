
    public sealed class PilotSettings
    {
        public readonly float SpeedRun;
        public readonly float SpeedFly;

        public PilotSettings(PilotData planeData)
        {
            SpeedRun = planeData.SpeedRun;
            SpeedFly = planeData.SpeedFly;
        }
    }