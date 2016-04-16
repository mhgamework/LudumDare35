using UnityEngine;

namespace Miscellaneous.ParameterBoxing.QuaternionParameter
{
    public class TransformQuaternionParameter : AQuaternionParameter
    {
        // .. ATTRIBUTES

        [SerializeField]
        private bool useWorldRotation = true;

        private Transform thisTransform;

        // .. INITIALIZATION

        void Awake()
        {
            thisTransform = GetComponent<Transform>();
        }

        // .. OPERATIONS

        public override Quaternion GetValue()
        {
            return useWorldRotation ? thisTransform.rotation : thisTransform.localRotation;
        }

        public override void SetValue(Quaternion value)
        {
            this.value = value;

            if (useWorldRotation)
                thisTransform.rotation = value;
            else
                thisTransform.localRotation = value;

            NotifyObservers();
        }

        void Update()
        {
            var c_value = GetValue();
            if (this.value != c_value)
                SetValue(c_value);
        }

    }
}
