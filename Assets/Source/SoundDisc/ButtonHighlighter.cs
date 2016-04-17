using UnityEngine;
using System.Collections;
using Miscellaneous.Easing;
using UnityEngine.UI;

public class ButtonHighlighter : AEntryVisualizer
{
    [SerializeField]
    private Image image = null;

    [SerializeField]
    private Color highLightColor = Color.white;
    [SerializeField]
    private float animationTime = 1f;

    public override void Highlight()
    {
        StartCoroutine("HighlightRoutine");
    }

    private IEnumerator HighlightRoutine()
    {
        var start_color = image.color;

        var elapsed = 0f;
        while (elapsed < animationTime)
        {
            image.color = EasingFunctions.Ease(EasingFunctions.TYPE.Out, elapsed / animationTime, highLightColor, start_color);
            elapsed += Time.deltaTime;
            yield return null;
        }

        image.color = start_color;
    }
}
