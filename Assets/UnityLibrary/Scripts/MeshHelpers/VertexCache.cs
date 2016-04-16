using UnityEngine;

namespace MeshHelpers
{
    /// <summary>
    /// A data class for storing vertex positions.
    /// </summary>
    public class VertexCache
    {
        public Vector3[] Vertices = null;

        public int Length { get { return Vertices.Length; } }
    }
}
