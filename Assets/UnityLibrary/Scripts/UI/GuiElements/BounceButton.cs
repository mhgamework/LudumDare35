using System;
using System.Collections;
using Miscellaneous.Easing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.GuiElements
{
    [RequireComponent(typeof(GameObjectAnimationScale))]
    public class BounceButton : AGuiElement, IPointerClickHandler
    {
        // .. ATTRIBUTES

        [SerializeField]
        private float BounceFactor = 1.25f;
        [SerializeField]
        private EasingFunctions.TYPE ShowEasingType = EasingFunctions.TYPE.OutElastic;
        [SerializeField]
        private EasingFunctions.TYPE HideEasingType = EasingFunctions.TYPE.In;
        [SerializeField]
        private bool FireClickAfterAnimation = false;

        private GameObjectAnimationScale scaleAnimator;
        private bool hidden;


        // .. ATTRIBUTES

        public ButtonClickedEvent onClick;

        // .. TYPES

        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        // .. INITIALIZATION

        void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            if (scaleAnimator != null)
                return;

            scaleAnimator = GetComponent<GameObjectAnimationScale>();
        }

        // ..OPERATIONS

        public void OnPointerClick(PointerEventData eventData)
        {
            TryInitialize();
            StopCoroutine("UpdateClicked");
            StartCoroutine("UpdateClicked", 0.25f);
        }

        public override void Show(float animation_time)
        {
            TryInitialize();
            hidden = false;
            gameObject.SetActive(true);
            StopCoroutine("UpdateShow");
            StopCoroutine("UpdateHide");
            StartCoroutine("UpdateShow", animation_time);
        }

        public override void Hide(float animation_time)
        {
            TryInitialize();
            hidden = true;
            gameObject.SetActive(true);
            StopCoroutine("UpdateShow");
            StopCoroutine("UpdateHide");
            StartCoroutine("UpdateHide", animation_time);
        }

        public override bool IsHidden()
        {
            return hidden;
        }


        // .. COROUTINES

        private IEnumerator UpdateClicked(float animation_time)
        {
            if (!FireClickAfterAnimation)
                onClick.Invoke();

            float grow_time = animation_time * 0.5f;
            float shrink_time = animation_time * 0.5f;
            Transform this_tranform = GetComponent<Transform>();

            scaleAnimator.StartAnimation(EasingFunctions.TYPE.BackInCubic, this_tranform.localScale, Vector3.one * BounceFactor, grow_time);
            yield return new WaitForSeconds(grow_time);
            scaleAnimator.StartAnimation(EasingFunctions.TYPE.Out, this_tranform.localScale, Vector3.one, shrink_time);
            yield return new WaitForSeconds(shrink_time);

            if (FireClickAfterAnimation)
                onClick.Invoke();
        }

        private IEnumerator UpdateShow(float animation_time)
        {
            Transform this_tranform = GetComponent<Transform>();
            scaleAnimator.StartAnimation(ShowEasingType, this_tranform.localScale, Vector3.one, animation_time);
            yield return new WaitForSeconds(animation_time);
        }

        private IEnumerator UpdateHide(float animation_time)
        {
            Transform this_tranform = GetComponent<Transform>();
            //ScaleAnimator.StartAnimation(EasingFunctions.TYPE.BackInCubic, this_tranform.localScale, Vector3.zero, animation_time);
            scaleAnimator.StartAnimation(HideEasingType, this_tranform.localScale, Vector3.zero, animation_time);
            yield return new WaitForSeconds(animation_time);
            gameObject.SetActive(false);
        }

    }
}
