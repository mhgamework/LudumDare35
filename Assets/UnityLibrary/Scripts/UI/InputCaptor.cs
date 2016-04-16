using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Captures mouse/touch input and tracks whether it has focus.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class InputCaptor : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        [Tooltip("The maximum allowed time in seconds between clicks to count as a 'double' click.")]
        private float DoubleClickGraceTime = 0.2f;

        private float lastClickedTime;

        // .. EVENTS

        [Serializable]
        public class ClickedEventHandler : UnityEvent { };
        [Serializable]
        public class DoubleClickedEventHandler : UnityEvent { };

        public ClickedEventHandler OnClicked;
        public DoubleClickedEventHandler OnDoubleClicked;

        // .. INITIALIZATION

        void Start()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnReceivedClick);
        }

        // .. OPERATIONS

        public bool HasFocus()
        {
            if (EventSystem.current.IsPointerOverGameObject() || (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                //print(EventSystem.current.currentSelectedGameObject);
                return EventSystem.current.currentSelectedGameObject == gameObject;
            }
            return false;
        }


        private void OnReceivedClick()
        {
            if (OnClicked != null)
                OnClicked.Invoke();

            if (Mathf.Abs(lastClickedTime - Time.timeSinceLevelLoad) < DoubleClickGraceTime)
            {
                //Debug.Log("Double Clicked!");

                if (OnDoubleClicked != null)
                    OnDoubleClicked.Invoke();
            }

            lastClickedTime = Time.timeSinceLevelLoad;
        }


    }
}
