using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shapes
{
    /// <summary>
    /// A line consisting of multiple sub-lines.
    /// </summary>
    public class MultiLine2D : IDrawable2D
    {
        // .. ATTRIBUTES

        private List<Line2D> SubLines = new List<Line2D>();

        // .. INITIALIZATION

        public MultiLine2D(Line2D start_line) //at least one subline is required for this class to work correctly
        {
            SubLines.Add(start_line);
        }

        private MultiLine2D()
        {

        }

        // .. OPERATIONS

        public void AddPoint(Vector2 point)
        {
            if (Vector2.Distance(GetPointAtTime(1), point) < 0.000001f)
                return;

            SubLines.Add(new Line2D(SubLines.Last().P1, point));
        }

        public Vector2 GetPointAtTime(float t)
        {
            t = Mathf.Clamp01(t);
            Line2D subline;
            float t_subline;
            GetSubLineForT(t, out subline, out t_subline);
            return subline.GetPointAtTime(t_subline);
        }

        public Vector2 GetDirectionAtTime(float t)
        {
            t = Mathf.Clamp01(t);
            Line2D subline;
            float t_subline;
            GetSubLineForT(t, out subline, out t_subline);
            return subline.GetDirectionAtTime(t_subline);
        }

        public List<Line2D> Get2DComponents()
        {
            return SubLines;
        }

        public MultiLine2D Get2DComponentsAsMultiLine()
        {
            return this;
        }

        public MultiLine2D GetInverted()
        {
            var parts = Get2DComponents();
            var last_part = parts[parts.Count - 1];
            var ret = new MultiLine2D(new Line2D(last_part.P1, last_part.P0));

            for (int i = parts.Count - 2; i >= 0; i--)
            {
                ret.AddPoint(parts[i].P0);
            }

            return ret;
        }
        

        public IDrawable2D Clone()
        {
            MultiLine2D
                clone;

            clone = new MultiLine2D();

            foreach (Line2D line in SubLines)
            {
                clone.SubLines.Add((Line2D)line.Clone());
            }

            return clone;
        }

        private void GetSubLineForT(float t, out Line2D subline, out float t_in_subline)
        {
            t = Mathf.Clamp01(t);
            var fraction = SubLines.Count * t;
            var index = Mathf.FloorToInt(fraction);

            if (index == SubLines.Count) //corner case, t = 1
                index--;

            subline = SubLines[index];
            t_in_subline = fraction - index;
        }


        // .. STATIC OPERATIONS

        public static MultiLine2D Merge(MultiLine2D line01, MultiLine2D line02)
        {
            var all_parts = line01.Get2DComponents();
            all_parts.AddRange(line02.Get2DComponents());

            MultiLine2D merged = new MultiLine2D(all_parts[0]);

            for (int i = 1; i < all_parts.Count; i++)
            {
                Line2D to_add = all_parts[i];
                Line2D prev = all_parts[i - 1];

                if (Vector2.Distance(prev.P1, to_add.P0) > 0.000001f)
                    merged.AddPoint(to_add.P0);

                merged.AddPoint(to_add.P1);
            }

            return merged;
        }

    }
}
