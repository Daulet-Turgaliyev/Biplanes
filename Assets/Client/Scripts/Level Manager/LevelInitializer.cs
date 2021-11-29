using UnityEngine;
using Zenject;

namespace Client.Scripts.Level_Manager
{
    public class LevelInitializer : MonoBehaviour
    {
        [Inject] 
        private WindowsManager _windowsManager;
    
        public void Start()
        {
            _windowsManager.OpenWindow<PlaneControllerWindow>();
        }
    }
}