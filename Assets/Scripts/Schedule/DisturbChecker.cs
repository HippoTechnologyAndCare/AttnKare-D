using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbChecker : MonoBehaviour
{
    public Transform BehaviorMG;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "POINTER")
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("DISTURB START");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "POINTER")
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("DISTURB END");
        }
    }
}
