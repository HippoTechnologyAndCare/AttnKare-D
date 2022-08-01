using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CheckData_NumCheck : MonoBehaviour
{

    public AutoButton AUTO;
    [Tooltip("If not in order")]
    public float wrongorder_stage1=0;
    public float wrongorder_stage2 = 0;
    [Tooltip("If not in right blank")]
    public float wrongTrigger_stage1 = 0;
    public float wrongTrigger_stage2 = 0;
    public float TotalTime = 0;
    public float distractedBy = 0;
    public float wrongColor = 0;
    public float time_Window = 0;
    public float time_Behind = 0;
    public float time_Disctracted = 0; //���� �� �ð�
    public int hitCharacter = 0;
    public List<float> hitSpeed;

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
    float data_711; // skip
    float data_712; // question
    float data_713; // window
    float data_714; //behind
    float data_715; //watching screen
    int data_716; //hit character
    List<float> data_717; //hit per time

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
    }

    public void Stage2()
    {
        start = false;
        data_701 = TotalTime;
        data_703 = TotalTime - data_702;
    }

    public void Answer(bool answer)
    {
        data_712 = answer ? 1 : 0;
    }
    public void Skip()
    {
        data_711 = 1;
    }
    public void GetAllData()
    {
        data_701 = TotalTime;
        data_704 = wrongorder_stage1;
        data_705 = wrongTrigger_stage1;
        data_706 = distractedBy;
        data_707 = AUTO.m_fBothering;
        data_708 = wrongorder_stage2;
        data_709 = wrongTrigger_stage2;
        data_710 = wrongColor;
        data_713 = time_Window;
        data_714 = time_Behind;
        data_715 = time_Disctracted;
        data_716 = hitCharacter;
      //  data_717 = hitSpeed;

        arrData = new float[] { data_701, data_702, data_703, data_704, data_705, data_706, data_707, data_708, data_709, data_710, data_711, data_712 , data_713, data_714, data_715, data_716  }; //data_717
        for (int i = 0; i < arrData.Length; i++)
        {
            Debug.Log(arrData[i]);
        }
    }
   
}
