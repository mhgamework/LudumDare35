using UnityEngine;
using System.Collections;

public class Mouth : MonoBehaviour
{
    [SerializeField]
    private RectTransform happyMouth = null;
    [SerializeField]
    private RectTransform sadMouth = null;

    private float initialScaleHappy;
    private float initialScaleSad;

    private float happyFactor = 1f;
    private float sadFactor = 1f;

    void Start()
    {
        initialScaleHappy = happyMouth.localScale.x;
        initialScaleSad = sadMouth.localScale.x;
    }

    public void SetMood(GooglyEyesController.Mood mood)
    {
        switch (mood)
        {
            case GooglyEyesController.Mood.HAPPY:
                happyFactor = 1f;
                sadFactor = 0f;
                break;
            case GooglyEyesController.Mood.SAD:
                happyFactor = 0f;
                sadFactor = 1f;
                break;
            case GooglyEyesController.Mood.SUPRISED:
                happyFactor = 1f;
                sadFactor = 1f;
                break;
        }
    }

    void Update()
    {
        happyMouth.localScale = new Vector3(initialScaleHappy, Mathf.Lerp(happyMouth.localScale.y, happyFactor * initialScaleHappy, 0.1f), initialScaleHappy);
        sadMouth.localScale = new Vector3(initialScaleSad, Mathf.Lerp(sadMouth.localScale.y, sadFactor * initialScaleSad, 0.1f), initialScaleSad);
    }
}
