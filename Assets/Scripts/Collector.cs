using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;

public class Collector : MonoBehaviour
{
    Vector3 lastPosition;
    public List<GameObject> collecteds;
    public GameObject PlusEffect;

    public void ClearCollectings()
    {
        lastPosition = Vector3.zero;
        foreach (GameObject item in collecteds)
        {
            item.GetComponent<SkinnedMeshRenderer>().enabled = false;
            Destroy(item, 1);
        }
        collecteds.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            Debug.Log("Collect");
            GameManager.Instance.AddBurger();
            Instantiate(PlusEffect, new Vector3(transform.position.x + 1f, transform.position.y + .5f, transform.position.z), Quaternion.identity);
            other.GetComponent<Collider>().enabled = false;
            other.transform.parent = transform;
            lastPosition += new Vector3(0, 0, other.GetComponent<BoxCollider>().size.z);
            other.transform.localPosition = lastPosition;
            other.transform.localEulerAngles = Vector3.zero;
            other.transform.localScale = Vector3.one;
            collecteds.Add(other.gameObject);
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
            SoundManager.Instance.playSound(SoundManager.GameSounds.Collect);
        }
    }

    public void CloseAllSoftBodies()
    {
        foreach (GameObject item in collecteds)
        {
            item.GetComponent<ObiSoftbody>().enabled = false;
        }
    }
}
