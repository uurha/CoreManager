#region license

// Copyright 2021 - 2021 Arcueid Elizabeth D'athemon
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
using CorePlugin.Cross.Events.Interface;
using CorePlugin.Cross.SceneData;
using CorePlugin.Samples.Scripts.EventTypes;
using CorePlugin.Samples.Scripts.Model;
using CorePlugin.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CorePlugin.Samples.Scripts
{
    public class SceneSwitcher : MonoBehaviour, IEventSubscriber
    {
        [SerializeField] private Button loadButton;
        [SerializeField] private SceneLoaderAsset scene;

        private DataTransfer _data;

        private void Awake()
        {
            loadButton.onClick.AddListener(LoadScene);
        }

        private void DataReceiver(DataTransfer data)
        {
            _data = data;
        }

        private void IsDataValid(bool isValid)
        {
            loadButton.interactable = isValid;
            if (isValid) return;
            _data = null;
        }

        private void LoadScene()
        {
            if (_data == null) return;
            SceneDataHandler.Instance.AddData(_data);
            SceneLoader.LoadSceneAsync(scene, new LoadSceneOptions {UseIntermediate = true, SceneLoadMode = LoadSceneMode.Single});
        }

        public Delegate[] GetSubscribers()
        {
            var delegates = new Delegate[] {(CustomEventTypes.IsValidDataParsedDelegate) IsDataValid, (CustomEventTypes.DataParsedDelegate) DataReceiver};
            return delegates;
        }
    }
}
