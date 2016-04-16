using UnityEngine;

namespace Miscellaneous.ParameterBoxing.BlendParameter
{
    /// <summary>
    /// A blendparameter exposed in the editor with a slider for testing.
    /// </summary>
    public class TestBlendParameter : ABlendParameter
    {
        // .. ATTRIBUTES

        [SerializeField]
        [Range(0f, 1f)]
        private float Parameter = 0f;


        // .. INITIALIZATION

        void OnValidate()
        {
            NotifyObservers();
        }

        public override float GetValue()
        {
            return Parameter;
        }
    }
}
