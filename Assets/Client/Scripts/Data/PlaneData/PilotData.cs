
    using UnityEngine;

    [CreateAssetMenu(menuName = "Gameplay/Content/Create a new pilot")]
    public class PilotData : ScriptableObject
    {
        [field: SerializeField] 
        public string PilotName { get; private set; }

        [field: SerializeField]
        public PilotBehaviour PilotBehaviour { get; private set; }

        [field: SerializeField] 
        public float Speed { get; private set; }


        private void OnValidate()
        {
            if (string.IsNullOrEmpty(PilotName))
                PilotName = "Default Pilot Name";

            if (ReferenceEquals(PilotBehaviour, null) == true)
                Debug.LogError($"Pilot Prefab is null in {nameof(PilotData)}");

            if (Speed < 0)
            {
                Debug.LogError($"Speed is less than zero");
                Speed = 0;
            }
            
        }
    }

