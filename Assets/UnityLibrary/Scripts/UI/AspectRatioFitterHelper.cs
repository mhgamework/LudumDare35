using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Sets the aspect ratio of the current image to the aspect ratio fitter component
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(AspectRatioFitter))]
    [ExecuteInEditMode]
    public class AspectRatioFitterHelper : MonoBehaviour
    {
        void Update()
        {
            var image_texture = GetComponent<Image>().mainTexture;
            if (image_texture == null)
                return;

            GetComponent<AspectRatioFitter>().aspectRatio = (float)image_texture.width / (float)image_texture.height;
        }
    }
}
