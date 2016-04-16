using System;
using UnityEngine;

namespace Miscellaneous
{
    /// <summary>
    /// Helper class for interpreting and retrieving values from a texture.
    /// </summary>
    public class TextureValueRetriever : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Texture2D SourceTexture = null;

        private float sourceWidth;
        private float sourceHeight;
        private bool isInitialized;

        // .. INITIALIZATION

        void Start()
        {
            TryInitialize();
        }

        public void TryInitialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;

            sourceWidth = SourceTexture.width;
            sourceHeight = SourceTexture.height;
        }

        // .. OPERATIONS

        public void SetTexture(Texture2D texture)
        {
            SourceTexture = texture;
            isInitialized = false;
        }

        public bool HasTexture(Texture2D texture)
        {
            return SourceTexture == texture;
        }

        public float GetValue(Vector2 uv, ref Func<Color, float> converter_function)
        {
            return converter_function(SourceTexture.GetPixel(Mathf.FloorToInt(uv.x * sourceWidth), Mathf.FloorToInt(uv.y * sourceHeight)));
        }

        public float[] GetValues(Vector2[] uv, ref Func<Color, float> converter_function)
        {
            var values = new float[uv.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = GetValue(uv[i], ref converter_function);
            }
            return values;
        }

        public bool GetValue(Vector2 uv, ref Func<Color, bool> converter_function)
        {
            return converter_function(SourceTexture.GetPixel(Mathf.FloorToInt(uv.x * sourceWidth), Mathf.FloorToInt(uv.y * sourceHeight)));
        }

        public bool[] GetValues(Vector2[] uv, ref Func<Color, bool> converter_function)
        {
            var values = new bool[uv.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = GetValue(uv[i], ref converter_function);
            }
            return values;
        }

        public Color GetValue(Vector2 uv)
        {
            return SourceTexture.GetPixel(Mathf.FloorToInt(uv.x * sourceWidth), Mathf.FloorToInt(uv.y * sourceHeight));
        }

        public Color[] GetValues(Vector2[] uv)
        {
            var values = new Color[uv.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = GetValue(uv[i]);
            }
            return values;
        }

    }
}
