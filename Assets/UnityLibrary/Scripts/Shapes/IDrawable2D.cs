using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
    public interface IDrawable2D
    {
        Vector2 GetPointAtTime(float t);
        Vector2 GetDirectionAtTime(float t);
        List<Line2D> Get2DComponents();
        MultiLine2D Get2DComponentsAsMultiLine();
        IDrawable2D Clone();
    }
}
