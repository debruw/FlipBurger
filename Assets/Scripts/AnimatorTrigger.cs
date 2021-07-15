using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    public Collector collector;

    public void CloseCollecteds()
    {
        collector.ClearCollectings();
    }
}
