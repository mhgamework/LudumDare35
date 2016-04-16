using System.Collections;
using Miscellaneous.Easing;
using UnityEngine;

namespace UI.GuiElements
{
    [RequireComponent(typeof(UIFader))]
    public class FadePanel : AGuiElement
    {
        // .. ATTRIBUTES

        [SerializeField]
        private bool VisibleOnStart = true;
        [SerializeField]
        private EasingFunctions.TYPE FadeInEasingType = EasingFunctions.TYPE.Regular;
        [SerializeField]
        private EasingFunctions.TYPE FadeOutEasingType = EasingFunctions.TYPE.Regular;

        private bool isVisible;
        private UIFader fader;
        private bool isInitialized;


        // .. INITIALIZATION

        void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;

            fader = GetComponent<UIFader>();

            isVisible = !VisibleOnStart;
            if (VisibleOnStart)
                Show();
            else
                Hide();
        }


        // .. OPERATIONS

        public override void Show(float animation_time = 0)
        {
            TryInitialize();

            if (isVisible)
                return;

            isVisible = true;
            gameObject.SetActive(true);
            StopCoroutine("UpdateShowHide");
            object[] args = new object[] { true, animation_time };
            StartCoroutine("UpdateShowHide", args);
        }

        public override void Hide(float animation_time = 0)
        {
            TryInitialize();

            if (!isVisible)
                return;

            isVisible = false;
            gameObject.SetActive(true);
            StopCoroutine("UpdateShowHide");
            object[] args = new object[] { false, animation_time };
            StartCoroutine("UpdateShowHide", args);
        }

        public override bool IsHidden()
        {
            TryInitialize();
            return !isVisible;
        }


        // .. COROUTINES

        IEnumerator UpdateShowHide(object[] args)
        {
            bool show = (bool)args[0];
            float animation_time = (float)args[1];

            if (show)
                fader.Fade(1, animation_time, FadeInEasingType, true);
            else
                fader.Fade(0, animation_time, FadeOutEasingType, false);

            yield return new WaitForSeconds(animation_time);

            gameObject.SetActive(show);
        }



    }
}
