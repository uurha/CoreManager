using System;
using UnityEngine;

namespace CorePlugin.Console.ConsoleElements
{
    /// <summary>
    /// Log count displayer for <see cref="CorePlugin.Console.RuntimeConsole"/>
    /// </summary>
    public class MinimizeCountDisplayer : CountDisplayer
    {
        public override CountDisplayer Initialize(ConsoleIcons icons)
        {
            _icons = icons;
            icon.sprite = icons.GetLogIconSprite(designatedType, true);
            return this;
        }

        public override CountDisplayer SetInteractionAction(Action<LogType, bool> onInteractWithDisplayer)
        {
            return this;
        }
    }
}