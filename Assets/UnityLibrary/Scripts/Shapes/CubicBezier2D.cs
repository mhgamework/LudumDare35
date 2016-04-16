using System;
using System.Collections.Generic;
using System.IO;
using ExtensionMethods;
using UnityEngine;

namespace Shapes
{
    public class CubicBezier2D : IDrawable2D
    {
        // .. ATTRIBUTES

        public Vector2 P0 { get; set; }
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }
        public Vector2 P3 { get; set; }
        public int Resolution { get { return resolution; } set { resolution = Math.Max(value, 0); } }

        private int resolution = 25;

        // .. INITIALIZATION

        public CubicBezier2D(Vector2 start, Vector2 start_control, Vector2 end_control, Vector2 end)
        {
            P0 = start;
            P1 = start_control;
            P2 = end_control;
            P3 = end;
        }

        private CubicBezier2D()
        {

        }

        // .. OPERATIONS

        /// <summary>
        /// Sets the start handle (P1) to match given rotation (degrees) and distance with regards to the starting point (P0).
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="distance"></param>
        public void SetStartHandle(float rotation, float distance)
        {
            P1 = P0 + GetOffset(rotation, distance);
        }

        /// <summary>
        /// Sets the end handle (P2) to match given rotation (degrees) and distance with regards to the end point (P3).
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="distance"></param>
        public void SetEndHandle(float rotation, float distance)
        {
            P2 = P3 + GetOffset(rotation, distance);
        }

        public Vector2 GetPointAtTime(float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * P0 +
                3f * oneMinusT * oneMinusT * t * P1 +
                3f * oneMinusT * t * t * P2 +
                t * t * t * P3;
        }

        public Vector2 GetDerivativeAtTime(float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (P1 - P0) +
                6f * oneMinusT * t * (P2 - P1) +
                3f * t * t * (P3 - P2);
        }

        public Vector2 GetDirectionAtTime(float t)
        {
            return GetDerivativeAtTime(t).normalized;
        }

        public List<Line2D> Get2DComponents()
        {
            var ret = new List<Line2D>();

            Vector2 prev_point = P0;
            for (float t = 0; t < 1; t += 1f / (float)Resolution)
            {
                var t_point = GetPointAtTime(t);
                ret.Add(new Line2D(prev_point, t_point));
                prev_point = t_point;
            }

            ret.Add(new Line2D(prev_point, P3));
            return ret;
        }

        public MultiLine2D Get2DComponentsAsMultiLine()
        {
            var lines = Get2DComponents();
            var multiline = new MultiLine2D(lines[0]);
            for (int i = 1; i < lines.Count; i++)
            {
                multiline.AddPoint(lines[i].P1);
            }
            return multiline;
        }

        public void Serialize(BinaryWriter binary_writer)
        {
            binary_writer.WriteVector2(P0);
            binary_writer.WriteVector2(P1);
            binary_writer.WriteVector2(P2);
            binary_writer.WriteVector2(P3);

        }

        public void Deserialize(BinaryReader binary_reader)
        {
            P0 = binary_reader.ReadVector2();
            P1 = binary_reader.ReadVector2();
            P2 = binary_reader.ReadVector2();
            P3 = binary_reader.ReadVector2();
        }

        public IDrawable2D Clone()
        {
            CubicBezier2D
                clone;

            clone = new CubicBezier2D();
            clone.P0 = P0;
            clone.P1 = P1;
            clone.P2 = P2;
            clone.P3 = P3;
            clone.resolution = resolution;

            return clone;
        }


        private Vector2 GetOffset(float rotation, float distance)
        {
            var rad = rotation / 180f * Mathf.PI;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * distance;
        }


    }
}

