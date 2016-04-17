using System.Collections;
using Miscellaneous;
using Miscellaneous.Easing;
using Miscellaneous.ParameterBoxing.FloatParameter;
using Miscellaneous.ParameterBoxing.Vector3Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(GameObjectAnimationScale))]
    public class SliderController : MonoBehaviour, IFloatParameterObserver, IVector3ParameterObserver
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Slider SliderComponent = null;
        [SerializeField]
        private Text NameLabel = null;
        [SerializeField]
        private Text ValueLabel = null;

        [SerializeField]
        private string InitialName = "NAME";
        [SerializeField]
        private float InitialValue = 100f;

        private float minValue, maxValue;

        [SerializeField]
        private bool Snap = false;
        [SerializeField]
        [ConditionalHide("Snap")]
        private float SnapValue = 0;

        [SerializeField]
        private AFloatParameter ObservedFloatParameter = null;
        [SerializeField]
        private BasicVector3Parameter ObservedVector3Parameter = null;


        private GameObjectAnimationScale scaleAnimator;
        private bool isInitialized;
        private float prevValue = float.MaxValue; //cache for displaying current value

        private bool changingValueFromSlider;

        // .. EVENTS

        //todo use unity event
        public delegate void SliderValueChangedEventHandler(SliderController sliderController);
        public event SliderValueChangedEventHandler SliderValueChanged;


        // .. INITIALIZATION

        void Start()
        {
            TryInitialize();
        }

        public void TryInitialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;

            if (Snap)
            {
                if (SliderComponent.minValue < 0)
                    SliderComponent.maxValue -= SliderComponent.minValue;
                SliderComponent.minValue = 0;

                SliderComponent.wholeNumbers = true;
                var nb_snaps = Mathf.FloorToInt((SliderComponent.maxValue - SliderComponent.minValue) / SnapValue);

                SliderComponent.maxValue = nb_snaps;

                minValue = SliderComponent.minValue;
                maxValue = SliderComponent.maxValue;
            }

            scaleAnimator = GetComponent<GameObjectAnimationScale>();
            SliderComponent.onValueChanged.AddListener(newValue => OnSliderValueChanged(newValue));

            NameLabel.text = InitialName;
            SetValue(InitialValue);

            OnSliderValueChanged(InitialValue); //force update

            if (ObservedFloatParameter != null)
                ObservedFloatParameter.RegisterObserver(this);
            if (ObservedVector3Parameter != null)
                ObservedVector3Parameter.RegisterObserver(this);
        }

        // .. OPERATIONS

        public void SetRange(float minValue, float maxValue)
        {
            SliderComponent.minValue = minValue;
            SliderComponent.maxValue = maxValue;
        }

        public float GetRange()
        {
            return Mathf.Abs(SliderComponent.maxValue - SliderComponent.minValue);
        }


        /// <summary>
        /// Sets the name of the slider to a fixed value.
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            NameLabel.text = name;
        }

        public void SetValue(float value)
        {
            TryInitialize();
            SliderComponent.value = value;
        }

        public void SetValueRelative(float relative_value)
        {
            if (relative_value < 0 || relative_value > 1)
                return;

            float new_value = (SliderComponent.maxValue - SliderComponent.minValue) * relative_value + SliderComponent.minValue;
            SetValue(new_value);
        }

        public string GetLabelName()
        {
            return NameLabel.text;
        }

        public float GetCurrentValue()
        {
            if (Snap)
                return SliderComponent.value * SnapValue;

            return SliderComponent.value;
        }

        public void Show(float animation_time)
        {
            TryInitialize();
            gameObject.SetActive(true);
            StopCoroutine("UpdateShow");
            StopCoroutine("UpdateHide");
            StartCoroutine("UpdateShow", animation_time);
        }

        public void Hide(float animation_time)
        {
            TryInitialize();
            gameObject.SetActive(true);
            StopCoroutine("UpdateShow");
            StopCoroutine("UpdateHide");
            StartCoroutine("UpdateHide", animation_time);
        }


        public void NotifyParameterChanged(AFloatParameter parameter, float value)
        {
            if (changingValueFromSlider || parameter != ObservedFloatParameter)
                return;

            SetValue(parameter.GetValue());
        }

        public void NotifyParameterChanged(AVector3Parameter parameter, Vector3 value)
        {
            if (changingValueFromSlider || parameter != ObservedVector3Parameter)
                return;

            SetValue(parameter.GetValue().x);
        }

        private void OnSliderValueChanged(float newValue)
        {
            changingValueFromSlider = true;

            newValue = GetCurrentValue();

            if (Mathf.Abs(newValue - prevValue) >= 0.01f)
            {
                ValueLabel.text = (newValue).ToString("F");
                prevValue = newValue;
            }

            if (ObservedFloatParameter != null)
                ObservedFloatParameter.SetValue(GetCurrentValue());
            if (ObservedVector3Parameter != null)
                ObservedVector3Parameter.SetValue(GetCurrentValue());

            if (SliderValueChanged != null)
                SliderValueChanged(this);

            changingValueFromSlider = false;
        }

        // .. COROUTINES

        private IEnumerator UpdateShow(float animation_time)
        {
            Transform this_tranform = GetComponent<Transform>();
            scaleAnimator.StartAnimation(EasingFunctions.TYPE.OutElastic, this_tranform.localScale, Vector3.one, animation_time);
            yield return new WaitForSeconds(animation_time);
        }

        private IEnumerator UpdateHide(float animation_time)
        {
            Transform this_tranform = GetComponent<Transform>();
            scaleAnimator.StartAnimation(EasingFunctions.TYPE.BackInCubic, this_tranform.localScale, Vector3.zero, animation_time);
            yield return new WaitForSeconds(animation_time);
            gameObject.SetActive(false);
        }



    }
}
