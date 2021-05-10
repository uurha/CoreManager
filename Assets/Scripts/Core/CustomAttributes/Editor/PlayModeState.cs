using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Core.CustomAttributes.Editor
{
    [InitializeOnLoad]
    internal static class PlayModeState
    {
        static PlayModeState()
        {
            EditorApplication.playModeStateChanged -= LogPlayModeState;
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;
            var pairs = Validation.ErrorObjectPairs();
            var errorObjectPairs = pairs as ErrorObjectPair[] ?? pairs.ToArray();
            if (IsPlayModeAvailable(errorObjectPairs)) return;
            EditorApplication.Beep();
            EditorApplication.ExitPlaymode();
            foreach (var errorObjectPair in errorObjectPairs) Debug.LogError(errorObjectPair.Key.Replace("\n", ""), errorObjectPair.Value);
        }

        private static bool IsPlayModeAvailable(IEnumerable<ErrorObjectPair> pairs)
        {
            return !pairs.Any();
        }
    }
}
