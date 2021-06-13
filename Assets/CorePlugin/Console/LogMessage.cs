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
using System.Globalization;
using CorePlugin.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CorePlugin.Console
{
    /// <summary>
    /// Log message for <see cref="CorePlugin.Console.RuntimeConsole"/>
    /// </summary>
    //TODO: Make found char highlighted
    [RequireComponent(typeof(CanvasGroup), typeof(LayoutElement))]
    public class LogMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        private string _logText;
        private string _stackTrace;
        private LogType _logType;
        private CanvasGroup _canvasGroup;
        private LayoutElement _layoutElement;
        private ConsoleTextSettings _currentSettings;

        public LogType Type => _logType;

        public string LogText => _logText;

        public string StackTrace => _stackTrace;

        /// <summary>
        /// Subscribes action to message button
        /// </summary>
        /// <param name="onClickAction"></param>
        /// <returns></returns>
        public LogMessage SubscribeOnButtonClick(Action<string> onClickAction)
        {
            button.onClick.AddListener(() => onClickAction?.Invoke(CombinedStackTrace(LogText, StackTrace, _currentSettings)));
            return this;
        }

        /// <summary>
        /// Setting active message in console
        /// </summary>
        /// <param name="state"></param>
        public void SetActive(bool state)
        {
            UIStateTools.ChangeGroupState(_canvasGroup, state);
            _layoutElement.ignoreLayout = !state;
        }

        /// <summary>
        /// Initializing console message
        /// </summary>
        /// <param name="logText"></param>
        /// <param name="stackTrace"></param>
        /// <param name="logType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public LogMessage Initialize(string logText, string stackTrace, LogType logType, ConsoleTextSettings settings)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _layoutElement = GetComponent<LayoutElement>();
            _logType = logType;
            _logText = logText;

            _stackTrace = stackTrace;
            _currentSettings = settings;

            icon.preserveAspect = true;
            icon.sprite = LoadLogIcon.GetLogIconSprite(_logType, true);

            var time = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern);
            
            textField.text =$"[{time}] {LogText}";
            textField.fontSize = settings.LogTextSize;

            textField.color = _logType switch
                              {
                                  LogType.Error => Color.red,
                                  LogType.Assert => Color.red,
                                  LogType.Warning => Color.yellow,
                                  LogType.Log => Color.white,
                                  LogType.Exception => Color.red,
                                  _ => throw new ArgumentOutOfRangeException()
                              };
            return this;
        }

        private static string CombinedStackTrace(string logText, string stackTrace, ConsoleTextSettings settings)
        {
            return $"<b><size={settings.LogTextSize}>{logText}</size></b>" +
                   $"\n\n<size={settings.StackTraceTextSize}>{stackTrace}</size>";
        }
    }
}
