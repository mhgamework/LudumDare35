using UnityEngine;
using System.Collections;

public class GooglyEyesController : MonoBehaviour
{
    [SerializeField]
    private Camera theCamera = null;
    [SerializeField]
    private GooglyEye leftEye = null;
    [SerializeField]
    private GooglyEye rightEye = null;
    [SerializeField]
    private Mouth mouth = null;

    public bool IsHappy = true;

    void Start()
    {
        if (theCamera == null)
            theCamera = Camera.main;
    }

    void Update()
    {
        var mouse_pos = theCamera.ScreenToWorldPoint(Input.mousePosition);
        leftEye.SetTarget(mouse_pos);
        rightEye.SetTarget(mouse_pos);

        mouth.SetIsHappy(IsHappy);
    }
}
