#region license

// Copyright 2021 Arcueid Elizabeth D'athemon
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//     http://www.apache.org/licenses/LICENSE-2.0 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using Core.Cross.SceneData.Interface;
using Core.CustomAttributes.Validation;
using Core.Singletons;

namespace Core.Cross.SceneData
{
    /// <summary>
    /// Singleton for data passing between scenes
    /// <seealso cref="Core.Cross.SceneData.Interface.ISceneData"/>
    /// </summary>
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

        /// <summary>
        /// Adding data to dictionary by passed Type
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        public void AddData<T>(T data) where T : ISceneData, new()
        {
            var isContains = _data.ContainsKey(typeof(T));

            if (isContains)
                _data[typeof(T)] = data;
            else
                _data.Add(typeof(T), data);
        }

        /// <summary>
        /// Getting data from dictionary by passed Type
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        public bool GetData<T>(out T data) where T : ISceneData, new()
        {
            var isGet = _data.TryGetValue(typeof(T), out var buffer);
            data = (T) buffer;
            return isGet;
        }

        /// <summary>
        /// Removing data from dictionary by passed Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveData<T>() where T : ISceneData, new()
        {
            if (_data.ContainsKey(typeof(T))) _data.Remove(typeof(T));
        }
    }
}
