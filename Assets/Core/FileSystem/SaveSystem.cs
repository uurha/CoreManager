#region license

// Copyright 2021 Arcueid Elizabeth D'athemon
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at // 
//     http://www.apache.org/licenses/LICENSE-2.0 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.

#endregion

using System;
using System.IO;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.FileSystem
{
    public class SaveSystem
    {
        private static SemaphoreSlim _semaphoreSlim;
        private readonly string _defaultExtension = ".json";
        private readonly string _defaultPath = Application.persistentDataPath;

        public SaveSystem()
        {
            _semaphoreSlim ??= new SemaphoreSlim(1, 1);
        }

        public SaveSystem(string path) : this()
        {
            _defaultPath = path;
        }

        public SaveSystem(string path, string extension) : this(path)
        {
            _defaultExtension = extension;
        }

        public void Save<T>(T data, Action<Exception> onError, Object context = null) where T : Serializable.Serializable
        {
            Save(data, typeof(T).Name, onError, context);
        }

        public void Save<T>(T data, string fileName, Action<Exception> onError, Object context = null) where T : Serializable.Serializable
        {
            SaveInternal(data, fileName, onError, context);
        }

        private async void SaveInternal<T>(T data, string fileName, Action<Exception> onError, Object context) where T : Serializable.Serializable
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                var bufferPath = Path.Combine(_defaultPath, fileName, _defaultExtension);
                if (!Directory.Exists(bufferPath)) Directory.CreateDirectory(_defaultPath);
                using var stream = new FileStream(bufferPath, FileMode.OpenOrCreate);
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(data.ToString());
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
                Debug.LogError(e, context);
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public void Load<T>(Action<T> onLoaded, Action<Exception> onError, Object context = null) where T : Serializable.Serializable
        {
            Load(typeof(T).Name, onLoaded, onError, context);
        }

        public void Load<T>(string fileName, Action<T> onLoaded, Action<Exception> onError, Object context = null) where T : Serializable.Serializable
        {
            LoadInternal(fileName, onLoaded, onError, context);
        }

        private async void LoadInternal<T>(string fileName, Action<T> onLoaded, Action<Exception> onError, Object context) where T : Serializable.Serializable
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                var bufferPath = Path.Combine(_defaultPath, fileName, _defaultExtension);

                if (!(Directory.Exists(_defaultPath) && File.Exists(bufferPath)))
                {
                    onLoaded?.Invoke(null);
                    return;
                }
                using var stream = new FileStream(bufferPath, FileMode.Open);
                using var writer = new StreamReader(stream);
                var loadedData = await writer.ReadToEndAsync();
                onLoaded?.Invoke(JsonUtility.FromJson<T>(loadedData));
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
                Debug.LogError(e, context);
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
