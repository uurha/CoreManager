using System.Linq;
using UnityEngine;

namespace CorePlugin.Extensions
{
    public struct MathfExtensions
    {
        public static Vector3 Average(params Vector3[] vector3s)
        {
            var t = vector3s.Aggregate(Vector3.zero, (current, vector3) => current + vector3);
            return t / vector3s.Length;
        }
        
        public static Vector3 MiddlePoint(Vector3 start, Vector3 end)
        {
            var t = start + end;
            return t / 2;
        }
        
        public static Vector2 MiddlePoint(Vector2 start, Vector2 end)
        {
            var t = start + end;
            return t / 2;
        }
    }
}