using System;
using System.IO;
using UnityEngine;

namespace CorePlugin.SceneManagement
{
    [Serializable]
    public class SceneLoaderAsset
    {
        [SerializeField] private string fullPath;
        [SerializeField] private string name;
        [SerializeField] private int instanceID;

        public string FullPath => fullPath;

        public string Name => name;

        public int InstanceID => instanceID;

        public SceneLoaderAsset(string path, int instanceID)
        {
            fullPath = path;
            this.instanceID = instanceID;
            name = Path.GetFileNameWithoutExtension(path);
        }

    }
}