using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsRect  
    {
        // -- PUBLIC

        // .. EXTENSION_METHODS

        public static Rect CalculateBoundingRect( 
            this Rect rect,
            params Vector2[] point_table
            )
        {
            rect.xMin = point_table[ 0 ].x;
            rect.xMax = point_table[ 0 ].x;
            rect.yMin = point_table[ 0 ].y;
            rect.yMax = point_table[ 0 ].y;

            foreach ( Vector2 point in point_table )
            {
                rect.xMin = Mathf.Min( point.x, rect.xMin );
                rect.xMax = Mathf.Max( point.x, rect.xMax );
                rect.yMin = Mathf.Min( point.y, rect.yMin );
                rect.yMax = Mathf.Max( point.y, rect.yMax );
            }

            return rect;
        }

        // ~~

        public static bool Contains(
            this Rect original,
            Rect smaller_rect
            )
        {
            if ( smaller_rect.xMin < original.xMin )
            {
                return false;
            }

            if ( smaller_rect.xMax > original.xMax )
            {
                return false;
            }

            if ( smaller_rect.yMin < original.yMin )
            {
                return false;
            }

            if ( smaller_rect.yMax > original.yMax )
            {
                return false;
            }

            return true;
        }

        // ~~

        public static bool ContainsExtended(
            this Rect original,
            Vector2 point
            )
        {
            if ( point.x < original.xMin )
            {
                return false;
            }

            if ( point.x > original.xMax )
            {
                return false;
            }

            if ( point.y < original.yMin )
            {
                return false;
            }

            if ( point.y > original.yMax )
            {
                return false;
            }

            return true;
        }
    }
}
