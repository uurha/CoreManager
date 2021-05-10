using System;
using UnityEngine;

namespace Core.CustomAttributes.Headers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class PrefabHeaderAttribute : HeaderAttribute
    {
        public PrefabHeaderAttribute() : base("Prefabs")
        {
        }
    }
}
