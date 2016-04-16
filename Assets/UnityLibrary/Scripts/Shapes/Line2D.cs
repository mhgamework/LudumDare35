using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
    public class Line2D : IDrawable2D
    {
        // .. ATTRIBUTES

        public Vector2 P0 { get; private set; }
        public Vector2 P1 { get; private set; }

        // .. INITIALIZATION

        public Line2D(Vector2 start, Vector2 end)
        {
            P0 = start;
            P1 = end;
        }

        private Line2D()
        {

        }

        // .. OPERATIONS

        public Vector2 GetPointAtTime(float t)
        {
            t = Mathf.Clamp01(t);
            return Vector2.Lerp(P0, P1, t);
        }

        public Vector2 GetDirectionAtTime(float t)
        {
            return (P1 - P0).normalized;
        }

        public List<Line2D> Get2DComponents()
        {
            return new List<Line2D> { this };
        }

        public MultiLine2D Get2DComponentsAsMultiLine()
        {
            return new MultiLine2D(this);
        }

        /// <summary>
        /// Does this line intersects with given line?
        /// </summary>
        /// <param name="other"></param>
        /// <param name="intersect_point"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        public bool Intersects(Line2D other, out Vector2 intersect_point, bool inclusive = true)
        {
            return Line2D.GetIntersectPoint(this, other, out intersect_point, inclusive);
        }

        public static bool GetIntersectPoint(Line2D line_a, Line2D line_b, out Vector2 intersect_point, bool inclusive = true)
        {
            intersect_point = new Vector2();
            Vector2 one = line_a.P0;
            Vector2 two = line_a.P1;
            Vector2 three = line_b.P0;
            Vector2 four = line_b.P1;

            float denominator = (one.x - two.x) * (three.y - four.y) - (one.y - two.y) * (three.x - four.x);
            if (Mathf.Abs(denominator) < 0.000001f) //if denominator is zero, the lines are parallel. (We ignore the case of coincident lines)
                return false;

            float x = ((one.x * two.y - one.y * two.x) * (three.x - four.x) - (one.x - two.x) * (three.x * four.y - three.y * four.x)) / denominator;
            float y = ((one.x * two.y - one.y * two.x) * (three.y - four.y) - (one.y - two.y) * (three.x * four.y - three.y * four.x)) / denominator;
            intersect_point = new Vector2(x, y);

            var x_inclusive_a = IsValueBetween(x, line_a.P0.x, line_a.P1.x, inclusive);
            var x_inclusive_b = IsValueBetween(x, line_b.P0.x, line_b.P1.x, inclusive);
            var y_inclusive_a = IsValueBetween(y, line_a.P0.y, line_a.P1.y, inclusive);
            var y_inclusive_b = IsValueBetween(y, line_b.P0.y, line_b.P1.y, inclusive);
            
            if (inclusive)
                return x_inclusive_a && x_inclusive_b && y_inclusive_a && y_inclusive_b;
            
            return (x_inclusive_a && y_inclusive_b) || (x_inclusive_b && y_inclusive_a); //keep cornercase in mind where line a or b are parallel to x- or y-axis.
        }

        public IDrawable2D Clone()
        {
            Line2D
                clone;

            clone = new Line2D();
            clone.P0 = P0;
            clone.P1 = P1;

            return clone;
        }


        /// <summary>
        /// Returns whether a is between b and c
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsValueBetween(float a, float b, float c, bool inclusive = true)
        {
            if (inclusive)
            {
                const float margin = 0.000001f; //margin to account for calculation rounding errors
                if (b < c)
                    return a >= b - margin && a <= c + margin;
                return a >= c - margin && a <= b + margin;
            }
            else
            {
                if (b < c)
                    return a > b && a < c;
                return a > c && a < b;
            }


        }
    }
}
