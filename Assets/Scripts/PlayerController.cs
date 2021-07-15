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
    public int TurnSpeed, zSpeed;

    public int fowSpeed = 10;
    public Animator animator;
    public Camera cam1, cam2;
    public GameObject Bun2;
    public Collector collector;
    public GameObject JumpEffect;
    bool isStarted, isGameOver;

    private void Start()
    {
        Application.targetFrameRate = 60;
        animator.SetBool("isTurning", true);
    }

    bool isButtonUp;
    float count = 1f;
    void Update()
    {
        if (!isStarted || isGameOver)
        {
            return;
        }
        //increase distance
        distance += zSpeed * Time.deltaTime;

        transform.position = BGCcMath.CalcPositionByDistance(distance);

        if (Input.GetMouseButton(0))
        {
            ModelTransform.Rotate(100 * TurnSpeed * Time.deltaTime, 0, 0);
            if (cam1.fieldOfView < 68)
            {
                cam1.fieldOfView += Time.deltaTime * fowSpeed;
                cam2.fieldOfView += Time.deltaTime * fowSpeed;
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

        if (isButtonUp && cam1.fieldOfView > 60)
        {
            cam1.fieldOfView -= Time.deltaTime * fowSpeed;
            cam2.fieldOfView -= Time.deltaTime * fowSpeed;
        }
    }

    public GameObject Restartbutton;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trampoline"))
        {
            //if (isButtonUp)
            //{
            other.GetComponent<Animator>().SetTrigger("Jump");
            Instantiate(JumpEffect, new Vector3(other.transform.position.x, .75f, other.transform.position.z), Quaternion.identity);
            collector.CloseAllSoftBodies();
            animator.SetTrigger("CloseBun");
            //}
            //else
            //{
            //    //GameOver
            //    Debug.Log("Game Over");
            //    isGameOver = true;
            //}
        }
        else if (other.CompareTag("FinishLine"))
        {
            isStarted = false;
            isGameOver = true;
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
