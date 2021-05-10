using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    #if UNITY_EDITOR
    public static class TagHelper
    {
        public static void AddTag(string tag)
        {
            if (GetTagsProperty(out var so, out var tagsProperty)) return;

            for (var i = 0; i < tagsProperty.arraySize; ++i)
                if (tagsProperty.GetArrayElementAtIndex(i).stringValue == tag)
                    return; // Tag already present, nothing to do.
            var index = tagsProperty.arraySize - 1;
            tagsProperty.InsertArrayElementAtIndex(index);
            tagsProperty.GetArrayElementAtIndex(index).stringValue = tag;
            so.ApplyModifiedProperties();
            so.Update();
        }

        public static bool IsTagsExists(params string[] tags)
        {
            if (GetTagsProperty(out var _, out var tagsProperty)) return false;

            foreach (var tag in tags)
            {
                var isExists = false;

                for (var i = 0; i < tagsProperty.arraySize; i++)
                    if (tag.Equals(tagsProperty.GetArrayElementAtIndex(i).stringValue))
                        isExists = true;
                if (isExists) continue;
                Debug.LogError($"Tag: \"{tag}\" not defined");
                return false;
            }
            return true;
        }

        internal static bool IsTagsExistsInternal(params string[] tags)
        {
            if (GetTagsProperty(out var _, out var tagsProperty)) return false;

            foreach (var tag in tags)
            {
                var isExists = false;

                for (var i = 0; i < tagsProperty.arraySize; i++)
                    if (tag.Equals(tagsProperty.GetArrayElementAtIndex(i).stringValue))
                        isExists = true;
                if (isExists) continue;
                return false;
            }
            return true;
        }

        private static bool GetTagsProperty(out SerializedObject so, out SerializedProperty tagsProperty)
        {
            tagsProperty = null;
            so = null;
            var asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");

            if (asset == null ||
                asset.Length <= 0)
                return true;
            so = new SerializedObject(asset[0]);
            tagsProperty = so.FindProperty("tags");
            return false;
        }
    }
    #endif
}
