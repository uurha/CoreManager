using UnityEditor;
using UnityEngine;

namespace Core.Extensions
{
    public static class UnityEditorExtension
    {
        public static void HelpBox(string message, MessageType type)
        {
            var style = new GUIStyle(EditorStyles.helpBox) {richText = true, fontSize = 11};

            var icon = IconName(type);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.TrTextContentWithIcon(message, icon), style);
        }
        
        public static void HelpBox(string message, MessageType type, GUIStyle style)
        {
            var icon = IconName(type);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.TrTextContentWithIcon(message, icon), style);
        }

        public static string IconName(MessageType type)
        {
            var icon = type switch
                       {
                           MessageType.Info => "console.infoicon",
                           MessageType.Warning => "console.warnicon",
                           MessageType.Error => "console.erroricon",
                           _ => ""
                       };
            return icon;
        }
    }
}
