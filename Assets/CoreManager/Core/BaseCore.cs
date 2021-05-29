﻿#region license

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
using CoreManager.Core.Interface;
using CoreManager.CustomAttributes.Headers;
using CoreManager.CustomAttributes.Validation;
using UnityEngine;

namespace CoreManager.Core
{
    /// <summary>
    /// Base implementation of IManager.
    /// </summary>
    public abstract class BaseCore : MonoBehaviour, ICore
    {
        [PrefabHeader]
        [SerializeField] [PrefabRequired] private protected List<GameObject> elements;

        public virtual void InitializeElements()
        {
            foreach (var o in elements.Select(m => Instantiate(m, transform)))
            {
                #if DEBUG
                Debug.Log($"Create element: {o.name}");
                #endif
            }
        }
    }
}
