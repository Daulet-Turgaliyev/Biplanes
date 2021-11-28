namespace Tools.Singletons
{
    public class Singletone<T> where T : Singletone<T>, new()
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }
    }
}