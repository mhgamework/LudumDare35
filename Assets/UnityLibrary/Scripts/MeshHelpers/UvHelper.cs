using System;
using System.Collections.Generic;
using System.Linq;
using AccelerationStructures.CompactGrid;
using ExtensionMethods;
using Miscellaneous;
using Shapes;
using UnityEngine;

namespace MeshHelpers
{
    /// <summary>
    /// A helper class for converting world to uv coordinates (and vice versa).
    /// Works on meshes with splitted vertices at uv seams (= standard unity imported mesh).
    /// Requires non-overlapping uv coordinates.
    /// Requires uv coordinates with minimal stretching (for uv offsetting to work correctly).
    /// </summary>
    public class UvHelper : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        [Tooltip("The mesh to operate on (optional, can be set from code).")]
        private GeneralMesh targetMesh = null;
        [SerializeField]
        [Tooltip("The uv scale of the target mesh (uv-to-worldspace scale.")]
        private float targetMeshUvScale = 1f;

        [Header("Debug")]
        [SerializeField]
        [Tooltip("An optional meshfilter to render the flattened uv-mesh.")]
        private MeshFilter OutputDebugMeshFilter = null;
        [SerializeField]
        private bool DrawDebugGizmos = false;


        private bool isInitialized;

        private Vector2 dummyVector2 = Vector2.zero;
        private Vector3 dummyVector3 = Vector3.zero;

        private GeneralMesh originalMesh;
        private Transform originalMeshTransform;
        private Vector3[] originalVertices;
        private Vector2[] originalUvs;
        private Vector3[] originalMeshNormals;

        private Mesh uvMesh;
        private float uvScale;

        private CompactGrid uvGrid;
        private CompactGrid originalMeshGrid;

        private CompactGridRaycastResult raycastResult = new CompactGridRaycastResult();

        private Line2D[] uvEdges = new Line2D[0]; //contains all uv seam edges that are not mesh border edges (so they all have a counterpart edge)
        private Dictionary<Line2D, Line2D> uvEdgeMap = new Dictionary<Line2D, Line2D>(); //maps every uv seam edge of the above list to its counterpart. Edges are oriented the same (key.P0 == value.P0, key.P1 == value.P1)

        // .. TYPES

        private struct Edge
        {
            public int VertexIndex_0;
            public int VertexIndex_1;
            public Vector3 Vertex_0;
            public Vector3 Vertex_1;
        }

        // .. INITIALIZATION

        void OnValidate()
        {
            if (Application.isPlaying && targetMesh != null)
            {
                Initialize(targetMesh, targetMeshUvScale);
            }
        }

        /// <summary>
        /// Initializes the uv helper with given mesh.
        /// GIVEN MESH MUST HAVE SPLITTED VERTICES AT UV SEAMS !!
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="uv_scale"></param>
        public void Initialize(GeneralMesh mesh, float uv_scale = 1f)
        {
            originalMesh = mesh;
            originalMeshTransform = originalMesh.GetComponent<Transform>();
            originalVertices = originalMesh.Mesh.vertices;
            originalUvs = originalMesh.Mesh.uv;
            originalMeshNormals = originalMesh.Mesh.normals;

            uvScale = uv_scale;
            uvMesh = new Mesh();
            CreateUvMesh(ref uvMesh, mesh.Mesh, uv_scale);

            originalMeshGrid = originalMesh.GetCompactGrid();
            uvGrid = CompactGrid.BuildFromMesh(uvMesh);

            isInitialized = true;

            InitializeSeamEdgesMap(originalVertices, originalMesh.Mesh.triangles, originalUvs);


            if (OutputDebugMeshFilter != null)
            {
                OutputDebugMeshFilter.mesh = uvMesh;
            }
        }

        private void InitializeSeamEdgesMap(Vector3[] vertices, int[] triangles, Vector2[] uvs)
        {
            //retrieve all edges
            var all_edges_set = new HashSet<Edge>();
            for (int i = 0; i < triangles.Length; i += 3)
            {
                //sort indices (so we will have same edge directions for triangles that share an edge)
                var indices = new List<int> { triangles[i], triangles[i + 1], triangles[i + 2] };
                var i0 = indices.Min();
                indices.Remove(i0);
                var i1 = indices.Min();
                indices.Remove(i1);
                var i2 = indices[0];

                var v0 = vertices[i0];
                var v1 = vertices[i1];
                var v2 = vertices[i2];

                all_edges_set.Add(new Edge { Vertex_0 = v0, Vertex_1 = v1, VertexIndex_0 = i0, VertexIndex_1 = i1 });
                all_edges_set.Add(new Edge { Vertex_0 = v0, Vertex_1 = v2, VertexIndex_0 = i0, VertexIndex_1 = i2 });
                all_edges_set.Add(new Edge { Vertex_0 = v1, Vertex_1 = v2, VertexIndex_0 = i1, VertexIndex_1 = i2 });
            }

            //filter the edges that are on a uv seam
            var all_edges_list = new List<Edge>();
            var set_to_list = all_edges_set.ToList();
            for (int i = 0; i < set_to_list.Count; i++)
            {
                var edge_i = set_to_list[i];
                if (IsUvSeam(uvs[edge_i.VertexIndex_0], uvs[edge_i.VertexIndex_1]))
                    all_edges_list.Add(edge_i);
            }

            var all_edges = all_edges_list.ToArray();

            //detect and store counterparts
            var uv_edges_list = new List<Line2D>();
            for (int i = 0; i < all_edges.Length; i++)
            {
                var edge_i = all_edges[i];
                for (int j = i + 1; j < all_edges.Length; j++)
                {
                    var edge_j = all_edges[j];
                    if (edge_i.Vertex_0 == edge_j.Vertex_0 && edge_i.Vertex_1 == edge_j.Vertex_1)
                    {
                        var line_a = new Line2D(uvs[edge_i.VertexIndex_0], uvs[edge_i.VertexIndex_1]);
                        var line_b = new Line2D(uvs[edge_j.VertexIndex_0], uvs[edge_j.VertexIndex_1]);
                        uv_edges_list.Add(line_a);
                        uv_edges_list.Add(line_b);
                        uvEdgeMap.Add(line_a, line_b);
                        uvEdgeMap.Add(line_b, line_a);
                    }
                    else if (edge_i.Vertex_0 == edge_j.Vertex_1 && edge_i.Vertex_1 == edge_j.Vertex_0)
                    {
                        var line_a = new Line2D(uvs[edge_i.VertexIndex_0], uvs[edge_i.VertexIndex_1]);
                        var line_b = new Line2D(uvs[edge_j.VertexIndex_1], uvs[edge_j.VertexIndex_0]);
                        uv_edges_list.Add(line_a);
                        uv_edges_list.Add(line_b);
                        uvEdgeMap.Add(line_a, line_b);
                        uvEdgeMap.Add(line_b, line_a);
                    }
                }
            }

            uvEdges = uv_edges_list.ToArray();
        }


        // .. OPERATIONS

        /// <summary>
        /// Transform a 3d raycast to a uv coordinate.
        /// Optionally returns the raycast-info.
        /// </summary>
        public bool UvFromRay(Ray ray, out Vector2 uv, CompactGridRaycastResult raycast_result = null)
        {
            if (!isInitialized)
            {
                uv = dummyVector2;
                return false;
            }

            var raycastresult = raycast_result != null ? raycast_result : raycastResult;
            if (!originalMeshGrid.Raycast(ray, ref raycastresult))
            {
                uv = dummyVector2;
                return false;
            }

            var hit_triangle_vertices = raycastresult.VertexIndices;
            var uv01 = originalUvs[hit_triangle_vertices[0]];
            var uv02 = originalUvs[hit_triangle_vertices[1]];
            var uv03 = originalUvs[hit_triangle_vertices[2]];

            uv = uv01 + raycastresult.Barycentric_Beta * (uv02 - uv01) + raycastresult.Barycentric_Gamma * (uv03 - uv01);
            return true;
        }

        /// <summary>
        /// Add an offset to a uv coordinate, taking uv seams into account.
        /// (if offset crosses uv seams, continues the offset on the bordering uv 'piece').
        /// ASSUMES GIVEN UV IS VALID (eg in range 0..1, on mesh).
        /// </summary>
        public bool AddToUv(Vector2 uv, Vector2 uv_offset, out Vector2 offsetted_uv)
        {
            if (!isInitialized)
            {
                offsetted_uv = dummyVector2;
                return false;
            }

            //check for uv seam edge intersections (retrieve closest intersection, if any)
            var offset_line = new Line2D(uv, uv + uv_offset);
            Line2D closest_intersection_edge = null;
            float closest_intersection_distance = float.MaxValue;
            var closest_intersect_point = new Vector2();

            for (int i = 0; i < uvEdges.Length; i++)
            {
                var uv_edge_i = uvEdges[i];

                Vector2 intersect_point_temp;
                if (!offset_line.Intersects(uv_edge_i, out intersect_point_temp, false)) //exclusive, because subsequent recursions start on counter uv seam of previous iteration.
                    continue;

                var distance = Vector2.Distance(uv, intersect_point_temp);
                if (distance < 0.000001f) //rounding error
                    continue;

                if (distance < closest_intersection_distance)
                {
                    closest_intersection_distance = distance;
                    closest_intersection_edge = uv_edge_i;
                    closest_intersect_point = intersect_point_temp;
                }
            }

            if (closest_intersection_edge == null) //no intersections
            {
                offsetted_uv = uv + uv_offset;
                return IsValidUv(offsetted_uv);
            }

            Line2D counterpart_edge;
            if (!uvEdgeMap.TryGetValue(closest_intersection_edge, out counterpart_edge)) //the edge has no counterpart, this case should not happen
            {
                throw new Exception("UvHelper: Huh?");
            }

            //calculate new uv start point and offset (starting on the point located on the counter edge)
            var closest_edge_vector = closest_intersection_edge.P1 - closest_intersection_edge.P0;
            var counter_edge_vector = counterpart_edge.P1 - counterpart_edge.P0;

            var outgoing_dir = uv_offset.Rotate(closest_edge_vector.Angle360(counter_edge_vector)).normalized;

            var factor = (closest_intersect_point - closest_intersection_edge.P0).magnitude / closest_edge_vector.magnitude * counter_edge_vector.magnitude;
            var outgoing_offset_point = factor * counter_edge_vector.normalized + counterpart_edge.P0;
            var outgoing_remaining_offset = outgoing_dir * (uv_offset.magnitude - closest_intersection_distance);

            return AddToUv(outgoing_offset_point, outgoing_remaining_offset, out offsetted_uv);
        }

        /// <summary>
        /// Transform a uv point to 3d world position.
        /// </summary>
        public bool UvToWorldPosition(Vector2 uv, out Vector3 world_position, float normal_offset = 0f)
        {
            Vector3 world_normal;
            return UvToWorldPosition(uv, out world_position, out world_normal, normal_offset);
        }

        /// <summary>
        /// Transform a uv point to 3d world position.
        /// </summary>
        public bool UvToWorldPosition(Vector2 uv, out Vector3 world_position, out Vector3 world_normal, float normal_offset = 0f)
        {
            if (!isInitialized)
            {
                world_position = dummyVector3;
                world_normal = dummyVector3;
                return false;
            }

            var ray = new Ray(new Vector3(uv.x, 1f, uv.y) * uvScale, Vector3.down);

            if (!uvGrid.Raycast(ray, ref raycastResult, RaycastCullMode.NONE, 0f, 1.5f))
            {
                world_position = dummyVector3;
                world_normal = dummyVector3;
                return false;
            }

            var hit_triangle_vertices = raycastResult.VertexIndices;
            var vertex01 = originalVertices[hit_triangle_vertices[0]];
            var vertex02 = originalVertices[hit_triangle_vertices[1]];
            var vertex03 = originalVertices[hit_triangle_vertices[2]];
            var interpolated_vertex_pos = raycastResult.Barycentric_Beta * (vertex02 - vertex01) + raycastResult.Barycentric_Gamma * (vertex03 - vertex01);

            var normal01 = originalMeshNormals[hit_triangle_vertices[0]];
            var normal02 = originalMeshNormals[hit_triangle_vertices[1]];
            var normal03 = originalMeshNormals[hit_triangle_vertices[2]];
            world_normal = (normal01 + normal02 + normal03) / 3f;

            var local_pos = vertex01 + interpolated_vertex_pos + world_normal * normal_offset;
            world_position = originalMeshTransform.TransformPoint(local_pos);
            return true;
        }

        /// <summary>
        /// Get the rotation of the uv tile containing given uv.
        /// </summary>
        /// <param name="uv"></param>
        /// <param name="rotation_degrees"></param>
        /// <returns></returns>
        public bool GetUvRotation(Vector2 uv, out float rotation_degrees)
        {
            //todo: might want to do some kind of cube mapping for near-flat (height diff == 0) surfaces

            rotation_degrees = 0f;

            Vector3 normal_world;
            Vector3 uv_world;
            if (!UvToWorldPosition(uv, out uv_world, out normal_world, 0.001f))
                return false;

            var uv_up = Vector2.up;
            Vector2 offsetted_uv;
            if (UvFromRay(new Ray(uv_world + new Vector3(0f, 0.0001f, 0f), -normal_world), out offsetted_uv))
            {
                uv_up = (offsetted_uv - uv).normalized;
            }
            else if (UvFromRay(new Ray(uv_world - new Vector3(0f, 0.0001f, 0f), -normal_world), out offsetted_uv))
            {
                uv_up = (uv - offsetted_uv).normalized;
            }
            else return false;

            rotation_degrees = Vector2.up.Angle360(uv_up);
            return true;

            /*
            Vector3 upper_point, lower_point;
            Vector2 offsetted_uv;
            if (AddToUv(uv, new Vector2(0, 0.00001f), out offsetted_uv))
            {
                UvToWorldPosition(uv, out lower_point);
                UvToWorldPosition(offsetted_uv, out upper_point);
            }
            else if (AddToUv(uv, new Vector2(0, -0.00001f), out offsetted_uv))
            {
                UvToWorldPosition(uv, out upper_point);
                UvToWorldPosition(offsetted_uv, out lower_point);
            }
            else
            {
                rotation_degrees = 0f;
                return false;
            }

            rotation_degrees = Vector3.Angle(Vector3.up, upper_point - lower_point);
            return true;*/
        }

        public static void CreateUvMesh(ref Mesh uv_mesh, Mesh original_mesh, float uv_scale)
        {
            var original_vertices = original_mesh.vertices;
            var original_triangles = original_mesh.triangles;
            var original_uvs = original_mesh.uv;

            var flattened_vertices = new Vector3[original_vertices.Length];

            for (int i = 0; i < original_vertices.Length; i++)
            {
                var uv_i = original_uvs[i];
                flattened_vertices[i] = new Vector3(uv_i.x, 0, uv_i.y) * uv_scale;
            }

            uv_mesh.Clear();

            uv_mesh.vertices = flattened_vertices;
            uv_mesh.triangles = original_triangles;

            uv_mesh.RecalculateNormals();
            uv_mesh.RecalculateBounds();
        }

        /// <summary>
        /// Does given uv lies within 0..1 range and does it lie on the mesh?
        /// </summary>
        private bool IsValidUv(Vector2 uv)
        {
            var ray = new Ray(new Vector3(uv.x, 1f, uv.y) * uvScale, Vector3.down);
            return uvGrid.Raycast(ray, ref raycastResult, RaycastCullMode.NONE, 0f, 1.5f);
        }

        /// <summary>
        /// Does given edge lies on an uv seam?
        /// </summary>
        private bool IsUvSeam(Vector2 uv_0, Vector2 uv_1)
        {
            var dir = (uv_0 - uv_1).normalized;
            var mid = (uv_0 + uv_1) * 0.5f;
            var perpendicular = new Vector2(-dir.y, dir.x);

            var left_point = mid + perpendicular * 0.001f * uvScale;
            var right_point = mid - perpendicular * 0.001f * uvScale;

            if (!IsValidUv(left_point))
                return true;

            return !IsValidUv(right_point);
        }


        void OnDrawGizmos()
        {
            if (!DrawDebugGizmos || !isInitialized)
                return;

            Gizmos.color = Color.green;
            for (int i = 0; i < uvEdges.Length; i++)
            {
                var edge_i = uvEdges[i];
                var p0 = edge_i.P0;
                var p1 = edge_i.P1;
                Gizmos.DrawLine(new Vector3(p0.x, 0.0025f, p0.y), new Vector3(p1.x, 0.0025f, p1.y));
            }
        }
    }
}
