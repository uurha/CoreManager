﻿#region license

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
using System.Linq;
using CorePlugin.Attributes.EditorAddons;
using CorePlugin.Attributes.Validation;
using CorePlugin.Cross.Events.Interface;
using CorePlugin.Extensions;
using CorePlugin.Samples.Scripts.EventTypes;
using CorePlugin.Samples.Scripts.Model;
using TMPro;
using UnityEngine;

namespace CorePlugin.Samples.Scripts.Demo
{
    [CoreManagerElement]
    public class CrossSceneDataSenderDemo : MonoBehaviour, IEventHandler
    {
        [SerializeField] [NotNull] private TMP_InputField intField;
        [SerializeField] [NotNull] private TMP_InputField colorField;
        [SerializeField] [NotNull] private TMP_InputField stringField;
        private readonly DataTransfer _data = new DataTransfer();

        private readonly bool[] _isFieldsValid = new bool[3];

        private event CustomEventTypes.DataParsedDelegate DataParsedEvent;
        private event CustomEventTypes.IsValidDataParsedDelegate IsValidDataParsedEvent;

        private void Awake()
        {
            intField.onValueChanged.AddListener(IntValidate);
            colorField.onValueChanged.AddListener(ColorValidate);
            stringField.onValueChanged.AddListener(StringValidate);
        }

        private void CheckValidity()
        {
            var isValid = _isFieldsValid.All(x => x);
            IsValidDataParsedEvent?.Invoke(isValid);
            if (!isValid) return;
            DataParsedEvent?.Invoke(_data);
        }

        private void ColorValidate(string data)
        {
            var bufferData = data.IndexOf('#') != 0 ? "#" + data : data;

            if (!ColorUtility.TryParseHtmlString(bufferData, out var colorData))
            {
                colorField.textComponent.color = Color.red;
                _isFieldsValid[1] = false;
                CheckValidity();
                return;
            }
            colorField.textComponent.color = Color.black;
            colorField.image.color = colorData;
            _isFieldsValid[1] = true;
            _data.ColorData = colorData;
            CheckValidity();
        }

        private void IntValidate(string data)
        {
            if (!int.TryParse(data, out var intData))
            {
                intField.textComponent.color = Color.red;
                _isFieldsValid[0] = false;
                CheckValidity();
                return;
            }
            intField.textComponent.color = Color.black;
            _isFieldsValid[0] = true;
            _data.IntData = intData;
            CheckValidity();
        }

        private void StringValidate(string data)
        {
            if (data.Length <= 0)
            {
                _isFieldsValid[2] = false;
                CheckValidity();
                return;
            }
            _isFieldsValid[2] = true;
            _data.StrData = data;
            CheckValidity();
        }

        public void InvokeEvents()
        {
            IsValidDataParsedEvent?.Invoke(false);
        }

        public void Subscribe(params Delegate[] subscribers)
        {
            EventExtensions.Subscribe(ref DataParsedEvent, subscribers);
            EventExtensions.Subscribe(ref IsValidDataParsedEvent, subscribers);
        }

        public void Unsubscribe(params Delegate[] subscribers)
        {
            EventExtensions.Unsubscribe(ref DataParsedEvent, subscribers);
            EventExtensions.Unsubscribe(ref IsValidDataParsedEvent, subscribers);
        }
    }

}
