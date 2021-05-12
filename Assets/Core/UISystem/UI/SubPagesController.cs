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

using System;
using System.Collections.Generic;
using Core.CustomAttributes.Validation;
using Core.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Core.UISystem.UI
{
    public class SubPagesController : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private float delayTime;
        [SerializeField] private bool showFirstOnAwake = true;

        private List<UIPage> _pages = new List<UIPage>();

        public void Initialize()
        {
            if (!showFirstOnAwake) return;
            HideAllTables();
            if (_pages.Count <= 0) return;
            if (_pages[0] == null) return;
            UIStateTools.ChangeGroupState(_pages[0].Group, true);
            SetText(_pages[0]);
        }

        /// <summary>
        /// Showing canvas group sent thought parameter and disabling all others.
        /// </summary>
        /// <param name="page"></param>
        public void OpenPage(UIPage page)
        {
            HideAllTables();
            if (IsContains(page)) return;
            UIStateTools.ChangeGroupState(page.Group, true);
            SetText(page);
        }

        public void AddPage(UIPage page, out Action openPage)
        {
            _pages.Add(page);
            openPage = () => { OpenPage(page); };
        }

        public void AddPage(UIPage page, out UnityAction openPage)
        {
            _pages.Add(page);
            openPage = () => { OpenPage(page); };
        }

        private void SetText(UIPage namedGroup)
        {
            if (textField != null) textField.text = namedGroup.PageName;
        }

        private bool IsContains(UIPage page)
        {
            var namedGroup = _pages.Contains(page);
            return namedGroup;
        }

        public void DelayedOpenPage(UIPage page)
        {
            HideAllTables();
            if (IsContains(page)) return;
            SetText(page);

            StartCoroutine(UIStateTools.ChangeGroupState(page.Group, true, delayTime));
        }

        public void HideAllTables()
        {
            for (var i = 0; i < _pages.Count; i++) UIStateTools.ChangeGroupState(_pages[i].Group, false);
        }
    }
}
