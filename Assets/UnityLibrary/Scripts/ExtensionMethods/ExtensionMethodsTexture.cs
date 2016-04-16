using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsTexture
    {
        // -- PUBLIC

        // .. EXENSION_METHODS

        public static RenderTexture CloneAsRenderTexture(
            this Texture texture,
            int depth = 0,
            RenderTextureFormat format = RenderTextureFormat.ARGB32
            )
        {
            RenderTexture
                result;

            result = new RenderTexture( texture.width, texture.height, depth, format );

            result.name = texture.name;

            texture.CopyToRenderTexture( ref result );

            return result;
        }

        // ~~

        public static Texture2D CloneAsTexture2D(
            this Texture texture
            )
        {
            return texture.CloneAsRenderTexture().CloneAsTexture2D();
        }

        // ~~

        public static void CopyToRenderTexture(
            this Texture texture,
            ref RenderTexture render_texture
            )
        {
            if ( CopyMaterial == null )
            {
                CopyMaterial = new Material( Shader.Find( "IStyling/CopyTextureToRenderTexture" ) );
            }

            Graphics.Blit( texture, render_texture, CopyMaterial );
        }

        // -- PRIVATE

        // .. VARIABLES

        static Material
            CopyMaterial;

    }
}
