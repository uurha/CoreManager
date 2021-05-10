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
using System.Collections;
using UnityEngine;

namespace Core.Extensions
{
    public static class UIStateTools
    {
        public static void ChangeGroupState(CanvasGroup group, bool isVisible)
        {
            group.alpha = isVisible ? 1 : 0;
            group.interactable = isVisible;
            group.blocksRaycasts = isVisible;
        }

        public static void ChangeCursorState(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = state;
        }

        public static IEnumerator ChangeGroupState(CanvasGroup canvas, bool isVisible, float delay,
                                                   Action<CanvasGroup> action = null)
        {
            canvas.alpha = isVisible ? 1 : 0;
            canvas.blocksRaycasts = isVisible;
            yield return new WaitForSeconds(delay);
            canvas.interactable = isVisible;
            action?.Invoke(canvas);
        }
    }
}
