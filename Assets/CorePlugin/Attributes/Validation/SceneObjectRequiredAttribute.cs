﻿#region license

// Copyright 2021 - 2022 Arcueid Elizabeth D'athemon
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
using System.Diagnostics;
using CorePlugin.Attributes.Base;
using CorePlugin.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

namespace CorePlugin.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional(EditorDefinition.UnityEditor)]
    public class SceneObjectRequiredAttribute : FieldValidationAttribute
    {
        public SceneObjectRequiredAttribute(bool showError) : base(showError)
        {
        }

        public SceneObjectRequiredAttribute() : base(false)
        {
        }

        private protected override bool ValidState(object obj)
        {
            #if UNITY_EDITOR
            var o = (Object)obj;
            return PrefabUtility.GetPrefabInstanceStatus(o) != PrefabInstanceStatus.NotAPrefab &&
                   PrefabUtility.GetPrefabAssetType(o) == PrefabAssetType.NotAPrefab;
            #else
            return true;
            #endif
        }

        private protected override string ErrorText()
        {
            return "should be instanced prefab";
        }
    }
}
