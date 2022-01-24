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
        ALLDONE
    }

    public static PADDLE_STATE m_ePSTATE;
    public int intStage;
    private int m_nComplete;
    private string m_strOrder;

    public Animation FriendAnimation;
    private int m_nAnimation;

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
        if (m_fTOTALTIME > TIMELIMIT_PADDLE) Hud.ErrorMessage("time limit");
        if (m_fTOTALTIME > TIMEOUT_PADDLE) Hud.ErrorMessage("time over");
    }

    void TimeCheck_Start()
    {
        m_fSTARTTIME += Time.deltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_listCOLLIDER = new List<PaddleCollider>(FindObjectsOfType<PaddleCollider>());
        Hud = GameObject.Find("Hud_Paddle").GetComponent<Hud_Paddle>();
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
            case PADDLE_STATE.ALLDONE:   break;
        }
    }
    void Make_INTRO()
    {
        foreach (PaddleCollider collider in m_listCOLLIDER) collider.GetComponent<Collider>().enabled = false;
        m_ePSTATE = PADDLE_STATE.INTRO;
    }

    void Run_INTRO()
    {
        TimeCheck_Start();
    }
    
    public void Make_START()
    {
        foreach (PaddleCollider collider in m_listCOLLIDER) collider.GetComponent<Collider>().enabled = true;
        m_ePSTATE = PADDLE_STATE.START;
    }
    
    void Run_START()
    {
        TimeCheck_Stage();
    }

    void Make_STAGE()
    {
        m_listSTAGE.Add(m_fTOTALTIME); 
        string animName = "Paddle" + intStage.ToString(); //Change Animation of Character
        FriendAnimation.Play(animName);
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
        m_ePSTATE = PADDLE_STATE.ALLDONE;
    }


    void NEXTSTAGE()
    {
        Debug.Log("CHECK_STAGE");
        Make_STAGE();
        m_arrWrongOrder[intStage] = m_nWrongOrder;
        m_arrWrongSpeed[intStage] = m_nWrongSpeed;
        m_nWrongOrder = m_nWrongSpeed = m_nComplete = 0;
        Manager_Paddle.intStage++;
        intStage = Manager_Paddle.intStage;
        Debug.Log(intStage);
        if (intStage > 2) Make_ALLDONE();
    }
    public void Check_Order() //write in hud and datacheck 
    {
        Hud.ErrorMessage("wrong order");
        m_nWrongOrder++;
        Debug.Log("CHECK_ORDER");

    }

    public void Check_Speed() //write in hud and data check
    {
        Debug.Log("CHECK_SPEED");
        Hud.ErrorMessage("wrong speed");
        m_nWrongSpeed++;
    }

    public void PaddleCheck(float time)
    {
        m_strOrder = Manager_Paddle.SDB[intStage].strHANDLE;
        Manager_Paddle.SDB[intStage].strHANDLE = null; //한바퀴 돌면 strHANDEL을 null 시킴
        if (m_strOrder != Manager_Paddle.SDB[intStage].strORDER) { Check_Order(); return; }
        if (time > 0.9f || time < -0.9f) { Check_Speed(); return; }
        m_nComplete += 1;
        if (Manager_Paddle.SDB[intStage].intCount == m_nComplete)
        {
            NEXTSTAGE();//다음 스테이지 넘어가기
            return;
        }
        Debug.Log("CHECK" + Manager_Paddle.SDB[intStage].fTime + "  ," + Manager_Paddle.SDB[intStage].intCount + " , " + m_nComplete);
    }
    
    Hud_Paddle Hud;
}
