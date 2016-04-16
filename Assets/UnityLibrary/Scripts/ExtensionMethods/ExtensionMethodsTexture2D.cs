using System.Linq;
using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsTexture2D
    {
        // -- PUBLIC

        // .. EXENSION_METHODS

        public static void Fill(
            this Texture2D texture,
            Color color
            )
        {
            int
                width,
                height;
            Color[]
                color_table;

            width = texture.width;
            height = texture.height;

            color_table = Enumerable.Repeat( color, width * height ).ToArray();
            texture.SetPixels( 0, 0, width, height, color_table );
            texture.Apply();
        }
    }
}
