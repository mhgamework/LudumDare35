using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

namespace MeshHelpers
{
    /// <summary>
    /// Interpolates normals of coinciding vertices.
    /// </summary>
    public class MeshNormalMerger
    {
        // .. TYPES

        private class NormalInterpolator
        {
            // .. ATTRIBUTES

            public Vector3 VertexPosition { get; private set; }
            public Vector3 InterpolatedNormal { get; private set; }

            private int[] vertexIndices = new int[0];


            // .. INITIALIZATION

            public NormalInterpolator(Vector3 vertex_position)
            {
                VertexPosition = vertex_position;
            }

            // .. OPERATIONS

            public void AddIndex(int index)
            {
                ArrayHelper.Add(ref vertexIndices, index);
            }

            /// <summary>
            /// Recaclulate the normal.
            /// </summary>
            /// <param name="normals"></param>
            public void UpdateInterpolatedNormal(Vector3[] normals)
            {
                InterpolatedNormal = Vector3.zero;
                for (int i = 0; i < vertexIndices.Length; i++)
                {
                    InterpolatedNormal += normals[vertexIndices[i]];
                }
                InterpolatedNormal.Normalize();
            }
        }

        // .. ATTRIBUTES

        private Mesh targetMesh;

        private List<NormalInterpolator> interpolators = new List<NormalInterpolator>();
        private NormalInterpolator[] indexedInterpolators = new NormalInterpolator[0]; //indexed by mesh vertex indices

        // .. OPERATIONS

        /// <summary>
        /// Initialize this merger to work on given mesh.
        /// (should also be called when mesh topology changes)
        /// </summary>
        /// <param name="source_mesh">The mesh used to determine which vertices are splitted.</param>
        /// <param name="target_mesh">The mesh where merged normals are assigned to. Can be the same as source_mesh.</param>
        public void SetMesh(Mesh source_mesh, Mesh target_mesh)
        {
            if (source_mesh.vertexCount != target_mesh.vertexCount)
                throw new InvalidOperationException("Source mesh and target mesh have different vertex count!");

            interpolators.Clear();
            indexedInterpolators = new NormalInterpolator[source_mesh.vertexCount];

            targetMesh = target_mesh;
            var vertices = source_mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex_i = vertices[i];

                var interpolator = interpolators.FirstOrDefault(e => e.VertexPosition == vertex_i);
                if (interpolator == null)
                {
                    interpolator = new NormalInterpolator(vertex_i);
                    interpolators.Add(interpolator);
                }

                interpolator.AddIndex(i);
                indexedInterpolators[i] = interpolator;
            }
        }

        /// <summary>
        /// Recalculate the mesh normals, interpolating normals of splitted vertices.
        /// </summary>
        public void RecalculateNormals()
        {
            var normals = targetMesh.normals;

            foreach (var interpolator in interpolators)
            {
                interpolator.UpdateInterpolatedNormal(normals);
            }
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = indexedInterpolators[i].InterpolatedNormal;
            }

            targetMesh.normals = normals;
        }
    }
}
