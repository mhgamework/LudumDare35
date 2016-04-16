using MeshHelpers;
using UnityEngine;

namespace Miscellaneous
{
    public class VertexIndexToGizmo : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Color color = Color.red;
        [SerializeField]
        private float radius = 0.005f;
        [SerializeField]
        private GeneralMesh mesh = null;
        [SerializeField]
        private int vertexIndex = 0;

        // .. OPERATIONS

        void OnDrawGizmos()
        {
            if (!Application.isPlaying || mesh == null || vertexIndex < 0)
                return;

            var the_mesh = mesh.Mesh;
            if (the_mesh == null || the_mesh.vertexCount <= vertexIndex)
                return;

            Gizmos.color = color;
            Gizmos.DrawWireSphere(the_mesh.vertices[vertexIndex], radius);
        }

    }
}
