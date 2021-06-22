using System;
using System.Collections.Generic;
using System.Linq;
using CorePlugin.Extensions;
using UnityEngine;

namespace CorePlugin.UISystem
{
    /// <summary>
    /// UI element hider. Useful than needed to hide elements on UI change its' size
    /// </summary>
    [ExecuteAlways]
    public class ItemHider : MonoBehaviour
    {
        [Serializable]
        public class LODGroup
        {
            [SerializeField] private CanvasGroup item;
            [SerializeField] private float size;
            [SerializeField] private Direction direction;

            public enum Direction
            {
                Horizontal,
                Vertical
            }
            
            public CanvasGroup Item => item;

            public float Size => size;

            public Direction Dir => direction;
        }

        [SerializeField] private List<LODGroup> lods;

        private List<LODGroup> _LODsToHide;
        private List<LODGroup> _LODsToShow;
        private RectTransform _thisRectTransform;

        private void Awake()
        {
            _LODsToHide = new List<LODGroup>(lods);
            _LODsToShow = new List<LODGroup>();
            _thisRectTransform = (RectTransform) transform;
        }

        private void OnRectTransformDimensionsChange()
        {
            var listInitialized = _LODsToHide != null && _LODsToShow != null;
            if(listInitialized && _LODsToHide.Count > 0)
            {
                _LODsToShow.AddRange(CheckLODGroups(ref _LODsToHide, false));
            }

            if (listInitialized && _LODsToShow.Count > 0)
            {
                _LODsToHide.AddRange(CheckLODGroups(ref _LODsToShow, true));
            }
        }

        private List<LODGroup> CheckLODGroups(ref List<LODGroup> lodGroups, bool state)
        {
            var range = lodGroups.Where(x =>
            {
                var sizeDelta = _thisRectTransform.sizeDelta;
                var checkingValue = x.Dir switch
                {
                    LODGroup.Direction.Horizontal => sizeDelta.x,
                    LODGroup.Direction.Vertical => sizeDelta.y,
                    _ => throw new ArgumentOutOfRangeException()
                };
                return state ? x.Size < checkingValue : x.Size > checkingValue;
            }).ToList();
        
            foreach (var item in range)
            {
                UIStateTools.ChangeGroupState(item.Item, state);
            }

            lodGroups = lodGroups.RemoveRange(range);
            return range;
        }

        private void OnValidate()
        {
            _LODsToHide = new List<LODGroup>(lods);
            _LODsToShow = new List<LODGroup>();
            _thisRectTransform = (RectTransform) transform;
        }
    }
}
