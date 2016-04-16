using UnityEngine;

namespace UI
{
    /// <summary>
    /// Tracks a 3d object in screen space.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class WorldObjectScreenAnchor : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        [Tooltip("The transform of the object to track.")]
        private Transform targetObjectTransform = null;

        [Header("Optional")]
        [SerializeField]
        [Tooltip("The camera that renders this object.")]
        private Camera mainCamera = null;

        private RectTransform thisTransform;

        // .. INITIALIZATION

        void Start()
        {
            thisTransform = GetComponent<RectTransform>();
        }

        // .. OPERATIONS

        public void SetTrackedTransform(Transform target_object_transform, Camera camera = null)
        {
            targetObjectTransform = target_object_transform;
            mainCamera = camera;
        }

        public Transform GetTrackedTransform()
        {
            return targetObjectTransform;
        }

        void Update()
        {
            if (targetObjectTransform == null)
                return;

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }


            thisTransform.position = mainCamera.WorldToScreenPoint(targetObjectTransform.position);
        }
    }
}
