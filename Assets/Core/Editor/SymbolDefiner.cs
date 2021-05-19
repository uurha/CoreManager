using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    /// <summary>
    /// Class for "Scripting Define Symbols" defining from CoreManager Inspector.
    /// <seealso cref="Core.Managers.CoreManager"/>
    /// <seealso href="https://docs.unity3d.com/Manual/PlatformDependentCompilation.html">Scripting Define Symbols</seealso>
    /// </summary>
    public class SymbolDefiner
    {
        private readonly Dictionary<string, bool> _symbols = new Dictionary<string, bool> {{EnableReleaseLogs, false}};
        private const string EnableReleaseLogs = "ENABLE_RELEASE_LOGS";

        /// <summary>
        /// Shows buttons in Inspector.
        /// </summary>
        public void ShowSymbolsButtons()
        {
            var bufferSymbols = new Dictionary<string, bool>(_symbols);

            foreach (var symbol in from symbol in bufferSymbols
                                   let text = symbol.Value ? $"Define {symbol.Key}" : $"Undefine {symbol.Key}"
                                   where GUILayout.Button(text)
                                   select symbol)
            {
                _symbols[symbol.Key] = !symbol.Value;
                SetScriptingDefine(symbol);
            }
        }

        private void SetScriptingDefine(KeyValuePair<string, bool> pair)
        {
            var allDefines = AllDefines();

            if (pair.Value)
            {
                allDefines.Add(pair.Key);
            }
            else
            {
                allDefines.Remove(pair.Key);
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                                                             EditorUserBuildSettings.selectedBuildTargetGroup,
                                                             string.Join(";", allDefines.ToArray()));
            AssetDatabase.Refresh();
        }

        private List<string> AllDefines()
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split(';').ToList();
            return allDefines;
        }

        /// <summary>
        /// Used for check already defined symbols OnEnable
        /// </summary>
        public void OnEnable()
        {
            var list = AllDefines();

            var buffer = new Dictionary<string, bool>(_symbols);
            foreach (var item in buffer.Keys)
            {
                _symbols[item] = !list.Contains(item);
            }
        }
    }
}
