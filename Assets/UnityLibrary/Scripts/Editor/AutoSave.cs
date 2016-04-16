using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.UnityLibrary.Scripts.Editor
{
    [InitializeOnLoad]
    public class AutoSave
    {
        static AutoSave()
        {
            EditorApplication.playmodeStateChanged = () =>
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                {
                    Debug.Log("Auto-Saving scene before entering Play mode: " + EditorSceneManager.GetActiveScene().name);

                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    EditorApplication.SaveAssets();
                }
            };
        }
    }
}