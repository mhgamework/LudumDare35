using UnityEngine;

namespace Shapes
{
    public struct LineSegment2D
    {
        // .. ATTRIBUTES

        public Vector2 Point0;
        public Vector2 Point1;

        // .. INITIALIZATION

        public LineSegment2D(Vector2 point0, Vector2 point1)
        {
            Point0 = point0;
            Point1 = point1;
        }

        // .. OPERATIONS

        public Vector2 CalculateNormalPerpendicular()
        {
            Vector2
                segment_vector;

            segment_vector = Point1 - Point0;

            return new Vector2(-segment_vector.y, segment_vector.x).normalized;
        }

        public float CalculateDistance(
            Vector2 point
            )
        {
            return Vector2.Dot(CalculateNormalPerpendicular(), point - Point0);
        }

        public float CalculateAbsoluteDistance(
            Vector2 point
            )
        {
            return Mathf.Abs(CalculateDistance(point));
        }

        public Vector2 CalculateMiddlePoint()
        {
            return (Point0 + Point1) * 0.5f;
        }

    }
}
