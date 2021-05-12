using System;
using Core.Cross.SceneData;
using Core.CustomAttributes.Headers;
using Core.CustomAttributes.Validation;
using Core.Demo.Scripts.Model;
using UnityEngine;

namespace Core.Demo.Scripts
{
    public class ObjectSpawner : MonoBehaviour
    {
        [ReferencesHeader]
        [SerializeField] [NotNull] private Transform spawnPoint;
        [PrefabHeader]
        [SerializeField] [PrefabRequired] private InstancedObject prefab;

        private void Start()
        {
            if (!CrossSceneDataHandler.Instance.GetData(out DataTransfer data)) return;

            for (int i = 0; i < data.IntData; i++)
            {
                var o = Instantiate(prefab, spawnPoint.position + Vector3.right * i * 2, Quaternion.identity, spawnPoint);
                o.Initialize(data.ColorData, $"#{i} {data.StrData}");
            }
        }
    }

}
