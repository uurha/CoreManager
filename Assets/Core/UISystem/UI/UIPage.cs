using System.Collections.Generic;
using System.Linq;
using Core.CustomAttributes.Headers;
using Core.CustomAttributes.Validation;
using UnityEngine;

namespace Core.UISystem.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPage : MonoBehaviour
    {
        [SerializeField] private string pageName;

        [PrefabHeader]
        [SerializeField] [PrefabRequired] private ButtonWithText pageButtonPrefab;

        [SerializeField] [PrefabRequired] private protected List<GameObject> elements;

        public ButtonWithText PageButtonPrefab => pageButtonPrefab;

        public string PageName => pageName;

        public CanvasGroup Group => _canvasGroup;

        private CanvasGroup _canvasGroup;

        public UIPage Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            foreach (var o in elements.Select(m => Instantiate(m, transform)))
            {
                #if DEBUG
                Debug.Log($"Create element: {o.name}");
                #endif
            }
            return this;
        }
    }
}
