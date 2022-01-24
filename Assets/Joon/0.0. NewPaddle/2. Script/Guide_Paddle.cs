using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide_Paddle : MonoBehaviour
{

    public static float TIMELIMIT_PADDLE = 150f;
    public static float TIMEOUT_PADDLE = 200f;

    public enum PADDLE_STATE
    {
        INTRO,
        START,
        STAGE,
        TIMELIMIT,
        TIMEDONE

    }

    public static PADDLE_STATE m_ePSTATE;
    public int intStage;
    private int intComplete;
    private string m_strOrder;

    int m_nWrongSpeed;
    int m_nWrongOrder;
    int[] m_arrWrongSpeed = new int[] { 0, 0, 0 };
    int[] m_arrWrongOrder = new int[] { 0, 0, 0 };


    float m_fTOTALTIME;
    float m_fSTARTTIME;
    List<float> m_listSTAGE = new List<float>();
    List<PaddleCollider> m_listCOLLIDER;

    void TimeCheck_Stage()
    {
        m_fTOTALTIME += Time.deltaTime;
        if (m_fTOTALTIME > TIMELIMIT_PADDLE) Debug.Log("time done");
        if (m_fTOTALTIME > TIMEOUT_PADDLE) Debug.Log("SCENE NEXT");
    }

    void TimeCheck_Start()
    {
        m_fSTARTTIME += Time.deltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_listCOLLIDER = new List<PaddleCollider>(FindObjectsOfType<PaddleCollider>());
        Make_INTRO(); //시작하기까지 시간체크
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_ePSTATE)
        {
            case PADDLE_STATE.INTRO:     Run_INTRO(); break;
            case PADDLE_STATE.START:     Run_START(); break;
            case PADDLE_STATE.STAGE:     Run_STAGE(); break;
            case PADDLE_STATE.TIMELIMIT: break;
            case PADDLE_STATE.TIMEDONE:  break;

        }

    }
    void Make_INTRO()
    {
        m_ePSTATE = PADDLE_STATE.INTRO;

    }

    void Run_INTRO()
    {
        TimeCheck_Start();

    }
    
    public void Make_START()
    {
        m_ePSTATE = PADDLE_STATE.START;
    }
    
    void Run_START()
    {
        TimeCheck_Stage();
    }

    void Make_STAGE()
    {
        m_listSTAGE.Add(m_fTOTALTIME);
        m_ePSTATE = PADDLE_STATE.STAGE;
        Debug.Log(m_listSTAGE.Count);

    }
    void Run_STAGE()
    {
        TimeCheck_Stage();
    }

    

    void Make_ALLDONE()
    {
        m_listSTAGE.Add(m_fTOTALTIME);
        Debug.Log("COMPLETE");
        foreach (PaddleCollider collider in m_listCOLLIDER) collider.GetComponent<Collider>().enabled = false; //paddle component 내부의 collider를 빼 더이상 체크 X
        m_ePSTATE = PADDLE_STATE.TIMEDONE;


    }


    void NEXTSTAGE()
    {
        Debug.Log("CHECK_STAGE");

        Make_STAGE();
        m_arrWrongOrder[intStage] = m_nWrongOrder;
        m_arrWrongSpeed[intStage] = m_nWrongSpeed;
        m_nWrongOrder = m_nWrongSpeed = intComplete = 0;
        Manager_Paddle.intStage++;
        intStage = Manager_Paddle.intStage;
        Debug.Log(intStage);
        if (intStage > 2) Make_ALLDONE();


    }
    public void Check_Order() //write in hud and datacheck 
    {
       m_nWrongOrder++; Debug.Log("CHECK_ORDER");

    }

    public void Check_Speed() //write in hud and data check
    {
        Debug.Log("CHECK_SPEED");
        m_nWrongSpeed++;
    }

    public void PaddleCheck(float time)
    {
        m_strOrder = Manager_Paddle.SDB[intStage].strHANDLE;
        Manager_Paddle.SDB[intStage].strHANDLE = null; //한바퀴 돌면 strHANDEL을 null 시킴
        if (m_strOrder != Manager_Paddle.SDB[intStage].strORDER) { Check_Order(); return; }
        if (time > 0.9f || time < -0.9f) { Check_Speed(); return; }
        intComplete += 1;
        if (Manager_Paddle.SDB[intStage].intCount == intComplete)
        {
            NEXTSTAGE();//다음 스테이지 넘어가기
            return;
        }
        Debug.Log("CHECK" + Manager_Paddle.SDB[intStage].fTime + "  ," + Manager_Paddle.SDB[intStage].intCount + " , " + intComplete);

    }
    
    Hud_Paddle Hud;
}
