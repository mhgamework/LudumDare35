using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsQuaternion
    {
        // -- PUBLIC

        // .. EXENSION_METHODS

        public static bool IsValid(
            this Quaternion quaternion
            )
        {
            return !( float.IsNaN( quaternion.x ) || float.IsNaN( quaternion.y ) || float.IsNaN( quaternion.z ) || float.IsNaN( quaternion.w ) );
        }
    }
}
