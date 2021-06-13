using System;
using UnityEngine;

namespace CorePlugin.Console
{
    /// <summary>
    /// Settings class for <see cref="CorePlugin.Console.RuntimeConsole"/>
    /// </summary>
    [Serializable]
    public class ConsoleTextSettings
    {
        [SerializeField] private float logTextSize = 22f;
        [SerializeField] private float stackTraceTextSize = 15f;

        public float LogTextSize => logTextSize;

        public float StackTraceTextSize => stackTraceTextSize;
    }
}
