using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] [NotNull] private List<NamedPage> pages;
        [SerializeField] private float delayTime;
        [SerializeField] private bool showFirstOnAwake = true;

        [Serializable]
        public class NamedPage : Named<string, CanvasGroup>
        {
        }

        public void Initialize()
        {
            if (!showFirstOnAwake) return;
            HideAllTables();
            if (pages.Count <= 0) return;
            if (pages[0] == null) return;
            UIStateTools.ChangeGroupState(pages[0].Value, true);
            SetText(pages[0]);
        }

        /// <summary>
        /// Showing canvas group sent thought parameter and disabling all others.
        /// </summary>
        /// <param name="canvasGroup"></param>
        public void OpenPage(CanvasGroup canvasGroup)
        {
            HideAllTables();
            var namedGroup = GetNamedPage(canvasGroup);
            if (namedGroup == null) return;
            UIStateTools.ChangeGroupState(namedGroup.Value, true);
            SetText(namedGroup);
        }

        public void AddPage(string pageName, CanvasGroup pageGroup, out Action openPage)
        {
            var namedGroup = new NamedPage() {Key = pageName, Value = pageGroup};
            pages.Add(namedGroup);
            openPage = () => { OpenPage(namedGroup.Value); };
        }

        public void AddPage(string pageName, CanvasGroup pageGroup, out UnityAction openPage)
        {
            var namedGroup = new NamedPage() {Key = pageName, Value = pageGroup};
            pages.Add(namedGroup);
            openPage = () => { OpenPage(namedGroup.Value); };
        }

        private void SetText(NamedPage namedGroup)
        {
            if (textField != null) textField.text = namedGroup.Key;
        }

        private NamedPage GetNamedPage(CanvasGroup canvasGroup)
        {
            var namedGroup = pages.FirstOrDefault(n => n.Value.GetHashCode() == canvasGroup.GetHashCode());
            return namedGroup;
        }

        public void DelayedOpenPage(CanvasGroup canvasGroup)
        {
            HideAllTables();
            var namedGroup = GetNamedPage(canvasGroup);
            if (namedGroup == null) return;
            SetText(namedGroup);

            StartCoroutine(UIStateTools.ChangeGroupState(namedGroup.Value, true, delayTime,
                                                         (canvas) => { namedGroup.Value = canvas; }));
        }

        public void HideAllTables()
        {
            for (var i = 0; i < pages.Count; i++) UIStateTools.ChangeGroupState(pages[i].Value, false);
        }
    }
}
