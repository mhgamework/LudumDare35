using UnityEngine;

namespace Miscellaneous
{
    /// <summary>
    /// Replaces the renderqueue of all materials of this renderer-component to a specified value.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class RenderQueueEditor : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private int RenderQueue = 4000;

        // .. INITIALIZATION

        void Start()
        {
            var Materials = GetComponent<Renderer>().materials;
            foreach (var m in Materials)
            {
                m.renderQueue = RenderQueue;
            }
        }
    }
}
