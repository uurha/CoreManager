using System;
using System.IO;
using System.Threading;
using Core.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.FileSystem
{
    public class SaveSystem
    {
        private readonly string _defaultPath = Application.persistentDataPath;
        private readonly string _defaultExtension = ".json";
        private static SemaphoreSlim _semaphoreSlim;

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

        public void Save<T>(T data,Action<Exception> onError, Object context = null) where T : Serializable
        {
            Save(data, typeof(T).Name, onError, context);
        }

        public void Save<T>(T data, string fileName,Action<Exception> onError, Object context = null) where T : Serializable
        {
            SaveInternal(data, fileName,onError, context);
        }

        private async void SaveInternal<T>(T data, string fileName,Action<Exception> onError, Object context) where T : Serializable
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                var bufferPath = Path.Combine(_defaultPath, fileName, _defaultExtension);

                if (!Directory.Exists(bufferPath))
                {
                    Directory.CreateDirectory(_defaultPath);
                }
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

        public void Load<T>(Action<T> onLoaded, Action<Exception> onError, Object context = null) where T : Serializable
        {
            Load(typeof(T).Name, onLoaded, onError, context);
        }

        public void Load<T>(string fileName, Action<T> onLoaded, Action<Exception> onError, Object context = null) where T : Serializable
        {
            LoadInternal(fileName, onLoaded, onError, context);
        }

        private async void LoadInternal<T>(string fileName, Action<T> onLoaded, Action<Exception> onError, Object context) where T : Serializable
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
