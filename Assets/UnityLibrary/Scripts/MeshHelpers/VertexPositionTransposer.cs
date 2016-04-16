using System;
using UnityEngine;
using UnityEngine.Events;

namespace MeshHelpers
{
    /// <summary>
    /// Workaround for unity vertex splitting shit.
    /// Transposes vertex position from a source mesh (supplied meshfilter) to a target mesh (this component). The mapping is done based on closest vertex. 
    /// </summary>
    [RequireComponent(typeof(GeneralMesh))]
    public class VertexPositionTransposer : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private bool DrawDebug = false;

        private VertexCache sourceVertices = null;
        private GeneralMesh targetMeshComponent;
        private Mesh targetMesh;
        private VertexIndexMapper vertexMapper;
        private bool isInitialized;

#if UNITY_EDITOR
        private Vector3[] debugPositions = new Vector3[0];
#endif


        // .. EVENTS

        [Serializable]
        public class VerticesUpdatedEventHandler : UnityEvent { }
        public VerticesUpdatedEventHandler OnVerticesUpdated;

        // .. OPERATIONS

        public void NotifySourceMeshUpdated()
        {
            if (!isInitialized)
                return;

            UpdateFromSourceMesh();
        }

        /// <summary>
        /// Sets the source vertices and calculates vertex mappings based on closest positions.
        /// Source vertices initial positions must be set!
        /// </summary>
        /// <param name="source_vertices"></param>
        public void SetSourceVertices(VertexCache source_vertices)
        {
            sourceVertices = source_vertices;
            InitializeVertexMap();
        }


        private void InitializeVertexMap()
        {
            targetMeshComponent = GetComponent<GeneralMesh>();

            targetMesh = targetMeshComponent.Mesh;
            var target_mesh_vertices = targetMesh.vertices;
            var source_mesh_vertices = sourceVertices.Vertices;

#if UNITY_EDITOR
            debugPositions = target_mesh_vertices;
#endif

            vertexMapper = new VertexIndexMapper();
            vertexMapper.Initialize(source_mesh_vertices, target_mesh_vertices);

            isInitialized = true;
        }

        private void UpdateFromSourceMesh()
        {
            targetMesh.vertices = vertexMapper.MapToB(sourceVertices.Vertices);

            if (OnVerticesUpdated != null)
                OnVerticesUpdated.Invoke();
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!DrawDebug || sourceVertices == null)
                return;

            Gizmos.color = Color.green;
            for (int i = 0; i < sourceVertices.Vertices.Length; i++)
            {
                Gizmos.DrawSphere(sourceVertices.Vertices[i], 0.01f);
            }
            /*
        Gizmos.color = Color.red;
        for (int i = 0; i < VertexCache.Length; i++)
        {
            Gizmos.DrawSphere(VertexCache[i], 0.01f);
        }
        */
            Gizmos.color = Color.cyan;
            for (int i = 0; i < debugPositions.Length; i++)
            {
                Gizmos.DrawSphere(debugPositions[i], 0.01f);
            }
        }
#endif


    }
}
