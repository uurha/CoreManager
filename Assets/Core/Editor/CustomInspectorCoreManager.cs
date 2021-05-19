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
using System.Reflection;
using Core.CustomAttributes.Editor;
using Core.Managers;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    /// <summary>
    /// Custom Editor CoreManager class.
    /// <seealso cref="Core.Managers.CoreManager"/>
    /// </summary>
    [CustomEditor(typeof(CoreManager))]
    public class CustomInspectorCoreManager : ValidationAttributeEditor
    {
        private CoreManager _manager;

        private const string EnableReleaseLogs = "ENABLE_RELEASE_LOGS";

        private Dictionary<string, bool> Symbols = new Dictionary<string, bool> {{EnableReleaseLogs, false}};

        protected override void OnEnable()
        {
            base.OnEnable();
            _manager = (CoreManager) target;
            var list = AllDefines();

            var buffer = new Dictionary<string, bool>(Symbols);
            foreach (var item in buffer.Keys)
            {
                Symbols[item] = !list.Contains(item);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (var value in from fieldInfo in GetConstants(typeof(CoreManager))
                                  select (string) fieldInfo.GetValue(_manager)
                                  into value
                                  where !TagHelper.IsTagsExistsInternal(value)
                                  where GUILayout.Button($"Define Tag: {value}")
                                  select value)
                TagHelper.AddTag(value);

            var bufferSymbols = new Dictionary<string, bool>(Symbols);
            foreach (var symbol in from symbol in bufferSymbols let text = symbol.Value ? $"Define {symbol.Key}" : $"Undefine {symbol.Key}" where GUILayout.Button(text) select symbol)
            {
                Symbols[symbol.Key] = !symbol.Value;
                SetScriptingDefine(symbol);
            }
        }

        private static IEnumerable<FieldInfo> GetConstants(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
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

        private static List<string> AllDefines()
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split(';').ToList();
            return allDefines;
        }
    }
}
