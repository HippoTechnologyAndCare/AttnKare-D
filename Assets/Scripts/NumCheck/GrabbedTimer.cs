using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class GrabbedTimer : MonoBehaviour
{
    public bool NcsryB;
    public bool UncsryB;
    // Start is called before the first frame update
    float NcsryGrab = 0f;
    float UncsryGrab = 0f;

    
    void Start()
    {
        
    }
    void Update()
    {
        if (UncsryB)
        {
            UncsryGrab += Time.deltaTime;
        }
        if (NcsryB)
        {
            NcsryGrab += Time.deltaTime;
        }

    }

    public void ResetGrabbed()
    {
        UncsryB = false;
        NcsryB = false;

    }
    
   

    // Update is called once per frame
    
}
