using System;
using System.Collections.Generic;
using CorePlugin.CustomAttributes.Headers;
using CorePlugin.CustomAttributes.Validation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CorePlugin.Console
{
    /// <summary>
    /// Log toggle for <see cref="CorePlugin.Console.RuntimeConsole"/>
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class LogToggle : MonoBehaviour
    {
        [SettingsHeader]
        [SerializeField] private LogType designatedType;

        [ReferencesHeader]
        [NotNull] [SerializeField] private TMP_Text countText;
        [NotNull] [SerializeField] private Image icon;

        private Toggle _toggle;

        private void SetActiveIcon(bool state)
        {
            icon.sprite = LoadLogIcon.GetLogIconSprite(designatedType, state);
        }
        
        public LogToggle Initialize(Action<LogType, bool> action)
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener((state) => action?.Invoke(designatedType, state));
            _toggle.onValueChanged.AddListener(SetActiveIcon);
            return this;
        }

        public void OnLogCountChanged(HashSet<LogType> types, int count)
        {
            if (types.Contains(designatedType)) countText.text = count <= 999 ? $"{count}" : "999+";
        }
    }

}
