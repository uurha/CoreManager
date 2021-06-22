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
using CorePlugin.Console.ConsoleElements;
using UnityEditor;
using UnityEngine;

namespace CorePlugin.Console
{
    /// <summary>
    /// Runtime log icon loader
    /// </summary>
    [InitializeOnLoad]
    internal static class LoadConsoleIcon
    {
        private static readonly ConsoleIcons Icons;

        static LoadConsoleIcon()
        {
            Icons = Resources.Load<ConsoleIcons>("LogIcons");
        }
        
        /// <summary>
        /// Fetching icon by log type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Sprite GetLogIconSprite(LogType type, bool active)
        {
            var sprite = type switch
                         {
                             LogType.Exception => active ? Icons.ErrorActive : Icons.ErrorInactive,
                             LogType.Assert => active ?Icons.ErrorActive: Icons.ErrorInactive,
                             LogType.Error =>active ? Icons.ErrorActive: Icons.ErrorInactive,
                             LogType.Warning =>active ? Icons.WarningActive: Icons.WarningInactive,
                             LogType.Log =>active ? Icons.InfoActive: Icons.InfoInactive,
                             _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                         };
            return sprite;
        }
    }
}
