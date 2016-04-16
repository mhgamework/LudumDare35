using UnityEngine;

namespace Shapes
{
    public abstract class ADrawableController : MonoBehaviour
    {
        public abstract IDrawable2D GetDrawable();
    }
}
