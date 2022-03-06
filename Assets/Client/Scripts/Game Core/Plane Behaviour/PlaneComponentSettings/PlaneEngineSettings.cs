
    public readonly struct PlaneEngineSettings
    {
        public readonly float MinSpeed;
        public readonly float MaxSpeed;

        public PlaneEngineSettings(PlaneData planeData)
        {
            MinSpeed = planeData.MinSpeed;
            MaxSpeed = planeData.MaxSpeed;
        }
    }
