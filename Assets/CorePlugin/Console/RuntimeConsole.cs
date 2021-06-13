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
using System.Text.RegularExpressions;
using CorePlugin.CustomAttributes.Headers;
using CorePlugin.CustomAttributes.Validation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CorePlugin.Console
{
    /// <summary>
    /// Runtime console class
    /// </summary>
    public class RuntimeConsole : MonoBehaviour
    {
        [SettingsHeader]
        [SerializeField] private bool reverseOrder;
        [SerializeField] private ConsoleTextSettings settings;

        [ReferencesHeader]
        [NotNull] [SerializeField] private TMP_InputField searchInputField;
        [NotNull] [SerializeField] private Button clearSearchButton;
        [NotNull] [SerializeField] private TMP_Text stackTraceTextField;
        [NotNull] [SerializeField] private Button clearLogsButton;
        [NotNull] [SerializeField] private Toggle reverseSortingToggle;
        [NotNull] [SerializeField] private ScrollRect logsScrollRect;
        [NotNull] [SerializeField] private ScrollRect stackTraceScrollRect;
        [NotNull] [SerializeField] private VerticalLayoutGroup layoutGroup;
        [NotNull] [SerializeField] private List<ConsoleLogToggle> logButtons;

        [PrefabHeader]
        [PrefabRequired] [SerializeField] private ConsoleMessage consoleMessagePrefab;

        private Action<HashSet<LogType>, int> onLogCountChanged;

        private readonly Dictionary<LogType, List<ConsoleMessage>> _logs = new Dictionary<LogType, List<ConsoleMessage>>();

        private readonly HashSet<LogType> _displayedLogTypes = new HashSet<LogType>
                                                               {
                                                                   LogType.Error,
                                                                   LogType.Assert,
                                                                   LogType.Warning,
                                                                   LogType.Log,
                                                                   LogType.Exception,
                                                               };

        private void Awake()
        {
            layoutGroup.reverseArrangement = reverseOrder;
            reverseSortingToggle.isOn = reverseOrder;
            Application.logMessageReceivedThreaded += MessageReceivedThreaded;

            foreach (var init in logButtons.Select(logButton => logButton.Initialize(OnStateChanged)))
            {
                onLogCountChanged += init.OnLogCountChanged;
            }
            searchInputField.onValueChanged.AddListener(SearchLogs);
            clearSearchButton.onClick.AddListener(ClearSearch);
            clearLogsButton.onClick.AddListener(ClearLogs);
            reverseSortingToggle.onValueChanged.AddListener(ToggleSorting);
        }

        private void ToggleSorting(bool state)
        {
            reverseOrder = state;
            layoutGroup.reverseArrangement = state;
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= MessageReceivedThreaded;
        }

        private void DisplayStackTrace(string stackTrace)
        {
            stackTraceTextField.text = stackTrace;
            var sizeDelta = stackTraceScrollRect.content.sizeDelta;
            stackTraceScrollRect.content.sizeDelta = new Vector2(sizeDelta.x,  stackTraceTextField.GetPreferredValues(stackTrace).y);
        }

        private void DisplayByLogType(ConsoleMessage message)
        {
            message.SetActive(_displayedLogTypes.Contains(message.Type));
        }

        private void OnStateChanged(LogType designatedType, bool state)
        {
            var states = LogTypes(designatedType);
            SetActive(states, state);
        }

        private void SetActive(IEnumerable<LogType> logTypes, bool state)
        {
            if (state)
            {
                _displayedLogTypes.UnionWith(logTypes);
            }
            else
            {
                _displayedLogTypes.ExceptWith(logTypes);
            }

            foreach (var logType in _logs.Keys)
            {
                foreach (var message in _logs[logType])
                {
                    message.SetActive(_displayedLogTypes.Contains(logType));
                }
            }
        }

        private void ClearLogs()
        {
            foreach (var message in _logs.Values.SelectMany(messages => messages))
            {
                Destroy(message.gameObject);
            }
            _logs.Clear();
            onLogCountChanged?.Invoke(new HashSet<LogType> {LogType.Log, LogType.Warning, LogType.Error}, 0);
        }

        private void ClearSearch()
        {
            searchInputField.text = string.Empty;
        }

        private void SearchLogs(string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var message in _logs.Values.SelectMany(messages => messages))
                {
                    message.SetActive(true).ClearHighlight();
                }
                return;
            }
            
            var regex = new Regex(searchText, RegexOptions.IgnoreCase);
            foreach (var message in _logs.Values.SelectMany(messages => messages))
            {
                var isMatch = regex.IsMatch(message.LogText);
                
                if(isMatch)
                {
                    message.SetActive(true).HighlightText(searchText);
                }
                else
                {
                    message.SetActive(false);
                }
            }
        }

        private void MessageReceivedThreaded(string condition, string stacktrace, LogType type)
        {
            var instance = Instantiate(consoleMessagePrefab, logsScrollRect.content)
                          .Initialize(condition, stacktrace, type, settings).SubscribeOnButtonClick(DisplayStackTrace);

            if (_logs.TryGetValue(type, out var logs))
            {
                logs.Add(instance);
                _logs[type] = logs;
            }
            else
            {
                _logs.Add(type, new List<ConsoleMessage> {instance});
            }
            onLogCountChanged?.Invoke(LogTypes(type), _logs[type].Count);
            DisplayByLogType(instance);
        }

        private HashSet<LogType> LogTypes(LogType designatedType)
        {
            var states = designatedType switch
            {
                LogType.Warning => new HashSet<LogType> {LogType.Warning},
                LogType.Log => new HashSet<LogType> {LogType.Log},
                LogType.Error => new HashSet<LogType> {LogType.Error, LogType.Exception, LogType.Assert},
                LogType.Assert => new HashSet<LogType> {LogType.Error, LogType.Exception, LogType.Assert},
                LogType.Exception => new HashSet<LogType> {LogType.Error, LogType.Exception, LogType.Assert},
                _ => throw new ArgumentOutOfRangeException()
            };
            return states;
        }
    }

}
