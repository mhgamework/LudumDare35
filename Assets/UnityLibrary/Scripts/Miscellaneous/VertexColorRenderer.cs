using UnityEngine;

namespace Miscellaneous
{
    /// <summary>
    /// Provides functionality to switch between displaying the materials of a mesh or its vertex colors.
    /// ASSUMES NO OTHER COMPONENT ALTERS THE MESH MATERIALS!
    /// </summary>
    public class VertexColorRenderer : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private string ShaderName = "IStyling/VertexColorShader";
        [SerializeField]
        private string[] FloatPropertiesToCopyFromMainShader = new string[] { "_ZValueOffset" };
        [SerializeField]
        private string[] VectorPropertiesToCopyFromMainShader = new string[0];
        [SerializeField]
        [Tooltip("The renderer component from which to visualize vertex colors. Can be set from code.")]
        private Renderer MeshRenderer = null;
        [SerializeField]
        private bool ShowVertexColorsAtStart = false;

        private Material[] restoreMaterials;
        private bool showingVertexColors;


        // .. INITIALIZATION

        void Start()
        {
            if (ShowVertexColorsAtStart && MeshRenderer != null)
                ShowVertexColors();
        }


        // .. OPERATIONS

        public void SetRenderer(Renderer renderer)
        {
            MeshRenderer = renderer;
            showingVertexColors = false;

            if (ShowVertexColorsAtStart)
                ShowVertexColors();
        }

        public void ShowVertexColors()
        {
            if (showingVertexColors)
                return;

            showingVertexColors = true;
            restoreMaterials = MeshRenderer.materials;

            var vertex_color_materials = new Material[restoreMaterials.Length];
            for (int i = 0; i < vertex_color_materials.Length; i++)
            {
                vertex_color_materials[i] = new Material(Shader.Find(ShaderName));

                foreach (var property in FloatPropertiesToCopyFromMainShader)
                {
                    if (restoreMaterials[i].HasProperty(property))
                        vertex_color_materials[i].SetFloat(property, restoreMaterials[i].GetFloat(property));
                }
                foreach (var property in VectorPropertiesToCopyFromMainShader)
                {
                    if (restoreMaterials[i].HasProperty(property))
                        vertex_color_materials[i].SetVector(property, restoreMaterials[i].GetVector(property));
                }
            }
            MeshRenderer.materials = vertex_color_materials;
        }

        public void ShowOriginalMaterials()
        {
            if (!showingVertexColors)
                return;

            MeshRenderer.materials = restoreMaterials;
            showingVertexColors = false;
        }

        public void ToggleVertexColors()
        {
            if (showingVertexColors)
                ShowOriginalMaterials();
            else
                ShowVertexColors();
        }

    }
}
