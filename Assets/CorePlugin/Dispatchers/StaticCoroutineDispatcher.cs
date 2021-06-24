using System.Collections;
using CorePlugin.Singletons;
using UnityEngine;

namespace CorePlugin.Dispatchers
{
    public class StaticCoroutineDispatcher : StaticObjectSingleton<StaticCoroutineDispatcher>
    {
        public static Coroutine StartStaticCoroutine(IEnumerator coroutine)
        {
            return GetInstance().StartCoroutine(coroutine);
        } 
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}