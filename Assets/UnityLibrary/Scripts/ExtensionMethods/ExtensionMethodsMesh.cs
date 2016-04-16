using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsMesh
    {
        /// <summary>
        /// Copies the data from this mesh to another mesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="copy"></param>
        public static void CopyTo(this Mesh mesh, ref Mesh copy, bool merge_in_one_submesh = false)
        {
            copy.Clear();

            copy.vertices = mesh.vertices;

            if (merge_in_one_submesh)
            {
                var all_triangles = new List<int>();
                for (int i = 0; i < mesh.subMeshCount; i++)
                {
                    all_triangles.AddRange(mesh.GetTriangles(i));
                }
                copy.SetTriangles(all_triangles, 0);
            }
            else
            {
                copy.subMeshCount = mesh.subMeshCount;
                for (int i = 0; i < mesh.subMeshCount; i++)
                {
                    copy.SetTriangles(mesh.GetTriangles(i), i);
                }
            }


            copy.uv = mesh.uv;
            copy.uv2 = mesh.uv2;
            copy.uv3 = mesh.uv3;
            copy.uv4 = mesh.uv4;

            copy.normals = mesh.normals;
            copy.tangents = mesh.tangents;

            copy.bindposes = mesh.bindposes;
            copy.boneWeights = mesh.boneWeights;

            copy.bounds = mesh.bounds;
            copy.colors = mesh.colors;
        }

        /// <summary>
        /// Returns a list containing all vertices of the specified submesh once, in the order of 'GetSubmeshVertexIndices'.
        /// !! Very slow if no submesh_vertex_indices array has been supplied !!
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="submesh_index"></param>
        /// <returns></returns>
        public static Vector3[] GetSubmeshVertices(this Mesh mesh, int submesh_index, int[] submesh_vertex_indices = null)
        {
            var all_verts = mesh.vertices;

            if (submesh_vertex_indices == null)
                submesh_vertex_indices = mesh.GetSubmeshVertexIndices(submesh_index);

            var submesh_verts = new Vector3[submesh_vertex_indices.Length];
            for (int i = 0; i < submesh_vertex_indices.Length; i++)
            {
                submesh_verts[i] = all_verts[submesh_vertex_indices[i]];
            }

            return submesh_verts;
        }

        /// <summary>
        /// Returns a list containing all normals of the specified submesh once, in the order of 'GetSubmeshVertexIndices'.
        /// !! Very slow if no submesh_vertex_indices array has been supplied !!
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="submesh_index"></param>
        /// <returns></returns>
        public static Vector3[] GetSubmeshNormals(this Mesh mesh, int submesh_index, int[] submesh_vertex_indices = null)
        {
            var all_normals = mesh.normals;

            if (submesh_vertex_indices == null)
                submesh_vertex_indices = mesh.GetSubmeshVertexIndices(submesh_index);

            var submesh_normals = new Vector3[submesh_vertex_indices.Length];
            for (int i = 0; i < submesh_vertex_indices.Length; i++)
            {
                submesh_normals[i] = all_normals[submesh_vertex_indices[i]];
            }

            return submesh_normals;
        }

        /// <summary>
        /// Returns a list containing all uvs of the specified submesh once, in the order of 'GetSubmeshVertexIndices'.
        /// !! Very slow if no submesh_vertex_indices array has been supplied !!
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="submesh_index"></param>
        /// <returns></returns>
        public static Vector2[] GetSubmeshUvs(this Mesh mesh, int submesh_index, int[] submesh_vertex_indices = null)
        {
            var all_uvs = mesh.uv;

            if (submesh_vertex_indices == null)
                submesh_vertex_indices = mesh.GetSubmeshVertexIndices(submesh_index);

            var submesh_uvs = new Vector2[submesh_vertex_indices.Length];
            for (int i = 0; i < submesh_vertex_indices.Length; i++)
            {
                submesh_uvs[i] = all_uvs[submesh_vertex_indices[i]];
            }

            return submesh_uvs;
        }

        /// <summary>
        /// !! Very slow, cache if possible !!
        /// Returns a list of containing all the vertex indices of the specified submesh ONCE, in increasing order.
        /// (note that this is very different from Mesh.GetIndices(int submesh_index), whih returns a triangle list)
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="submesh_index"></param>
        /// <returns></returns>
        public static int[] GetSubmeshVertexIndices(this Mesh mesh, int submesh_index)
        {
            var submesh_triangle_list = mesh.GetIndices(submesh_index);

            var index_set = new HashSet<int>();
            for (int i = 0; i < submesh_triangle_list.Length; i++)
            {
                index_set.Add(submesh_triangle_list[i]);
            }

            return index_set.OrderBy(e => e).ToArray();
        }
    }
}
