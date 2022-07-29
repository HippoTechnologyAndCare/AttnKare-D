using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distracted : MonoBehaviour
{
    public CheckData_NumCheck data;
    // Start is called before the first frame updat
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "HeadCollision")
        {
            data.time_Behind += Time.deltaTime;
        }
    }
}
