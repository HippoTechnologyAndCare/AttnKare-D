using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CheckData_NumCheck : MonoBehaviour
{

    public AutoButton AUTO;
    [Tooltip("If not in order")]
    public float wrongorder=0;
    [Tooltip("If not in right blank")]
    public float wrongTrigger = 0;
    public float TotalTime = 0;
    public float distractedBy = 0;
    public float wrongColor = 0; 

    [HideInInspector]
    public bool start = false;
    public bool searching = false;


    float data_701; //total time
    float data_702; //wrong order
    float data_703; //wrong trigger
    float data_704; //distracted by
    float data_705; //bothering
    float data_706; //stage 2 wrong order
    float data_707; //stage 2 wrong trigger
    float data_708; //stage 2 wrong color

    Guide_NumCheck Guide; 
 

    // Start is called before the first frame update
    void Start()
    {
        Guide = GameObject.Find("Guide").GetComponent<Guide_NumCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if(start)TotalTime += Time.deltaTime;
    }
    public void Stage1()
    {
        data_702 = wrongorder;
        data_703 = wrongTrigger;
        data_704 = distractedBy;
        data_705 = AUTO.m_fBothering;

        wrongorder = wrongTrigger = 0;
    }
   
}
