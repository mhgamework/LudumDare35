using UnityEngine;

namespace Miscellaneous.ParameterBoxing.Vector3Parameter
{
    /// <summary>
    /// A vector3 parameter that syncs its value with the transofmr position of its gameobject.
    /// </summary>
    public class TransformVector3Parameter : AVector3Parameter
    {
        // .. ATTRIBUTES

        [SerializeField]
        private bool useWorldPosition = true;

        private Transform thisTransform;

        // .. INITIALIZATION

        void Awake()
        {
            thisTransform = GetComponent<Transform>();
        }

        // .. OPERATIONS

        public override Vector3 GetValue()
        {
            return useWorldPosition ? thisTransform.position : thisTransform.localPosition;
        }

        public override void SetValue(Vector3 value)
        {
            this.value = value;

            if (useWorldPosition)
                thisTransform.position = value;
            else
                thisTransform.localPosition = value;

            NotifyObservers();
        }

        void Update()
        {
            var c_value = GetValue();
            if (value != c_value)
                SetValue(c_value);
        }

    }
}
