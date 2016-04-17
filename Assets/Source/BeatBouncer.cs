using System;
using UnityEngine;
using System.Collections;
using Miscellaneous.Easing;

public class BeatBouncer : MonoBehaviour
{
    [SerializeField]
    private BeatEmitter beatEmitter = null;
    [SerializeField]
    private float bounceFactor = 1.2f;

    private Vector3 targetScale;
    private bool toggle;

    private Transform thisTransform;

    void Start()
    {
        beatEmitter.OnBeatChanged.AddListener(OnBeatEmitted);
        thisTransform = GetComponent<Transform>();

        StartCoroutine("Bounce");
    }

    private void OnBeatEmitted(int arg0)
    {
        targetScale = toggle ? Vector3.one : Vector3.one * bounceFactor;
        toggle = !toggle;
    }

    private IEnumerator Bounce()
    {
        while (true)
        {
            thisTransform.localScale = Vector3.Lerp(thisTransform.localScale, targetScale, 0.1f);

            yield return null;
        }
    }

}
