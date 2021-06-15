using System;
using System.Collections.Generic;
using CorePlugin.CustomAttributes.Headers;
using CorePlugin.CustomAttributes.Validation;
using CorePlugin.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace CorePlugin.Console
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MinimizedConsole : MonoBehaviour
    {
        [ReferencesHeader]
        [NotNull] [SerializeField] private Button maximizeButton;
        [NotNull] [SerializeField] private List<CountDisplayer> countDisplayers;

        private CanvasGroup _consoleCanvasGroup;
        private Action onConsoleMaximized;
        
        public List<CountDisplayer> CountDisplayers => countDisplayers;

        public MinimizedConsole Initialize(Action onMaximized)
        {
            _consoleCanvasGroup = GetComponent<CanvasGroup>();
            
            maximizeButton.onClick.AddListener(Maximize);
            onConsoleMaximized += onMaximized;
            
            return this;
        }

        private void Maximize()
        {
            SetActive(false);
            onConsoleMaximized?.Invoke();
        }

        public void SetActive(bool state)
        {
            UIStateTools.ChangeGroupState(_consoleCanvasGroup, state);
        }
    }
}