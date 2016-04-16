using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
    public class LineRenderer2D : MonoBehaviour
    {
        // .. ATTRIBUTES
        [SerializeField]
        private Texture2D Image = null;
        [SerializeField]
        private ADrawableController[] Drawables = null;
        [SerializeField]
        private Renderer DebugRenderer = null;
        private RenderTexture DebugRenderTarget;

        private RenderTexture EmptyTexture;
        private RenderTexture RenderTarget_A;
        private RenderTexture RenderTarget_B;

        private Material PencilMaterial;
        private bool IsInitialized;
        private float BrushSize = 0.005f;

        private List<IDrawable2D> StuffToDraw = new List<IDrawable2D>();

        // .. INITIALIZATION

        void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            if (IsInitialized)
                return;

            foreach (var drawable in Drawables)
            {
                StuffToDraw.Add(drawable.GetDrawable());
            }

            var empty_tex2d = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            empty_tex2d.SetPixel(0, 0, new Color(1, 1, 1, 0));
            empty_tex2d.Apply();

            PencilMaterial = new Material(Shader.Find("IStyling/PencilTool"));

            EmptyTexture = new RenderTexture(1024, 1024, 0);
            RenderTarget_A = new RenderTexture(1024, 1024, 0);
            RenderTarget_B = new RenderTexture(1024, 1024, 0);

            Graphics.Blit(empty_tex2d, EmptyTexture, new Material(Shader.Find("IStyling/CopyTextureToRenderTexture")));

            if (DebugRenderer != null)
            {
                DebugRenderTarget = new RenderTexture(1024, 1024, 0);
                DebugRenderer.material.mainTexture = DebugRenderTarget;
            }

            IsInitialized = true;
        }


        // .. OPERATIONS

        public void OutputToTexture(ref RenderTexture render_target)
        {
            TryInitialize();

            var all_lines = new List<Line2D>();
            foreach (var drawable2D in StuffToDraw)
            {
                all_lines.AddRange(drawable2D.Get2DComponents());
            }

            var draw_count = all_lines.Count;
            for (int i = 0; i < draw_count; i++)
            {
                RenderTexture source = i % 2 == 0 ? RenderTarget_A : RenderTarget_B;
                RenderTexture target = i % 2 == 0 ? RenderTarget_B : RenderTarget_A;

                if (i == 0)
                    source = EmptyTexture;
                if (i == draw_count - 1)
                    target = render_target;

                DrawLine(all_lines[i], ref source, ref target);
            }
        }

        private void DrawLine(Line2D line, ref RenderTexture source, ref RenderTexture target)
        {
            PencilMaterial.SetTexture("_Template", Image);
            PencilMaterial.SetVector("_TemplateSize", new Vector2(BrushSize, BrushSize));
            PencilMaterial.SetVector("_PencilUVPositionStart", line.P0);
            PencilMaterial.SetVector("_PencilUVPositionEnd", line.P1);

            Graphics.Blit(source, target, PencilMaterial);
        }
       
        void Update()
        {
            if (DebugRenderer != null)
                OutputToTexture(ref DebugRenderTarget);
        }
    }
}
