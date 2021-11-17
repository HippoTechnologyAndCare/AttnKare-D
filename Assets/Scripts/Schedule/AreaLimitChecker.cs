using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLimitChecker : MonoBehaviour
{
    public Transform BehaviorMG;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "HeadCollision")
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE END");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.name == "HeadCollision")
        {
            BehaviorMG.GetComponent<BNG.CollectData>().AddTimeStamp("ESCAPE START");
        }
    }
}
