using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    Vector3 lastPosition;
    public List<GameObject> collecteds;

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
            other.GetComponent<Collider>().enabled = false;
            other.transform.parent = transform;
            lastPosition += new Vector3(0, 0, other.GetComponent<BoxCollider>().size.z);
            other.transform.localPosition = lastPosition;
            other.transform.localEulerAngles = Vector3.zero;
            other.transform.localScale = Vector3.one;            
            collecteds.Add(other.gameObject);
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
