using System.Collections;
using CorePlugin.Singletons;
using UnityEngine;

namespace CorePlugin.Dispatchers
{
    public class StaticCoroutineDispatcher : StaticObjectSingleton<StaticCoroutineDispatcher>
    {
        /// <summary>
        /// Start coroutine on CoroutineDispatcher
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
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