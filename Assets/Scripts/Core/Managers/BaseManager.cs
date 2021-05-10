using System.Collections.Generic;
using System.Linq;
using Core.CustomAttributes.Headers;
using Core.CustomAttributes.Validation;
using Core.Managers.Interface;
using UnityEngine;

namespace Core.Managers
{
    public abstract class BaseManager : MonoBehaviour, IManager
    {
        [PrefabHeader]
        [SerializeField] [PrefabRequired] private protected List<GameObject> elements;

        public virtual void InitializeElements()
        {
            foreach (var o in elements.Select(m => Instantiate(m, transform)))
            {
                #if DEBUG
                Debug.Log($"Create element: {o.name}");
                #endif
            }
        }
    }
}
