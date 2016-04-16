using UnityEngine;

namespace Miscellaneous.ParameterBoxing.QuaternionParameter
{
    public interface IQuaternionParameterObserver
    {
        void NotifyParameterChanged(AQuaternionParameter parameter, Quaternion value); 
    }
}
