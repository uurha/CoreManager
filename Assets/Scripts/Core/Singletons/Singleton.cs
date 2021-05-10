using UnityEngine;
using Core.Extensions;

namespace Core.Singletons
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private protected static T _instance;

        public static bool IsInitialised => _instance != null;

        public static T Instance
        {
            get
            {
                if (IsInitialised) return _instance;
                return !UnityExtensions.TryToFindObjectOfType(out _instance) ? null : _instance;
            }
        }

        protected virtual void OnDestroy()
        {
            Debug.Log("OnDestroy: " + typeof(T));
            if (_instance == this) _instance = null;
        }
    }
}
