using System;
using UnityEngine;

namespace CorePlugin.Console
{
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