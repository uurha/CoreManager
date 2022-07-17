#region license

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

using CorePlugin.Attributes.Validation;
using CorePlugin.Cross.SceneData;
using CorePlugin.Samples.Scripts.Model;
using UnityEngine;

namespace CorePlugin.Samples.Scripts
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] [NotNull] private Transform spawnPoint;
        [SerializeField] [PrefabRequired] private InstancedObject prefab;
        
        private void Start()
        {
            if (!SceneDataHandler.GetData(out DataTransfer data)) return;

            for (var i = 0; i < data.IntData; i++)
            {
                var o = Instantiate(prefab, spawnPoint.position + Vector3.right * i * 2, Quaternion.identity, spawnPoint);
                o.Initialize(data.ColorData, $"#{i} {data.StrData}");
            }
        }
    }

}
