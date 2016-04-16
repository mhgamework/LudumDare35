using UnityEngine;

namespace Cameras
{
    public abstract class ACameraController : MonoBehaviour
    {
        // .. OPERATIONS

        public abstract void SetTargetController(GenericCameraControl controller);
        public abstract void Activate();
        public abstract void Deactivate();
    }
}
