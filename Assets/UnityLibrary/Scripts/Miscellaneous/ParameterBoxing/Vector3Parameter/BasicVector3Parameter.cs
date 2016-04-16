using UnityEngine;

namespace Miscellaneous.ParameterBoxing.Vector3Parameter
{
    public class BasicVector3Parameter : AVector3Parameter
    {
        public void SetValue(float value)
        {
            SetValue(Vector3.one * value);
        }

        public override Vector3 GetValue()
        {
            return value;
        }

        public override void SetValue(Vector3 value)
        {
            this.value = value;
            NotifyObservers();
        }
    }
}
