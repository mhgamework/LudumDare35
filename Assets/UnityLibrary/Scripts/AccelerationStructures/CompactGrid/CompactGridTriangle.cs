using Miscellaneous;
using Miscellaneous.DataStructures;
using UnityEngine;

namespace AccelerationStructures.CompactGrid
{
    /// <summary>
    /// Representation of the objects a CompactGrid works on.
    /// Conceptually it coincides with Unity mesh triangles.
    /// </summary>
    public class CompactGridTriangle
    {
        // .. ATTRIBUTES

        public Vector3 P01 { get; private set; }
        public Vector3 P02 { get; private set; }
        public Vector3 P03 { get; private set; }

        public Vector3 Normal { get; private set; }


        private float a;
        private float b;
        private float c;
        private float d;
        private float e;
        private float f;

        private Bounds bounds;
        private Point3 sourceVertexIndices;


        // .. INITIALIZATION

        /// <summary>
        /// Order determines winding. Assumes clockwise order given.
        /// </summary>
        /// <param name="p01"></param>
        /// <param name="p02"></param>
        /// <param name="p03"></param>
        public CompactGridTriangle(Vector3 p01, Vector3 p02, Vector3 p03, int source_vertex_index_01, int source_vertex_index_02, int source_vertex_index_03)
        {
            SetPoints(p01, p02, p03, source_vertex_index_01, source_vertex_index_02, source_vertex_index_03);
        }

        // .. OPERATIONS

        /// <summary>
        /// Order determines winding. Assumes clockwise order given.
        /// </summary>
        /// <param name="p01"></param>
        /// <param name="p02"></param>
        /// <param name="p02"></param>
        public void SetPoints(Vector3 p01, Vector3 p02, Vector3 p03, int source_vertex_index_01, int source_vertex_index_02, int source_vertex_index_03)
        {
            P01 = p01;
            P02 = p02;
            P03 = p03;

            sourceVertexIndices = new Point3(source_vertex_index_01, source_vertex_index_02, source_vertex_index_03);

            Normal = Vector3.Cross(P02 - P01, P03 - P01).normalized;

            a = P01.x - P02.x;
            b = P01.y - P02.y;
            c = P01.z - P02.z;
            d = P01.x - P03.x;
            e = P01.y - P03.y;
            f = P01.z - P03.z;

            float min_x, min_y, min_z, max_x, max_y, max_z;
            min_x = Mathf.Min(Mathf.Min(P01.x, P02.x), P03.x);
            min_y = Mathf.Min(Mathf.Min(P01.y, P02.y), P03.y);
            min_z = Mathf.Min(Mathf.Min(P01.z, P02.z), P03.z);
            max_x = Mathf.Max(Mathf.Max(P01.x, P02.x), P03.x);
            max_y = Mathf.Max(Mathf.Max(P01.y, P02.y), P03.y);
            max_z = Mathf.Max(Mathf.Max(P01.z, P02.z), P03.z);

            bounds = new Bounds
            {
                min = new Vector3(min_x, min_y, min_z),
                max = new Vector3(max_x, max_y, max_z)
            };
        }

        public Bounds GetBounds()
        {
            return bounds;
        }

        /// <summary>
        /// Checkes whether this triangle is hit by given ray and stores the intersection distance in given RaycastResult.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="min_distance"></param>
        /// <param name="max_distance"></param>
        /// <param name="cull_mode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool IsHit(Ray ray, float min_distance, float max_distance, RaycastCullMode cull_mode, ref CompactGridRaycastResult result)
        {
            var direction = ray.direction;
            if (cull_mode == RaycastCullMode.BACK && Vector3.Dot(Normal, direction) > 0)
                return false;
            if (cull_mode == RaycastCullMode.FRONT && Vector3.Dot(Normal, direction) < 0)
                return false;

            var origin = ray.origin;
            if (!bounds.IntersectRay(ray) && !bounds.Contains(origin))
                return false;

            //i know the code below looks like mumbo-jumbo, just trust me that it is correct ;)

            float g = direction.x;
            float h = direction.y;
            float i = direction.z;
            float j = P01.x - origin.x;
            float k = P01.y - origin.y;
            float l = P01.z - origin.z;

            float beta, gamma, t; //beta, gamma: barycentric coordinates; t = distance

            float eiMinushf = e * i - h * f;
            float gfMinusdi = g * f - d * i;
            float dhMinuseg = d * h - e * g;
            float akMinusjb = a * k - j * b;
            float jcMinusal = j * c - a * l;
            float blMinuskc = b * l - k * c;
            float M = 1f / (a * eiMinushf + b * gfMinusdi + c * dhMinuseg);

            t = -(f * akMinusjb + e * jcMinusal + d * blMinuskc) * M;
            if (t < min_distance || t > max_distance)
                return false;

            gamma = (i * akMinusjb + h * jcMinusal + g * blMinuskc) * M;
            if (gamma < 0 || gamma > 1)
                return false;

            beta = (j * eiMinushf + k * gfMinusdi + l * dhMinuseg) * M;
            if (beta < 0 || beta > 1 - gamma)
                return false;

            result.Distance = t;
            result.SurfaceNormal = Normal;
            result.Barycentric_Beta = beta;
            result.Barycentric_Gamma = gamma;
            result.VertexIndices = sourceVertexIndices;

            return true;
        }

    }
}
