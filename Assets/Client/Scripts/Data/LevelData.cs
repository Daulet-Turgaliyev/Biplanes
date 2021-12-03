using UnityEngine;

namespace AcinusProject.Game_Core.Plane_Behaviour
{
    [CreateAssetMenu(menuName = "Gameplay/Content/Create a new level data")]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField]
        public Vector2[] SpawnPoints { get; private set; }
    }
}