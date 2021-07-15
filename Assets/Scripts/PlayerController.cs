using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using Obi;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float distance;
    public BGCcMath BGCcMath;
    public Transform ModelTransform;
    [Range(0, 10)]
    public int TurnSpeed;
    public Animator animator;
    public Camera cam;
    public GameObject Bun2;
    public Collector collector;
    public GameObject JumpEffect;
    bool isStarted;

    private void Start()
    {
        Application.targetFrameRate = 60;
        animator.SetBool("isTurning", true);
    }

    bool isButtonUp;
    float count = 1;
    void Update()
    {
        if (!isStarted)
        {
            return;
        }
        //increase distance
        distance += 5 * Time.deltaTime;

        transform.position = BGCcMath.CalcPositionByDistance(distance);

        if (Input.GetMouseButton(0))
        {
            ModelTransform.Rotate(100 * TurnSpeed * Time.deltaTime, 0, 0);
            if (cam.fieldOfView < 68)
            {
                cam.fieldOfView += Time.deltaTime * 10;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isTurning", true);
            isButtonUp = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isTurning", false);
            isButtonUp = true;
        }

        if (isButtonUp && count > 0)
        {
            count -= Time.deltaTime;
            ModelTransform.Rotate(20 * count * TurnSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            isButtonUp = false;
            count = 1;
        }

        if (isButtonUp && cam.fieldOfView > 60)
        {
            cam.fieldOfView -= Time.deltaTime * 10;
        }
    }

    public GameObject Restartbutton;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trampoline"))
        {
            //TODO trampoline animation
            //TODO closing burger animation
            other.GetComponent<Animator>().SetTrigger("Jump");
            Instantiate(JumpEffect, new Vector3(other.transform.position.x, .75f, other.transform.position.z), Quaternion.identity);
            collector.CloseAllSoftBodies();
            animator.SetTrigger("CloseBun");
        }
        else if (other.CompareTag("FinishLine"))
        {
            ModelTransform.DORotate(Vector3.zero, 1);
            animator.SetBool("isTurning", false);
            animator.SetBool("isFinish", true);
            Restartbutton.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SartGame()
    {
        isStarted = true;
        animator.SetBool("isTurning", false);
    }
}
