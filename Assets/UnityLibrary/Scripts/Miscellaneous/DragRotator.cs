using UnityEngine;

namespace Miscellaneous
{
    /// <summary>
    /// Provides functionality to control an object's rotation by clicking and dragging.
    /// </summary>
    public class DragRotator : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private KeyCode MouseButtonCode = KeyCode.Mouse0;
        [SerializeField]
        private bool UseActivationKey = false;
        [SerializeField]
        private KeyCode ActivationKey = KeyCode.LeftControl;

        [SerializeField]
        private float Sensitivity = 1f;

        private Transform ThisTransform;
        private Quaternion OriginalRotation;

        private float RotationY;

        // .. INITIALIZATION

        void Awake()
        {
            ThisTransform = GetComponent<Transform>();
            RotationY = ThisTransform.localRotation.y;
            OriginalRotation = transform.localRotation;
        }

        // .. OPERATIONS

        void Update()
        {
            if (UseActivationKey && !Input.GetKey(ActivationKey))
                return;

            if (!Input.GetKey(MouseButtonCode)) //left mouse button down
                return;

            float deltaAxisX = Input.GetAxis("Mouse X");
            RotationY -= deltaAxisX * Sensitivity;

            //Update camera rotation
            transform.localRotation = OriginalRotation * Quaternion.AngleAxis(RotationY, Vector3.up);
        }

    }
}
