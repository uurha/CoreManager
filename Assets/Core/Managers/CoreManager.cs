#region license

// Copyright 2021 Arcueid Elizabeth D'athemon
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
using Core.CustomAttributes.Headers;
using Core.CustomAttributes.Validation;
using Core.Managers.Interface;
using Core.ReferenceDistribution;
using UnityEngine;

namespace Core.Managers
{
    /// <summary>
    /// Manager for initialization of sub manager in the scene.
    /// <seealso cref="Core.ReferenceDistribution.ReferenceDistributor"/>
    /// <seealso cref="Core.Managers.Interface.IManager"/>
    /// </summary>
    [OnlyOneInScene]
    public class CoreManager : MonoBehaviour
    {
        [ReferencesHeader]
        [SerializeField] [NotNull]
        private ReferenceDistributor referenceDistributor;

        [PrefabHeader]
        [SerializeField] [PrefabRequired] [HasComponent(typeof(IManager))]
        private List<GameObject> managers;

        private void Awake()
        {
            //Instantiate all managers.
            InitializeManagers();
            referenceDistributor.Initialize();
        }

        private void Start()
        {
            GameManager.InitializeSubscriptions();
            GameManager.InvokeBase();
        }

        /// <summary>
        /// Create and initialize managers from the list.
        /// </summary>
        private void InitializeManagers()
        {
            foreach (var o in managers.Select(m => Instantiate(m, transform)))
            {
                #if DEBUG
                Debug.Log($"Create manager: {o.name}");
                #endif
                if (!o.TryGetComponent(out IManager manager)) continue;
                manager.InitializeElements();
            }
        }
    }
}
