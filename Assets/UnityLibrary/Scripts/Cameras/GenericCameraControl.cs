using UnityEngine;

namespace Cameras
{
    /// <summary>
    /// A camera controller that controls the camera based on some paramaters.
    /// The control of these parameters is done by an external controller.
    /// </summary>
    public class GenericCameraControl : MonoBehaviour
    {
        // .. ATTRIBUTES

        [Header("Current Values")]
        public float Height;
        public float Zoom;
        public float RotationH;
        public float RotationV;

        [Header("Ranges")]
        [SerializeField]
        private float minRotationH = -360f;
        [SerializeField]
        private float maxRotationH = 360f;
        [SerializeField]
        private float minRotationV = -25f;
        [SerializeField]
        private float maxRotationV = 25f;
        [SerializeField]
        private float minZoom = 0.25f;
        [SerializeField]
        private float maxZoom = 10f;
        [SerializeField]
        private float minHeight = -2f;
        [SerializeField]
        private float maxHeight = 2f;

        #region Properties

        public float MinRotationH { get { return minRotationH; } }
        public float MaxRotationH { get { return maxRotationH; } }
        public float MinRotationV { get { return minRotationV; } }
        public float MaxRotationV { get { return maxRotationV; } }
        public float MinZoom { get { return minZoom; } }
        public float MaxZoom { get { return maxZoom; } }
        public float MinHeight { get { return minHeight; } }
        public float MaxHeight { get { return maxHeight; } }

        public Vector3 CameraTargetPosition { get { return cameraTargetTransform.position; } }
        public Vector3 CameraPosition { get { return cameraTransform.position; } }

        public Quaternion Rotation
        {
            get { return cameraTargetTransform.rotation; }
            set
            {
                RotationH = value.eulerAngles.x;
                RotationV = value.eulerAngles.y;
            }
        }

        #endregion

        [Header("Camera Rig")]
        [SerializeField]
        private Transform cameraTargetTransform = null;
        [SerializeField]
        private Transform cameraTransform = null;
        [SerializeField]
        private Camera theCamera = null;

        [SerializeField]
        private bool useFovForZoom = true;

        [SerializeField]
        private ACameraController controller = null;

        private float initialRadius;
        private float initialFov;

        // .. INITIALIZATION

        void Start()
        {
            initialRadius = cameraTransform.localPosition.z;
            initialFov = theCamera.fieldOfView;
            //initialRotation = cameraTargetTransform.localRotation;

            Height = cameraTargetTransform.position.y;
            Zoom = useFovForZoom ? theCamera.fieldOfView * initialRadius / initialFov : initialRadius;
            RotationH = cameraTargetTransform.localEulerAngles.y;
            RotationV = cameraTargetTransform.localEulerAngles.z;

            controller.SetTargetController(this);
            controller.Activate();
        }

        // .. OPERATIONS

        public void SetController(ACameraController camera_controller)
        {
            controller.Deactivate();
            controller = camera_controller;
            controller.SetTargetController(this);
            controller.Activate();
        }

        void Update()
        {
            RotationH = ClampAngle(RotationH, minRotationH, maxRotationH);
            RotationV = ClampAngle(RotationV, minRotationV, maxRotationV);
            Zoom = Mathf.Clamp(Zoom, minZoom, maxZoom);
            Height = Mathf.Clamp(Height, minHeight, maxHeight);

            //Update camera rotation
            cameraTargetTransform.localRotation = /*initialRotation **/ Quaternion.AngleAxis(RotationH, Vector3.up) * Quaternion.AngleAxis(RotationV, -Vector3.right);
            cameraTargetTransform.eulerAngles = new Vector3(cameraTargetTransform.eulerAngles.x, cameraTargetTransform.eulerAngles.y, 0f);

            //Update camera zoom
            var fov = useFovForZoom ? Zoom / initialRadius * initialFov : initialFov;
            var radius_to_set = useFovForZoom ? initialRadius : Zoom;

            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraTransform.localPosition.y, radius_to_set);
            theCamera.fieldOfView = fov;

            //Update height
            cameraTargetTransform.position = new Vector3(cameraTargetTransform.position.x, Height, cameraTargetTransform.position.z);
        }

        private float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360f;
            return Mathf.Clamp(angle, min, max);
        }

    }
}
