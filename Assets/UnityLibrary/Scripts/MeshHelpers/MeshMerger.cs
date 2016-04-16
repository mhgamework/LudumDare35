using System.Collections.Generic;
using UnityEngine;

namespace MeshHelpers
{
    /// <summary>
    /// Provides functionality to merge multiple meshes together.
    /// </summary>
    public static class MeshMerger
    {
        // .. TYPES

        public class MeshData
        {
            public Vector3[] Vertices;
            public int[] Triangles;
            public Vector2[] Uv;
            public Vector3[] Normals;
            public Vector4[] Tangents;

            public static MeshData CreateFromMesh(Mesh m)
            {
                var m_data = new MeshData
                {
                    Vertices = m.vertices,
                    Triangles = m.triangles,
                    Uv = m.uv,
                    Normals = m.normals,
                    Tangents = m.tangents
                };

                return m_data;
            }
        }

        // .. ATTRIBUTES

        private static readonly MeshNormalMerger NormalMerger = new MeshNormalMerger();

        // .. OPERATIONS

        /// <summary>
        /// Merge given meshes into one mesh.
        /// </summary>
        /// <param name="merged_mesh"></param>
        /// <param name="meshes_to_merge"></param>
        /// <param name="interpolate_splitted_normals">Should normals of coinciding vertices be interpolated?</param>
        public static void MergeMeshes(ref Mesh merged_mesh, List<Mesh> meshes_to_merge, bool interpolate_splitted_normals = true)
        {
            var mesh_data_to_merge = new List<MeshData>();
            foreach (var mesh in meshes_to_merge)
            {
                mesh_data_to_merge.Add(MeshData.CreateFromMesh(mesh));
            }

            MergeMeshData(ref merged_mesh, mesh_data_to_merge, interpolate_splitted_normals);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merged_mesh"></param>
        /// <param name="data_to_merge"></param>
        /// <param name="interpolate_splitted_normals">Should normals of coinciding vertices be interpolated?</param>
        public static void MergeMeshData(ref Mesh merged_mesh, List<MeshData> data_to_merge, bool interpolate_splitted_normals = true)
        {
            merged_mesh.Clear();

            var merged_vertices = new List<Vector3>();
            var merged_triangles = new List<int>();
            var merged_uv = new List<Vector2>();
            var merged_normals = new List<Vector3>();
            var merged_tangents = new List<Vector4>();

            foreach (var mesh_data in data_to_merge)
            {
                var triangles = mesh_data.Triangles;
                OffsetIndices(triangles, merged_vertices.Count);
                merged_triangles.AddRange(triangles);

                merged_vertices.AddRange(mesh_data.Vertices);
                merged_uv.AddRange(mesh_data.Uv);
                merged_normals.AddRange(mesh_data.Normals);
                merged_tangents.AddRange(mesh_data.Tangents);
            }

            merged_mesh.SetVertices(merged_vertices);
            merged_mesh.SetUVs(0, merged_uv);
            merged_mesh.SetNormals(merged_normals);
            merged_mesh.SetTangents(merged_tangents);

            merged_mesh.SetTriangles(merged_triangles, 0);

            merged_mesh.RecalculateNormals();
            merged_mesh.RecalculateBounds();

            if (interpolate_splitted_normals)
            {
                NormalMerger.SetMesh(merged_mesh, merged_mesh);
                NormalMerger.RecalculateNormals();
            }
        }

        private static void OffsetIndices(int[] indices, int offset)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] += offset;
            }
        }

    }
}
