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

            var intermediateCoroutine = UnityMainThreadDispatcher.StartCoroutine(SceneLoaderSettings.IntermediateScene.SceneLoadOperation(options.SceneLoadMode, (sceneAsyncOperation) => sceneAsyncOperation.allowSceneActivation = true));
            
            AsyncOperation nextSceneOperation = null;

            var sceneOperationCoroutine = UnityMainThreadDispatcher.StartCoroutine(asset.SceneLoadOperation(options.SceneLoadMode, (sceneAsyncOperation) => nextSceneOperation = sceneAsyncOperation));
            
            yield return new WaitForSeconds(SceneLoaderSettings.TimeInIntermediateScene);
            yield return intermediateCoroutine;
            yield return sceneOperationCoroutine;
            
            nextSceneOperation.allowSceneActivation = true;

            if(options.SceneLoadMode == LoadSceneMode.Single) yield break;
            
            yield return new WaitUntil(() => nextSceneOperation.isDone);
            
            yield return currentScene.SceneUnloadOperation(options.SceneUnloadMode, operation => { 
                operation.allowSceneActivation = true;
            });
        }
        
        private static IEnumerator LoadScene(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            var currentScene = SceneManager.GetActiveScene();

            AsyncOperation nextSceneOperation = null;
            yield return asset.SceneLoadOperation(options.SceneLoadMode, (sceneOperation) => nextSceneOperation = sceneOperation);

            nextSceneOperation.allowSceneActivation = true;

            if(options.SceneLoadMode == LoadSceneMode.Single) yield break;
            
            yield return new WaitUntil(() => nextSceneOperation.isDone);
            
            yield return currentScene.SceneUnloadOperation(options.SceneUnloadMode, operation => { 
                operation.allowSceneActivation = true;
            });
        }
    }
}