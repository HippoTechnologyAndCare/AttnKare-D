using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KetosGames.SceneTransition; //scene transition
using BNG;
namespace CleanUp {
public class Guide : MonoBehaviour {
    /**************************************************************************
    // Global Constant / Parameter Definition
    ***************************************************************************/
    public static  int      TOTAL_OBJECT    = 18;   //잡을 수 있는 물건수
    public static  int      ARRANGE_OBJECT  = 9;    //정리해야할 물건수
    public static  float    TIMEOUT_ARRANGE = 120f; //방청소 시간제한 2분_Timer
    public static  float    TIMEOUT_SCENE   = 300f; //Scene의 시간제한 5분, 다음게임으로    
    public int              NEXT_SCENE      = 6;
    public enum STATE { 
        INTRO,         
        ARRANGE,         
        END,  
        NEXT 
     }
    public enum HUD_REPORT  {
        PLAYED_HOWTO,        
        PLAYED_WELLDONE,
        PLAYED_TIMEOUT,
        PLAYED_MOVING,
        NONE,
    }
 
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/
    public static STATE   m_eState;        
    public static Arrange.ARRANGE       m_eGrabbedArrange; //잡은물건     
    bool    m_bGrabbable;      //Player가 잡을 수 있는지 여부
        
    //Data for Evaluation
    bool    m_bTimeOutArrange; //방청소시간 초과 여부 / 총 2분중
    bool    m_bTImeOutScene;   //Scene Play시간 초과여부  / 총 5분중 
    int     m_nTotArranged;    //제위치에 정리된 갯수
    float   m_fTimeTaken;      //정리하는데 걸리는 시간 : 모두정리, 사용자가 중단, Timeout되어 중단되든 

    float   m_fTimeLookValid;  //Player가 필요한곳을 보는 시간 
    float   m_fTimeLookInvalid;//Player가 불필요한곳을 보는 시간  
    //int     m_nHearingReplay;  //Player가 다시듣기 재생횟수 : 재생횟수 : SpeechBubles 처리?-- 어떤상황에서 활성화?
    int     m_nGrabCount;      //Player가 물건을 잡은 횟수
    float   m_fMoveDistance;   //Player 총이동거리 
    int     m_nObstacleTouch;  //Player가 방해 물체를 건든 횟수        
    int     m_nFinBtnDown;     //Player가 Fin Button 클릿횟수        
    /**************************************************************************
    // Method Start
    ***************************************************************************/    

    //정리할 물건을 잡음 Grabbed시에 체크
    public void GrabArrangeable(GameObject fgo) {                
        m_Hud.ShowStarParticle(fgo.transform);
        Make_Arrange();
    }
    //정리할 물건을 제자리에 위치시킴 from Arrange.cs
    public void SetArranged(Arrange.ARRANGE arranged, Transform dst) {
        //정리할 물건일 경우 정리된 것으로 바꾸고 파티클 전시     
        m_nTotArranged++;       
        m_Hud.PopUpCount(m_nTotArranged,true);
        m_Hud.ShowStarParticle(dst);
        //정리한 물건 문자열 갱신       
        GetArrangeStr(); 
        m_Hud.NoteUpdateArrange(arrangedStr,arrangeableStr);
        if (m_nTotArranged >= ARRANGE_OBJECT) m_Hud.PlayWellDone();        
    }
    //from Hud.cs
    public void HudReport(HUD_REPORT eHudReport) {
        switch(eHudReport) {
        case HUD_REPORT.PLAYED_HOWTO  : 
            m_Hud.DisplayLeftHint();
            m_Hud.PlayVideo(true);
            m_Hud.PlayDuck(true);  
            Make_Arrange();      
            break;
        //case HUD_REPORT.PLAYED_INTRO_CLEAN  : Make_Clean();     break;
        case HUD_REPORT.PLAYED_WELLDONE:Make_End();       break;
        case HUD_REPORT.PLAYED_TIMEOUT: Make_Next();      break;  // check more later
        case HUD_REPORT.PLAYED_MOVING : Make_Next();      break;
        }
    }
    
    //Right Grabber에 있는 GrabbablesInTrigger 스크립트 enable시켜주면 손으로 잡을 수 있습니다.    
    //Right Controller의 RightHandPointer gameobject enable 이때 RightHandPointer에 있는 LaserPonter.cs가 작동되어 레이져로 잡을수 있습니다    
    public GameObject m_goRightGrabber;  
    public GameObject m_goRightHandPointer;
    void MakeGrabbable(bool bOn) {
        m_bGrabbable = bOn;
        m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled = m_bGrabbable;          
        m_goRightHandPointer.SetActive(m_bGrabbable);
        //Debug.Log("MakeGrabble="+m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled);
    }

    //OnGrab Event처리 --> RightHand>Grabber에 Event에 할당   
    Grabber  m_RightGrabber;
    public void OnGrabRight() { 
        m_nGrabCount++;        
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject; 
        Arrange arrange = grab.GetComponent<Arrange>();
        if(arrange) {
            arrange.OnGrab();
            m_eGrabbedArrange = arrange.m_eArrange;
        }
        if(grab.tag != Manager.saTag[(int)Manager.TAG.NECESSARY])m_nObstacleTouch++; 
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck을 잡으면
            m_Hud.PlayDuck(true,HUD.DUCK_ACTION.GRABBED);
        }        
    }

    //OnGrabRelease Event처리 --> RightHand>Grabber에 Event에 할당   
    public void OnGrabRightRelease() {
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject;
        Arrange arrange = grab.GetComponent<Arrange>();
        if(arrange) arrange.OnGrabRelease();
        m_eGrabbedArrange = Arrange.ARRANGE.NONE;
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck을 놓으면
            m_Hud.PlayDuck(true);
        }        
    }

    //Fin Button 처리    
    public void OnFinButtonDown() {
        m_nFinBtnDown++;
        if(m_nFinBtnDown<2) m_Hud.PlayWarning();
        if(m_nFinBtnDown==2) {
            m_Hud.ShowMoving();
            Make_End();
        }       
    }
    //평가관련 : 총이동거리를 계산합니다.
    public Transform  m_Character; //CenterEyeAnchor Transform할당
    Vector3 m_v3OldPosCharacter;
    void CalculateMoveDistance() {
        m_fMoveDistance += Vector3.Distance(m_Character.position, m_v3OldPosCharacter);
        m_v3OldPosCharacter = m_Character.position;
    }   

    //필요한 곳을 보는시간 : 찾기와 정리시간중에 necessary 이외 보는시간이 1초이상경과시 체크    
    void CalculateLookTime() {
         RaycastHit hit;
        if (Physics.Raycast(m_Character.position, m_Character.forward, out hit))  {
            if(hit.transform.gameObject.tag == Manager.saTag[(int)Manager.TAG.NECESSARY]) 
                m_fTimeLookValid += Time.deltaTime;
            else m_fTimeLookInvalid += Time.deltaTime;
        } else  m_fTimeLookInvalid += Time.deltaTime;                
    }

    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    public HUD    m_Hud;
    public GameObject goArranges;
    Arrange[] m_aArranges; //정리할 정보를 갖고 있음

    // Start is called before the first frame update
    void Start() {        
        m_RightGrabber  = m_goRightGrabber.GetComponent<Grabber>();
        m_aArranges     = goArranges.GetComponentsInChildren<Arrange>(true);
        Make_Intro();         
    }

    // Update is called once per frame
    STATE oldState;
    void Update() {
        switch (m_eState){
        case STATE.INTRO  :         Run_Intro();          break;        
        case STATE.ARRANGE:         Run_Arrange();        break;        
        case STATE.END    :         Run_End();            break;
        case STATE.NEXT   :         Run_Next();           break;
        }        
        CalculateMoveDistance();
        if(m_eState != oldState) Debug.Log(m_eState);
        oldState = m_eState;
    }
    
    void Make_Intro() {                
        MakeGrabbable(false);
        m_Hud.PlayHowTo();
        m_eState = STATE.INTRO;        
    }

    void Run_Intro() { 
    }

    void Make_Arrange() {      
        MakeGrabbable(true);
        m_eState = STATE.ARRANGE;
    }

    void Run_Arrange() { 
        MeasureTime();
        CalculateLookTime();
        if(m_bTImeOutScene) Make_End();
    }

    void Make_End() {
        m_Hud.PlayDuck(false);
        m_Hud.PlayVideo(false);        
        MakeGrabbable(false);
        //마지막에 해야할 평가작업등을 추가하십시요
        ReportData();
        //이전상태가 TIMEOUT_SCENE상태에서 넘어오면 아쉬지만...표시
        if(m_bTImeOutScene) {
            m_Hud.PlayTimeOut();
        } else m_Hud.PlayMoving();                       

        m_eState = STATE.END;
    }
    void Run_End() { 

    }

    void Make_Next() {      
        Load_Next_Scene();
        m_eState = STATE.NEXT;
    }
    void Run_Next() { }
    

    void ReportData() {

        //Summary 데이타 처리
        string eval = ""
        +"m_bTimeOutArrange= " + m_bTimeOutArrange    +"\r\n"     //방청소시간 초과 여부 / 총 2분중
        +"m_bTImeOutScene= "    + m_bTImeOutScene     +"\r\n"     //Scene Play시간 초과여부  / 총 5분중 
        +"m_nTotArranged= "     + m_nTotArranged      +"\r\n"     //제위치에 정리된 갯수
        +"m_fTimeTaken= "       + m_fTimeTaken        +"\r\n"     //정리하는데 걸리는 시간 : 모두정리, 사용자        
        +"m_fTimeLookValid= "   + m_fTimeLookValid    +"\r\n"     //Player가 필요한곳을 보는 시간 
        +"m_fTimeLookInvalid= " + m_fTimeLookInvalid  +"\r\n"      //Player가 불필요한곳을 보는 시간            
        +"m_nGrabCount= "       + m_nGrabCount        +"\r\n"     //Player가 물건을 잡은 횟수
        +"m_fMoveDistance= "    + m_fMoveDistance     +"\r\n"     //Player 총이동거리 
        +"m_nObstacleTouch= "   + m_nObstacleTouch    +"\r\n"     //Player가 방해 물체를 건든 횟수        
        +"m_nFinBtnDown= "      + m_nFinBtnDown       +"\r\n";     //Player가 Fin Button 클릿횟수        
        
        Debug.Log(eval);   // to console
        Util.ELOG(eval);   // to logfile .//Eval.txt
                 
         //각 물건별 개별(Arrange.CDB 데이타 처리        
        foreach(Arrange arrange in m_aArranges) 
        {
            arrange.SaveResult();
            eval = "" 
               +"eArrange= "    + Arrange.CDB[(int)arrange.m_eArrange].eArrange    + ", "     //종류
               +"Object_name= " + Arrange.CDB[(int)arrange.m_eArrange].Object_name + ", "     //나에게 와야할 GameObject Name 
               +"bCleanable= "  + Arrange.CDB[(int)arrange.m_eArrange].bCleanable  + ", "     //청소대상여부
               +"bPositioned= " + Arrange.CDB[(int)arrange.m_eArrange].bPositioned + ", "     //제위치여부
               +"nGrabCount= "  + Arrange.CDB[(int)arrange.m_eArrange].nGrabCount  + ", "     //Player가 내 물건을 잡은 횟수
               +"fGrabTime= "   + Arrange.CDB[(int)arrange.m_eArrange].fGrabTime   + ", "     //Player가 내 물건을 잡은 시간
               +"nPosiCount = " + Arrange.CDB[(int)arrange.m_eArrange].nPosiCount  + ", "     //자기위치에 둔 횟수
               +"kor_name= "    + Arrange.CDB[(int)arrange.m_eArrange].kor_name    + ", "     //한글명
               +"\r\n";
            Debug.Log(eval);   // to console
            Util.ELOG(eval);   // to logfile .//Eval.txt
         }
    }

    /*****************************************************************************
    // Helper & Utility
    ******************************************************************************/
    void MeasureTime() {
        m_fTimeTaken += Time.deltaTime; 
        if(m_fTimeTaken > TIMEOUT_ARRANGE) m_bTimeOutArrange = true;
        if(m_fTimeTaken > TIMEOUT_SCENE)  m_bTImeOutScene = true;
        if(!m_bTimeOutArrange) m_Hud.DrawTimeTaken(TIMEOUT_ARRANGE - m_fTimeTaken); 
    }
    // 다음 Scene으로 넘어가는 코드를 추가하세요
    void Load_Next_Scene()  {
        KetosGames.SceneTransition.SceneLoader.LoadScene(NEXT_SCENE);
        Debug.Log("다음신을 로드합니다");
    }
    //정리할 물건, 정리한 물건 정보 문자열 처리 -for hud
    string arrangedStr, arrangeableStr;
    void GetArrangeStr() {
        arrangedStr = arrangeableStr = "";
        foreach(Arrange arrange in m_aArranges) {
            if (arrange.m_bCleanable) {
                string name = Arrange.CDB[(int)arrange.m_eArrange].kor_name;
                if(arrange.m_bPositioned) arrangedStr += name+",";
                else arrangeableStr += name+",";
            }            
        }
    }    
}
}
