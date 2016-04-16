namespace Miscellaneous
{
    public enum RaycastCullMode
    {
        FRONT, //ignore stuff facing in the opposite direction of the ray
        BACK, //ignore stuff facing in the direction of the ray
        NONE //don't ignore anything
    }
}
