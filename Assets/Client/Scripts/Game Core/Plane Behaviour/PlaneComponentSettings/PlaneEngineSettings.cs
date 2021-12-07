
    public readonly struct PlaneEngineSettings
    {
        public float MinSpeed { get; }
        public float MaxSpeed { get; }

        public PlaneEngineSettings(PlaneData planeData)
        {
            MinSpeed = planeData.MinSpeed;
            MaxSpeed = planeData.MaxSpeed;
        }
    }
