using AccelerationStructures.CompactGrid;
using MeshHelpers;
using UnityEngine;
using UnityEngine.Events;

namespace Miscellaneous
{
    /// <summary>
    /// Helper class for raycasting a mesh and placing an object at the hit position.
    /// </summary>
    public class ClickAndDragObjectPlacer : MonoBehaviour
    {
        [SerializeField]
        private Transform targetTransform = null;
        [SerializeField]
        private GeneralMesh targetMesh = null;

        [SerializeField]
        [Tooltip("Uses main camera if empty.")]
        private Camera theCamera = null;
        [SerializeField]
        private float offset = 0.1f;
        [SerializeField]
        private Vector3 eulerOffset = Vector3.zero;

        private CompactGrid targetGrid;
        private Transform targetMeshTransform;
        private CompactGridRaycastResult raycastResult;

        private bool isActive = true;

        // .. TYPES

        public UnityEvent ObjectPositionChanged;

        // .. INITIALIZATION

        void Start()
        {
            if (theCamera == null)
                theCamera = Camera.main;

            if (targetMesh != null)
                SetTargetMesh(targetMesh);

            raycastResult = new CompactGridRaycastResult();
        }

        // .. OPERATIONS

        public void SetTargetMesh(GeneralMesh mesh)
        {
            targetMesh = mesh;
            targetMeshTransform = targetMesh.GetComponent<Transform>();
            targetGrid = targetMesh.GetCompactGrid();
        }

        /// <summary>
        /// Should the objectplacer automatically update every frame?
        /// </summary>
        /// <param name="is_active"></param>
        public void SetAutoUpdate(bool is_active)
        {
            isActive = is_active;
        }

        void Update()
        {
            if (!isActive)
                return;

            if (Input.GetMouseButton(0))
            {
                var mouse_ray = theCamera.ScreenPointToRay(Input.mousePosition);
                PlaceObject(mouse_ray);
            }
        }

        /// <summary>
        /// Place the object by raycasting with given ray.
        /// Returns whether the ray hit the targetmesh.
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        public bool PlaceObject(Ray ray)
        {
            if (targetMesh == null)
                return false;

            if (!targetGrid.Raycast(ray, ref raycastResult) || raycastResult.SurfaceNormal.magnitude < 0.01f)
                return false;

            var hit_position = ray.origin + ray.direction * raycastResult.Distance;
            var hit_normal = targetMeshTransform.TransformDirection(raycastResult.SurfaceNormal);

            targetTransform.position = hit_position + offset * hit_normal;
            targetTransform.eulerAngles = Quaternion.LookRotation(hit_normal, Vector3.up).eulerAngles + eulerOffset;

            if (ObjectPositionChanged != null)
                ObjectPositionChanged.Invoke();

            return true;
        }

        public bool CanPlaceObject(Ray ray)
        {
            if (targetMesh == null)
                return false;

            if (!targetGrid.Raycast(ray, ref raycastResult) || raycastResult.SurfaceNormal.magnitude < 0.01f)
                return false;

            return true;
        }


    }
}
