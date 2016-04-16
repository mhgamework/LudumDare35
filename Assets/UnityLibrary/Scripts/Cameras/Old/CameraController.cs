using UI;
using UnityEngine;

namespace Cameras.Old
{
    /// <summary>
    /// Responsible for switching between DragCam-style and FancyCam-style camera control.
    /// </summary>
    [RequireComponent(typeof(DragCam))]
    [RequireComponent(typeof(FancyCam))]
    public class CameraController : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private float InputTimeout = 5f;
        [SerializeField]
        [Tooltip("An optional input captor object. If none specified, any input will be registered.")]
        private InputCaptor InputCaptor = null;

        private DragCam DragCameraController;
        private FancyCam FancyCameraController;
        private float currentInputTimeout;


        // .. INITIALIZATION

        void Start()
        {
            DragCameraController = GetComponent<DragCam>();
            FancyCameraController = GetComponent<FancyCam>();

            DragCameraController.SetEnabledState(false);
            FancyCameraController.SetEnabledState(false);
        }

        // .. OPERATIONS

        void Update()
        {
            UpdateInputTimeout();

            if (currentInputTimeout >= InputTimeout)
            {
                DragCameraController.SetEnabledState(false);
                FancyCameraController.SetEnabledState(true);
            }
            else
            {
                DragCameraController.SetEnabledState(true);
                FancyCameraController.SetEnabledState(false);
            }
        }

        private void UpdateInputTimeout()
        {
            if (InputCaptor == null)
            {
                if (Input.touchCount == 0 && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
                    currentInputTimeout += Time.deltaTime;
                else
                    currentInputTimeout = 0f;
            }
            else
            {
                if (!InputCaptor.HasFocus())
                    currentInputTimeout += Time.deltaTime;
                else if (Input.touchCount == 0 && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
                    currentInputTimeout += Time.deltaTime;
                else
                    currentInputTimeout = 0f;
            }
        }

    }
}
