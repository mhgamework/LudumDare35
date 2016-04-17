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
    private float happyFactor = 1f; //1 = happy, 0 = sad

    void Start()
    {
        initialScaleHappy = happyMouth.localScale.x;
        initialScaleSad = sadMouth.localScale.x;
    }

    public void SetIsHappy(bool is_happy)
    {
        happyFactor = is_happy ? 1f : 0f;
    }

    void Update()
    {
        happyMouth.localScale = new Vector3(initialScaleHappy, Mathf.Lerp(happyMouth.localScale.y, happyFactor * initialScaleHappy, 0.1f), initialScaleHappy);
        sadMouth.localScale = new Vector3(initialScaleSad, Mathf.Lerp(sadMouth.localScale.y, (1 - happyFactor) * initialScaleSad, 0.1f), initialScaleSad);
    }
}
