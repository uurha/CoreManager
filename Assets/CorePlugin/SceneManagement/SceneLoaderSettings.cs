using System.Collections.Generic;
using UnityEngine;

namespace CorePlugin.SceneManagement
{
    [CreateAssetMenu(menuName = "Create SceneLoaderSettings", fileName = "SceneLoaderSettings", order = 0)]
    public class SceneLoaderSettings : ScriptableObject
    {
        [SerializeField] private List<SceneLoaderAsset> scenes;
        [SerializeField] private SceneLoaderAsset intermediateScene;
        [Min(0)][SerializeField] private float timeInIntermediateScene;

        public List<SceneLoaderAsset> Scenes => scenes;

        public SceneLoaderAsset IntermediateScene => intermediateScene;

        public float TimeInIntermediateScene => timeInIntermediateScene;
    }
}