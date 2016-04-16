using System;
using AccelerationStructures;
using UnityEngine;

namespace MeshHelpers
{
    /// <summary>
    /// Maps vertex indices between 2 meshes based on closest vertices.
    /// Takes care of caching data structures.
    /// </summary>
    public class VertexIndexMapper
    {
        // .. ATTRIBUTES

        private int[] AToBMap;
        private int[] BToAMap;

        private Vector3[] VectorACache;
        private Vector3[] VectorBCache;

        private SimpleKDTree KdTree;


        // .. OPERATIONS

        public void Initialize(Vector3[] mesh_a_vertices, Vector3[] mesh_b_vertices)
        {
            //initialize data structures
            AToBMap = new int[mesh_a_vertices.Length];
            BToAMap = new int[mesh_b_vertices.Length];

            VectorACache = new Vector3[mesh_a_vertices.Length];
            VectorBCache = new Vector3[mesh_b_vertices.Length];

            //build index maps
            CalculateIndexMap(mesh_a_vertices, mesh_b_vertices, ref AToBMap);
            CalculateIndexMap(mesh_b_vertices, mesh_a_vertices, ref BToAMap);
        }

        /// <summary>
        /// Map an array of vectors indexed by vertex indices 'A' to an array that is index by vertex indices 'B'.
        /// </summary>
        /// <param name="a_vectors"></param>
        /// <returns></returns>
        public Vector3[] MapToB(Vector3[] a_vectors)
        {
            for (int i = 0; i < VectorBCache.Length; i++)
            {
                VectorBCache[i] = a_vectors[BToAMap[i]];
            }

            return VectorBCache;
        }

        /// <summary>
        /// Map an array of vectors indexed by vertex indices 'B' to an array that is index by vertex indices 'A'.
        /// </summary>
        /// <param name="b_vectors"></param>
        /// <returns></returns>
        public Vector3[] MapToA(Vector3[] b_vectors)
        {
            for (int i = 0; i < VectorACache.Length; i++)
            {
                VectorACache[i] = b_vectors[AToBMap[i]];
            }

            return VectorACache;
        }



        /// <summary>
        /// Stores for every vertex of vertices_01 the index of the closest vertex of vertices_02 in given map.
        /// (eg given map must have m01.vertexcount entries)
        /// </summary>
        private void CalculateIndexMap(Vector3[] vertices_01, Vector3[] vertices_02, ref int[] index_map)
        {
            if (index_map.Length != vertices_01.Length)
                throw new InvalidOperationException("Vertex count mismatch!");

            KdTree = SimpleKDTree.MakeFromPoints(vertices_02);
            for (int i = 0; i < vertices_01.Length; i++)
            {
                index_map[i] = KdTree.FindNearest(vertices_01[i]);
            }
        }


    }
}
