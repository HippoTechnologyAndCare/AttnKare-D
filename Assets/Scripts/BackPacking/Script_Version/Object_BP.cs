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
     * ���г� ���� �ʿ��� ������Ʈ
     * 1�ܰ�
     * ���� 3��, ��������, �ʷ� ����, ���찳 (�ʿ� ���� �п�ǰ : ��������, �Ķ�����)
     * ���� �Ѳ�
     * 
     * 2�ܰ�
     * ����
     * ������ : ����, ����, �̼�, ���� (�ʿ� ���� å : ��ȸ, ����, ����, ü��, ����)
     * ��Ǯ
     * 
     * ���ع�
     *  �峭�� ����, �峭�� ��, ����, ����, ��Ʈ
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

  
    public InputBridge XrRig;
    public GameObject CenterEye;
    public GameObject prefabFadeIn;
    public GameObject prefabFadeOut;
    public Transform RightController;
    public static bool bGrabbed;
    public GameObject[] arrStage2;
    public GameObject VideoPlayer;
    bool m_bTotalTime = false; //�ѽð� Ȯ��
    public float m_fTotalTime;
    public bool m_bStageChangeTime = false; //1�ܰ� �Ϸ� �� 2�ܰ� ���۱��� �ɸ��� �ð�
    float m_fStageChangeTime;
    bool bTimeLimit;
    bool bTimeDone;
    GameObject m_tPencilcase;
    GameObject m_goFade;
    GameObject m_tRightPointer;
    Grabber m_tGrabber;
    UI_BP Hud;
    BagPack_BP Bag;
    Pencilcase_BP Pencilcase;
    public List<Grabbable> listGrabbable; //list of all grabbable;
    public int buildindex;
    public CollectData DataCollect;
    AddDelimiter Delimiter;
    void Start()
    {
        foreach (Grabbable grab in listGrabbable)
        {
            grab.enabled = false;
        }
        m_tPencilcase = GameObject.Find("Pencilcase_complete");
        Hud = GameObject.Find("UI").GetComponent<UI_BP>();
        Pencilcase = GameObject.Find("Pencilcase_Collider").GetComponent<Pencilcase_BP>();
        Bag = GameObject.Find("Bag_Collider").GetComponent<BagPack_BP>();
        Delimiter = GameObject.Find("DataCheck_Manager").GetComponent<AddDelimiter>();
        m_tRightPointer = RightController.Find("RightHandPointer").gameObject;
        m_tGrabber = RightController.Find("Grabber").GetComponent<Grabber>();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        m_tRightPointer.SetActive(false);
        m_tGrabber.enabled = false;
        m_goFade =  Instantiate(prefabFadeIn, CenterEye.transform.position, Quaternion.identity);
        m_goFade.transform.SetParent(CenterEye.transform);
        yield return new WaitForSeconds(1.2f);
        NextScene();
    }

    IEnumerator FadeIn()
    {
        m_goFade = Instantiate(prefabFadeIn, CenterEye.transform.position, Quaternion.identity);
        m_goFade.transform.SetParent(CenterEye.transform);
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(Hud.CanvasStart());
    }
    void Update()
    {
        if (XrRig.RightTrigger > 0.8f) bGrabbed = true;
        if (XrRig.RightTrigger < 0.2f) bGrabbed = false;
        if (m_bTotalTime)
        {
            m_fTotalTime += Time.deltaTime;
            if (m_fTotalTime >= 150f & !bTimeLimit) { TotalTime("TIME LIMIT"); bTimeLimit = true; }
            if (m_fTotalTime >= 200f & !bTimeDone) { TotalTime("TIME OUT"); GameDone(); bTimeDone = true; }
        }
        if (m_bStageChangeTime) m_fStageChangeTime += Time.deltaTime;

    }

    public void Stage1()
    {
        Debug.Log("Stage1");
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
        Debug.Log("stage2");
        m_bStageChangeTime = true;
        Bag.bStage2 = true;
        Hud.ChangeMemo();
        StartCoroutine(Hud.StageNotification(2));
    }

    void TotalTime(string strTime)
    {
        Debug.Log("TIME");
        DataCollect.AddTimeStamp(strTime);
        StartCoroutine(Hud.TimeCheck(strTime));
    }

    void GameDone()
    {
        StartCoroutine(Hud.GameFinish());
        Delimiter.endEverything();
        DataCollect.AddTimeStamp("MISSION END");
        StartCoroutine(FadeOut());
    }
    public void NextScene()
    {
        KetosGames.SceneTransition.SceneLoader.LoadScene(buildindex);
    }

}
