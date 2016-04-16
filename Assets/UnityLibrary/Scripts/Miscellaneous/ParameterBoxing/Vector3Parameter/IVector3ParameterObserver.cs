using UnityEngine;

namespace Miscellaneous.ParameterBoxing.Vector3Parameter
{
    public interface IVector3ParameterObserver
    {
        void NotifyParameterChanged(AVector3Parameter parameter, Vector3 value);
    }
}
