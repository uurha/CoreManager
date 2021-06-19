using CorePlugin.Logger;
using UnityEngine;

namespace CorePlugin.Singletons
{
    /// <summary>
    /// Base for all singletons.
    /// Strongly recommended to use singletons as little as possible.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonWithCreating<T> : MonoBehaviour where T : MonoBehaviour
    {
        private protected static T _instance;

        public static bool IsInitialised => _instance != null;

        public static T Instance
        {
            get
            {
                if (IsInitialised) return _instance;
                return _instance = new GameObject(nameof(T)).AddComponent<T>();
            }
        }

        protected virtual void OnDestroy()
        {
            CustomLogger.Log("OnDestroy: " + typeof(T));
            if (_instance == this) _instance = null;
        }
    }
}