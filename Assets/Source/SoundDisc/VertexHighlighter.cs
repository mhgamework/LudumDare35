using UnityEngine;
using System.Collections;
using ExtensionMethods;
using MeshHelpers;

public class VertexHighlighter : AEntryVisualizer
{
    [SerializeField]
    private GeneralMesh generalMesh = null;

    [SerializeField]
    private Color defaultColor = Color.white;
    [SerializeField]
    private Color highLightColor = Color.cyan;

    [SerializeField]
    private bool[] vertexMask = new bool[0];
    [SerializeField]
    private bool recalculateMask;

    [SerializeField]
    [Range(0, 360)]
    private float startAngle = 0f;
    [SerializeField]
    [Range(0, 360)]
    private float endAngle360 = 45f;

    private Mesh mesh;

    void Start()
    {
        mesh = generalMesh.Mesh;
    }

    void OnValidate()
    {
        if (recalculateMask && Application.isEditor)
        {
            RecalculateVertexMask();
            recalculateMask = false;
        }
    }

    private void RecalculateVertexMask()
    {
        mesh = generalMesh.Mesh;

        var vertices = mesh.vertices;
        vertexMask = new bool[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            vertexMask[i] = isValidVertex(vertices[i]);
        }
    }

    private bool isValidVertex(Vector3 v)
    {
        var flat_vector = new Vector2(v.x, v.y);

        var angle = Vector2.up.Angle360(flat_vector);
        return angle >= startAngle && angle <= endAngle360;
    }

    public override void Highlight()
    {
        StartCoroutine("HighlightRoutine");
    }

    private IEnumerator HighlightRoutine()
    {
        var colors = new Color[mesh.vertexCount];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.green;
        }

        mesh.colors = colors;

        yield return new WaitForSeconds(0.15f);

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.red;
        }
        mesh.colors = colors;
    }
}
