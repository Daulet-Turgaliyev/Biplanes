using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    [CreateAssetMenu(menuName = "Gameplay/Content/Create a new plane")]
    public class PlaneData : ScriptableObject
    {
        [field: SerializeField] 
        public string PlaneName { get; private set; }

        [field: SerializeField]
        public PlaneBase PlanePrefab { get; private set; }

        [field: SerializeField] 
        public float SpeedRotation { get; private set; }
        
        [field: SerializeField] 
        public float MinSpeed { get; private set; }
        
        [field: SerializeField] 
        public float MaxSpeed { get; private set; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(PlaneName))
                PlaneName = "Default Plane Name";

            if (ReferenceEquals(PlanePrefab, null) == true)
                Debug.LogError($"Plane Prefab is null in {nameof(PlaneData)}");

            if (SpeedRotation < 0)
            {
                Debug.LogError($"SpeedRotation is less than zero");
                SpeedRotation = 0;
            }
            
            if (MaxSpeed < 0)
            {
                Debug.LogError($"Speed is less than zero");
                MaxSpeed = 0;
            }
            
            if (MinSpeed < 0)
            {
                Debug.LogError($"Speed is less than zero");
                MinSpeed = 0;
            }

            if (MaxSpeed < MinSpeed)
            {
                Debug.LogError($"MaxSpeed cannot be less than minimum");
                MaxSpeed = MinSpeed + 1;
            }
        }
    }

}