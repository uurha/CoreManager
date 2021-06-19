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
using CorePlugin.CustomAttributes.Headers;
using CorePlugin.CustomAttributes.Validation;
using UnityEngine;

namespace CorePlugin.Console
{
    /// <summary>
    /// Initialize minimized and maximized console
    /// </summary>
    public class ConsoleInitializer : MonoBehaviour
    {
        [SettingsHeader]
        [SerializeField] private bool initializeMinimized;
        
        [ReferencesHeader]
        [NotNull] [SerializeField] private RuntimeConsole maximizedConsole;
        [NotNull] [SerializeField] private MinimizedConsole minimizedConsole;
        
        private void Awake()
        {
#if DEBUG || ENABLE_RELEASE_CONSOLE
            Initialize();
#else
            Destroy(gameObject);
#endif
        }

        private void Initialize()
        {
            if (FindObjectsOfType<RuntimeConsole>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            
            var onCountChanged = 
                minimizedConsole.CountDisplayers.Aggregate<CountDisplayer, Action<HashSet<LogType>, int>>(null, (current, countDisplayer) 
                => current + countDisplayer.Initialize().OnLogCountChanged);

            maximizedConsole.OnLogCountUpdated += onCountChanged;
            minimizedConsole.Initialize(OnConsoleMaximized).SetActive(initializeMinimized);
            maximizedConsole.Initialize(OnConsoleMinimized).SetActive(!initializeMinimized);
        }

        private void OnConsoleMinimized()
        {
            minimizedConsole.SetActive(true);
        }
        
        private void OnConsoleMaximized()
        {
            maximizedConsole.SetActive(true);
        }
    }
}
