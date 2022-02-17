using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using EPOOutline;
using DG.Tweening;
using KetosGames.SceneTransition;
using UnityEngine.SceneManagement;

public class Object_BP : MonoBehaviour
{
    /*
     * 고학년 버전 필요한 오브젝트
     * 1단계
     * 연필 3개, 빨간볼펜, 초록 볼펜, 지우개 (필요 없는 학용품 : 검정볼펜, 파랑볼펜)
     * 필통 뚜껑
     * 
     * 2단계
     * 필통
     * 교과서 : 국어, 과학, 미술, 영어 (필요 없는 책 : 사회, 수학, 음악, 체육, 도덕)
     * 딱풀
     * 
     * 방해물
     *  장난감 기차, 장난감 차, 폴더, 물감, 노트
     */
    public enum OBJ_BP { DISTURB, PENCIL, PEN, ERASER, TXTBOOK, GLUE, PCAP, PCASE}
    public enum TAG_BP {NECESSARY, UNNECESSARY, NECESSARY_PENCIL, NECESSARY_BOOK}
    public enum KIND_BP { NONE, GREEN, RED, BLUE, PURLPLE, BLACK, KOREAN, SCIENCE, ART, ENGLISH, SOCIALS, MATH, MUSIC, GYM, ETHICS, SCHOOL, TOY }

    public enum GAZE_BP { MEMO, TV, TIMETABLE, NOTWATCHING }
    public enum CASE_BP { INCORRECT, CORRECT, COMPLETE, STAR, BEEP, APPEAR, PENCILCASE }
    public enum STATE { ENTER, EXIT }
    public struct BP_INFO
    {
        public OBJ_BP eObj;
        public KIND_BP eKind;
        public bool bCorrect;

        public BP_INFO(OBJ_BP eObj, KIND_BP eKind, bool bCorrect)
        {
            this.eObj = eObj;
            this.eKind = eKind;
            this.bCorrect = bCorrect;
        } 
    }
    public static BP_INFO[] BP1DB = new BP_INFO[]
    {
        
        new BP_INFO(OBJ_BP.PENCIL, KIND_BP.NONE, true ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.GREEN, true ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.RED, true ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.BLUE, false ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.PURLPLE, false ),
        new BP_INFO(OBJ_BP.PEN, KIND_BP.BLACK, false )


    };
    public static BP_INFO[] BP2DB = new BP_INFO[]
    {

        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.KOREAN, true ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.SCIENCE, true ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.ART, true ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.ENGLISH, true ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.SOCIALS, false ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.MATH, false ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.ETHICS, false ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.MUSIC, false ),
        new BP_INFO(OBJ_BP.TXTBOOK, KIND_BP.GYM, false ),
        new BP_INFO(OBJ_BP.GLUE, KIND_BP.NONE, true),
        new BP_INFO(OBJ_BP.PCASE, KIND_BP.NONE, true)

    };

    //PACK_DISTRACTION
    // Start is called before the first frame update

    UI_BP Hud;
    BagPack_BP Bag;
    Pencilcase_BP Pencilcase;

    public InputBridge XrRig;
    public GameObject CenterEye;
    public GameObject prefabFadeIn;
    public GameObject prefabFadeOut;
    public Transform RightController;
    public static bool bGrabbed;
    public GameObject[] arrStage2;
    public GameObject VideoPlayer;
    public GameDataManager SaveData;

    //DATA
    public CollectData DataCollect;
    public AutoVoiceRecording BehaviorData;
    AddDelimiter Delimiter;
    bool m_bTotalTime = false; //총시간 확인
    public float m_fTotalTime;
    public bool m_bStageChangeTime = true; //1단계 완료 후 2단계 시작까지 걸리는 시간
    float m_fStageChangeTime;
    bool bTimeLimit;
    bool bTimeDone;

    bool m_bHudEnd = false; //wait for coroutine in hud to end (in total time)
 //   GameObject m_tPencilcase;
    GameObject m_goFade;
    GameObject m_tRightPointer;
    Grabber m_tGrabber;

    /*DATA NEEDED
     * TOTAL TIME (501)
     * MEMO GAZE TIME (502)
     * TIMETABLE GAZE TIME (503)
     * UNNECESSARY GRAB TIME (504) //방해요소와 필요한 학용품 구분
     * WRONMG PUT COUNT(505)
     * STAGE 2 GRAB IN STAGE 1 (506) //없애기
     * STAGE 1 -> STAGE 2 TIME (507)
     * DISTARCTED VIDEO TIME (508)
     * SKIP(509)
     * UNNECESSARY GRAB COUNT(509)
     */


    public List<Grabbable> listGrabbable; //list of all grabbable;
    public int buildindex;

    void Start()
    {
        foreach (Grabbable grab in listGrabbable)
        {
            grab.enabled = false;
        }
 //       m_tPencilcase = GameObject.Find("Pencilcase_complete");
        Hud = GameObject.Find("UI").GetComponent<UI_BP>();
        Pencilcase = GameObject.Find("Pencilcase_Collider").GetComponent<Pencilcase_BP>();
        Bag = GameObject.Find("Bag_Collider").GetComponent<BagPack_BP>();
        Delimiter = GameObject.Find("DataCheck_Manager").GetComponent<AddDelimiter>();
        m_tRightPointer = RightController.Find("RightHandPointer").gameObject;
        m_tGrabber = RightController.Find("Grabber").GetComponent<Grabber>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        m_tRightPointer.SetActive(false);
        m_tGrabber.enabled = false;
        m_goFade =  Instantiate(prefabFadeOut, CenterEye.transform.position, Quaternion.identity);
        m_goFade.transform.SetParent(CenterEye.transform);
        yield return new WaitForSeconds(1.2f);
        KetosGames.SceneTransition.SceneLoader.LoadScene(buildindex);
    }

    IEnumerator FadeIn()
    {
        m_goFade = Instantiate(prefabFadeIn, CenterEye.transform.position, Quaternion.identity);
        m_goFade.transform.SetParent(CenterEye.transform);
        Debug.Log("wait");
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(Hud.CanvasStart());
        DataCollect.AddTimeStamp("GUIDE START");
    }
    void Update()
    {
        if (XrRig.RightTrigger > 0.8f) bGrabbed = true;
        if (XrRig.RightTrigger < 0.2f) bGrabbed = false;
        if (m_bTotalTime)
        {
            m_fTotalTime += Time.deltaTime;
            if (m_fTotalTime >= 150f & !bTimeLimit) { TotalTime("TIME LIMIT"); bTimeLimit = true; }
            if (m_fTotalTime >= 200f & !bTimeDone) {  TotalTime("TIME OUT"); StartCoroutine(GameDone()); bTimeDone = true; }
        }
        if (m_bStageChangeTime) m_fStageChangeTime += Time.deltaTime;

    }

    public void Stage1()
    {
        DataCollect.AddTimeStamp("GUIDE END");
        DataCollect.AddTimeStamp("MISSION START");
        VideoPlayer.SetActive(true);
        m_bTotalTime = true;
        StartCoroutine(Hud.StageNotification(1));
        m_tRightPointer.SetActive(true);
        m_tGrabber.enabled = true;
        foreach (Grabbable grab in listGrabbable)
        {
            grab.enabled = true;
        }
    }
    public void Stage2()
    {
        m_bStageChangeTime = true;
        Bag.bStage2 = true;
        Hud.ChangeMemo();
        StartCoroutine(Hud.StageNotification(2));
    }

    void TotalTime(string strTime) //Add timestamp (TIME LIMIT, TIME OVER) & Show Warning
    {
        DataCollect.AddTimeStamp(strTime);
        StartCoroutine(Hud.TimeCheck(strTime));
    }
    public IEnumerator GameDone()
    {
        yield return new WaitUntil(() => Hud.bEndUI == true);
        yield return StartCoroutine(Hud.GameFinish());
        NextScene();
    }

  
    public void NextScene()
    {
        Debug.Log("CALLED");
        m_bTotalTime = false;
        Delimiter.endEverything();
        DataCollect.AddTimeStamp("MISSION END");
        SaveData.SaveCurrentData();
        BehaviorData.StopRecordingNBehavior();
        StartCoroutine(FadeOut());
    }
   
}
