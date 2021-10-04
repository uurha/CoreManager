﻿#region license

// Copyright 2021 - 2021 Arcueid Elizabeth D'athemon
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

using TMPro;
using UnityEngine;

namespace CorePlugin.Samples.Scripts.Model
{
    [RequireComponent(typeof(Renderer))]
    public class InstancedObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private Renderer _renderer;

        public void Initialize(Color color, string str)
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.color = color;
            text.text = str;
        }
    }
}
