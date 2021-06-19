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

using System;
using System.Collections.Generic;
using CorePlugin.CustomAttributes.Headers;
using CorePlugin.CustomAttributes.Validation;
using CorePlugin.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CorePlugin.Console
{
    /// <summary>
    /// Minimized console
    /// <remarks>
    /// Works together with <seealso cref="CorePlugin.Console.RuntimeConsole"/>
    /// </remarks>
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class MinimizedConsole : MonoBehaviour, IPointerClickHandler, IBeginDragHandler
    {
        [ReferencesHeader]
        [NotNull] [SerializeField] private List<CountDisplayer> countDisplayers;

        private CanvasGroup _consoleCanvasGroup;
        private Action onConsoleMaximized;
        private bool _previouslyDragged;
        
        public List<CountDisplayer> CountDisplayers => countDisplayers;

        public MinimizedConsole Initialize(Action onMaximized)
        {
            _consoleCanvasGroup = GetComponent<CanvasGroup>();
            onConsoleMaximized += onMaximized;
            
            return this;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_previouslyDragged)
            {
                Maximize();
            }
            else
            {
                _previouslyDragged = false;
            }
        }

        private void Maximize()
        {
            SetActive(false);
            onConsoleMaximized?.Invoke();
        }

        public void SetActive(bool state)
        {
            UIStateTools.ChangeGroupState(_consoleCanvasGroup, state);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _previouslyDragged = true;
        }
    }
}