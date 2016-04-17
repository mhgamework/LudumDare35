using UnityEngine;
using System.Collections;

public class GooglyEye : MonoBehaviour
{
    [SerializeField]
    private Transform irisTransform = null;
    [SerializeField]
    private float radius = 1f;

    private Vector3 initial_pos;
    private Vector3 lookAtPosition;

    void Start()
    {
        initial_pos = irisTransform.position;
        lookAtPosition = initial_pos;
    }

    public void SetTarget(Vector3 position)
    {
        lookAtPosition = position;
    }

    void Update()
    {
        var dir = lookAtPosition - initial_pos;
        dir = new Vector3(dir.x, 0f, dir.z);
        dir.Normalize();
        irisTransform.position = initial_pos + dir * radius * irisTransform.lossyScale.x;
    }
}
