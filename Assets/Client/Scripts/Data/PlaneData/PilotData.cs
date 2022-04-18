
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Gameplay/Content/Create a new pilot")]
    public class PilotData : ScriptableObject
    {
        [field: SerializeField] 
        public string PilotName { get; private set; }

        [field: SerializeField]
        public PilotBehaviour PilotBehaviour { get; private set; }

        [field: SerializeField] 
        public float SpeedRun { get; private set; }

        [field: SerializeField] 
        public Vector2 FallSpeedLimit { get; private set; }

        [field: SerializeField] 
        public float FlightControlSpeed { get; private set; }
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(PilotName))
                PilotName = "Default Pilot Name";

            if (ReferenceEquals(PilotBehaviour, null) == true)
                throw new NullReferenceException($"Pilot Prefab is null in {nameof(PilotData)}");

            if (SpeedRun < 0)
            {
                SpeedRun = 0;
                throw new ArgumentOutOfRangeException($"{SpeedRun} less zero");
            }
            
            if (FallSpeedLimit.magnitude < 0)
            {
                FallSpeedLimit = Vector2.zero;
                throw new ArgumentOutOfRangeException($"{SpeedRun} less zero");
            }
            
            if (FlightControlSpeed < 0)
            {
                FlightControlSpeed = 0;
                throw new ArgumentOutOfRangeException($"{SpeedRun} less zero");
            }
        }
    }

