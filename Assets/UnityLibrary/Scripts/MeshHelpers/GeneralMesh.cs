using System;
using AccelerationStructures.CompactGrid;
using UnityEngine;

namespace MeshHelpers
{
    /// <summary>
    /// Abstracts common properties of MeshFilter and SkinnedMeshRenderer.
    /// </summary>
    public class GeneralMesh : MonoBehaviour
    {
        // .. TYPES

        private interface IMeshHelper
        {
            Mesh GetSharedMesh();
            void SetSharedMesh(Mesh mesh);
            Mesh GetMesh();
            void SetMesh(Mesh mesh);

            /// <summary>
            /// Retrieve a compact grid acceleration structure of the current mesh state (and with the current transform).
            /// </summary>
            /// <returns></returns>
            CompactGrid GetCompactGrid();
        }

        private class SkinnedMeshHelper : IMeshHelper
        {
            // .. ATTRIBUTES

            private SkinnedMeshRenderer SkinnedMesh;
            private Mesh OriginalSharedMesh;
            private bool compactGridUpToDate;
            private CompactGrid compactGrid;

            // .. INITIALIZATION

            public SkinnedMeshHelper(SkinnedMeshRenderer skinned_mesh)
            {
                SkinnedMesh = skinned_mesh;
                OriginalSharedMesh = SkinnedMesh.sharedMesh;
                SkinnedMesh.sharedMesh = Instantiate(SkinnedMesh.sharedMesh); // make sure the sharedmesh is a copy of the mesh resource (and not the resource itself)
            }

            // .. OPERATIONS

            public Mesh GetSharedMesh()
            {
                return OriginalSharedMesh;
            }

            public void SetSharedMesh(Mesh mesh)
            {
                OriginalSharedMesh = mesh;
            }

            public Mesh GetMesh()
            {
                return SkinnedMesh.sharedMesh; //(sharedmesh is instantiated)
            }

            public void SetMesh(Mesh mesh)
            {
                SkinnedMesh.sharedMesh = mesh; //(sharedmesh is instantiated)
                compactGridUpToDate = false;
            }

            /// <summary>
            /// Retrieve a compact grid acceleration structure of the current mesh state (and with the current transform).
            /// </summary>
            /// <returns></returns>
            public CompactGrid GetCompactGrid()
            {
                if (compactGridUpToDate)
                    return compactGrid;

                compactGrid = CompactGrid.BuildFromMesh(GetMesh(), SkinnedMesh.GetComponent<Transform>());
                compactGridUpToDate = true;
                return compactGrid;
            }

        }

        private class NormalMeshHelper : IMeshHelper
        {
            // .. ATTRIBUTES

            private MeshFilter MeshFilter;
            private bool compactGridUpToDate;
            private CompactGrid compactGrid;

            // .. INITIALIZATION

            public NormalMeshHelper(MeshFilter mesh_filter)
            {
                MeshFilter = mesh_filter;
            }

            // .. OPERATIONS

            public Mesh GetSharedMesh()
            {
                return MeshFilter.sharedMesh;
            }

            public void SetSharedMesh(Mesh mesh)
            {
                MeshFilter.sharedMesh = mesh;
            }

            public Mesh GetMesh()
            {
                return MeshFilter.mesh;
            }

            public void SetMesh(Mesh mesh)
            {
                MeshFilter.mesh = mesh;
                compactGridUpToDate = false;
            }

            /// <summary>
            /// Retrieve a compact grid acceleration structure of the current mesh state (and with the current transform).
            /// </summary>
            /// <returns></returns>
            public CompactGrid GetCompactGrid()
            {
                if (compactGridUpToDate)
                    return compactGrid;

                compactGrid = CompactGrid.BuildFromMesh(GetMesh(), MeshFilter.GetComponent<Transform>());
                compactGridUpToDate = true;
                return compactGrid;
            }
        }


        // .. ATTRIBUTES
        

        private IMeshHelper meshHelper;
        private bool isInitialized;


        // .. INITIALIZATION
        
        private void TryInitialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;

            var skinned_mesh = GetComponent<SkinnedMeshRenderer>();
            var mesh_filter = GetComponent<MeshFilter>();

            if (skinned_mesh == null && mesh_filter == null)
                throw new InvalidOperationException("No skinnedmeshrenderer or meshfilter present!");

            if (skinned_mesh != null)
                meshHelper = new SkinnedMeshHelper(skinned_mesh);
            else
                meshHelper = new NormalMeshHelper(mesh_filter);
        }

        // .. OPERATIONS

        public Mesh SharedMesh
        {
            get
            {
                TryInitialize();
                return meshHelper.GetSharedMesh();
            }
            set
            {
                TryInitialize();
                meshHelper.SetSharedMesh(value);
            }
        }

        public Mesh Mesh
        {
            get
            {
                TryInitialize();
                return meshHelper.GetMesh();
            }
            set
            {
                TryInitialize();
                meshHelper.SetMesh(value);
            }
        }

        /// <summary>
        /// Retrieve a compact grid acceleration structure of the current mesh state (and with the current transform).
        /// </summary>
        /// <returns></returns>
        public CompactGrid GetCompactGrid()
        {
            TryInitialize();
            return meshHelper.GetCompactGrid();
        }

    }
}
