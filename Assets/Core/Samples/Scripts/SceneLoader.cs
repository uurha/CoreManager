﻿#region license

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
using Core.Cross.Events.Interface;
using Core.Cross.SceneData;
using Core.Samples.Scripts.EventTypes;
using Core.Samples.Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Samples.Scripts
{
    public class SceneLoader : MonoBehaviour, ICrossEventSubscriber
    {
        [SerializeField] private Button loadButton;
        [SerializeField] private string scene;

        private DataTransfer _data;

        private void Awake()
        {
            loadButton.onClick.AddListener(LoadScene);
        }

        private void IsDataValid(bool isValid)
        {
            loadButton.interactable = isValid;
            if (isValid) return;
            _data = null;
        }

        private void DataReceiver(DataTransfer data)
        {
            _data = data;
        }

        private void LoadScene()
        {
            if (_data == null) return;
            CrossSceneDataHandler.Instance.AddData(_data);
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        public IEnumerable<Delegate> GetSubscribers()
        {
            var delegates = new Delegate[] {(CustomEventTypes.IsValidDataParsedDelegate) IsDataValid, (CustomEventTypes.DataParsedDelegate) DataReceiver};
            return delegates;
        }
    }
}
