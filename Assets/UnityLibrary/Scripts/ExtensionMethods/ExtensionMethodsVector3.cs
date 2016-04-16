using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsVector3
    {
        /// <summary>
        /// Returns the closest point on (unbounded) line a->b from this vector.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="line_point_a"></param>
        /// <param name="line_point_b"></param>
        /// <returns></returns>
        public static Vector3 ProjectOnLine(this Vector3 v, Vector3 line_point_a, Vector3 line_point_b)
        {
            var a_to_v = v - line_point_a;
            var a_to_b_unit = line_point_b - line_point_a;
            return line_point_a + Vector3.Dot(a_to_b_unit, a_to_v) * a_to_b_unit;
        }
    }
}
