using System;
using Core.CustomAttributes.Validation.Base;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Core.CustomAttributes.Validation
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SceneObjectRequiredAttribute : FieldValidationAttribute
    {
        public SceneObjectRequiredAttribute(bool showError = false) : base(showError)
        {
        }

        private protected override bool ValidState(object obj)
        {
            #if UNITY_EDITOR
            return PrefabUtility.GetPrefabInstanceStatus((Object) obj) != PrefabInstanceStatus.NotAPrefab &&
                   PrefabUtility.GetPrefabAssetType((Object) obj) == PrefabAssetType.NotAPrefab;
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
