using UnityEngine;

namespace Miscellaneous
{
    /// <summary>
    /// Closes the application when the escape button is pressed.
    /// </summary>
    public class EscapeQuitter : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}
