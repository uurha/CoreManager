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
using System.Linq;
using Core.Serializable.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Extensions
{

    /// <summary>
    /// Extension class for default Unity classes
    /// </summary>
    public static class UnityExtensions
    {
        /// <summary>
        /// Checks if left item is null and not equals right item
        /// </summary>
        /// <param name="lci">Left compare item</param>
        /// <param name="rci">Right compare item</param>
        /// <returns></returns>
        public static bool IsNotNullAndNotEqual(this ISerializable lci, ISerializable rci)
        {
            return lci?.Equals(rci) == false;
        }

        /// <summary>
        /// Checks if left item is null and equals right item
        /// </summary>
        /// <param name="lci">Left compare item</param>
        /// <param name="rci">Right compare item</param>
        /// <returns></returns>
        public static bool IsNotNullAndEqual(this ISerializable lci, ISerializable rci)
        {
            return lci?.Equals(rci) == true;
        }

        /// <summary>
        /// Clearing list and destroying it items
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static void Clear<T>(ref List<T> list) where T : MonoBehaviour
        {
            foreach (var item in list) Object.Destroy(item.gameObject);
            list.Clear();
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, T oldValue, T newValue)
            where T : ISerializable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Select(x => EqualityComparer<T>.Default.Equals(x, oldValue) ? newValue : x);
        }

        public static IList<T> ReplaceOrAdd<T>(this IList<T> source, IEnumerable<T> newValues) where T : ISerializable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var serializables = newValues as T[] ?? newValues.ToArray();

            for (var i = 0; i < serializables.Count(); i++)
            {
                var foundIndex = source.Replace(serializables[i]);
                if (foundIndex == -1) source.Add(serializables[i]);
            }
            return source;
        }

        public static int Replace<T>(this IList<T> source, T newValue) where T : ISerializable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var findItem = source.FirstOrDefault(x => x.Equals(newValue));
            var index = findItem == null ? -1 : source.IndexOf(findItem);
            if (index != -1) source[index] = newValue;
            return index;
        }

        /// <summary>
        /// Trying to find object on scene what inherited from T
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryToFindObjectOfType<T>(out T result)
        {
            result = default;
            if (TryToFindObjectsOfType(out IEnumerable<T> bufferResults)) result = bufferResults.FirstOrDefault();
            return result != null;
        }

        /// <summary>
        /// Trying to find objects on scene what inherited from T
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryToFindObjectsOfType<T>(out IEnumerable<T> result)
        {
            result = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            return result.Any();
        }

        /// <summary>
        /// Trying to find object on scene what inherited from T
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryToFindObjectsOfType<T>(out IList<T> result)
        {
            result = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>().ToList();
            return result != null;
        }
    }
}
