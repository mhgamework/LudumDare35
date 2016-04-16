using UnityEngine;

namespace Cameras
{
    [ExecuteInEditMode]
    public class PostEffect : MonoBehaviour
    {
        [SerializeField]
        private Shader shader = null;

        private Material material;

        // .. INITIALIZATION

        void Awake()
        {
            if (shader == null)
                return;

            material = new Material(shader);
        }

        // .. OPERATIONS

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (material == null)
                return;

            // Postprocess the image
            Graphics.Blit(source, destination, material);
        }

    }
}
