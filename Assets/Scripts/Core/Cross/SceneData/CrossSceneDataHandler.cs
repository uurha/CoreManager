using System;
using System.Collections.Generic;
using Core.Cross.SceneData.Interface;
using Core.CustomAttributes.Validation;
using Core.Singletons;

namespace Core.Cross.SceneData
{
    [OnlyOneInScene]
    public class CrossSceneDataHandler : Singleton<CrossSceneDataHandler>
    {
        private readonly Dictionary<Type, ISceneData> _data = new Dictionary<Type, ISceneData>();

        private void Awake()
        {
            if (_instance != null &&
                _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
            _instance = this;
        }

        public void AddData<T>(T data) where T : ISceneData, new()
        {
            var isContains = _data.ContainsKey(typeof(T));

            if (isContains)
                _data[typeof(T)] = data;
            else
                _data.Add(typeof(T), data);
        }

        public bool GetData<T>(out T data) where T : ISceneData, new()
        {
            var isGet = _data.TryGetValue(typeof(T), out var buffer);
            data = (T) buffer;
            return isGet;
        }

        public void RemoveData<T>() where T : ISceneData, new()
        {
            if (_data.ContainsKey(typeof(T))) _data.Remove(typeof(T));
        }
    }
}
