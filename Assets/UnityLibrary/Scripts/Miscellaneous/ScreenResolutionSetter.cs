using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Miscellaneous
{
    /// <summary>
    /// Alters the screen resolution at runtime.
    /// </summary>
    public class ScreenResolutionSetter : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        [Tooltip("Values are rounded down to closest integer.")]
        private Vector2 DesiredResolution = new Vector2(1920, 1080);
        [SerializeField]
        private bool IsFullScreen = true;
        [SerializeField]
        private bool SwitchResolutionAtStart = true;

        // .. EVENTS

        [Serializable]
        public class ResolutionChangedEventHandler : UnityEvent { }
        public ResolutionChangedEventHandler OnResolutionChanged;

        // .. INITIALIZATION

        void Start()
        {
            if (SwitchResolutionAtStart)
                StartCoroutine("SwitchResolution", new object[] { DesiredResolution, IsFullScreen });
        }

        // .. OPERATIONS

        public void SwitchResolution(Vector2 desired_resolution, bool is_full_screen = true)
        {
            DesiredResolution = desired_resolution;
            IsFullScreen = is_full_screen;

            StartCoroutine("SwitchResolution", new object[] { DesiredResolution, IsFullScreen });
        }

        // .. COROUTINES

        private IEnumerator SwitchResolution(object[] args)
        {
            Vector2 new_resolution = (Vector2)args[0];
            bool fullscreen = (bool)args[1];
            Screen.SetResolution(Mathf.FloorToInt(new_resolution.x), Mathf.FloorToInt(new_resolution.y), fullscreen);

            yield return null;

            if (OnResolutionChanged != null)
                OnResolutionChanged.Invoke();
        }

    }
}
