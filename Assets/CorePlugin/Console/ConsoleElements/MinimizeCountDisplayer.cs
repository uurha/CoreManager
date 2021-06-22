using System;
using UnityEngine;

namespace CorePlugin.Console.ConsoleElements
{
    /// <summary>
    /// Log count displayer for <see cref="CorePlugin.Console.RuntimeConsole"/>
    /// </summary>
    public class MinimizeCountDisplayer : CountDisplayer
    {
        public override CountDisplayer Initialize()
        {
            icon.sprite = LoadConsoleIcon.GetLogIconSprite(designatedType, true);
            return this;
        }

        public override CountDisplayer SetInteractionAction(Action<LogType, bool> onInteractWithDisplayer)
        {
            return this;
        }
    }
}