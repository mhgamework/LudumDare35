namespace Miscellaneous.ParameterBoxing.BlendParameter
{
    public interface IBlendParameterObserver
    {
        void NotifyParameterChanged(ABlendParameter parameter, float normalized_value);
    }
}
