using System.Collections.Generic;
using UnityEngine;

namespace Miscellaneous.ParameterBoxing.Vector3Parameter
{
    public abstract class AVector3Parameter : MonoBehaviour
    {
        // .. ATTRIBUTES

        protected Vector3 value;
        private List<IVector3ParameterObserver> observers = new List<IVector3ParameterObserver>();


        // .. OPERATIONS

        public void RegisterObserver(IVector3ParameterObserver observer)
        {
            if (observers.Contains(observer))
                return;

            observers.Add(observer);
            observer.NotifyParameterChanged(this, GetValue());
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.NotifyParameterChanged(this, GetValue());
            }
        }

        public abstract Vector3 GetValue();

        public abstract void SetValue(Vector3 value);
    }
}
