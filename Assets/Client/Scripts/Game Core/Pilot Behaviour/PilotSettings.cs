
    public sealed class PilotSettings
    {
        public float SpeedRun { get; }
        public float SpeedFly { get; }

        public PilotSettings(PilotData planeData)
        {
            SpeedRun = planeData.SpeedRun;
            SpeedFly = planeData.SpeedFly;
        }
    }