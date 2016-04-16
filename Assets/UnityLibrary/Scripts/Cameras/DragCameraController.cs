using UnityEngine;
using UnityEngine.EventSystems;

namespace Cameras
{
    public class DragCameraController : ACameraController
    {
        // .. ATTRIBUTES

        [Header("Control")]
        [SerializeField]
        private string MouseWheelAxisName = "Mouse ScrollWheel";
        [SerializeField]
        private bool HoldControlToActivate = false;
        [SerializeField]
        private GameObject MouseCaptorGameObject = null;
        [SerializeField]
        private bool InvertX = false;
        [SerializeField]
        private bool InvertY = false;

        [Header("Sensitivity")]
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

        private GenericCameraControl cameraController;
        private bool isActive;

        private float deltaRotationX;
        private float deltaRotationY;
        private float deltaRadius;
        private float deltaHeight;

        private float axisXMultiplier = 1f;
        private float axisYMultiplier = 1f;

        // .. INITIALIZATION

        void Start()
        {
            touchSensitivityX = sensitivityX * touchSensitivityModifier;
            touchSensitivityY = sensitivityY * touchSensitivityModifier;
            touchZoomSensitivity = zoomSensitivity * 0.025f;
            touchHeightPanSensitivity = heightPanSensitivity * 0.025f;

            axisXMultiplier = InvertX ? -1f : 1f;
            axisYMultiplier = InvertY ? -1f : 1f;
        }

        // .. OPERATIONS

        public override void SetTargetController(GenericCameraControl controller)
        {
            cameraController = controller;
        }

        public override void Activate()
        {
            isActive = true;
        }

        public override void Deactivate()
        {
            isActive = false;
        }

        void Update()
        {
            if (!isActive)
                return;
            if ((HoldControlToActivate && !Input.GetKey(KeyCode.LeftControl)) || (!HoldControlToActivate && Input.GetKey(KeyCode.LeftControl)))
                return;
            if (EventSystem.current != null && MouseCaptorGameObject != null && EventSystem.current.currentSelectedGameObject != MouseCaptorGameObject && !Input.GetMouseButton(1))
                return;

            deltaHeight = 0;
            deltaRadius = 0;
            deltaRotationX = 0;
            deltaRotationY = 0;

            if (Input.touchCount > 0)
                CalculateFromTouch();
            else
                CalculateFromMouse();

            deltaHeight = ScaleByDistanceToClampRanges(cameraController.Height, cameraController.MinHeight, cameraController.MaxHeight, deltaHeight) * Time.deltaTime * 100f;
            deltaRadius = ScaleByDistanceToClampRanges(cameraController.Zoom, cameraController.MinZoom, cameraController.MaxZoom, deltaRadius) * Time.deltaTime * 100;
            deltaRotationX *= Time.deltaTime * 100;
            deltaRotationY *= Time.deltaTime * 100;

            cameraController.RotationH += deltaRotationX;
            cameraController.RotationV += deltaRotationY;
            cameraController.Zoom += deltaRadius;
            cameraController.Height += deltaHeight;
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

            /*
            var xDeltaDiff = touch_b.deltaPosition.x - touch_a.deltaPosition.x;
            if (Mathf.Abs(xDeltaDiff) > 0)
            {
                var xDiff = touch_b.position.x - touch_a.position.x;
                deltaRadius += xDiff < 0
                    ? xDeltaDiff * touchZoomSensitivity
                    : -xDeltaDiff * touchZoomSensitivity;

                zoom_changed = true;
            }

            var yDeltaDiff = touch_b.deltaPosition.y - touch_a.deltaPosition.y;
            if (Mathf.Abs(yDeltaDiff) > 0)
            {
                var yDiff = touch_b.position.y - touch_a.position.y;
                deltaRadius += yDiff < 0
                    ? yDeltaDiff * touchZoomSensitivity
                    : -yDeltaDiff * touchZoomSensitivity;

                zoom_changed = true;
            }*/

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
            /*
            var touch_a = Input.touches[0];
            var touch_b = Input.touches[1];

            var a_yDiff = touch_a.deltaPosition.y;
            var b_yDiff = touch_b.deltaPosition.y;

            if (a_yDiff * b_yDiff <= 0)
                return false; //diffs must be in same direction (up or down)

            deltaHeight -= Math.Max(a_yDiff, b_yDiff) * touchHeightPanSensitivity;*/
            return true;
        }

        private void CalculateFromMouse()
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

        private float ScaleByDistanceToClampRanges(float current_value, float min_value, float max_value, float value_to_scale)
        {
            var min_dist = Mathf.Min(current_value - min_value, max_value - current_value);
            var total_dist = max_value - min_dist;

            var factor = Mathf.Clamp01(0.1f + min_dist / (0.5f * total_dist));
            return value_to_scale * factor;
        }

        private float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360f;
            return Mathf.Clamp(angle, min, max);
        }

    }
}
