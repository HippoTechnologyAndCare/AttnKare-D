using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CheckData_NumCheck : MonoBehaviour
{


    [Header("Check Order")]
    public float wrongorder=0;
    public float wrongTrigger = 0;
    float crnt=0;
    float prev=0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckOrder(float now)
    {
        crnt = now;
        if(prev == 0&& crnt != 1)
        {
            Debug.Log("Should start from 1");
        }
        if(crnt != prev+1)
        {
            wrongorder += 1;
            Debug.Log("Wrong Order");
        }


    }
}
