using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class Named<TKey, TValue>
    {
        [SerializeField] protected TKey _key;
        [SerializeField] protected TValue _value;

        public TKey Key
        {
            get => _key;
            set => _key = value;
        }

        public TValue Value
        {
            get => _value;
            set => _value = value;
        }
    }

    public class Named<TName, TKey, TValue>
    {
        [SerializeField] private protected TName _name;
        [SerializeField] private protected TKey _key;
        [SerializeField] private protected TValue _value;

        public TName Name
        {
            get => _name;
            set => _name = value;
        }

        public TKey Key
        {
            get => _key;
            set => _key = value;
        }

        public TValue Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
