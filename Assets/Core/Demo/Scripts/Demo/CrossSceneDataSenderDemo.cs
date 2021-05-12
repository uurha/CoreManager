using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cross.Events.Interface;
using Core.CustomAttributes.Validation;
using Core.Demo.Scripts.EventTypes;
using Core.Demo.Scripts.Model;
using TMPro;
using UnityEngine;

namespace Core.Demo.Scripts.Demo
{
    public class CrossSceneDataSenderDemo : MonoBehaviour, ICrossEventHandler
    {
        [SerializeField] [NotNull] private TMP_InputField intField;
        [SerializeField] [NotNull] private TMP_InputField colorField;
        [SerializeField] [NotNull] private TMP_InputField stringField;

        private event CustomEventTypes.DataParsedDelegate DataParsedEvent;
        private event CustomEventTypes.IsValidDataParsedDelegate IsValidDataParsedEvent;

        private readonly bool[] _isFieldsValid = new bool[3];
        private readonly DataTransfer _data = new DataTransfer();

        private void Awake()
        {
            intField.onValueChanged.AddListener(IntValidate);
            colorField.onValueChanged.AddListener(ColorValidate);
            stringField.onValueChanged.AddListener(StringValidate);
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
            if(data.Length <= 0)
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

        public void InvokeEvents()
        {
            IsValidDataParsedEvent?.Invoke(false);
        }

        public void Subscribe(IEnumerable<Delegate> subscribers)
        {
            var enumerable = subscribers as Delegate[] ?? subscribers.ToArray();

            foreach (var parsedDelegate in enumerable.OfType<CustomEventTypes.DataParsedDelegate>())
            {
                DataParsedEvent += parsedDelegate;
            }

            foreach (var parsedDelegate in enumerable.OfType<CustomEventTypes.IsValidDataParsedDelegate>())
            {
                IsValidDataParsedEvent += parsedDelegate;
            }
        }

        public void Unsubscribe(IEnumerable<Delegate> unsubscribers)
        {
            var enumerable = unsubscribers as Delegate[] ?? unsubscribers.ToArray();

            foreach (var parsedDelegate in enumerable.OfType<CustomEventTypes.DataParsedDelegate>())
            {
                DataParsedEvent -= parsedDelegate;
            }

            foreach (var parsedDelegate in enumerable.OfType<CustomEventTypes.IsValidDataParsedDelegate>())
            {
                IsValidDataParsedEvent -= parsedDelegate;
            }
        }
    }

}
