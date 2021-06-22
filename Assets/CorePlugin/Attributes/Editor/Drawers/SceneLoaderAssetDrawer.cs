using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CorePlugin.Extensions;
using CorePlugin.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace CorePlugin.Attributes.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(SceneLoaderAsset))]
    public class SceneLoaderAssetDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);
            var targetObject = property.serializedObject.targetObject;
            
            var value = fieldInfo.GetValue(targetObject);

            DrawItem(position, value, property, targetObject, label);
            EditorGUI.EndProperty();
        }

        private void DrawItem(Rect position, object value, SerializedProperty property, Object targetObject, GUIContent label)
        {
            SceneLoaderAsset sceneManagerAsset;
            List<SceneLoaderAsset> bufferList = null;
            var index = -1;
            switch (value)
            {
                case List<SceneLoaderAsset> list:
                {
                    var s = Regex.Match(property.propertyPath, @"\[(.*?)\]").Value.Trim('[',']');
                    index = int.Parse(s);
                    sceneManagerAsset = list[index];
                    bufferList = list;
                    label = new GUIContent($"Element {index}");
                    break;
                }
                case SceneLoaderAsset asset:
                    sceneManagerAsset = asset;
                    break;
                default:
                    return;
            }

            SceneAsset oldScene = null;
            if (sceneManagerAsset.InstanceID != 0)
            {
                oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GetAssetPath(sceneManagerAsset.InstanceID));
            }

            CheckBuildScene(sceneManagerAsset.FullPath, oldScene);
            EditorGUI.BeginChangeCheck();

            position = new Rect(position.x, position.y + 1f, position.width, EditorGUIUtility.singleLineHeight);
            
            var newScene = EditorGUI.ObjectField(position, label, oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (!EditorGUI.EndChangeCheck()) return;
            var newPath = AssetDatabase.GetAssetPath(newScene);
            SceneLoaderAsset newManagerAsset = null;
            if (newScene is { })
            {
                newManagerAsset = new SceneLoaderAsset(newPath, newScene.GetInstanceID());
            }

            if (bufferList == null)
            {
                fieldInfo.SetValue(targetObject, newManagerAsset);
            }
            else
            {
                bufferList[index] = newManagerAsset;
                fieldInfo.SetValue(targetObject, bufferList);
            }

            CheckBuildScene(newPath, newScene);
        }
    
        private static void CheckBuildScene(string path, SceneAsset sceneToCheck)
        {
            if(sceneToCheck == null) return;
        
            var buildScene = EditorBuildSettings.scenes.FirstOrDefault(x => x.path == path);
    
            if (buildScene == null)
            {
                UnityEditorExtension.HelpBox($"Scene <b>{sceneToCheck.name}</b> not in build", MessageType.Error);
            }
            else if (!buildScene.enabled)
            {
                UnityEditorExtension.HelpBox($"Scene <b>{sceneToCheck.name}</b> not enabled", MessageType.Error);
            }
        }
    }
}