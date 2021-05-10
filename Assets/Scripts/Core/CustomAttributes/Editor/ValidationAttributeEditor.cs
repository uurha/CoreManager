using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.CustomAttributes.Validation.Base;
using Core.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.CustomAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class ValidationAttributeEditor : UnityEditor.Editor
    {
        private IEnumerable<FieldInfo> _fields = new FieldInfo[0];
        private IEnumerable<Attribute> _classAttributes = new ClassValidationAttribute[0];

        private bool _shouldShowErrors = true;

        protected virtual void OnEnable()
        {
            _fields = Validation.GetAllFields(target.GetType());
            _classAttributes = Validation.GetAllAttributes(target.GetType());
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            foreach (var field in _fields) ValidateField(field);
            foreach (var attribute in _classAttributes) ValidateClass(attribute);
            _shouldShowErrors = serializedObject.ApplyModifiedProperties();
        }

        private void ValidateClass(Attribute attribute)
        {
            ValidateClassAttribute(attribute as ClassValidationAttribute);
        }

        private void ValidateField(FieldInfo field)
        {
            var prop = GetSerializedProperty(field);
            if (prop == null) return;
            var atts = field.GetCustomAttributes(typeof(FieldValidationAttribute), true);
            foreach (var att in atts) ValidateFieldAttribute(att as FieldValidationAttribute, field);
        }

        private void ValidateFieldAttribute(FieldValidationAttribute attribute, FieldInfo field)
        {
            if (attribute.Validate(field, target)) return;
            UnityEditorExtension.HelpBox(attribute.ErrorMessage, MessageType.Error);
            if (attribute.ShowError && _shouldShowErrors) Validation.ShowError(attribute.ErrorMessage, target);
        }

        private void ValidateClassAttribute(ClassValidationAttribute attribute)
        {
            if (attribute.Validate(target)) return;
            UnityEditorExtension.HelpBox(attribute.ErrorMessage, MessageType.Error);
            if (attribute.ShowError && _shouldShowErrors) Validation.ShowError(attribute.ErrorMessage, target);
        }

        

        private SerializedProperty GetSerializedProperty(FieldInfo field)
        {
            // Do not display properties marked with HideInInspector attribute
            var hideAtts = field.GetCustomAttributes(typeof(HideInInspector), true);
            return hideAtts.Length > 0 ? null : serializedObject.FindProperty(field.Name);
        }

    }
}
