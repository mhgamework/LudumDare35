namespace Miscellaneous.ParameterBoxing.FloatParameter
{
    public interface IFloatParameterObserver
    {
        void NotifyParameterChanged(AFloatParameter parameter, float value);
    }
}
