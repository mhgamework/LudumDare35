using System.IO;
using UnityEngine;

namespace Miscellaneous
{
    /// <summary>
    /// Helper clas to take screenshots and export them to png.
    /// </summary>
    public class ScreenshotHelper : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private string ScreenshotFolder = string.Empty;
        [SerializeField]
        private string ScreenShotName = "screenshot";
        [SerializeField]
        private int Width = 3840;
        [SerializeField]
        private int Height = 2160;

        [SerializeField]
        [Tooltip("The camera to take a screenshot with.")]
        private UnityEngine.Camera ScreenshotCamera = null;
        

        private RenderTexture renderTarget;

        // .. INITIALIZATION

        void Start()
        {
            renderTarget = new RenderTexture(Width, Height, 0, RenderTextureFormat.ARGB32);
        }

        // .. OPERATIONS

        /// <summary>
        /// Take a new screnshot.
        /// </summary>
        public void TakeScreenshot()
        {
            var original_target = ScreenshotCamera.targetTexture;
            var original_active = RenderTexture.active;

            ScreenshotCamera.targetTexture = renderTarget;
            ScreenshotCamera.Render();
            RenderTexture.active = renderTarget;

            var screenshot = new Texture2D(Width, Height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
            screenshot.Apply(false);

            RenderTexture.active = original_active;
            ScreenshotCamera.targetTexture = original_target;

            var bytes = screenshot.EncodeToPNG();
            var file_path = GetNewFilePath();

            File.WriteAllBytes(file_path, bytes);

            Debug.Log("Saved screenshot to " + file_path);
        }

        private string GetNewFilePath()
        {
            var index = 0;

            while (true)
            {
                var file_path = string.Format("{0}{1}{2}", Path.Combine(ScreenshotFolder, ScreenShotName), index.ToString("D3"), ".png");
                if (!File.Exists(file_path))
                    return file_path;

                index++;
            }
        }


    }
}
