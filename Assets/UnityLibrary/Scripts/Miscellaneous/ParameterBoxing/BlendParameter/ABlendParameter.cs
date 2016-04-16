using System.Collections.Generic;
using UnityEngine;

namespace Miscellaneous.ParameterBoxing.BlendParameter
{
    /// <summary>
    /// A blendparameter has range [0..1]
    /// </summary>
    public abstract class ABlendParameter : MonoBehaviour
    {
        private List<IBlendParameterObserver> observers = new List<IBlendParameterObserver>();

        public void RegisterObserver(IBlendParameterObserver observer)
        {
            if (observers.Contains(observer))
                return;

            observers.Add(observer);

            observer.NotifyParameterChanged(this, GetValue());
        }

        public abstract float GetValue();

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.NotifyParameterChanged(this, GetValue());
            }
        }
    }
}
