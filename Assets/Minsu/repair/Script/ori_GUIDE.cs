using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KetosGames.SceneTransition; //scene transition
using BNG;

namespace defaultType {
    public class ori_Guide : MonoBehaviour {
        /**************************************************************************
        // Global Constant / Parameter Definition
        ***************************************************************************/

        public static float TIMEOUT_ARRANGE = 120f; //방청소 시간제한 2분_Timer
        public static float TIMEOUT_SCENE = 300f; //Scene의 시간제한 5분, 다음게임으로    
        public int NEXT_SCENE = 2;

        public enum STATE {
            INTRO,
            PLAY,
            PAUSE,
            END,
            NEXT
        }
        public enum HUD_REPORT {
            PLAYED_HOWTO,
            PLAYED_PAUSE,
            PLAYED_WELLDONE,
            PLAYED_TIMEOUT,
            PLAYED_MOVING,
            NONE,
        }


        /**************************************************************************/
        /* Member Variable
        /**************************************************************************/

        public static STATE m_eState;


        //Data for Evaluation
        bool m_bTimeOutArrange; //방청소시간 초과 여부 / 총 2분중
        bool m_bTImeOutScene;   //Scene Play시간 초과여부  / 총 5분중     
        float m_fTimeTaken;      //정리하는데 걸리는 시간 : 모두정리, 사용자가 중단, Timeout되어 중단되든 

        float m_fTimeLookValid;  //Player가 필요한곳을 보는 시간 
        float m_fTimeLookVideo;  //Player가 불필요한 비디오를 보는 시간
        float m_fTimeLookInvalid;//Player가 불필요한곳을 보는 시간  
                                 //int     m_nHearingReplay;  //Player가 다시듣기 재생횟수 : 재생횟수 : SpeechBubles 처리?-- 어떤상황에서 활성화?

        float m_fMoveDistance;   //Player 총이동거리 
        int m_nObstacleTouch;  //Player가 방해 물체를 건든 횟수        
        int m_nFinBtnDown;     //Player가 Fin Button 클릿횟수

        /**************************************************************************
        // Method Start
        ***************************************************************************/


        //from Hud.cs
        /*
        public ori_HUD m_Hud;
     public void HudReport(HUD_REPORT eHudReport) {
     switch(eHudReport) {
     case HUD_REPORT.PLAYED_HOWTO  : 
         //m_Hud.DisplayLeftHint();
         m_Hud.PlayVideo(true);
         m_Hud.PlayDuck(true);  
         Make_Arrange();      
         break;
     //case HUD_REPORT.PLAYED_INTRO_CLEAN  : Make_Clean();     break;
     case HUD_REPORT.PLAYED_WELLDONE:
                 //new WaitForSeconds(3f);
                 Make_End();       break;
     case HUD_REPORT.PLAYED_TIMEOUT: Make_Next();      break;  // check more later
     case HUD_REPORT.PLAYED_MOVING : Make_Next();      break;
     }
 }
        */
        /*
 //Right Grabber에 있는 GrabbablesInTrigger 스크립트 enable시켜주면 손으로 잡을 수 있습니다.    
 //Right Controller의 RightHandPointer gameobject enable 이때 RightHandPointer에 있는 LaserPonter.cs가 작동되어 레이져로 잡을수 있습니다    
 public GameObject m_goRightGrabber;  
 public GameObject m_goRightHandPointer;

 //OnGrab Event처리 --> RightHand>Grabber에 Event에 할당   
 Grabber  m_RightGrabber;

 //Fin Button 처리    
 public void OnFinButtonDown() {
         /*
     m_nFinBtnDown++;
     if(m_nFinBtnDown<2) m_Hud.PlayWarning();
     if(m_nFinBtnDown==2) {
         m_Hud.ShowMoving();
         Make_End();
     }


         m_Hud.ShowMoving();
         Make_End();
 }
         */
        //평가관련 : 총이동거리를 계산합니다.
        public Transform m_Character; //CenterEyeAnchor Transform할당
        Vector3 m_v3OldPosCharacter;
        void CalculateMoveDistance() {
            m_fMoveDistance += Vector3.Distance(m_Character.position, m_v3OldPosCharacter);
            m_v3OldPosCharacter = m_Character.position;
        }

        //필요한 곳을 보는시간 : 찾기와 정리시간중에 necessary 이외 보는시간이 1초이상경과시 체크    
        void CalculateLookTime() {
            /*
            RaycastHit hit;
            if (Physics.Raycast(m_Character.position, m_Character.forward, out hit)) {
                if (hit.transform.gameObject.tag == Manager.saTag[(int)Manager.TAG.NECESSARY])
                    m_fTimeLookValid += Time.deltaTime;
                else if (hit.transform.gameObject.tag == Manager.saTag[(int)Manager.TAG.UNNECESSARY])
                    m_fTimeLookVideo += Time.deltaTime;
                else m_fTimeLookInvalid += Time.deltaTime;
            }
            else m_fTimeLookInvalid += Time.deltaTime;
            */
        }

        /**************************************************************************
        // Monobehavier Start
        ***************************************************************************/

        //public ori_HUD m_Hud;
        Grabber m_RightGrabber;
        public GameObject m_goRightGrabber;
        public GameObject m_goRightHandPointer;
        // Start is called before the first frame update
        void Start() {
            m_RightGrabber = m_goRightGrabber.GetComponent<Grabber>();
            Make_Intro();
        }

        // Update is called once per frame
        STATE oldState;
        public TextMeshPro debugMod;
        public bool debugModOnoff = true;
        void Update() {
            if (debugModOnoff) {
                debugMod.text = ""
        + "numberOfTrash=" + "" + "\r\n"; //Constant - 방에 존재하는 쓰레기 수


        }

            switch (m_eState) {
                case STATE.INTRO: Run_Intro(); break;
                case STATE.PLAY: Run_Arrange(); break;
                case STATE.PAUSE: Run_Arrange(); break;
                case STATE.END: Run_End(); break;
                case STATE.NEXT: Run_Next(); break;
            }
            CalculateMoveDistance();
            if (m_eState != oldState) Debug.Log(m_eState);
            oldState = m_eState;
        }

        void Make_Intro() {
            //MakeGrabbable(false);
            //m_Hud.PlayHowTo();
            //m_eState = STATE.INTRO;        
        }

        void Run_Intro() {
        }

        void Make_Arrange() {
            //MakeGrabbable(true);
            m_eState = STATE.PLAY;
        }

        void Run_Arrange() {
            //MeasureTime();
            CalculateLookTime();
            if (m_bTImeOutScene) Make_End();
        }

        public void Make_End() {
            /*
            if (oneSurvey == false)
            {
                oneSurvey = true;
                m_Hud.survey();
            }    
            else
            {

                m_Hud.PlayDuck(false);
                m_Hud.PlayVideo(false);
                MakeGrabbable(false);
                //마지막에 해야할 평가작업등을 추가하십시요
                m_nSurveyResult = buttonQA.m_nResult;
                if (m_bTImeOutScene) m_nFinBtnDown = 3;
                ReportData();


                //이전상태가 TIMEOUT_SCENE상태에서 넘어오면 아쉬지만...표시
                if (m_bTImeOutScene)
                {
                    m_Hud.PlayTimeOut();
                }
                else m_Hud.PlayMoving();
            */
            m_eState = STATE.END;
        }

        void Run_End() {

        }

        void Make_Next() {


        }
        void Run_Next() { }
        /*
        public void Make_Next()
            {

                Load_Next_Scene();
                m_eState = STATE.NEXT;
            }
            

        public float[] m_dataReportFloat = new float[9];// = new float[10];
            public GameDataManager saveJson_MG;
            public AutoVoiceRecording saveVoice_MG;
            public SceneData_Send DataSend;
        
        void ReportData() {

                //Summary 데이타 처리
                m_dataReportFloat[0] = Trash.TOTAL_POSITIONED+Books.TOTAL_POSITIONED; // 바르게 수행한 횟수
                m_dataReportFloat[1] = m_fTimeLookVideo; //흥미로운 영상 시청시간
                m_dataReportFloat[2] = m_fTimeTaken; // 완료까지 걸린 시간
                m_dataReportFloat[3] = Trash.TOTAL_GRAB_COUNT + Books.TOTAL_GRAB_COUNT; //Player가 필요한 물건을 잡은 총 횟수
                m_dataReportFloat[4] = m_fMoveDistance; // 총 이동 거리
                m_dataReportFloat[5] = m_nFinBtnDown; //Player가 Fin Button 클릭 횟수   
                m_dataReportFloat[6] = m_nObstacleTouch; // 방해물체(오리)를 잡은 총 횟수
                m_dataReportFloat[7] = Trash.TOTAL_TRASH- Trash.TOTAL_POSITIONED; // 남은 쓰레기 수
                m_dataReportFloat[8] = Books.TOTAL_BOOK - Books.TOTAL_POSITIONED; // 남은 책 수


                //m_dataReportFloat[4] = Trash.TOTAL_GRAB_TIME + Books.TOTAL_GRAB_TIME; //Player가 필요한 물건을 잡은 총 시간
                //m_dataReportFloat[8] = 0;
                //m_dataReportFloat[9] = 0;


                string eval = ""
                + "numberOfTrash=" + Trash.TOTAL_TRASH + "\r\n" //Constant - 방에 존재하는 쓰레기 수
                + "CleanedTrash= " + Trash.TOTAL_POSITIONED + "\r\n" //Constant - 치운 쓰레기 갯수
                + "TrashgrabCount= " + Trash.TOTAL_GRAB_COUNT + "\r\n" //Player가 쓰레기를 잡은 총 횟수
                + "TrashgrabTime= " + Trash.TOTAL_GRAB_TIME + "\r\n" //Player가 쓰레기를 잡은 총 시간
                + "numberOfTrash=" + Books.TOTAL_BOOK + "\r\n" //Constant - 방에 존재하는 책 수
                + "CleanedTrash= " + Books.TOTAL_POSITIONED + "\r\n" //Constant - 정리한 책 갯수
                + "TrashgrabCount= " + Books.TOTAL_GRAB_COUNT + "\r\n" //Player가 책을 잡은 총 횟수
                + "TrashgrabTime= " + Books.TOTAL_GRAB_TIME + "\r\n" //Player가 책을 잡은 총 시간

                + "m_bTimeOutArrange= " + m_bTimeOutArrange + "\r\n"       //방청소시간 초과 여부 / 총 2분중
                + "m_bTImeOutScene= " + m_bTImeOutScene + "\r\n"        //Scene Play시간 초과여부  / 총 5분중         
                + "m_fTimeTaken= " + m_fTimeTaken + "\r\n"        //정리하는데 걸리는 시간 : 모두정리, 사용자        
                + "m_fTimeLookValid= " + m_fTimeLookValid + "\r\n"        //Player가 필요한곳을 보는 시간 
                + "m_fTimeLookVideo= " + m_fTimeLookVideo + "\r\n"        //Player가 불필요한곳을 보는 시간             
                + "m_fTimeLookInvalid= " + m_fTimeLookInvalid + "\r\n"        //Player가 불필요한곳을 보는 시간             
                + "m_fMoveDistance= " + m_fMoveDistance + "\r\n"        //Player 총이동거리 
                + "m_nObstacleTouch= " + m_nObstacleTouch + "\r\n"        //Player가 방해 물체를 건든 횟수        
                + "m_nFinBtnDown= " + m_nFinBtnDown + "\r\n"       //Player가 Fin Button 클릿횟수        

                + (Trash.TOTAL_POSITIONED + Books.TOTAL_POSITIONED) + "\r\n" // 바르게 수행한 횟수
                + m_fTimeLookVideo+ "\r\n"
                + m_fTimeTaken + "\r\n"
                + (Trash.TOTAL_GRAB_COUNT + Books.TOTAL_GRAB_COUNT) + "\r\n"
                + m_fMoveDistance + "\r\n"
                + m_nFinBtnDown + "\r\n"
                + m_nObstacleTouch + "\r\n"
                + (Trash.TOTAL_TRASH - Trash.TOTAL_POSITIONED) + "\r\n"
                + (Books.TOTAL_BOOK - Books.TOTAL_POSITIONED) + "\r\n"
                ;
                /*
                string eval = ""
                +"m_nTotArrangeable= "  + Arrange.TOTAL_ARRANGE      +"\r\n" //Constant - 손으로 잡을수 있는 물건수
                +"m_nTotCleanable= "    + Arrange.TOTAL_TOCLEAN      +"\r\n" //Constant - 처음부터 어질러놓은, 정리할 물건수, 나머지는 제자리에 있는 잡을수 있는 물건
                +"m_nTotCleaned= "      + Arrange.TOTAL_CLEANED      +"\r\n" //정리할물건이 제위치에 정리된 갯수
                +"m_nTotPositoned= "    + Arrange.TOTAL_POSITIONED   +"\r\n" //전체 손을로 잡을수 있는 물건중 최종적으로 제자리에 있는 물건수        
                +"m_nTotGrabCount= "    + Arrange.TOTAL_GRAB_COUNT   +"\r\n" //Player가 물건을 잡은 총 횟수
                +"m_nTotGrabTime= "     + Arrange.TOTAL_GRAB_TIME    +"\r\n" //Player가 물건을 잡은 총 시간
                +"m_nTotPosiCount= "    + Arrange.TOTAL_POSI_COUNT   +"\r\n" //Player가 물건을 제자리에 갖다 놓으 횟수
                +"m_bTimeOutArrange= "  + m_bTimeOutArrange    +"\r\n"       //방청소시간 초과 여부 / 총 2분중
                +"m_bTImeOutScene= "    + m_bTImeOutScene     +"\r\n"        //Scene Play시간 초과여부  / 총 5분중         
                +"m_fTimeTaken= "       + m_fTimeTaken        +"\r\n"        //정리하는데 걸리는 시간 : 모두정리, 사용자        
                +"m_fTimeLookValid= "   + m_fTimeLookValid    +"\r\n"        //Player가 필요한곳을 보는 시간 
                +"m_fTimeLookInvalid= " + m_fTimeLookInvalid  +"\r\n"        //Player가 불필요한곳을 보는 시간             
                +"m_fMoveDistance= "    + m_fMoveDistance     +"\r\n"        //Player 총이동거리 
                +"m_nObstacleTouch= "   + m_nObstacleTouch    +"\r\n"        //Player가 방해 물체를 건든 횟수        
                +"m_nFinBtnDown= "      + m_nFinBtnDown       +"\r\n";       //Player가 Fin Button 클릿횟수        
                */
        //Debug.Log(eval);   // to console

        //Util.ELOG(eval);   // to logfile .//Eval.txt

        //각 물건별 개별(Arrange.CDB 데이타 처리        
        /*
       for(int i=0; i < Arrange.TOTAL_ARRANGE; i++) {           
           eval = "" 
              +"eArrange= "    + Arrange.CDB[i].eArrange    + ", "     //종류
              +"Object_name= " + Arrange.CDB[i].Object_name + ", "     //GameObject Name 
              +"bCleanable= "  + Arrange.CDB[i].bCleanable  + ", "     //청소대상여부
              +"bPositioned= " + Arrange.CDB[i].bPositioned + ", "     //제위치여부
              +"nGrabCount= "  + Arrange.CDB[i].nGrabCount  + ", "     //Player가 내 물건을 잡은 횟수
              +"fGrabTime= "   + Arrange.CDB[i].fGrabTime   + ", "     //Player가 내 물건을 잡은 시간
              +"nPosiCount = " + Arrange.CDB[i].nPosiCount  + ", "     //자기위치에 둔 횟수
              +"kor_name= "    + Arrange.CDB[i].kor_name    + ", "     //한글명
              +"\r\n";
           //Debug.Log(eval);   // to console
           //Util.ELOG(eval);   // to logfile .//Eval.txt

        }
           */
        /*
        saveJson_MG.SaveCurrentData();
        saveVoice_MG.StopRecordingNBehavior();
        DataSend.GetSceneData();
    }

    /*****************************************************************************
    // Helper & Utility
    ******************************************************************************/
        /*
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
        */
    } }