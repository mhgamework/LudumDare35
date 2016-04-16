using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Extends the unity dropdown menu with events that inform the receiver of the index of the selected element.
    /// Usage: call HandleValueChanged() from the unity dropdown valuechangedeventhandler
    /// </summary>
    [RequireComponent(typeof(Dropdown))]
    public class UnityDropDownExtender : MonoBehaviour
    {
        // .. TYPES

        public class BoxedInteger
        {
            public int Value;
        }

        // .. EVENTS

        [Serializable]
        public class ValueChangedEventHandler : UnityEvent<BoxedInteger> { }
        public ValueChangedEventHandler OnValueChanged;

        // .. INITIALIZATION

        void Start()
        {
            GetComponent<Dropdown>().onValueChanged.AddListener(arg0 => HandleValueChanged());
        }

        // .. OPERATIONS

        public void HandleValueChanged()
        {
            if (OnValueChanged != null)
                OnValueChanged.Invoke(new BoxedInteger { Value = GetComponent<Dropdown>().value });
        }

    }
}
