using UnityEngine;

namespace Cameras
{
    /// <summary>
    /// Helper class to copy transform and camera parameters.
    /// </summary>
    public class CameraSynchronizer : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        [Tooltip("The camera to copy values from.")]
        private UnityEngine.Camera sourceCamera = null;
        [SerializeField]
        [Tooltip("The camera to copy values to.")]
        private UnityEngine.Camera targetCamera = null;

        private Transform sourceTransform;
        private Transform targetTransform;

        // .. INITIALIZATION

        void Start()
        {
            if (sourceCamera == null)
                sourceCamera = UnityEngine.Camera.main;

            sourceTransform = sourceCamera.GetComponent<Transform>();
            targetTransform = targetCamera.GetComponent<Transform>();
        }

        // .. OPERATIONS

        void Update()
        {
            targetTransform.position = sourceTransform.position;
            targetTransform.rotation = sourceTransform.rotation;
            targetTransform.localScale = sourceTransform.localScale;

            targetCamera.fieldOfView = sourceCamera.fieldOfView;
        }


    }
}
