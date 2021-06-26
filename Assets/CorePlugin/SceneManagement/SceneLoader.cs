using System.Collections;
using CorePlugin.Dispatchers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CorePlugin.SceneManagement
{
    /// <summary>
    /// Scene Loader at run-time
    /// </summary>
    public static class SceneLoader
    {
        private static readonly SceneLoaderSettings SceneLoaderSettings;

        static SceneLoader()
        {
            SceneLoaderSettings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="loadSceneOptions"></param>
        public static void LoadSceneAsync(SceneLoaderAsset asset, LoadSceneOptions loadSceneOptions)
        {
            switch (loadSceneOptions.UseIntermediate)
            {
                case true:
                    StaticCoroutineDispatcher.StartStaticCoroutine(LoadSceneWithIntermediate(asset, loadSceneOptions));
                    break;
                case false:
                    StaticCoroutineDispatcher.StartStaticCoroutine(LoadScene(asset, loadSceneOptions));
                    break;
            }
        }
        
        /// <summary>
        /// Loads async SceneLoaderAsset with default options
        /// </summary>
        /// <param name="asset"></param>
        public static void LoadSceneAsync(SceneLoaderAsset asset)
        {
            LoadSceneAsync(asset, new LoadSceneOptions());
        }

        private static IEnumerator LoadSceneWithIntermediate(SceneLoaderAsset asset, LoadSceneOptions options)
        {  
            var currentScene = SceneManager.GetActiveScene();

            var intermediateCoroutine = StaticCoroutineDispatcher.StartStaticCoroutine(SceneLoaderSettings.IntermediateScene.SceneLoadOperation(options.SceneLoadMode, (sceneAsyncOperation) => sceneAsyncOperation.allowSceneActivation = true));
            
            AsyncOperation nextSceneOperation = null;

            var sceneOperationCoroutine = StaticCoroutineDispatcher.StartStaticCoroutine(asset.SceneLoadOperation(options.SceneLoadMode, (sceneAsyncOperation) => nextSceneOperation = sceneAsyncOperation));
            
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