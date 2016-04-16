using UnityEngine;
using UnityEngine.EventSystems;

namespace Cameras.Old
{
    public class DragCam : MonoBehaviour
    {
        // .. ATTRIBUTES
        [SerializeField]
        private Transform CameraTransform = null;

        [SerializeField]
        private GameObject MouseCaptorGameObject = null;

        [SerializeField]
        private bool InvertX = false;
        [SerializeField]
        private bool InvertY = false;

        [SerializeField]
        private float sensitivityX = 15f;
        private float touchSensitivityX;

        [SerializeField]
        private float sensitivityY = 15f;
        private float touchSensitivityY;

        [SerializeField]
        private float touchSensitivityModifier = 0.1f;

        [SerializeField]
        private float heightPanSensitivity = 0.2f;
        private float touchHeightPanSensitivity;

        [SerializeField]
        private float zoomSensitivity = 2f;
        private float touchZoomSensitivity;

        [SerializeField]
        private float minimumX = -360f;
        [SerializeField]
        private float maximumX = 360f;
        [SerializeField]
        private float minimumY = -60f;
        [SerializeField]
        private float maximumY = 60f;
        [SerializeField]
        private float minimumZoom = 1f;
        [SerializeField]
        private float maximumZoom = 10f;
        [SerializeField]
        private float minimumHeight = 0.5f;
        [SerializeField]
        private float maximumHeight = 2.5f;

        [SerializeField]
        private string MouseWheelAxisName = "Mouse ScrollWheel";

        [SerializeField]
        private bool HoldControlToActivate = false;

        [SerializeField]
        private bool UseFovForZoom = true;
        [SerializeField]
        private float FixedRadius = 4f;

        private Transform ThisTransform;

        private float height;
        private float radius;
        private float rotationX = 0f;
        private float rotationY = 0f;

        private float deltaRotationX;
        private float deltaRotationY;
        private float deltaRadius;
        private float deltaHeight;

        private float axisXMultiplier = 1f;
        private float axisYMultiplier = 1f;

        private Quaternion originalRotation;
        private float OriginalFov;
        private UnityEngine.Camera TheCamera;

        private bool resetZoom = false;

        private float minHeightStart;
        private float maxHeightStart;



        // .. INITIALIZATION

        void Awake()
        {
            ThisTransform = GetComponent<Transform>();
        }

        void Start()
        {
            TheCamera = CameraTransform.GetComponent<UnityEngine.Camera>();
            OriginalFov = TheCamera.fieldOfView;

            SaveCurrentCameraParameters();

            touchSensitivityX = sensitivityX * touchSensitivityModifier;
            touchSensitivityY = sensitivityY * touchSensitivityModifier;
            touchZoomSensitivity = zoomSensitivity * 0.025f;
            touchHeightPanSensitivity = heightPanSensitivity * 0.025f;

            axisXMultiplier = InvertX ? -1f : 1f;
            axisYMultiplier = InvertY ? -1f : 1f;

            minHeightStart = minimumHeight;
            maxHeightStart = maximumHeight;
        }

        // .. OPERATIONS

        public void SetEnabledState(bool isEnabled)
        {
            if (isEnabled != enabled)
                SaveCurrentCameraParameters();

            enabled = isEnabled;
        }

        void SaveCurrentCameraParameters()
        {
            height = CameraTransform.localPosition.y;
            radius = CameraTransform.localPosition.z;
            originalRotation = ThisTransform.localRotation;
            rotationX = ThisTransform.localRotation.x;
            rotationY = ThisTransform.localRotation.y;
        }

        void Update()
        {
            if(!resetZoom)
            {
                if (HoldControlToActivate && !Input.GetKey(KeyCode.LeftControl))
                    return;

                if (!HoldControlToActivate && Input.GetKey(KeyCode.LeftControl))
                    return;

                if (EventSystem.current != null && MouseCaptorGameObject != null && EventSystem.current.currentSelectedGameObject != MouseCaptorGameObject && !Input.GetMouseButton(1))
                    return;
            }
            

            //if (EventSystem.current.IsPointerOverGameObject())
            //    Debug.Log(EventSystem.current.currentSelectedGameObject);

            deltaHeight = 0;
            deltaRadius = 0;
            deltaRotationX = 0;
            deltaRotationY = 0;

            if (Input.touchCount > 0)
                CalculateFromTouch();
            else
                calculateFromMouse();

            //deltaHeight = scaleByDistanceToClampRanges(height, minimumHeight, maximumHeight, deltaHeight) * Time.deltaTime * 100f;
            deltaRadius = scaleByDistanceToClampRanges(radius, minimumZoom, maximumZoom, deltaRadius) * Time.deltaTime * 100;
            deltaRotationX *= Time.deltaTime * 100;
            deltaRotationY *= Time.deltaTime * 100;

            rotationX = ClampAngle(rotationX + deltaRotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY + deltaRotationY, minimumY, maximumY);

            if (resetZoom == true)
            {
                radius = maximumZoom;
                height = 0f;
                resetZoom = false;
            }
            else
            {
                var factor = (maximumZoom - radius)/(maximumZoom - minimumZoom);
                minimumHeight = Mathf.Lerp(0f, minHeightStart, factor);
                maximumHeight = Mathf.Lerp(0f, maxHeightStart, factor);

                radius = Mathf.Clamp(radius + deltaRadius, minimumZoom, maximumZoom);
                height = Mathf.Clamp(height + deltaHeight, minimumHeight, maximumHeight);
            }


            //Update camera rotation
            ThisTransform.localRotation = originalRotation * Quaternion.AngleAxis(rotationX, Vector3.up) * Quaternion.AngleAxis(rotationY, -Vector3.right);
            ThisTransform.eulerAngles = new Vector3(ThisTransform.eulerAngles.x, ThisTransform.eulerAngles.y, 0f);

            //Update camera radius
            var localPos = CameraTransform.localPosition;

            var fov = UseFovForZoom ? radius / FixedRadius * OriginalFov : OriginalFov;
            var radius_to_set = UseFovForZoom ? FixedRadius : radius;

            CameraTransform.localPosition = new Vector3(localPos.x, height, radius_to_set);
            TheCamera.fieldOfView = fov;
        }

        private void CalculateFromTouch()
        {
            if (Input.touchCount > 1)
            {
                CalculateHeightFromTouch();
                CalculateZoomFromTouch();

                //if (!CalculateHeightFromTouch())
                //    CalculateZoomFromTouch();

                return;
            }

            float deltaAxisX = Input.touches[0].deltaPosition.x;
            float deltaAxisY = Input.touches[0].deltaPosition.y;

            deltaRotationX -= deltaAxisX * touchSensitivityX * axisXMultiplier;
            deltaRotationY -= deltaAxisY * touchSensitivityY * axisYMultiplier;
        }

        private bool CalculateZoomFromTouch()
        {
            var zoom_changed = false;

            if (Input.touchCount < 2)
                return zoom_changed;

            var touch_a = Input.touches[0];
            var touch_b = Input.touches[1];

            var current_pos_a = touch_a.position;
            var current_pos_b = touch_b.position;
            var prev_pos_a = current_pos_a - touch_a.deltaPosition;
            var prev_pos_b = current_pos_b - touch_b.deltaPosition;
            var prev_mean_pos = (prev_pos_a + prev_pos_b) * 0.5f;

            //calc diff
            var prev_dist = Vector3.Distance(prev_mean_pos, prev_pos_a) + Vector3.Distance(prev_mean_pos, prev_pos_b);
            var current_dist = Vector3.Distance(prev_mean_pos, current_pos_a) + Vector3.Distance(prev_mean_pos, current_pos_b);
            deltaRadius -= (current_dist - prev_dist) * touchZoomSensitivity;
            zoom_changed = true;

           

            return zoom_changed;
        }

        private bool CalculateHeightFromTouch()
        {
            if (Input.touchCount < 2)
                return false;

            var touch_a = Input.touches[0];
            var touch_b = Input.touches[1];

            var current_pos = (touch_a.position + touch_b.position) * 0.5f;
            var prev_pos = current_pos - (touch_a.deltaPosition + touch_b.deltaPosition) * 0.5f;
            var pos_change_y = current_pos.y - prev_pos.y;
            deltaHeight -= pos_change_y * touchHeightPanSensitivity;
            
            return true;
        }

        private void calculateFromMouse()
        {
            deltaRadius -= Input.GetAxis(MouseWheelAxisName);

            float deltaAxisX = Input.GetAxis("Mouse X");
            float deltaAxisY = Input.GetAxis("Mouse Y");

            if (Input.GetMouseButton(1)) //right mouse button down
            {
                deltaHeight -= deltaAxisY * heightPanSensitivity;
                return;
            }

            if (!Input.GetMouseButton(0)) //left mouse button down
                return;

            deltaRotationX -= deltaAxisX * sensitivityX * axisXMultiplier;
            deltaRotationY -= deltaAxisY * sensitivityY * axisYMultiplier;
        }

        private float scaleByDistanceToClampRanges(float current_value, float min_value, float max_value, float value_to_scale)
        {
            var min_dist = Mathf.Min(current_value - min_value, max_value - current_value);
            var total_dist = max_value - min_dist;

            var factor = Mathf.Clamp01(0.1f + min_dist / (0.5f * total_dist));
            return value_to_scale * factor;
        }

        // .. STATIC FUNCTIONS

        public static float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360f;
            return Mathf.Clamp(angle, min, max);
        }

        public void ResetZoom()
        {
            resetZoom = true;
        }


    }
}
