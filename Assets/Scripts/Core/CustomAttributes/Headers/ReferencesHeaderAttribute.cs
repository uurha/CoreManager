using System;
using UnityEngine;

namespace Core.CustomAttributes.Headers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ReferencesHeaderAttribute : HeaderAttribute
    {
        public ReferencesHeaderAttribute() : base("References")
        {
        }
    }
}
