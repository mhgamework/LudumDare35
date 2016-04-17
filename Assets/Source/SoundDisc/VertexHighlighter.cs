using UnityEngine;
using System.Collections;
using ExtensionMethods;
using MeshHelpers;
using Miscellaneous.Easing;

public class VertexHighlighter : AEntryVisualizer
{
    [SerializeField]
    private GeneralMesh generalMesh = null;

    [SerializeField]
    private Color defaultColor = Color.white;
    [SerializeField]
    private Color highLightColor = Color.cyan;

    [SerializeField]
    [Range(0, 360)]
    private float startAngle = 0f;
    [SerializeField]
    [Range(0, 360)]
    private float endAngle360 = 45f;

    private float[] vertexWeights = new float[0]; //values between 0 and 1
    private Mesh mesh;

    [SerializeField]
    [Range(0, 10f)]
    private float angleWeightModifier = 1f;
    [SerializeField]
    private float radiusWeightThresholdLow = 0.5f;
    [SerializeField]
    private float radiusWeightThresholdHigh = 1f;

    [SerializeField]
    private float animationTime = 0.25f;


    private Color[] highLightColors;
    void Start()
    {
        mesh = generalMesh.Mesh;
        RecalculateVertexMask();

        var colors = new Color[mesh.vertexCount];
        highLightColors = new Color[mesh.vertexCount];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = defaultColor;
            highLightColors[i] = Color.Lerp(defaultColor, highLightColor, vertexWeights[i]);
        }
        mesh.colors = colors;




    }

    void OnValidate()
    {
        if (Application.isPlaying && mesh != null)
        {
            for (int i = 0; i < highLightColors.Length; i++)
            {
                highLightColors[i] = Color.Lerp(defaultColor, highLightColor, vertexWeights[i]);
            }
        }
    }

    private void RecalculateVertexMask()
    {
        var vertices = mesh.vertices;
        vertexWeights = new float[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            vertexWeights[i] = GetVertexWeight(vertices[i]);
        }
    }

    private float GetVertexWeight(Vector3 v)
    {
        var flat_vector = new Vector2(v.x, v.y);

        var angle = Vector2.up.Angle360(flat_vector);

        if (angle < startAngle || angle > endAngle360 || v.magnitude < radiusWeightThresholdLow)
            return 0f;

        var total_angle = endAngle360 - startAngle;
        var avg_angle = startAngle + total_angle * 0.5f;

        var dist_weight = Mathf.Clamp01(v.magnitude / radiusWeightThresholdHigh);

        return (1f - Mathf.Abs(avg_angle - angle) / (total_angle * angleWeightModifier)) * dist_weight;
    }

    public override void Highlight()
    {
        StartCoroutine("HighlightRoutine");
    }

    private IEnumerator HighlightRoutine()
    {
        var colors = new Color[mesh.vertexCount];

        Color[] ori_colors;

        var fade_time = animationTime; //* 0.5f;
        /*
        //fade in
        var elapsed = 0f;
        while (elapsed < fade_time)
        {
            ori_colors = mesh.colors;
            for (int i = 0; i < colors.Length; i++)
            {
                if (vertexWeights[i] > 0)
                    colors[i] = EasingFunctions.Ease(EasingFunctions.TYPE.In, elapsed / fade_time, defaultColor, highLightColors[i]);
                else
                    colors[i] = ori_colors[i];
            }
            mesh.colors = colors;

            elapsed += Time.deltaTime;
            yield return null;
        }
        */
        //fade out
        var elapsed = 0f;
        while (elapsed < fade_time)
        {
            ori_colors = mesh.colors;
            for (int i = 0; i < colors.Length; i++)
            {
                if (vertexWeights[i] > 0)
                    colors[i] = EasingFunctions.Ease(EasingFunctions.TYPE.Out, elapsed / fade_time, highLightColors[i], defaultColor);
                else
                    colors[i] = ori_colors[i];
            }
            mesh.colors = colors;

            elapsed += Time.deltaTime;
            yield return null;
        }

        //restore original
        ori_colors = mesh.colors;
        for (int i = 0; i < colors.Length; i++)
        {
            if (vertexWeights[i] > 0)
                colors[i] = defaultColor;
            else
                colors[i] = ori_colors[i];
        }
        mesh.colors = colors;
    }
}
