using System.Collections.Generic;
using UnityEngine;

namespace Miscellaneous.ParameterBoxing.FloatParameter
{
    /// <summary>
    /// A component representing a float parameter that can be observed.
    /// </summary>
    public abstract class AFloatParameter : MonoBehaviour
    {
        // .. ATTRIBUTES

        private float value;
        private List<IFloatParameterObserver> observers = new List<IFloatParameterObserver>();

        // .. OPERATIONS

        /// <summary>
        /// Register a new observer that is informed of any value changes.
        /// </summary>
        /// <param name="observer"></param>
        public void RegisterObserver(IFloatParameterObserver observer)
        {
            if (observers.Contains(observer))
                return;

            observers.Add(observer);
            observer.NotifyParameterChanged(this, GetValue());
        }

        /// <summary>
        /// Inform any observers of the current value.
        /// </summary>
        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.NotifyParameterChanged(this, GetValue());
            }
        }

        /// <summary>
        /// Get the current value.
        /// </summary>
        /// <returns></returns>
        public float GetValue()
        {
            return value;
        }

        /// <summary>
        /// Set given value as current value, notify observers.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(float value)
        {
            this.value = value;
            NotifyObservers();
        }
    }
}
