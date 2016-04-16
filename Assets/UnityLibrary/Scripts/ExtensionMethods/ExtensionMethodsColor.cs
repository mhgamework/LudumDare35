using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsColor
    {
        // -- PUBLIC

        // .. EXENSION_METHODS

        public static Color CloneAdjustedAlpha(
            this Color base_color,
            float alpha
            )
        {
            Color
                color;

            color = base_color;
            color.a = alpha;

            return color;
        }

        // ~~

        public static void ConvertRBGToHSV(
            this Color color,
            out float hue,
            out float saturation,
            out float value,
            out float alpha
            )
        {
            float
                maximum,
                minimum,
                delta;

            maximum = Mathf.Max( color.r, color.g, color.b );
            minimum = Mathf.Min( color.r, color.g, color.b );
            delta = maximum - minimum;

            value = maximum * 100.0f;

            if ( maximum == minimum )
            {
                hue = 0.0f;
                saturation = 0.0f;
                alpha = 0.0f;
                return;
            }
            else if ( maximum != 0 )
            {
                saturation = delta / maximum;
            }
            else
            {
                saturation = 0.0f;
                hue = -1.0f;
                alpha = 0.0f;
                return;
            }

            if ( color.r == maximum )
            {
                hue = ( color.g - color.b ) / delta;
            }
            else if ( color.g == maximum )
            {
                hue = 2 + ( color.b - color.r ) / delta;
            }
            else
            {
                hue = 4 + ( color.r - color.g ) / delta;
            }

            hue *= 60.0f;

            if ( hue < 0 )
            {
                hue += 360;
            }

            saturation *= 100.0f;

            alpha = color.a;
        }

        // ~~

        public static Color ConvertHSVToRGB(
            this Color color,
            float hue,
            float saturation,
            float value,
            float alpha
            )
        {
            Color
                new_color;
            int 
                i;
            float 
                f, 
                p,
                q,
                t;

            saturation /= 100.0f;
            value /= 100.0f;

            new_color = new Color();

            if ( saturation == 0.0f )
            {
                // achromatic (grey)
                new_color.r = new_color.g = new_color.b = value;
                return new_color;
            }

            hue /= 60.0f;			// sector 0 to 5
            i = ( int ) Mathf.Floor( hue );
            f = hue - i;			// factorial part of h
            p = value * ( 1.0f - saturation );
            q = value * ( 1.0f - saturation * f );
            t = value * ( 1.0f - saturation * ( 1.0f - f ) );

            switch ( i )
            {
                case 0:
                {
                    new_color.r = value;
                    new_color.g = t;
                    new_color.b = p;
                    break;
                }
                case 1:
                {
                    new_color.r = q;
                    new_color.g = value;
                    new_color.b = p;
                    break;
                }
                case 2:
                {
                    new_color.r = p;
                    new_color.g = value;
                    new_color.b = t;
                    break;
                }
                case 3:
                {
                    new_color.r = p;
                    new_color.g = q;
                    new_color.b = value;
                    break;
                }
                case 4:
                {
                    new_color.r = t;
                    new_color.g = p;
                    new_color.b = value;
                    break;
                }
                default:		// case 5:
                {
                    new_color.r = value;
                    new_color.g = p;
                    new_color.b = q;
                    break;
                }
            }

            return new_color;
        }

        // -- PRIVATE

        // .. FUNCTIONS
    }
}