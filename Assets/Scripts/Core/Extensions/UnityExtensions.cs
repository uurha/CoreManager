using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Extensions
{

    public static class UnityExtensions
    {
        public static bool IsNotNullAndNotEqual(this ISerializable handledItem, ISerializable newItem)
        {
            return handledItem?.Equals(newItem) == false;
        }

        public static bool IsNotNullAndEqual(this ISerializable handledItem, ISerializable newItem)
        {
            return handledItem?.Equals(newItem) == true;
        }

        public static void Clear<T>(ref List<T> list) where T : MonoBehaviour
        {
            for (var index = 0; index < list.Count; index++) Object.Destroy(list[index].gameObject);
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

        public static bool TryToFindObjectOfType<T>(out T result)
        {
            result = default;
            if (TryToFindObjectsOfType(out IEnumerable<T> bufferResults)) result = bufferResults.FirstOrDefault();
            return result != null;
        }

        public static bool TryToFindObjectsOfType<T>(out IEnumerable<T> result)
        {
            result = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            return result.Any();
        }

        public static bool TryToFindObjectsOfType<T>(out IList<T> result)
        {
            result = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>().ToList();
            return result != null;
        }
    }
}
