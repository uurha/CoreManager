using System.IO;
using Core.Cross.SceneData;
using Core.Managers;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public class CoreMenuItems
    {
        [MenuItem("Core/Create Core Manager")]
        private static void CreateCoreManager()
        {
            CreatePrefab<CoreManager>();
        }

        [MenuItem("Core/Create Cross Scene Data Handler")]
        private static void CreateCrossSceneDataHandler()
        {
            CreatePrefab<CrossSceneDataHandler>();
        }

        private static string PrefabPath<T>()
        {
            return Path.Combine("Prefabs", typeof(T).Name);
        }

        private static void CreatePrefab<T>() where T : MonoBehaviour
        {
            var prefabPath = PrefabPath<T>();
            var componentOrGameObject = Resources.Load<T>(prefabPath);

            if (componentOrGameObject != null)
            {
                var objects = Object.FindObjectsOfType(typeof(T));

                if (objects.Length > 0)
                {
                    foreach (var o in objects) ShowError($"Should be only one {typeof(T).Name} in scene", o);
                    return;
                }
                var prefab = (GameObject) PrefabUtility.InstantiatePrefab(componentOrGameObject);
                prefab.name = componentOrGameObject.name;
                prefab.transform.SetAsLastSibling();
            }
            else
            {
                var c = Path.Combine("..", "Core", nameof(Resources), prefabPath);
                var message = $"Probably you move or rename {typeof(T).Name} prefab from initial path ({c}).";
                ShowError(message);
            }
        }

        private static void ShowError(string error, Object context = null)
        {
            EditorApplication.Beep();
            Debug.LogError(error, context);
        }
    }
}
