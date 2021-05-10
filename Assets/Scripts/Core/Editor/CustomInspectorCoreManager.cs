﻿#region license

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
    [CustomEditor(typeof(CoreManager))]
    public class CustomInspectorCoreManager : ValidationAttributeEditor
    {
        private CoreManager _manager;

        protected override void OnEnable()
        {
            base.OnEnable();
            _manager = (CoreManager) target;
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
        }

        private static IEnumerable<FieldInfo> GetConstants(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }
    }
}
