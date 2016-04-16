using Miscellaneous.DataStructures;
using UnityEngine;

namespace AccelerationStructures.CompactGrid
{
    /// <summary>
    /// The data resulting from a successful raycast in a CompactGrid acceleration structure.
    /// </summary>
    public class CompactGridRaycastResult
    {
        // .. ATTRIBUTES

        public float Distance;
        public Vector3 SurfaceNormal; //the normal of the mesh at hit position

        public float Barycentric_Beta; //vertex01 <> vertex02
        public float Barycentric_Gamma; //vertex01 <> vertex03
        public Point3 VertexIndices;

        // .. OPERATIONS

        public void CopyTo(CompactGridRaycastResult other)
        {
            other.Distance = Distance;
            other.SurfaceNormal = SurfaceNormal;
            other.Barycentric_Beta = Barycentric_Beta;
            other.Barycentric_Gamma = Barycentric_Gamma;
            other.VertexIndices = VertexIndices;
        }
    }
}
