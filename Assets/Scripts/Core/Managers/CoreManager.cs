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
