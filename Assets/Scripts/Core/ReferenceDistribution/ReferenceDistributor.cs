using System.Collections.Generic;
using System.Linq;
using Core.CustomAttributes.Validation;
using Core.Extensions;
using Core.Managers;
using Core.ReferenceDistribution.Interface;
using UnityEngine;

namespace Core.ReferenceDistribution
{
    [RequireComponent(typeof(CoreManager))]
    [OnlyOneInScene]
    public class ReferenceDistributor : MonoBehaviour
    {
        private IEnumerable<IDistributingReference> _distributingReferences;
        private bool _isInitialized;

        private static ReferenceDistributor _instance;

        public void Initialize()
        {
            _isInitialized = UnityExtensions.TryToFindObjectsOfType(out _distributingReferences);
            _instance = this;
        }

        /// <summary>
        /// Getting reference by type from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetReference<T>() where T : MonoBehaviour, IDistributingReference
        {
            return _instance._isInitialized ? _instance._distributingReferences.OfType<T>().First() : null;
        }

        /// <summary>
        /// Finding reference if passed parameter not null.
        /// Use this if you need reference not in Start() and/or reference should be received in some event
        /// </summary>
        /// <param name="reference"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool AskReference<T>(ref T reference) where T : MonoBehaviour, IDistributingReference
        {
            reference ??= GetReference<T>();
            return ReferenceEquals(reference, null);
        }

        public static IEnumerable<T> GetReferences<T>() where T : MonoBehaviour, IDistributingReference
        {
            return _instance._isInitialized ? _instance._distributingReferences.OfType<T>() : null;
        }

        private void OnDisable()
        {
            _instance = null;
        }
    }
}
