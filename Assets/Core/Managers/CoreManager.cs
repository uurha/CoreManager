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
using Core.Singletons;
using UnityEngine;

namespace Core.Managers
{
    /// <summary>
    /// Main Core manager.
    /// </summary>
    [OnlyOneInScene]
    public class CoreManager : Singleton<CoreManager>
    {
        private const string MainCanvasTag = "Canvas/UI";
        private const string PlayerTag = "Player/Player";

        [ReferencesHeader]
        [SerializeField] [NotNull]
        private ReferenceDistributor referenceDistributor;

        [PrefabHeader]
        [SerializeField] [PrefabRequired] [HasComponent(typeof(IManager))]
        private List<GameObject> managers;

        private Transform _mainCanvas;
        private Transform _playerTransform;

        public Transform MainCanvas => _mainCanvas;

        public Transform PlayerTransform => _playerTransform;

        private void Awake()
        {
            //Instantiate all managers.
            InitializeManagers();
            InitializeLayers();
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
                if (!o.TryGetComponent(out IManager manager)) continue;
                manager.InitializeElements();
                #if DEBUG
                Debug.Log($"Create manager: {o.name}");
                #endif
            }
        }

        /// <summary>
        /// try to find the layer by tag.
        /// </summary>
        private void InitializeLayers()
        {
            _mainCanvas = GameObject.FindWithTag(MainCanvasTag)?.transform;
            _playerTransform = GameObject.FindWithTag(PlayerTag)?.transform;
        }
    }
}
