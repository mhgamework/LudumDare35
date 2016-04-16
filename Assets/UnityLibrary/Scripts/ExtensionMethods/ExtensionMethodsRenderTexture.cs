using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsRenderTexture
    {
        // -- PUBLIC

        // .. EXENSION_METHODS

        public static Texture2D CloneAsTexture2D(
            this RenderTexture render_texture
            )
        {
            int
                width,
                height;
            Texture2D
                result;
            RenderTexture
                previous_active_render_texture;

            width = render_texture.width;
            height = render_texture.height;
        
            result = new Texture2D(width, height);
            result.name = render_texture.name;

            previous_active_render_texture = RenderTexture.active;
            RenderTexture.active = render_texture;

            result.ReadPixels( new Rect( 0, 0, width, height ), 0, 0 );
            result.Apply();

            RenderTexture.active = previous_active_render_texture;

            return result;
        }
    }
}
