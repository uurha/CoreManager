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
