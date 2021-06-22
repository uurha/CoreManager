using System;
using System.Collections;
using CorePlugin.MainThreadDispatcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CorePlugin.SceneManagement
{
    public static class SceneLoader
    {
        [Serializable]
        public class LoadSceneOptions
        {
            public LoadSceneMode SceneLoadMode { get; set; } = LoadSceneMode.Additive;
            public UnloadSceneOptions SceneUnloadMode { get; set; } = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;
            public bool UseIntermediate { get; set; } = true;
        }
        
        private static readonly SceneLoaderSettings SceneLoaderSettings;
        private static readonly UnityMainThreadDispatcher UnityMainThreadDispatcher;

        static SceneLoader()
        {
            SceneLoaderSettings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            UnityMainThreadDispatcher = UnityMainThreadDispatcher.Instance;
        }

        public static void LoadSceneAsync(SceneLoaderAsset asset, LoadSceneOptions loadSceneOptions)
        {
            switch (loadSceneOptions.UseIntermediate)
            {
                case true:
                    UnityMainThreadDispatcher.StartCoroutine(LoadSceneWithIntermediate(asset, loadSceneOptions));
                    break;
                case false:
                    UnityMainThreadDispatcher.StartCoroutine(LoadScene(asset, loadSceneOptions));
                    break;
            }
        }

        private static IEnumerator LoadSceneWithIntermediate(SceneLoaderAsset asset, LoadSceneOptions options)
        {  
            var currentScene = SceneManager.GetActiveScene();

            var intermediateOperation = UnityMainThreadDispatcher.StartCoroutine(SceneLoaderSettings.IntermediateScene.SceneLoadOperation(options.SceneLoadMode, (sceneOperation) => sceneOperation.allowSceneActivation = true));
            
            AsyncOperation nextSceneOperation = null;

            var sceneOperation = UnityMainThreadDispatcher.StartCoroutine(asset.SceneLoadOperation(options.SceneLoadMode, (sceneOperation) => nextSceneOperation = sceneOperation));
            
            yield return new WaitForSeconds(SceneLoaderSettings.TimeInIntermediateScene);
            yield return intermediateOperation;
            yield return sceneOperation;

            yield return currentScene.SceneUnloadOperation(options.SceneUnloadMode, operation => { 
                nextSceneOperation.allowSceneActivation = true;
                operation.allowSceneActivation = true;
            });
        }
        
        private static IEnumerator LoadScene(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            var currentScene = SceneManager.GetActiveScene();

            AsyncOperation nextSceneOperation = null;
            yield return asset.SceneLoadOperation(options.SceneLoadMode, (sceneOperation) => nextSceneOperation = sceneOperation);

            yield return currentScene.SceneUnloadOperation(options.SceneUnloadMode, operation => { 
                nextSceneOperation.allowSceneActivation = true;
                operation.allowSceneActivation = true;
            });
        }

    }

    internal static class SceneLoaderExtensions
    {
        public static IEnumerator SceneUnloadOperation(this Scene scene, UnloadSceneOptions mode, Action<AsyncOperation> onSceneReadyToSwitch, Action<float> onProgressChanged = null)
        {
            var sceneOperation = SceneManager.UnloadSceneAsync(scene, mode);
            sceneOperation.allowSceneActivation = false;
            yield return new WaitUntil(() => Until(onProgressChanged, sceneOperation));
            onSceneReadyToSwitch?.Invoke(sceneOperation);
        }
        
        public static bool Until(Action<float> onProgressChanged, AsyncOperation sceneOperation)
        {
            onProgressChanged?.Invoke(sceneOperation.progress);
            return sceneOperation.progress >= 0.9f;
        }

        public static IEnumerator SceneLoadOperation(this SceneLoaderAsset sceneAsset, LoadSceneMode mode, Action<AsyncOperation> onSceneReadyToSwitch, Action<float> onProgressChanged = null)
        {
            var sceneOperation = SceneManager.LoadSceneAsync(sceneAsset.Name, mode);
            sceneOperation.allowSceneActivation = false;
            
            yield return new WaitUntil(() => Until(onProgressChanged, sceneOperation));
            
            onSceneReadyToSwitch?.Invoke(sceneOperation);
        }
        
        public static IEnumerator SceneUnloadOperation(this string name, UnloadSceneOptions mode, Action<AsyncOperation> onSceneReadyToSwitch, Action<float> onProgressChanged = null)
        {
            var sceneOperation = SceneManager.UnloadSceneAsync(name, mode);
            sceneOperation.allowSceneActivation = false;
            
            yield return new WaitUntil(() => Until(onProgressChanged, sceneOperation));
            
            onSceneReadyToSwitch?.Invoke(sceneOperation);
        }
    } 
}