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
    float data_702; //stage 1 time
    float data_703; //stage 2 time
    float data_704; //wrong order
    float data_705; //wrong trigger
    float data_706; //distracted by
    float data_707; //bothering
    float data_708; //stage 2 wrong order
    float data_709;//stage 2 wrong trigger
    float data_710;//stage 2 wrong color

    public float[] arrData;

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
        data_702 = TotalTime;
        data_704 = wrongorder;
        data_705 = wrongTrigger;
        data_706 = distractedBy;
        data_707 = AUTO.m_fBothering;

        Debug.Log("data");
        wrongorder = wrongTrigger = 0;
    }

    public void Stage2()
    {
        start = false;
        data_701 = TotalTime;
        data_703 = TotalTime - data_702;
        data_708 = wrongorder;
        data_709 = wrongTrigger;
        data_710 = wrongColor;

        arrData = new float[] { data_701, data_702, data_703, data_704, data_705, data_706, data_707, data_708, data_709, data_710 };
        for(int i =0; i < arrData.Length; i++)
        {
            Debug.Log(arrData[i]);
        }
    }
   
}
