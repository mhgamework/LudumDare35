using Miscellaneous;
using Miscellaneous.DataStructures;
using UnityEngine;

namespace AccelerationStructures.CompactGrid
{
    /// <summary>
    /// Acceleration structure for raytracing, by Ares Lagae & Philip Dutré
    /// http://people.cs.kuleuven.be/~ares.lagae/publications/LD08CFRGRT/LD08CFRGRT.pdf
    /// </summary>
    public class CompactGrid
    {
        private class Point3
        {
            public int x;
            public int y;
            public int z;

            public Point3(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        private const int DENSITY = 4; //values between 2 and 10 work best. Doesn't have too much impact on performance.
        private Bounds totalBox; //the bounding box containing the complete grid
        private Vector3 totalBoxMin;
        private Vector3 totalBoxMax;

        private int[] C; //List representing the cells of the grid. C[i] records the size of of the object list of the cell with 1D index i.
        private int[] L; //Concatenation of all object-lists. L[i] is the index of an object stored in the triangle-list.
        private CompactGridTriangle[] Triangles;

        private int N;  // # objects
        private int Mx; // # cells in x-direction
        private int My; // # cells in y-direction
        private int Mz; // # cells in z-direction

        private Vector3 cellSize;

        private TransformValue meshTransform = null;


        // .. INITIALIZATION

        public CompactGrid(CompactGridTriangle[] triangles, Transform transform = null)
        {
            meshTransform = transform == null ? null : new TransformValue(transform);
            Triangles = triangles;

            if (Triangles.Length == 0)
            {
                Debug.LogWarning("(CompactGrid) Given list contains no triangles!");
                return;
            }

            CalculateTotalBounds();
            BuildGrid();
        }

        public static CompactGrid BuildFromMesh(Mesh mesh, Transform mesh_transform = null)
        {
            var mesh_triangles = mesh.triangles;
            var mesh_verts = mesh.vertices;

            var triangles = new CompactGridTriangle[Mathf.FloorToInt(mesh_triangles.Length / 3f)];
            for (int i = 0; i < triangles.Length; i++)
            {
                var index_01 = mesh_triangles[i * 3];
                var index_02 = mesh_triangles[i * 3 + 1];
                var index_03 = mesh_triangles[i * 3 + 2];

                triangles[i] = new CompactGridTriangle(
                    mesh_verts[index_01],
                    mesh_verts[index_02],
                    mesh_verts[index_03],
                    index_01,
                    index_02,
                    index_03
                    );
            }

            return new CompactGrid(triangles, mesh_transform);
        }

        // .. OPERATIONS

        /// <summary>
        /// Raycasts the objects in this grid and stores the result of the closest intersection in given RaycastResult.
        /// Raycast-result only holds valid data when raycast was succesfull.
        /// </summary>
        /// <param name="ray">The world-space ray</param>
        /// <param name="result"></param>
        /// <param name="cull_mode"></param>
        /// <param name="min_distance"></param>
        /// <param name="max_distance"></param>
        /// <returns></returns>
        public bool Raycast(Ray ray, ref CompactGridRaycastResult result, RaycastCullMode cull_mode = RaycastCullMode.BACK, float min_distance = 0f, float max_distance = float.MaxValue)
        {
            //Grid-traversal algorithm from http://www.cse.yorku.ca/~amana/research/grid.pdf

            if (Triangles.Length == 0)
                return false;

            var local_ray = ToLocalRay(ray);

            float distance;
            if (!totalBox.IntersectRay(local_ray, out distance) || distance < min_distance || distance > max_distance)
            {
                if (!totalBox.Contains(local_ray.origin))
                    return false;
            }

            var start_position = local_ray.origin;
            if (!InsideGrid(start_position))
                start_position += distance * local_ray.direction;


            var raycast_direction = local_ray.direction;
            float dir_x = Mathf.Abs(raycast_direction.x) < float.Epsilon ? float.Epsilon : raycast_direction.x; //cheat to avoid rounding errors when direction components are very close to zero (see also ToGridCoordinate offset)
            float dir_y = Mathf.Abs(raycast_direction.y) < float.Epsilon ? float.Epsilon : raycast_direction.y;
            float dir_z = Mathf.Abs(raycast_direction.z) < float.Epsilon ? float.Epsilon : raycast_direction.z;
            raycast_direction = new Vector3(dir_x, dir_y, dir_z).normalized;
            local_ray.direction = raycast_direction;

            int step_x = 1;
            int step_y = 1;
            int step_z = 1;

            if (raycast_direction.x < 0)
                step_x = -1;
            if (raycast_direction.y < 0)
                step_y = -1;
            if (raycast_direction.z < 0)
                step_z = -1;

            Vector3 tMax = new Vector3(0, 0, 0);
            Vector3 current_position = start_position;

            Point3 current_cell = ToGridCoordinate(current_position);

            if (raycast_direction.x < 0.0)
                tMax.x = (WorldCoordinateX(current_cell.x) - current_position.x) / raycast_direction.x;
            if (raycast_direction.x > 0.0)
                tMax.x = (WorldCoordinateX((current_cell.x + 1)) - current_position.x) / raycast_direction.x;
            if (raycast_direction.y < 0.0)
                tMax.y = (WorldCoordinateY(current_cell.y) - current_position.y) / raycast_direction.y;
            if (raycast_direction.y > 0.0)
                tMax.y = (WorldCoordinateY((current_cell.y + 1)) - current_position.y) / raycast_direction.y;
            if (raycast_direction.z < 0.0)
                tMax.z = (WorldCoordinateZ(current_cell.z) - current_position.z) / raycast_direction.z;
            if (raycast_direction.z > 0.0)
                tMax.z = (WorldCoordinateZ((current_cell.z + 1)) - current_position.z) / raycast_direction.z;

            Vector3 tDelta = new Vector3(cellSize.x / Mathf.Abs(raycast_direction.x), cellSize.y / Mathf.Abs(raycast_direction.y), cellSize.z / Mathf.Abs(raycast_direction.z));

            while (InsideGrid(current_cell))
            {
                if (IsCellHit(local_ray, min_distance, max_distance, current_cell, cull_mode, ref result))
                {
                    return true; // automatically is closest hit
                }

                if (tMax.x < tMax.y)
                {
                    if (tMax.x < tMax.z)
                    {
                        current_cell.x += step_x;
                        tMax.x += tDelta.x;
                    }
                    else
                    {
                        current_cell.z += step_z;
                        tMax.z += tDelta.z;
                    }
                }
                else
                {
                    if (tMax.y < tMax.z)
                    {
                        current_cell.y += step_y;
                        tMax.y += tDelta.y;
                    }
                    else
                    {
                        current_cell.z += step_z;
                        tMax.z += tDelta.z;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Calculates the bounds of the full compactgrid. Assumes Triangles has been set.
        /// </summary>
        private void CalculateTotalBounds()
        {
            var triangle_0_bounds = Triangles[0].GetBounds();

            var min_x = triangle_0_bounds.min.x;
            var min_y = triangle_0_bounds.min.y;
            var min_z = triangle_0_bounds.min.z;
            var max_x = triangle_0_bounds.max.x;
            var max_y = triangle_0_bounds.max.y;
            var max_z = triangle_0_bounds.max.z;

            for (int i = 1; i < Triangles.Length; i++)
            {
                var bounds_i = Triangles[i].GetBounds();
                var min_i = bounds_i.min;
                var max_i = bounds_i.max;

                min_x = Mathf.Min(min_x, min_i.x);
                min_y = Mathf.Min(min_y, min_i.y);
                min_z = Mathf.Min(min_z, min_i.z);

                max_x = Mathf.Max(max_x, max_i.x);
                max_y = Mathf.Max(max_y, max_i.y);
                max_z = Mathf.Max(max_z, max_i.z);
            }

            totalBox.min = new Vector3(min_x - 0.0001f, min_y - 0.0001f, min_z - 0.0001f); //slightly increase totalbox size (to prevent border-issues)
            totalBox.max = new Vector3(max_x + 0.0001f, max_y + 0.0001f, max_z + 0.0001f);

            //cached for performance
            totalBoxMin = totalBox.min;
            totalBoxMax = totalBox.max;
        }

        /// <summary>
        /// Initializes the data structures of thecompactgrid. Assumes Triangles has been set. Assumes TotalBounds has been Calculated.
        /// </summary>
        private void BuildGrid()
        {
            N = Triangles.Length;

            //Calculate how many cells there will be, and their size
            var size_x = totalBoxMax.x - totalBoxMin.x;
            var size_y = totalBoxMax.y - totalBoxMin.y;
            var size_z = totalBoxMax.z - totalBoxMin.z;
            var coeff = Mathf.Pow(DENSITY * N / (size_x * size_y * size_z), 1f / 3f);

            Mx = (int)Mathf.Ceil(size_x * coeff);
            My = (int)Mathf.Ceil(size_y * coeff);
            Mz = (int)Mathf.Ceil(size_z * coeff);

            cellSize = new Vector3(size_x / Mx, size_y / My, size_z / Mz);
            C = new int[Mx * My * Mz + 1];


            //Store for every cell i the size of its object list in C[i]
            for (int i = 0; i < Triangles.Length; i++)
            {
                var triangle = Triangles[i];
                Point3 min = ToGridCoordinate(triangle.GetBounds().min);
                Point3 max = ToGridCoordinate(triangle.GetBounds().max);

                for (int x = min.x; x <= max.x; x++)
                {
                    for (int y = min.y; y <= max.y; y++)
                    {
                        for (int z = min.z; z <= max.z; z++)
                        {
                            C[ToCellIndex(x, y, z)]++;
                        }
                    }
                }
            }

            for (int i = 1; i < C.Length; i++)
            {
                C[i] += C[i - 1];
            }

            //Build the object list L
            L = new int[C[C.Length - 1]];
            for (int m = 0; m < L.Length; m++)
                L[m] = -1;

            for (int j = N - 1; j >= 0; j--)
            {
                var triangle = Triangles[j];
                Point3 min = ToGridCoordinate(triangle.GetBounds().min);
                Point3 max = ToGridCoordinate(triangle.GetBounds().max);

                for (int x = min.x; x <= max.x; x++)
                {
                    for (int y = min.y; y <= max.y; y++)
                    {
                        for (int z = min.z; z <= max.z; z++)
                        {
                            L[--C[ToCellIndex(x, y, z)]] = j;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns whether an object in the cell at given location is hit and if so, stores the closest RaycastResult in given result.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="min_distance"></param>
        /// <param name="max_distance"></param>
        /// <param name="result"></param>
        /// <param name="cell_coordinates"></param>
        /// <returns></returns>
        private bool IsCellHit(Ray ray, float min_distance, float max_distance, Point3 cell_coordinates, RaycastCullMode cull_mode, ref CompactGridRaycastResult result)
        {
            int cellIndex = ToCellIndex(cell_coordinates.x, cell_coordinates.y, cell_coordinates.z);

            result.Distance = float.PositiveInfinity;
            var temp_result = new CompactGridRaycastResult();

            for (int i = C[cellIndex]; i < C[cellIndex + 1]; i++)
            {
                var cObject = Triangles[L[i]];

                if (cObject.IsHit(ray, min_distance, max_distance, cull_mode, ref temp_result))
                {
                    if (temp_result.Distance > 0 && temp_result.Distance < result.Distance)
                    {
                        temp_result.CopyTo(result);
                    }
                }
            }

            if (result.Distance == float.PositiveInfinity)
                return false;

            Point3 hit_position = ToGridCoordinate(ray.origin + ray.direction * result.Distance);
            if (!(hit_position.x == cell_coordinates.x && hit_position.y == cell_coordinates.y && hit_position.z == cell_coordinates.z)) // Check closest intersection occurs in cell_coordinates
                return false;

            return true;
        }

        private Point3 ToGridCoordinate(Vector3 v)
        {
            const float offset = float.Epsilon; // offset so rounded cellcoords on the totalbox are in the grid

            var grid_min = totalBoxMin;
            var x = (v.x - grid_min.x) / cellSize.x - offset;
            var y = (v.y - grid_min.y) / cellSize.y - offset;
            var z = (v.z - grid_min.z) / cellSize.z - offset;

            var cell_x = (int)x;
            var cell_y = (int)y;
            var cell_z = (int)z;

            if (cell_x < 0)
                cell_x = 0;
            if (cell_y < 0)
                cell_y = 0;
            if (cell_z < 0)
                cell_z = 0;
            if (cell_x >= Mx)
                cell_x = Mx - 1;
            if (cell_y >= My)
                cell_y = My - 1;
            if (cell_z >= Mz)
                cell_z = Mz - 1;

            return new Point3(cell_x, cell_y, cell_z);
        }

        private int ToCellIndex(int x, int y, int z)
        {
            return (((My * z) + y) * Mx) + x;
        }

        private bool InsideGrid(Vector3 v)
        {
            return totalBox.Contains(v);
        }

        private bool InsideGrid(Point3 p)
        {
            return 0 <= p.x && p.x < Mx && 0 <= p.y && p.y < My && 0 <= p.z && p.z < Mz;
        }

        private float WorldCoordinateX(int x)
        {
            return x * cellSize.x + totalBoxMin.x;
        }

        private float WorldCoordinateY(int y)
        {
            return y * cellSize.y + totalBoxMin.y;
        }

        private float WorldCoordinateZ(int z)
        {
            return z * cellSize.z + totalBoxMin.z;
        }

        private Ray ToLocalRay(Ray world_ray)
        {
            if (meshTransform == null)
                return world_ray;

            var local_position = meshTransform.InverseTransformPoint(world_ray.origin);
            var local_direction = meshTransform.InverseTransformDirection(world_ray.direction);
            return new Ray(local_position, local_direction);
        }

    }
}
