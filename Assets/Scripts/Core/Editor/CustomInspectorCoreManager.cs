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
