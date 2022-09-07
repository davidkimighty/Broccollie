using UnityEngine;

namespace CollieMollie.System
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if ((object)instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (instance == null)
                    {
                        GameObject singletoneObject = new GameObject(typeof(T).ToString());
                        instance = singletoneObject.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
    }
}