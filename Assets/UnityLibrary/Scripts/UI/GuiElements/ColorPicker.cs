using System.Collections;
using ExtensionMethods;
using Miscellaneous.Easing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GuiElements
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(GameObjectAnimationScale))]
    [RequireComponent(typeof(GameObjectAnimationColor))]
    public class ColorPicker : AGuiElement, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // .. ATTRIBUTES

        public Color SelectedColor { get; set; }

        private GameObjectAnimationScale scaleAnimator;
        private GameObjectAnimationColor colorAnimator;
        private Image colorPickerImage;
        private bool hasPointer;
        private bool pointerDown;
        private PointerEventData lastPointerEventData;
        private bool hidden;
        private bool isInitialized;

        // .. EVENTS

        public class ColorValueChanged : UnityEvent<Color> { }
        public class HueSaturationValueChange : UnityEvent<float, float, float, float> { }

        public ColorValueChanged onColorValueChanged = new ColorValueChanged();
        public HueSaturationValueChange onHsvValueChanged = new HueSaturationValueChange();

        // .. INITIALIZATION

        private void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;

            scaleAnimator = GetComponent<GameObjectAnimationScale>();
            colorAnimator = GetComponent<GameObjectAnimationColor>();
        }

        // .. OPERATIONS

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

        public void OnPointerDown(PointerEventData eventData)
        {
            lastPointerEventData = eventData;
            pointerDown = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            lastPointerEventData = eventData;
            hasPointer = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            lastPointerEventData = eventData;
            hasPointer = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            lastPointerEventData = eventData;
            pointerDown = false;
        }


        private void UpdatePickedColor()
        {
            RectTransform m_DraggingPlane;
            if (lastPointerEventData == null || lastPointerEventData.pointerEnter.transform as RectTransform == null)
                return;

            m_DraggingPlane = lastPointerEventData.pointerEnter.transform as RectTransform;

            Vector2 pointer_local_pos;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(m_DraggingPlane, lastPointerEventData.position, lastPointerEventData.pressEventCamera, out pointer_local_pos))
                return;

            int xPos = (int)(pointer_local_pos.x);
            int yPos = (int)(pointer_local_pos.y);
            xPos += Mathf.FloorToInt(m_DraggingPlane.rect.width * m_DraggingPlane.pivot.x);
            yPos += Mathf.FloorToInt(m_DraggingPlane.rect.height * m_DraggingPlane.pivot.y);

            float textureWidth = colorPickerImage.sprite.texture.width;
            float textureHeight = colorPickerImage.sprite.texture.height;
            float imageWidth = m_DraggingPlane.rect.width;
            float imageHeight = m_DraggingPlane.rect.height;

            int colorX = Mathf.FloorToInt(xPos / imageWidth * textureWidth);
            int colorY = Mathf.FloorToInt(yPos / imageHeight * textureHeight);

            SelectedColor = colorPickerImage.sprite.texture.GetPixel(colorX, colorY);

            onColorValueChanged.Invoke(SelectedColor);

            {
                float
                    hue,
                    saturation,
                    value,
                    alpha;

                SelectedColor.ConvertRBGToHSV(out hue, out saturation, out value, out alpha);

                onHsvValueChanged.Invoke(hue, saturation, value, alpha);
            }
        }

        private void Update()
        {
            if (colorPickerImage == null)
                colorPickerImage = GetComponent<Image>();

            if (!hasPointer || !pointerDown)
                return;

            UpdatePickedColor();
        }

        // .. COROUTINES

        private IEnumerator UpdateShow(float animation_time)
        {
            Transform this_tranform = GetComponent<Transform>();
            scaleAnimator.StartAnimation(EasingFunctions.TYPE.OutElastic, this_tranform.localScale, Vector3.one, animation_time);
            colorAnimator.StartAnimation(0f, 1f, animation_time);
            yield return new WaitForSeconds(animation_time);
        }

        private IEnumerator UpdateHide(float animation_time)
        {
            Transform this_tranform = GetComponent<Transform>();
            scaleAnimator.StartAnimation(EasingFunctions.TYPE.FarIn, this_tranform.localScale, new Vector3(1, 0, 0), animation_time);
            colorAnimator.StartAnimation(1f, 0f, animation_time);
            yield return new WaitForSeconds(animation_time);
            gameObject.SetActive(false);
        }

    }
}
