using System.Collections;
using CorePlugin.Logger;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CorePlugin.Samples.Scripts.Demo
{
    public class ConsoleDemoTester : MonoBehaviour
    {
        [Min(0.1f)] [SerializeField] private float delay = 0.1f;

        private Coroutine _loggingCoroutine;
        
        private void Awake()
        {
            _loggingCoroutine = StartCoroutine(Logging());
        }

        private IEnumerator Logging()
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                var random = Random.Range(0, 3);

                switch (random)
                {
                    case 0:
                        CustomLogger.Log("Simple Log");
                        break;
                    case 1:
                        CustomLogger.LogError("Simple Log Error");
                        break;
                    case 2:
                        CustomLogger.LogWarning("Simple Log Warning");
                        break;
                }
            }
        }

        private void OnDestroy()
        {
            StopCoroutine(_loggingCoroutine);
        }
    }
}
