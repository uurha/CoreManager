using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CorePlugin.SceneManagement
{
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