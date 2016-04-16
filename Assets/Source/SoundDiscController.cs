using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Miscellaneous.ParameterBoxing.FloatParameter;

public class SoundDiscController : MonoBehaviour, IFloatParameterObserver
{
    [Serializable]
    private class BlendShapeData
    {
        public int PositiveShapeIndex;
        public int NegativeShapeIndex;

        [Tooltip("Ranging from 0..100")]
        public AFloatParameter Parameter;
    }

    [SerializeField]
    private SkinnedMeshRenderer blendshapeRenderer = null;

    [SerializeField]
    private BlendShapeData[] blendShapes = new BlendShapeData[0];

    private Dictionary<AFloatParameter, BlendShapeData> blendDataMap = new Dictionary<AFloatParameter, BlendShapeData>();

    void Start()
    {
        foreach (var blendShapeData in blendShapes)
        {
            var parameter = blendShapeData.Parameter;
            blendDataMap.Add(parameter, blendShapeData);
            parameter.RegisterObserver(this);
        }
    }

    public void NotifyParameterChanged(AFloatParameter parameter, float value)
    {
        BlendShapeData data;
        if (!blendDataMap.TryGetValue(parameter, out data))
            return;

        value = Mathf.Clamp(value, 0, 100);
        var negative_weight = Mathf.Clamp(50 - value, 0, 50) * 2;
        var positive_weight = Mathf.Clamp(value - 50, 0, 50) * 2;

        blendshapeRenderer.SetBlendShapeWeight(data.NegativeShapeIndex, negative_weight);
        blendshapeRenderer.SetBlendShapeWeight(data.PositiveShapeIndex, positive_weight);
    }
}
