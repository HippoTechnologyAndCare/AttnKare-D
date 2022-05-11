using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastsc : MonoBehaviour
{
    private RaycastHit hit;

    void Update()
    {
        
            Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
    }

   
}
