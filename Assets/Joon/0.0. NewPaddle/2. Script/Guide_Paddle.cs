using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UserData;
using KetosGames.SceneTransition;
using EPOOutline;

public class Guide_Paddle : MonoBehaviour
{

    public static float TIMELIMIT_PADDLE = 180f;
    public static float TIMEOUT_PADDLE = 220f;

    public enum PADDLE_STATE
    {
        INTRO,
        START,
        STAGE,
        ALLDONE
    }

    public static PADDLE_STATE m_ePSTATE;
    [HideInInspector]public int intStage;

    public MoveVehicle CableCar;
    private int m_nComplete;
    private string m_strOrder;
    public GrabPaddle GrabPaddle;
    public Animation FriendAnimation;
    public bool m_bStamp = false; //스탬프 1회씩만 박는용 bool
    float m_nWrongSpeed;
    float m_nWrongOrder;

    float m_fPrevTime = 0;
    float m_fStageTime = 0;
    float m_fTOTALTIME =0;
    float m_fSTARTTIME;
    float[] m_listSTAGE = new float[] { 0, 0, 0, 0 };
    float[] m_listOrder = new float[] { 0, 0, 0, 0 };
    float[] m_listSpeed = new float[] { 0, 0, 0, 0 };
    List<PaddleCollider> m_listCOLLIDER;

    //DATA
    public GameDataManager DataManager;
    public CollectData BehaviorData;
    float data_401; //시작버튼 누르는데까지 걸린 시간
    float data_402;//완료까지 걸린 총 시간
    float data_403; //친구의 페달을 건드린 횟수
    float data_404; //아무 행동도 하지 않은 총시간
    float data_405 =0; //중도포기
    float data_406; //친구 페달을 건드린 시간
    float data_407; //페달에서 손을 땐 횟수
    float data_408, data_409, data_410, data_411; //스테이지 별 걸린 시간
    float data_412, data_413, data_414, data_415; //스테이지별 협동을 지키지 않은 횟수
    float data_416, data_417, data_418, data_419; //스테이지별 방향을 맞추지 않은 횟수

    DataManager m_dataManager;
    private string gradeLH;
    public int buildIndex = 9;

    public float[] arrData;
    void TimeCheck_Stage()
    {
        m_fTOTALTIME += Time.deltaTime;
        if (m_fTOTALTIME >= TIMELIMIT_PADDLE && m_bStamp) { Debug.Log("TIME OVER"); Hud.AudioController("time limit"); BehaviorData.AddTimeStamp("TIME LIMIT"); m_bStamp = false; }
        if (m_fTOTALTIME >= TIMEOUT_PADDLE&& !m_bStamp) { Debug.Log("TIME FIN"); Hud.AudioController("time over"); BehaviorData.AddTimeStamp("TIME OUT"); GameFinish(); m_bStamp = true; }
        }

    void TimeCheck_Start()
    {
        m_fSTARTTIME += Time.deltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        /*  GameObject JasonManager = GameObject.Find("DataManager");
          m_dataManager = JasonManager.GetComponent<DataManager>();
          gradeLH = m_dataManager.userInfo.Grade;
          if (gradeLH == "L") buildIndex = 4;
           if (gradeLH == "H") buildIndex = 8;*/

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
        //+hud start voice and canvas
        Hud.AudioController("guide");
        BehaviorData.AddTimeStamp("GUIDE START");
        m_ePSTATE = PADDLE_STATE.INTRO;
    }

    void Run_INTRO()
    {
        TimeCheck_Start();
        if (!Hud.bCoroutine)
        {
            if (m_bStamp) { Debug.Log("STAMP");  BehaviorData.AddTimeStamp("GUIDE END"); m_bStamp = false; }
        }
    }
    public void Make_START()
    {
        m_bStamp = true;
        if (Hud.bCoroutine)
        {
            BehaviorData.AddTimeStamp("GUIDE SKIP");
            Hud.bCoroutine = false;
        }
        foreach (PaddleCollider collider in m_listCOLLIDER) collider.GetComponent<Collider>().enabled = true;
        CableCar.GetComponent<Outlinable>().enabled = true;
        StartCoroutine(Hud.CountDown());
        FriendAnimation.Play("Intro");
        Hud.BGMplay(true);
        StartCoroutine(Wait_TimeStart());
    }
    IEnumerator Wait_TimeStart()
    {
        yield return new WaitForSeconds(4f);
        Hud.bTimeStart = true;
        m_ePSTATE = PADDLE_STATE.START;
        BehaviorData.AddTimeStamp("MISSION START");
    }
    
    void Run_START()
    {
        if (!Hud.bCoroutine)
        {
            Hud.AudioController("bgm");
            AnimationStart();
            Hud.bCoroutine = true;
        }
        TimeCheck_Stage();
    }

    void Make_STAGE()
    {
        AnimationStart();
        m_ePSTATE = PADDLE_STATE.STAGE;
    }
    void Run_STAGE()
    {
        TimeCheck_Stage();
    }
   
    void Make_ALLDONE()
    {

        GameFinish();
    }

    void AnimationStart()
    {
        string animName = "Paddle" + intStage.ToString(); //Change Animation of Character
        FriendAnimation.Play(animName);
    }

    void StageTimeAdd()
    {
        m_fStageTime = m_fTOTALTIME - m_fPrevTime;
        Debug.Log("PREVTIME" + m_fPrevTime + m_fStageTime);
        m_fPrevTime = m_fTOTALTIME;
        m_listSTAGE[intStage] = m_fStageTime;
        Debug.Log("STAGE TIME" + m_fStageTime + "STAGE" + intStage +"TOTALTIME" + m_fTOTALTIME +"PREVTIME" +m_fPrevTime);
      //  m_nWrongOrder = m_listOrder[intStage];
      //  m_nWrongSpeed= m_listSpeed[intStage];
      //  m_nWrongSpeed = m_nWrongOrder = 0;  //요부분 바꿈 바로바로 저장되게
    }
    void NEXTSTAGE()
    {
        Hud.AudioController("stage");
        Debug.Log("CHECK_STAGE");
        m_nComplete = 0;
        Debug.Log(intStage);
        StageTimeAdd();
        if (intStage >= 3) { Make_ALLDONE(); return; }
        Manager_Paddle.intStage++;
        intStage = Manager_Paddle.intStage;
        Make_STAGE();
    }
    public void Check_Order() //write in hud and datacheck 
    {
        Hud.AudioController("wrong order");
        m_listOrder[intStage] += 1;
        Debug.Log("CHECK_ORDER");

    }

    public void Check_Speed() //write in hud and data check
    {
        Debug.Log("CHECK_SPEED");
        Hud.AudioController("wrong speed");
        m_listSpeed[intStage] += 1;
    }

    public void PaddleCheck(float time)
    {
        m_strOrder = Manager_Paddle.SDB[intStage].strHANDLE;
        Manager_Paddle.SDB[intStage].strHANDLE = null; //한바퀴 돌면 strHANDEL을 null 시킴
        if (m_strOrder != Manager_Paddle.SDB[intStage].strORDER) { Debug.Log(m_strOrder+ intStage); Check_Order(); return; }
        if (time > 0.4f || time < -0.4f) { Check_Speed(); return; }
        m_nComplete += 1;
        CableCar.PlusDistance();
        Hud.SetDistance(intStage);
        if (Manager_Paddle.SDB[intStage].intCount == m_nComplete)
        {
            Debug.Log(m_listOrder[intStage]);
            NEXTSTAGE();//다음 스테이지 넘어가기
            return;
        }
        Hud.AudioController("correct");
        Debug.Log("CHECK" + Manager_Paddle.SDB[intStage].fTime + "  ," + Manager_Paddle.SDB[intStage].intCount + " , " + m_nComplete);
    }

    void GameFinish() // 게임 끝나면 어떻게 할지 여기에 추가
    {
        Debug.Log("COMPLETE");
        foreach (PaddleCollider collider in m_listCOLLIDER) collider.GetComponent<Collider>().enabled = false; //paddle component 내부의 collider를 빼 더이상 체크 X
        m_ePSTATE = PADDLE_STATE.ALLDONE;
        BehaviorData.AddTimeStamp("MISSION END");
        Hud.AudioController("complete");
        GrabPaddle.AllFinish();
        CableCar.GameFinish();
        FriendAnimation.Play("Finish");
        Datacollect();
    }

    public void Skip()
    {
        data_405 = 1;
        GameFinish();
    }
  
    IEnumerator NextScene()
    {
        StartCoroutine(Hud.NextScene());
        yield return new WaitUntil(() => Hud.bCoroutine == false);
        Debug.Log("FIN");
        yield return new WaitForSeconds(5);
        KetosGames.SceneTransition.SceneLoader.LoadScene(9);

    }

    void Datacollect()
    {
        BehaviorData.GetComponent<AutoVoiceRecording>().StopRecordingNBehavior();

        data_401 = m_fSTARTTIME;
        data_402 = m_fTOTALTIME;
        data_403 = GrabPaddle.fDisturbCount;
        data_404 = GrabPaddle.fIdleTime;
        data_406 = GrabPaddle.fDisturbTime;
        data_407 = GrabPaddle.fIdleCount;
        data_408 = m_listSTAGE[0];
        data_409 = m_listSTAGE[1];
        data_410 = m_listSTAGE[2];
        data_411 = m_listSTAGE[3];
        data_412 = m_listSpeed[0];
        data_413 = m_listSpeed[1];
        data_414 = m_listSpeed[2];
        data_415 = m_listSpeed[3];
        data_416 = m_listOrder[0];
        data_417 = m_listOrder[1];
        data_418 = m_listOrder[2];
        data_419 = m_listOrder[3];
        /*
        data_403 = m_listSTAGE[0];
        data_404 = m_listSTAGE[1];
        data_405 = m_listSTAGE[2];
        data_405 = m_listSTAGE[2];
        data_406 = m_listSpeed[0];
        data_407 = m_listSpeed[1];
        data_408 = m_listSpeed[2];
        data_409 = m_listOrder[0];
        data_410 = m_listOrder[1];
        data_411 = m_listOrder[2];
        data_412 = GrabPaddle.fDisturbCount;
        data_413 = GrabPaddle.fIdleTime;
        data_415 = GrabPaddle.fDisturbTime;
        data_416 = GrabPaddle.fIdleCount;
        */
        Debug.Log("MID");
        arrData = new float[] { data_401, data_402, data_403, data_404, data_405, 
            data_406, data_407, data_408, data_409, data_410 , data_411 ,data_412 , 
            data_413, data_414, data_415, data_416,data_417, data_418, data_419 };
        for(int i = 0; i < arrData.Length; i++)
        {
           Debug.Log(arrData[i]);
        }
        DataManager.SaveCurrentData();
        StartCoroutine(NextScene());
    }

    Hud_Paddle Hud;
}
