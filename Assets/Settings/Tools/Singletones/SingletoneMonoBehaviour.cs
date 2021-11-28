using UnityEngine;

namespace Tools.Singletons {
    public class SingletoneMonoBehaviour<T> : MonoBehaviour where T: SingletoneMonoBehaviour<T> {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject(nameof(T));
                    _instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }
    }
}