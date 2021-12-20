using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CheckData_NumCheck : MonoBehaviour
{


    [Tooltip("If not in order")]
    public float wrongorder=0;
    [Tooltip("If not in right blank")]
    public float wrongTrigger = 0;
    public float TotalTime = 0;
    [HideInInspector]
    public bool start = false;
    public bool searching = false;

    NumOrderManager manager; 
 

    // Start is called before the first frame update
    void Start()
    {
        manager = transform.GetComponent<NumOrderManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(start)TotalTime += Time.deltaTime;
    }
   
}
