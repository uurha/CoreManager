using System;
using System.Collections.Generic;
using System.Linq;
using CorePlugin.CustomAttributes.Headers;
using CorePlugin.CustomAttributes.Validation;
using UnityEngine;

namespace CorePlugin.Console
{
    public class ConsoleToggler : MonoBehaviour
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
