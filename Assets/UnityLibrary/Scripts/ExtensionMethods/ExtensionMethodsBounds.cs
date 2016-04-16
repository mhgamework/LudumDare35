using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsBounds 
    {
        // -- PUBLIC

        // .. EXTENSION_METHODS

        public static Bounds CalculateBoundingBounds(
            this Bounds bounds,
            params Vector3[] point_table
            )
        {
            Vector3
                minimum,
                maximum;

            minimum = point_table[ 0 ];
            maximum = point_table[ 0 ];

            foreach ( Vector3 point in point_table )
            {
                minimum.x = Mathf.Min( point.x, minimum.x );
                minimum.y = Mathf.Min( point.y, minimum.y );
                minimum.z = Mathf.Min( point.z, minimum.z );

                maximum.x = Mathf.Max( point.x, maximum.x );
                maximum.y = Mathf.Max( point.y, maximum.y );
                maximum.z = Mathf.Max( point.z, maximum.z );
            }
        
            bounds.min = minimum;
            bounds.max = maximum;

            return bounds;
        }

        // ~~

        public static bool Contains(
            this Bounds bounds,
            Bounds smaller_bounds
            )
        {
            if ( smaller_bounds.min.x < bounds.min.x )
            {
                return false;
            }

            if ( smaller_bounds.min.y < bounds.min.y )
            {
                return false;
            }

            if ( smaller_bounds.min.z < bounds.min.z )
            {
                return false;
            }

            if ( smaller_bounds.max.x > bounds.max.x )
            {
                return false;
            }

            if ( smaller_bounds.max.y > bounds.max.y )
            {
                return false;
            }

            if ( smaller_bounds.max.z > bounds.max.z )
            {
                return false;
            }

            return true;
        }

        // ~~

        public static bool Collides(
            this Bounds bounds,
            Bounds other
            )
        {
            if ( other.max.x < bounds.min.x )
            {
                return false;
            }
            else if ( other.max.y < bounds.min.y )
            {
                return false;
            }
            else if ( other.max.z < bounds.min.z )
            {
                return false;
            }
            else if ( other.min.x > bounds.max.x )
            {
                return false;
            }
            else if ( other.min.y > bounds.max.y )
            {
                return false;
            }
            else if ( other.min.z > bounds.max.z )
            {
                return false;
            }

            return true;
        }

        // ~~

        public static bool CollidesWithSphere(
            this Bounds bounds,
            Vector3 center,
            float radian
            )
        {
            float
                square_magnitude;

            square_magnitude = ( center - bounds.ClosestPoint( center ) ).sqrMagnitude;

            return radian * radian >= square_magnitude;
        }

        // ~~

        public static Bounds MultiplyWithMatrix(
            this Bounds bounds,
            Matrix4x4 matrix
            )
        {
            bounds.SetMinMax( matrix.MultiplyVector( bounds.min ), matrix.MultiplyVector( bounds.max ) );

            return bounds;
        }

        // ~~

        public static Bounds Scale(
            this Bounds bounds,
            Vector3 scale
            )
        {
            bounds.SetMinMax( 
                new Vector3( bounds.min.x * scale.x, bounds.min.y * scale.y, bounds.min.z * scale.z ), 
                new Vector3( bounds.max.x * scale.x, bounds.max.y * scale.y, bounds.max.z * scale.z )
                );

            return bounds;
        }

        // ~~

        public static Bounds Append(
            this Bounds bounds,
            Bounds other_bounds
            )
        {
            bounds.SetMinMax(
                new Vector3( Mathf.Min( bounds.min.x, other_bounds.min.x ), Mathf.Min( bounds.min.y, other_bounds.min.y ), Mathf.Min( bounds.min.z, other_bounds.min.z ) ),
                new Vector3( Mathf.Max( bounds.max.x, other_bounds.max.x ), Mathf.Max( bounds.max.y, other_bounds.max.y ), Mathf.Max( bounds.max.z, other_bounds.max.z ) )
                );

            return bounds;
        }
    }
}
