
    public class PilotSettings
    {
        public float Speed { get; }

        public PilotSettings(PilotData planeData)
        {
            Speed = planeData.Speed;
        }
    }