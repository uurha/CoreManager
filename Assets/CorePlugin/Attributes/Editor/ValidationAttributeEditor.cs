#region license

// Copyright 2021 - 2021 Arcueid Elizabeth D'athemon
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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CorePlugin.Attributes.EditorAddons;
using CorePlugin.Attributes.Validation.Base;
using CorePlugin.Editor.Extensions;
using CorePlugin.Extensions;
using UnityEditor;
using UnityEngine;

namespace CorePlugin.Attributes.Editor
{
    /// <summary>
    /// Custom editor for Unity.Object class.
    /// If you want to create your own custom editor inherit from this class.
    /// <seealso cref="ValidationAttribute"/>
    /// <seealso cref="FieldValidationAttribute"/>
    /// <seealso cref="ClassValidationAttribute"/>
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class ValidationAttributeEditor : UnityEditor.Editor
    {
        private IEnumerable<ClassValidationAttribute> _classAttributes = Enumerable.Empty<ClassValidationAttribute>();

        private IEnumerable<KeyValuePair<MethodInfo, IEnumerable<EditorButtonAttribute>>> _methodButtonsAttributes =
            Enumerable.Empty<KeyValuePair<MethodInfo, IEnumerable<EditorButtonAttribute>>>();

        private IEnumerable<KeyValuePair<FieldInfo, IEnumerable<FieldValidationAttribute>>> _fields =
            Enumerable.Empty<KeyValuePair<FieldInfo, IEnumerable<FieldValidationAttribute>>>();

        private bool _shouldShowErrors = true;
        private Object _bufferTarget;

        protected virtual void OnEnable()
        {
            _bufferTarget = target;
            var type = _bufferTarget.GetType();
            _fields = type.GetFieldsAttributes<FieldValidationAttribute>();
            _classAttributes = type.GetClassAttributes<ClassValidationAttribute>();
            _methodButtonsAttributes = type.GetMethodsAttributes<EditorButtonAttribute>();
        }

        private void DrawButtons(IEnumerable<KeyValuePair<MethodInfo, IEnumerable<EditorButtonAttribute>>> buttons)
        {
            foreach (var valuePair in buttons)
            {
                var editorButtonAttribute = valuePair.Value;
                EditorGUILayout.BeginHorizontal();

                foreach (var attribute in editorButtonAttribute)
                    if (GUILayout.Button(attribute.GetButtonName(valuePair.Key.GetPrettyMemberName()), new GUIStyle(GUI.skin.button)
                                             {
                                                 stretchWidth = true,
                                                 richText = true,
                                                 stretchHeight = true,
                                                 wordWrap = true
                                             }))
                        valuePair.Key.Invoke(_bufferTarget, attribute.InvokeParams);
                EditorGUILayout.EndHorizontal();
            }
        }

        private SerializedProperty GetSerializedProperty(FieldInfo field)
        {
            var hideAtts = field.GetCustomAttributes(typeof(HideInInspector), true);
            return hideAtts.Length > 0 ? null : serializedObject.FindProperty(field.Name);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            DrawButtons(_methodButtonsAttributes);
            foreach (var field in _fields) ValidateField(field);
            foreach (var attribute in _classAttributes) ValidateClassAttribute(attribute);
            _shouldShowErrors = serializedObject.ApplyModifiedProperties();
        }

        private void ValidateClassAttribute(ClassValidationAttribute attribute)
        {
            if (attribute.Validate(_bufferTarget)) return;
            UnityEditorExtension.HelpBox(attribute.ErrorMessage, MessageType.Error);
            if (attribute.ShowError && _shouldShowErrors) AttributeExtensions.ShowError(attribute.ErrorMessage, target);
        }

        private void ValidateField(KeyValuePair<FieldInfo, IEnumerable<FieldValidationAttribute>> fieldsPairs)
        {
            var prop = GetSerializedProperty(fieldsPairs.Key);
            if (prop == null) return;
            foreach (var att in fieldsPairs.Value) ValidateFieldAttribute(att, fieldsPairs.Key);
        }

        private void ValidateFieldAttribute(FieldValidationAttribute attribute, FieldInfo field)
        {
            if (attribute.Validate(field, _bufferTarget)) return;
            UnityEditorExtension.HelpBox(attribute.ErrorMessage, MessageType.Error);
            if (attribute.ShowError && _shouldShowErrors) AttributeExtensions.ShowError(attribute.ErrorMessage, target);
        }
    }
}
