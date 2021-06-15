using System;
using UnityEngine;

namespace CorePlugin.Extensions
{
    [Serializable]
    public struct Range<T> where T : new()
    {
        [SerializeField] private T min;
        [SerializeField] private T max;
            
        public T Min => min;
        public T Max => max;
            
        public Range(T minValue, T maxValue)
        {
            min = minValue;
            max = maxValue;
        }

        public Range<T> UpdateMin(T minValue)
        {
            min = minValue;
            return this;
        }
            
        public Range<T> UpdateMax(T maxValue)
        {
            max = maxValue;
            return this;
        }
    }
}