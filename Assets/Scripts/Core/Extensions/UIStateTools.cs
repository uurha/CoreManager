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
