using System.Collections;
using Miscellaneous.Easing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GuiElements
{
    public class RingButton : MonoBehaviour, IPointerClickHandler
    {
        public Color DefaultColor = Color.white;
        public Color SelectedColorCenter = Color.gray;
        public Color SelectedColorRing = Color.gray;

        [SerializeField]
        private Image CenterImage = null;
        [SerializeField]
        private Image RingImage = null;

        [SerializeField]
        private float bounceTime = 0.5f;
        [SerializeField]
        private float bounceFactor = 1.1f;
        [SerializeField]
        private float colorBlendTime = 0.2f;

        [SerializeField]
        private bool StartSelected = true;

        [SerializeField]
        private bool DeselectWithClick = true;

        private bool isSelected;
        private GameObjectAnimationScale ringScaleAnimator;
        private GameObjectAnimationColor centerColorAnimator;
        private GameObjectAnimationColor ringColorAnimator;

        // .. EVENTS

        public UnityEvent OnSelected;
        public UnityEvent OnDeselected;

        // .. INITIALIZATION

        void Start()
        {
            ringScaleAnimator = RingImage.GetComponent<GameObjectAnimationScale>() ?? RingImage.gameObject.AddComponent<GameObjectAnimationScale>();
            centerColorAnimator = CenterImage.GetComponent<GameObjectAnimationColor>() ?? CenterImage.gameObject.AddComponent<GameObjectAnimationColor>();
            ringColorAnimator = RingImage.GetComponent<GameObjectAnimationColor>() ?? RingImage.gameObject.AddComponent<GameObjectAnimationColor>();

            centerColorAnimator.SetColor(DefaultColor);
            ringColorAnimator.SetColor(DefaultColor);

            StartCoroutine("InitializeRoutine");
        }

        // .. OPERATIONS

        public void OnPointerClick(PointerEventData eventData)
        {
            if (DeselectWithClick)
            {
                if (!isSelected)
                    Select();
                else
                    Deselect();
            }
            else
            {
                Select();
            }

        }

        public void Select()
        {
            if (isSelected)
                return;
            isSelected = true;

            if (OnSelected != null)
                OnSelected.Invoke();

            StartCoroutine("Bounce");
            centerColorAnimator.StartAnimation(DefaultColor, SelectedColorCenter, 2 * colorBlendTime);
            ringColorAnimator.StartAnimation(DefaultColor, SelectedColorRing, colorBlendTime);
        }

        public void Deselect()
        {
            if (!isSelected)
                return;
            isSelected = false;

            if (OnDeselected != null)
                OnDeselected.Invoke();

            StartCoroutine("Bounce");
            centerColorAnimator.StartAnimation(SelectedColorCenter, DefaultColor, 4 * colorBlendTime);
            ringColorAnimator.StartAnimation(SelectedColorRing, DefaultColor, colorBlendTime);
        }

        // .. COROUTINES

        private IEnumerator InitializeRoutine()
        {
            yield return null;

            if (StartSelected)
                Select();
            else
                Deselect();
        }

        private IEnumerator Bounce()
        {
            float grow_time = bounceTime * 0.5f;
            float shrink_time = bounceTime * 0.5f;

            ringScaleAnimator.StartAnimation(EasingFunctions.TYPE.BackInCubic, Vector3.one, Vector3.one * bounceFactor, grow_time);
            yield return new WaitForSeconds(grow_time);
            ringScaleAnimator.StartAnimation(EasingFunctions.TYPE.Out, Vector3.one * bounceFactor, Vector3.one, shrink_time);
            yield return new WaitForSeconds(shrink_time);
        }
    }
}
