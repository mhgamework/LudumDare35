using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestEntryVisualizer : AEntryVisualizer
{
    [SerializeField]
    private Image image = null;

    public override void Highlight()
    {
        StartCoroutine("HighlightRoutine");
    }

    private IEnumerator HighlightRoutine()
    {
        image.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        image.color = Color.white;
    }
}
