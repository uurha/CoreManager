using System.Linq;
using Core.CustomAttributes.Headers;
using Core.CustomAttributes.Validation;
using Core.Managers;
using Core.UISystem.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Core.UISystem
{
    [RequireComponent(typeof(SubPagesController))]
    public class UIManager : BaseManager
    {
        [ReferencesHeader]
        [SerializeField] [NotNull] private Transform pageButtonHolder;

        [SerializeField] [NotNull] private Transform uiPageHolder;
        private SubPagesController _subPagesController;

        public override void InitializeElements()
        {
            _subPagesController = GetComponent<SubPagesController>();

            foreach (var o in elements.Select(m => Instantiate(m, uiPageHolder)))
            {
                #if DEBUG
                Debug.Log($"Create element: {o.name}");
                #endif
                if (!o.TryGetComponent(out UIPage page)) continue;
                var pageButton = Instantiate(page.PageButtonPrefab, pageButtonHolder);
                _subPagesController.AddPage(page.PageName, page.Initialize(), out UnityAction action);
                pageButton.onClick.AddListener(action);
                pageButton.text = page.PageName;
            }
            _subPagesController.Initialize();
        }
    }
}
