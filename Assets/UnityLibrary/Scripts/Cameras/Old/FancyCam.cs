using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Cameras.Old
{
    public class FancyCam : MonoBehaviour
    {
        private class ShotParameters
        {
            public float Height;
            public float Radius;
            public float YRotation;
            public float VerticalDelta;
            public float AngleDelta;
        }

        // -- PUBLIC

        // .. EVENTS

        [Serializable]
        public class FancyCamShotChangedEventHandler : UnityEvent { }
        public FancyCamShotChangedEventHandler OnShotChanged;

        // .. OPERATIONS

        public void SetEnabledState(bool isEnabled)
        {
            var enabledChanged = isEnabled != enabled;

            if (!isEnabled)
            {
                StopAllCoroutines();
                SetScreenOverlayColor(new Color(0, 0, 0, 0));
            }
            else if (enabledChanged)
            {
                StartCoroutine("ResumeFancyCam");
            }

            enabled = isEnabled;
        }

        // -- PRIVATE

        // .. OPERATIONS

        void Start()
        {
            fadeTexture = new Texture2D(1, 1);
            backgroundStyle.normal.background = fadeTexture;
            SetScreenOverlayColor(currentOverlayColor);

            InitializeShotParameters();

            InitialFov = CameraTransform.GetComponent<UnityEngine.Camera>().fieldOfView;

            SetNewShotParameters();

            if (EnableOnStart)
                StartCoroutine("UpdateFancyCam");
        }

        void OnGUI()
        {
            // if the current color of the screen is not equal to the desired color: keep fading!
            if (currentOverlayColor != targetOverlayColor)
            {
                // if the difference between the current alpha and the desired alpha is smaller than delta-alpha * deltaTime, then we're pretty much done fading:
                if (Mathf.Abs(currentOverlayColor.a - targetOverlayColor.a) < Mathf.Abs(deltaColor.a) * Time.deltaTime)
                {
                    currentOverlayColor = targetOverlayColor;
                    SetScreenOverlayColor(currentOverlayColor);
                    deltaColor = new Color(0, 0, 0, 0);
                }
                else
                {
                    // fade!
                    SetScreenOverlayColor(currentOverlayColor + deltaColor * Time.deltaTime);
                }
            }
        }

        private void SetNewShotParameters()
        {
            currentShotTime = 0;
            currentParameters = shotParameterList[Random.Range(0, shotParameterList.Count)];
        }

        private void InitializeShotParameters()
        {
            shotParameterList = new List<ShotParameters>();

            var radiusList = new List<float> { Mathf.Lerp(MinRadius, MaxRadius, 0f), Mathf.Lerp(MinRadius, MaxRadius, 0.25f), Mathf.Lerp(MinRadius, MaxRadius, 0.5f), Mathf.Lerp(MinRadius, MaxRadius, 0.75f), Mathf.Lerp(MinRadius, MaxRadius, 1f) };
            var yRotationList = new List<float> { 45, -45, 135, -135, 0f };
            var verticalDeltaList = new List<float> { 0.05f * HeightSpeedControlFactor, -0.05f * HeightSpeedControlFactor, 0f * HeightSpeedControlFactor };
            var angleDeltaList = new List<float> { 10f * AngleSpeedControlFactor, -10f * AngleSpeedControlFactor, 0f * AngleSpeedControlFactor };

            foreach (var radius in radiusList)
            {
                foreach (var yRotation in yRotationList)
                {
                    foreach (var verticalDelta in verticalDeltaList)
                    {
                        foreach (var angleDelta in angleDeltaList)
                        {
                            if (verticalDelta == 0f && angleDelta == 0f)
                                continue;


                            if (verticalDelta > 0)
                            {
                                shotParameterList.Add(new ShotParameters
                                {
                                    AngleDelta = angleDelta,
                                    Radius = radius,
                                    VerticalDelta = verticalDelta,
                                    YRotation = yRotation,
                                    Height = MinHeight,
                                });
                            }
                            else if (verticalDelta < 0)
                            {
                                shotParameterList.Add(new ShotParameters
                                {
                                    AngleDelta = angleDelta,
                                    Radius = radius,
                                    VerticalDelta = verticalDelta,
                                    YRotation = yRotation,
                                    Height = MaxHeight,
                                });
                            }
                            else
                            {
                                shotParameterList.Add(new ShotParameters
                                {
                                    AngleDelta = angleDelta,
                                    Radius = radius,
                                    VerticalDelta = verticalDelta,
                                    YRotation = yRotation,
                                    Height = MinHeight + (0.75f * MaxHeight - MinHeight),
                                });
                            }
                        }
                    }
                }
            }
        }

        #region Fading

        // instantly set the current color of the screen-texture to "newScreenOverlayColor"
        // can be usefull if you want to start a scene fully black and then fade to opague
        private void SetScreenOverlayColor(Color newScreenOverlayColor)
        {
            if (FaderImage == null)
                return;

            FaderImage.color = newScreenOverlayColor;
            currentOverlayColor = newScreenOverlayColor;
        }

        // initiate a fade from the current screen color (set using "SetScreenOverlayColor") towards "newScreenOverlayColor" taking "fadeDuration" seconds
        private void StartFade(Color newScreenOverlayColor, float fadeDuration)
        {
            if (fadeDuration <= 0.0f)		// can't have a fade last -2455.05 seconds!
            {
                SetScreenOverlayColor(newScreenOverlayColor);
            }
            else					// initiate the fade: set the target-color and the delta-color
            {
                targetOverlayColor = newScreenOverlayColor;
                deltaColor = (targetOverlayColor - currentOverlayColor) / fadeDuration;
            }
        }

        #endregion Fading

        // .. COROUTINES

        private IEnumerator ResumeFancyCam()
        {
            float currentFadeTime = 0f;

            while (currentFadeTime < FadeDuration)
            {
                currentFadeTime += Time.deltaTime;
                StartFade(Color.black, FadeDuration);

                yield return null;
            }

            SetNewShotParameters();
            StartCoroutine("UpdateFancyCam");
        }

        private IEnumerator UpdateFancyCam()
        {
            var thecamera = CameraTransform.GetComponent<UnityEngine.Camera>();
            while (true)
            {
                if (currentShotTime > ShotDuration)
                {
                    //init stuff
                    SetNewShotParameters();

                    if (OnShotChanged != null)
                        OnShotChanged.Invoke();
                }
                if (currentShotTime < FadeDuration)
                {
                    StartFade(new Color(0, 0, 0, 0), FadeDuration);
                }
                if (currentShotTime > ShotDuration - FadeDuration)
                {
                    StartFade(Color.black, FadeDuration);
                }

                currentShotTime += Time.deltaTime;

                transform.rotation = Quaternion.AngleAxis(currentParameters.YRotation + currentParameters.AngleDelta * currentShotTime, Vector3.up);

                var camHeight = Mathf.Clamp(currentParameters.Height + currentParameters.VerticalDelta * currentShotTime, MinHeight, MaxHeight);

                var radius = UseFovForZoom ? FixedRadius : currentParameters.Radius;
                var fov = UseFovForZoom ? currentParameters.Radius / FixedRadius * InitialFov : InitialFov;

                CameraTransform.localPosition = new Vector3(0, camHeight, radius);
                thecamera.fieldOfView = fov;

                yield return null;
            }
        }

        // .. ATTRIBUTES

        [SerializeField]
        private Transform CameraTransform = null;

        [SerializeField]
        private float ShotDuration = 7f;

        [SerializeField]
        private float FadeDuration = 1f;

        [SerializeField]
        private float MinRadius = 1f;
        [SerializeField]
        private float MaxRadius = 3f;

        [SerializeField]
        private float AngleSpeedControlFactor = 1f;
        [SerializeField]
        private float HeightSpeedControlFactor = 1f;

        [SerializeField]
        private float MinHeight = -0.5f;
        [SerializeField]
        private float MaxHeight = 0.5f;

        [SerializeField]
        private bool EnableOnStart = true;

        private List<ShotParameters> shotParameterList;

        private float currentShotTime;
        private ShotParameters currentParameters;

        private GUIStyle backgroundStyle = new GUIStyle();		    // Style for background tiling
        private Texture2D fadeTexture;				                // 1x1 pixel texture used for fading
        private Color currentOverlayColor = new Color(0, 0, 0, 0);	// default starting color: black and fully transparrent
        private Color targetOverlayColor = new Color(0, 0, 0, 0);	// default target color: black and fully transparrent
        private Color deltaColor = new Color(0, 0, 0, 0);		    // the delta-color is basically the "speed / second" at which the current color should change

        [SerializeField]
        private Image FaderImage = null;

        [SerializeField]
        private bool UseFovForZoom = true;
        [SerializeField]
        private float FixedRadius = 4f;

        private float InitialFov;
    }
}
