using System;
using UnityEngine;

namespace Core.Interface
{
    [Serializable]
    public class Serializable : ISerializable
    {
        private string _guid;

        public string Guid => _guid;

        public Serializable()
        {
            _guid = System.Guid.NewGuid().ToString();
        }

        public bool Equals(ISerializable item)
        {
            return Guid.Equals(item.Guid);
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this,true);
        }
    }
}