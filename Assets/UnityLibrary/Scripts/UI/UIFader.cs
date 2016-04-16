using System.Collections;
using Miscellaneous.Easing;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Provides functionality for fading in/out a ui element (including all of its children).
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFader : MonoBehaviour
    {
        // .. ATTRIBUTES

        private CanvasGroup canvasGroup;

        // .. INITIALIZATION

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // .. OPERATIONS

        public void Fade(float target_alpha, float time, EasingFunctions.TYPE easing_type = EasingFunctions.TYPE.Regular, bool block_raycast = true)
        {
            StopCoroutine("UpdateFade");

            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.blocksRaycasts = block_raycast;

            if (!gameObject.activeInHierarchy)
            {
                canvasGroup.alpha = target_alpha;
                return;
            }

            object[] args = new object[] { target_alpha, time, easing_type };
            StartCoroutine("UpdateFade", args);
        }

        // .. COROUTINES

        IEnumerator UpdateFade(object[] args)
        {
            float target_alpha = Mathf.Clamp01((float)args[0]);
            float animation_time = (float)args[1];
            EasingFunctions.TYPE easing_type = (EasingFunctions.TYPE)args[2];

            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            float start_alpha = canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < animation_time)
            {
                canvasGroup.alpha = EasingFunctions.Ease(easing_type, elapsed / animation_time, start_alpha, target_alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = target_alpha;

            canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
            canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
        }


    }
}
