using System.Collections.Generic;
using UnityEngine;

namespace Miscellaneous.ParameterBoxing.QuaternionParameter
{
    /// <summary>
    /// A parameter representing a queternion that can be observed.
    /// </summary>
    public abstract class AQuaternionParameter : MonoBehaviour
    {
        // .. ATTRIBUTES

        protected Quaternion value;
        private List<IQuaternionParameterObserver> observers = new List<IQuaternionParameterObserver>();

        // .. OPERATIONS

        public void RegisterObserver(IQuaternionParameterObserver observer)
        {
            if (observers.Contains(observer))
                return;

            observers.Add(observer);
            observer.NotifyParameterChanged(this, GetValue());
        }

        public void NotifyObservers()
        {
            var q = GetValue();
            foreach (var observer in observers)
            {
                observer.NotifyParameterChanged(this, q);
            }
        }

        public abstract Quaternion GetValue();
        public abstract void SetValue(Quaternion value);

    }
}
