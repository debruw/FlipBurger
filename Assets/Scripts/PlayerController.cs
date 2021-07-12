using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;

public class PlayerController : MonoBehaviour
{
    private float distance;
    public BGCcMath BGCcMath;
    public Transform ModelTransform;
    [Range(0, 10)]
    public int TurnSpeed;
    public Animator animator;

    void Update()
    {
        //increase distance
        distance += 5 * Time.deltaTime;

        //calculate position and tangent
        Vector3 tangent;
        transform.position = BGCcMath.CalcPositionAndTangentByDistance(distance, out tangent);

        if (Input.GetMouseButton(0))
        {
            ModelTransform.Rotate(100 * TurnSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isTurning", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isTurning", false);
        }
    }
}
