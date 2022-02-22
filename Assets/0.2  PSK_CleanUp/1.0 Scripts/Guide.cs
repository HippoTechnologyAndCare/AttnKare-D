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
    public static  int      TOTAL_OBJECT    = 18;   //���� �� �ִ� ���Ǽ�
    public static  int      ARRANGE_OBJECT  = 9;    //�����ؾ��� ���Ǽ�
    public static  float    TIMEOUT_ARRANGE = 120f; //��û�� �ð����� 2��_Timer
    public static  float    TIMEOUT_SCENE   = 300f; //Scene�� �ð����� 5��, ������������    
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
    public static Arrange.ARRANGE       m_eGrabbedArrange; //��������     
    bool    m_bGrabbable;      //Player�� ���� �� �ִ��� ����
        
    //Data for Evaluation
    bool    m_bTimeOutArrange; //��û�ҽð� �ʰ� ���� / �� 2����
    bool    m_bTImeOutScene;   //Scene Play�ð� �ʰ�����  / �� 5���� 
    int     m_nTotArranged;    //����ġ�� ������ ����
    float   m_fTimeTaken;      //�����ϴµ� �ɸ��� �ð� : �������, ����ڰ� �ߴ�, Timeout�Ǿ� �ߴܵǵ� 

    float   m_fTimeLookValid;  //Player�� �ʿ��Ѱ��� ���� �ð� 
    float   m_fTimeLookInvalid;//Player�� ���ʿ��Ѱ��� ���� �ð�  
    //int     m_nHearingReplay;  //Player�� �ٽõ�� ���Ƚ�� : ���Ƚ�� : SpeechBubles ó��?-- ���Ȳ���� Ȱ��ȭ?
    int     m_nGrabCount;      //Player�� ������ ���� Ƚ��
    float   m_fMoveDistance;   //Player ���̵��Ÿ� 
    int     m_nObstacleTouch;  //Player�� ���� ��ü�� �ǵ� Ƚ��        
    int     m_nFinBtnDown;     //Player�� Fin Button Ŭ��Ƚ��        
    /**************************************************************************
    // Method Start
    ***************************************************************************/    

    //������ ������ ���� Grabbed�ÿ� üũ
    public void GrabArrangeable(GameObject fgo) {                
        m_Hud.ShowStarParticle(fgo.transform);
        Make_Arrange();
    }
    //������ ������ ���ڸ��� ��ġ��Ŵ from Arrange.cs
    public void SetArranged(Arrange.ARRANGE arranged, Transform dst) {
        //������ ������ ��� ������ ������ �ٲٰ� ��ƼŬ ����     
        m_nTotArranged++;       
        m_Hud.PopUpCount(m_nTotArranged,true);
        m_Hud.ShowStarParticle(dst);
        //������ ���� ���ڿ� ����       
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
    
    //Right Grabber�� �ִ� GrabbablesInTrigger ��ũ��Ʈ enable�����ָ� ������ ���� �� �ֽ��ϴ�.    
    //Right Controller�� RightHandPointer gameobject enable �̶� RightHandPointer�� �ִ� LaserPonter.cs�� �۵��Ǿ� �������� ������ �ֽ��ϴ�    
    public GameObject m_goRightGrabber;  
    public GameObject m_goRightHandPointer;
    void MakeGrabbable(bool bOn) {
        m_bGrabbable = bOn;
        m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled = m_bGrabbable;          
        m_goRightHandPointer.SetActive(m_bGrabbable);
        //Debug.Log("MakeGrabble="+m_goRightGrabber.GetComponent<GrabbablesInTrigger>().enabled);
    }

    //OnGrab Eventó�� --> RightHand>Grabber�� Event�� �Ҵ�   
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
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck�� ������
            m_Hud.PlayDuck(true,HUD.DUCK_ACTION.GRABBED);
        }        
    }

    //OnGrabRelease Eventó�� --> RightHand>Grabber�� Event�� �Ҵ�   
    public void OnGrabRightRelease() {
        GameObject grab = m_RightGrabber.HeldGrabbable.gameObject;
        Arrange arrange = grab.GetComponent<Arrange>();
        if(arrange) arrange.OnGrabRelease();
        m_eGrabbedArrange = Arrange.ARRANGE.NONE;
        if(grab.name == Manager.DDB[(int)Manager.DISTURB.DUCK].name){ //duck�� ������
            m_Hud.PlayDuck(true);
        }        
    }

    //Fin Button ó��    
    public void OnFinButtonDown() {
        m_nFinBtnDown++;
        if(m_nFinBtnDown<2) m_Hud.PlayWarning();
        if(m_nFinBtnDown==2) {
            m_Hud.ShowMoving();
            Make_End();
        }       
    }
    //�򰡰��� : ���̵��Ÿ��� ����մϴ�.
    public Transform  m_Character; //CenterEyeAnchor Transform�Ҵ�
    Vector3 m_v3OldPosCharacter;
    void CalculateMoveDistance() {
        m_fMoveDistance += Vector3.Distance(m_Character.position, m_v3OldPosCharacter);
        m_v3OldPosCharacter = m_Character.position;
    }   

    //�ʿ��� ���� ���½ð� : ã��� �����ð��߿� necessary �̿� ���½ð��� 1���̻����� üũ    
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
    Arrange[] m_aArranges; //������ ������ ���� ����

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
        //�������� �ؾ��� ���۾����� �߰��Ͻʽÿ�
        ReportData();
        //�������°� TIMEOUT_SCENE���¿��� �Ѿ���� �ƽ�����...ǥ��
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

        //Summary ����Ÿ ó��
        string eval = ""
        +"m_bTimeOutArrange= " + m_bTimeOutArrange    +"\r\n"     //��û�ҽð� �ʰ� ���� / �� 2����
        +"m_bTImeOutScene= "    + m_bTImeOutScene     +"\r\n"     //Scene Play�ð� �ʰ�����  / �� 5���� 
        +"m_nTotArranged= "     + m_nTotArranged      +"\r\n"     //����ġ�� ������ ����
        +"m_fTimeTaken= "       + m_fTimeTaken        +"\r\n"     //�����ϴµ� �ɸ��� �ð� : �������, �����        
        +"m_fTimeLookValid= "   + m_fTimeLookValid    +"\r\n"     //Player�� �ʿ��Ѱ��� ���� �ð� 
        +"m_fTimeLookInvalid= " + m_fTimeLookInvalid  +"\r\n"      //Player�� ���ʿ��Ѱ��� ���� �ð�            
        +"m_nGrabCount= "       + m_nGrabCount        +"\r\n"     //Player�� ������ ���� Ƚ��
        +"m_fMoveDistance= "    + m_fMoveDistance     +"\r\n"     //Player ���̵��Ÿ� 
        +"m_nObstacleTouch= "   + m_nObstacleTouch    +"\r\n"     //Player�� ���� ��ü�� �ǵ� Ƚ��        
        +"m_nFinBtnDown= "      + m_nFinBtnDown       +"\r\n";     //Player�� Fin Button Ŭ��Ƚ��        
        
        Debug.Log(eval);   // to console
        Util.ELOG(eval);   // to logfile .//Eval.txt
                 
         //�� ���Ǻ� ����(Arrange.CDB ����Ÿ ó��        
        foreach(Arrange arrange in m_aArranges) 
        {
            arrange.SaveResult();
            eval = "" 
               +"eArrange= "    + Arrange.CDB[(int)arrange.m_eArrange].eArrange    + ", "     //����
               +"Object_name= " + Arrange.CDB[(int)arrange.m_eArrange].Object_name + ", "     //������ �;��� GameObject Name 
               +"bCleanable= "  + Arrange.CDB[(int)arrange.m_eArrange].bCleanable  + ", "     //û�Ҵ�󿩺�
               +"bPositioned= " + Arrange.CDB[(int)arrange.m_eArrange].bPositioned + ", "     //����ġ����
               +"nGrabCount= "  + Arrange.CDB[(int)arrange.m_eArrange].nGrabCount  + ", "     //Player�� �� ������ ���� Ƚ��
               +"fGrabTime= "   + Arrange.CDB[(int)arrange.m_eArrange].fGrabTime   + ", "     //Player�� �� ������ ���� �ð�
               +"nPosiCount = " + Arrange.CDB[(int)arrange.m_eArrange].nPosiCount  + ", "     //�ڱ���ġ�� �� Ƚ��
               +"kor_name= "    + Arrange.CDB[(int)arrange.m_eArrange].kor_name    + ", "     //�ѱ۸�
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
    // ���� Scene���� �Ѿ�� �ڵ带 �߰��ϼ���
    void Load_Next_Scene()  {
        KetosGames.SceneTransition.SceneLoader.LoadScene(NEXT_SCENE);
        Debug.Log("�������� �ε��մϴ�");
    }
    //������ ����, ������ ���� ���� ���ڿ� ó�� -for hud
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
