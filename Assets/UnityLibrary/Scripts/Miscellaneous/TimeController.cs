using UnityEngine;

namespace Miscellaneous
{
    /// <summary>
    /// Slows down or speeds up the timescale.
    /// </summary>
    public class TimeController : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 2f)]
        private float TimeScale = 1f;

        void Update()
        {
            Time.timeScale = TimeScale;
        }
    }
}
