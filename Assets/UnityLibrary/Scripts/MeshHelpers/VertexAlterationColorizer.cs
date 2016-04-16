using System.Collections;
using UnityEngine;

namespace MeshHelpers
{
    /// <summary>
    /// Colorizes vertex colors based on vertex position offsets (relative to the initial vertex positions)
    /// </summary>
    public class VertexAlterationColorizer : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private SkinnedMeshRenderer MeshRenderer = null;
        [SerializeField]
        private float UpdateFrequency = 1f;
        [SerializeField]
        private float MaxDistance = 0.5f;

        private Vector3[] InitialVertexPositions;
        private Color[] VertexColors;


        // .. INITIALIZATION

        void Awake()
        {
            InitialVertexPositions = MeshRenderer.sharedMesh.vertices;
            VertexColors = new Color[InitialVertexPositions.Length];
        }

        void OnEnable()
        {
            StartCoroutine("UpdateVertexColors");
        }

        // .. OPERATIONS

        private Color ColorFromValue(float value)
        {
            /*if (value < MinDistance)
            return DefaultColor;*/

            return new Color(
                Mathf.Lerp(0f, 1f, value / MaxDistance),
                Mathf.Lerp(0f, 1f, value * 1.5f / MaxDistance),
                Mathf.Lerp(0f, 1f, value * 3f / MaxDistance));
            /*
            if (value < MaxDistance * 0.333f)
            {
                return new Color(DefaultColor.r, DefaultColor.g, Mathf.Lerp(DefaultColor.b, 1f, value * 3f / MaxDistance));
            }
            else if (value < MaxDistance * 0.666f)
            {
                return new Color(DefaultColor.r, Mathf.Lerp(DefaultColor.g, 1f, value * 1.5f / MaxDistance), DefaultColor.b);
            }
            else if (value < MaxDistance)
            {
                return new Color(Mathf.Lerp(DefaultColor.r, 1f, value / MaxDistance), DefaultColor.g, DefaultColor.b);
            }

            return Color.red;*/
        }

        // .. COROUTINES

        IEnumerator UpdateVertexColors()
        {
            var elapsed = 0f;

            while (true)
            {
                elapsed += Time.deltaTime;
                if (elapsed > UpdateFrequency)
                {
                    elapsed = 0f;

                    var new_vertex_positions = MeshRenderer.sharedMesh.vertices;
                    for (int i = 0; i < VertexColors.Length; i++)
                    {
                        VertexColors[i] = ColorFromValue(Vector3.Distance(new_vertex_positions[i], InitialVertexPositions[i]));
                    }

                    MeshRenderer.sharedMesh.colors = VertexColors;
                }

                yield return null;
            }
        }


    }
}
