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
using CorePlugin.Attributes.Headers;
using CorePlugin.Attributes.Validation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CorePlugin.Console.ConsoleElements
{
    /// <summary>
    /// Log count displayer for <see cref="CorePlugin.Console.RuntimeConsole"/>
    /// </summary>
    public abstract class CountDisplayer : MonoBehaviour
    {
        [SettingsHeader] 
        [SerializeField] private protected LogType designatedType;
        
        [ReferencesHeader]
        [NotNull] [SerializeField] private protected TMP_Text countText;
        [NotNull] [SerializeField] private protected Image icon;
        
        protected ConsoleIcons _icons;

        public virtual CountDisplayer Initialize(ConsoleIcons icons)
        {
            _icons = icons;
            icon.sprite = icons.GetLogIconSprite(designatedType, true);
            return this;
        }

        /// <summary>
        /// Setting action when interaction with CountDisplayer happens
        /// </summary>
        /// <param name="onInteractWithDisplayer"></param>
        /// <returns></returns>
        public abstract CountDisplayer SetInteractionAction(Action<LogType, bool> onInteractWithDisplayer);

        /// <summary>
        /// Displaying new count
        /// </summary>
        /// <param name="types"></param>
        /// <param name="count"></param>
        public virtual void OnLogCountChanged(HashSet<LogType> types, int count)
        {
            if (types.Contains(designatedType)) countText.text = count <= 999 ? $"{count}" : "999+";
        }
    }
}
