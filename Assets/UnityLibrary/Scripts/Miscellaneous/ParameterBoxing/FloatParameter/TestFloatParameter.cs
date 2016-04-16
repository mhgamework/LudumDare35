using UnityEngine;

namespace Miscellaneous.ParameterBoxing.FloatParameter
{
    public class TestFloatParameter : AFloatParameter
    {
        // .. ATTRIBUTES

        [SerializeField]
        [Range(0f, 10f)]
        private float Value = 0f;

        // .. OPERATIONS

        void OnValidate()
        {
            SetValue(Value);
        }
    }
}
