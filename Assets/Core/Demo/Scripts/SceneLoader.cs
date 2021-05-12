using System;
using System.Collections.Generic;
using Core.Cross.Events.Interface;
using Core.Cross.SceneData;
using Core.Demo.Scripts.EventTypes;
using Core.Demo.Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Demo.Scripts
{
    public class SceneLoader : MonoBehaviour, ICrossEventSubscriber
    {
        [SerializeField] private Button loadButton;
        [SerializeField] private int sceneIndex;

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
            if (sceneIndex >= SceneManager.sceneCountInBuildSettings) return;
            CrossSceneDataHandler.Instance.AddData(_data);
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }

        public IEnumerable<Delegate> GetSubscribers()
        {
            var delegates = new Delegate[] {(CustomEventTypes.IsValidDataParsedDelegate) IsDataValid, (CustomEventTypes.DataParsedDelegate) DataReceiver};
            return delegates;
        }
    }
}
