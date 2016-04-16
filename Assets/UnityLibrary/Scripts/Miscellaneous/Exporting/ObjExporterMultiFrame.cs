using System.Collections;
using System.IO;
using UnityEngine;

namespace Miscellaneous.Exporting
{
    public class ObjExporterMultiFrame : MonoBehaviour
    {
#if UNITY_EDITOR
        public int FrameCount = 10;
        public int FramesPerSecond = 30;

        [Tooltip("The folder to export to.")]
        public string FolderPath = string.Empty;
        [Tooltip("The name of the file to export, without index or extension.")]
        public string FileName = string.Empty;


        public void DoExportMultiframe()
        {
            if (FrameCount <= 0)
                return;

            StartCoroutine("ExportMultiFrameRoutine", new object[] { FrameCount, FolderPath, FileName });
        }

        private IEnumerator ExportMultiFrameRoutine(object[] args)
        {
            int frame_count = (int)args[0];
            string folder_path = (string)args[1];
            string file_name = (string)args[2];

            float slow_down_factor = 0.05f;
            float seconds_per_frame = 1f / FramesPerSecond;

            Time.timeScale = slow_down_factor;

            var counter = 0;
            while (counter < frame_count)
            {
                yield return new WaitForSeconds(seconds_per_frame);
                counter++;

                var file_path = string.Format("{0}{1}{2}", Path.Combine(folder_path, file_name), counter.ToString("D3"), ".obj");
                ObjExporter.DoExport(true, file_path);
            }

            Time.timeScale = 1f;
        }
#endif
    }
}
