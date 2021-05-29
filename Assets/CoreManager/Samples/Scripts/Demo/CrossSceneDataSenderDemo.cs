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
using System.Linq;
using CoreManager.Cross.Events.Interface;
using CoreManager.CustomAttributes.Validation;
using CoreManager.Samples.Scripts.EventTypes;
using CoreManager.Samples.Scripts.Model;
using TMPro;
using UnityEngine;

namespace CoreManager.Samples.Scripts.Demo
{
    public class CrossSceneDataSenderDemo : MonoBehaviour, IEventHandler
    {
        [SerializeField] [NotNull] private TMP_InputField intField;
        [SerializeField] [NotNull] private TMP_InputField colorField;
        [SerializeField] [NotNull] private TMP_InputField stringField;
        private readonly DataTransfer _data = new DataTransfer();

        private readonly bool[] _isFieldsValid = new bool[3];

        private void Awake()
        {
            intField.onValueChanged.AddListener(IntValidate);
            colorField.onValueChanged.AddListener(ColorValidate);
            stringField.onValueChanged.AddListener(StringValidate);
        }

        public void InvokeEvents()
        {
            IsValidDataParsedEvent?.Invoke(false);
        }

        public void Subscribe(IEnumerable<Delegate> subscribers)
        {
            var enumerable = subscribers as Delegate[] ?? subscribers.ToArray();
            foreach (var parsedDelegate in enumerable.OfType<CustomEventTypes.DataParsedDelegate>()) DataParsedEvent += parsedDelegate;
            foreach (var parsedDelegate in enumerable.OfType<CustomEventTypes.IsValidDataParsedDelegate>()) IsValidDataParsedEvent += parsedDelegate;
        }

        public void Unsubscribe(IEnumerable<Delegate> unsubscribers)
        {
            var enumerable = unsubscribers as Delegate[] ?? unsubscribers.ToArray();
            foreach (var parsedDelegate in enumerable.OfType<CustomEventTypes.DataParsedDelegate>()) DataParsedEvent -= parsedDelegate;
            foreach (var parsedDelegate in enumerable.OfType<CustomEventTypes.IsValidDataParsedDelegate>()) IsValidDataParsedEvent -= parsedDelegate;
        }

        private event CustomEventTypes.DataParsedDelegate DataParsedEvent;
        private event CustomEventTypes.IsValidDataParsedDelegate IsValidDataParsedEvent;

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

        private void CheckValidity()
        {
            var isValid = _isFieldsValid.All(x => x);
            IsValidDataParsedEvent?.Invoke(isValid);
            if (!isValid) return;
            DataParsedEvent?.Invoke(_data);
        }
    }

}
