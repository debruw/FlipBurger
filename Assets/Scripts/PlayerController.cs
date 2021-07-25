using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using Obi;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TapticPlugin;

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
    public Rigidbody[] ragdollRigidbodies;

    private void Start()
    {
        animator.SetBool("isTurning", true);
        ragdollRigidbodies = animator.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody item in ragdollRigidbodies)
        {
            item.useGravity = false;
            item.isKinematic = true;
            item.GetComponent<Collider>().enabled = false;
        }
    }

    bool isButtonUp;
    float count = 1f;
    void Update()
    {
        if (!GameManager.Instance.isGameStarted || GameManager.Instance.isGameOver)
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
            if (GameManager.Instance.currentLevel == 1 && GameManager.Instance.Tutorial1.activeSelf)
            {
                GameManager.Instance.Tutorial1.SetActive(false);
                GameManager.Instance.Tutorial2.SetActive(true);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isTurning", false);
            isButtonUp = true;
            if (GameManager.Instance.currentLevel == 1)
            {
                GameManager.Instance.Tutorial2.SetActive(false);
            }
        }

        ModelTransform.Rotate(5 * TurnSpeed * Time.deltaTime, 0, 0);

        if (isButtonUp && cam1.fieldOfView > 60)
        {
            cam1.fieldOfView -= Time.deltaTime * fowSpeed;
            cam2.fieldOfView -= Time.deltaTime * fowSpeed;
        }
    }

    public void OpenRagdoll()
    {
        animator.enabled = false;
        collector.ClearCollectings();
        collector.gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        foreach (Rigidbody item in ragdollRigidbodies)
        {
            item.useGravity = true;
            item.isKinematic = false;
            item.AddForce(Vector3.forward * 500);
            item.GetComponent<Collider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (other.CompareTag("Trampoline"))
        {
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
                TapticManager.Impact(ImpactFeedback.Medium);
            SoundManager.Instance.playSound(SoundManager.GameSounds.Jump);
            if (isButtonUp)
            {
                other.GetComponent<Animator>().SetTrigger("Jump");
                Instantiate(JumpEffect, new Vector3(other.transform.position.x, .75f, other.transform.position.z), Quaternion.identity);
                collector.CloseAllSoftBodies();
                animator.SetTrigger("CloseBun");
            }
            else
            {
                //GameOver
                Debug.Log("Game Over");
                GameManager.Instance.isGameOver = true;
                OpenRagdoll();
                StartCoroutine(GameManager.Instance.WaitAndGameLose());
            }
        }
        else if (other.CompareTag("FinishLine"))
        {
            GameManager.Instance.isGameStarted = false;
            GameManager.Instance.isGameOver = true;
            ModelTransform.DORotate(Vector3.zero, 1);
            animator.SetBool("isTurning", false);
            animator.SetBool("isFinish", true);
            StartCoroutine(GameManager.Instance.WaitAndGameWin());
        }
        else if (other.CompareTag("CloseBun"))
        {
            collector.CloseAllSoftBodies();
            animator.SetTrigger("CloseBun");
        }
    }
}
