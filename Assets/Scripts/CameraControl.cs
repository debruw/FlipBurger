using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public Transform camTransform;
    public Vector3 Offset;
    public float SmoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public bool isCamTurn;

    // Start is called before the first frame update
    void Start()
    {
        Offset = camTransform.position - target.position;
    }

    private void Update()
    {
        transform.LookAt(target.position + new Vector3(0, 1, 0));
        transform.Translate(Vector3.right * Time.deltaTime * 5);
    }

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + Offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
    }
}
