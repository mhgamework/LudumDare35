using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsVector2
    {
        /// <summary>
        /// Returns the angle in degrees between this vector and given vector, counterclockwise, range [0..360[.
        /// </summary>
        public static float Angle360(this Vector2 this_vector, Vector2 other_vector)
        {
            // angle in [0,180]
            float angle = Vector3.Angle(this_vector, other_vector);
            float sign = Mathf.Sign(Vector3.Dot(Vector3.forward, Vector3.Cross(this_vector, other_vector)));

            // angle in [-179,180]
            float signed_angle = angle * sign;

            // angle in [0,360]
            return (signed_angle + 360) % 360;
        }

        /// <summary>
        /// Rotate this vector by given degrees.
        /// </summary>
        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            var quaternion = Quaternion.AngleAxis(degrees, Vector3.forward);
            var rotated = quaternion * vector;
            return new Vector2(rotated.x, rotated.y);
        }
    }
}
