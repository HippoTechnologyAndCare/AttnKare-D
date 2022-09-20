using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BNG;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;
using SceneLoader = KetosGames.SceneTransition.SceneLoader;

public enum ESoundType02
{
    In,
    Click,
    Cnt,
    Put,
    Pop
}

namespace Scheduler
{
    [RequireComponent(typeof(Transform))]
    public class ScheduleManager2 : MonoBehaviour
    {
        public Dictionary<string, int> CardCtnDic;
        public Dictionary<int, string> SchedulerDict;
        public CollectData collectData;
        public AutoVoiceRecording voiceRecording;
        public DataChecker dataChecker;
        public BGMcontroller02 audioCon;
        public ScoreManager02 scoreManager;
        private string buildScene = "NewPaddle";
        
        private const float TimeLimit = 150; //시간 제한 사용 방향 기획 필요
        private const float TimeLimitForFinish = 180; //강제종료시간
        private const int TotalCardsCtn = 6;         // 총 카드 수
        //private const int YelCardCtn = 2;             // 노란 카드의 총 개수

        [SerializeField] private Transform rightPointer;

        [SerializeField] private Transform hud;
        [SerializeField] private Transform mainUi;
        [SerializeField] private Transform subUi;
        [SerializeField] private Transform board;
        [SerializeField] private Transform finishPanel;
        [SerializeField] private Transform cardTitle;
        [SerializeField] private Transform startBtn;
        [SerializeField] private Transform wellDoneAndBye;
        [SerializeField] private Transform arrows;
        [SerializeField] private Transform title;
        
        [Header("UI Reference")]
        [SerializeField] private TextMeshProUGUI finishCntDwn;
        [SerializeField] private Material yellowMat;

        [SerializeField] private Transform bgmController;

        [SerializeField] private Text result;

        [SerializeField] private Transform btnFinish;
        [SerializeField] private Transform starParticle;
        [SerializeField] private Transform boomParticle;
        [SerializeField] private Transform tesEmoji;

        [SerializeField] private TextMeshProUGUI textTitle;

        [SerializeField] private Text toShow;

        [SerializeField] private GameObject defCards;
        [SerializeField] private Transform slot;
        [SerializeField] private Transform grp;
        [SerializeField] private Transform originPos;

        [SerializeField] private Text timerText;

        [SerializeField] private List<Transform> slotList; 
        public List<Transform> grpList;
        private List<Transform> oPosList;
        
        //[SerializeField] private string[] yelCardsArr = new string[YelCardCtn];

        public object[] Scene2Arr { get; set; }

        public bool is1stInfoSkip;
        public bool isReset;
        public bool pointerLock;
       
        private bool clickedReset;

        private bool leGogo;
        private bool beforeStart;
        private bool firstSelect;
        private bool beforeStartGuideCheck;

        private bool checkTimeLimit;
        private bool checkTimeOut;

        private float _planData01;
        private float _planData02;
        
        private float guide_Length = 25;

        [SerializeField] private int completionCtn;
        private int timerMin = 2;
        private int timerSec = 30;
        
        private float totalElapsedTime;         //수행한 시간 계산용
        private float totalElapsedTimeForCalc;  //수행한 시간 보여주기용

        public bool m_bClickOneTime = false;

        //float TotalScenePlayingTime = 0;    //컨텐츠 시작부터 끝까지 총 시간 TimerForBeforeStarted + TotalElapsedTimeForCalc

        [Header("Data Field")]
        [SerializeField] private int totalMovingCnt;      //이동 횟수
        [SerializeField] private int resetCnt;            //초기화 누른 횟수
        [SerializeField] private int selectNoCtn;                 //아니오 누른 횟수
        [SerializeField] private int skipYn;              //스킵 여부
        [SerializeField] private float timerForBeforeStarted;    //시작하기 누르기까지 시간
        [SerializeField] private float timerForFirstSelect;      //시작하기 누르고 첫 선택까지 시간
        [SerializeField] private float data210;
        [SerializeField] private float data211;
        [SerializeField] private float data212;
        [SerializeField] private float data213;

        [Header("Audio Clips")] 
        [FormerlySerializedAs("sound_Count")] public AudioClip soundCount;
        [FormerlySerializedAs("sound_In")] public AudioClip soundIn;
        [FormerlySerializedAs("sound_Click")] public AudioClip soundClick;
        [FormerlySerializedAs("sound_Put")] public AudioClip soundPut;
        [SerializeField] private AudioClip soundPop;
        private AudioSource audioSource;

        [Header("Managers")]
        [FormerlySerializedAs("SetPlayerData")] public Transform setPlayerData;
        [FormerlySerializedAs("GameDataManager")] public Transform gameDataManager;
        [SerializeField] private SceneData_Send DataSend;

        private void Awake()
        {
            data210 = 0;
            data211 = -1;
            data212 = -1;
            data213 = -1;

            completionCtn = 0;
            skipYn = 0;

            clickedReset = false;
            pointerLock = false;
            beforeStart = true;
            isReset = false;

            CardCtnDic = new Dictionary<string, int>();
            SchedulerDict = new Dictionary<int, string>();
            
            audioSource = this.GetComponent<AudioSource>();
            slotList = new List<Transform>();
            grpList = new List<Transform>();
            oPosList = new List<Transform>();
            
        }

        private void Start()
        {
            InitSlotList();
            InitGrpList();
            InitOposList();
            InitSchedulerDict();

            collectData.AddTimeStamp("GUIDE START");
        }

        private void Update()
        {
            if (leGogo)
            {
                totalElapsedTime += Time.deltaTime;

                if (totalElapsedTime > 1)
                {
                    totalElapsedTime = 0;
                    totalElapsedTimeForCalc += 1;

                    if (timerMin != 0 || timerSec != 0)
                    {
                        timerSec -= 1;

                        if (timerSec < 0 && timerMin > 0)
                        {
                            timerSec = 59;
                            timerMin -= 1;
                        }

                        string textSec = "";

                        if (timerSec < 10)
                        {
                            textSec = "0" + timerSec.ToString();
                        }
                        else
                        {
                            textSec = timerSec.ToString();
                        }


                        timerText.text = "0" + timerMin.ToString() + ":" + textSec;
                    }

                    if (!checkTimeLimit && totalElapsedTimeForCalc >= TimeLimit)
                    {
                        Debug.Log("TIME LIMIT");
                        //시간제한
                        checkTimeLimit = true;
                        StartCoroutine(TimeLimitAndKeepGoing());
                    }

                    if (!checkTimeOut && totalElapsedTimeForCalc >= TimeLimitForFinish)
                    {
                        Debug.Log("TIME OUT");
                        //강제종료
                        checkTimeOut = true;
                        StartCoroutine(TimeOutAndFinish());
                    }
                }
            }

            if (beforeStart)
            {
                timerForBeforeStarted += Time.deltaTime;

                if (!beforeStartGuideCheck && timerForBeforeStarted > guide_Length)
                {
                    collectData.AddTimeStamp("GUIDE END");
                    beforeStartGuideCheck = true;
                }
            }

            if (firstSelect)
            {
                timerForFirstSelect += Time.deltaTime;
            }
        }

        IEnumerator TimeLimitAndKeepGoing()
        {
            collectData.AddTimeStamp("TIME LIMIT");
            bgmController.GetComponent<BGMcontroller02>().PlayBGMByTypes("LIMIT");
            rightPointer.gameObject.SetActive(false);

            timerSec = 30;

            yield return new WaitForSeconds(6);
            rightPointer.gameObject.SetActive(true);
            
            bgmController.GetComponent<BGMcontroller02>().PlayBGMByTypes("BGM");
        }
        
        private IEnumerator TimeOutAndFinish()
        {
            collectData.AddTimeStamp("TIME OUT");
            bgmController.GetComponent<BGMcontroller02>().PlayBGMByTypes("OUT");
            rightPointer.gameObject.SetActive(false);

            yield return new WaitForSeconds(6);
            rightPointer.gameObject.SetActive(true);

            FinishPanel_Yes(true);
        }

        private IEnumerator ResetDelay(float wait)
        {
            isReset = true;
            yield return new WaitForSeconds(wait);
            isReset = false;           
        }

        private List<Transform> InitSlotList()
        {
            
            foreach (Transform s in slot)
            {
                slotList.Add(s);
            }
            return slotList;
        }

        public List<Transform> InitOposList()
        {
            foreach (var oP in oPosList)
            {
                oPosList.Add(oP);
            }
            return oPosList;
        }
        
        public List<Transform> InitGrpList()
        {
            if (grpList == null) return grpList;
            RemoveGrpList();
            AddGrpList();
            Debug.Log("grpList 초기화");
            return grpList;
        }

        private void AddGrpList()
        {
            foreach (Transform g in grp)
            {
                grpList.Add(g);
            }
            Debug.Log("grpList 추가");
        }

        private void RemoveGrpList()
        {
            foreach (Transform g in grp)
            {
                grpList.Remove(g);
            }
            Debug.Log("grpList 삭제");
        }

        private List<Transform> ResetGrpList()
        {            
            string keyword = "(Clone)";
            
            foreach (Transform g in grp)
            {
                if(g != null)
                {
                    if (RemoveWord.EndsWithWord(g.name, keyword))
                    {
                        grpList.Remove(g);
                    }
                }                                              
            }            
            return grpList;
        }

        public void LockAllCollision(Transform obj)
        {
            foreach (var go in grpList)
            {
                if (go != obj)
                {
                    go.GetComponent<PlanCubeController2>().enabled = false;
                }
            }
        }

        public void ReleaseAllCollision()
        {
            foreach (Transform go in grpList)
            {
                go.GetComponent<PlanCubeController2>().enabled = true;
            }
        }

        public void CheckMovingCnt()
        {
            totalMovingCnt += 1;
            firstSelect = false;
        }

        bool CheckEmptySlot()
        {
            bool check = false;

            foreach (var s in slotList)
            {
                if (s.GetComponent<PlanSlotController2>().passenger != null)
                {
                    check = true;
                }
            }

            return check;
        }

        public void ClickedReset()
        {
            clickedReset = true;
            ReSetAll();
        }
        
        public void ReSetAll()
        {
            PlaySoundByTypes(ESoundType02.Click);
            foreach (var oP in oPosList)
            {
                oP.GetComponent<OriginPosController01>().ResetOriginPos();
            }
            
            foreach(var t in grpList)
            {
                StartCoroutine(t.GetComponent<PlanCubeController2>().ResetPlanCube(0.07f));
            }

            if (CheckEmptySlot())
            {
                StartCoroutine(ResetDelay(0.05f));

                foreach (var t in slotList)
                {
                    t.GetComponent<PlanSlotController2>().ResetPlanSlot();
                }

                foreach (var t in slotList)
                {
                    StartCoroutine(t.GetComponent<PlanSlotController2>().ResetSlotMesh(0.1f));
                }
                ResetGrpList();

                btnFinish.gameObject.SetActive(false);

                if (clickedReset)
                {
                    resetCnt += 1;
                    clickedReset = false;
                }
               
            }
            m_bClickOneTime = false;
        }

        public void CheckAllScheduleOnSlot()
        {
            var allDone = true;

            foreach (var aSlot in slotList.Where((aSlot => aSlot.GetComponent<PlanSlotController2>().passenger == null)))
            {
                allDone = false;
            }

            if (allDone)
            {
                btnFinish.gameObject.SetActive(true);
                PlaySoundByTypes(ESoundType02.Pop);
                starParticle.GetComponent<ParticleSystem>().Play();
            }
            else btnFinish.gameObject.SetActive(false);
        }

        private void InitSchedulerDict()
        {
            for (var i = 0; i < TotalCardsCtn; i++)
            {
                SchedulerDict.Add(key:i + 1, value: "");
            }
        }
        
        // CardDict를 0으로 초기화하는 함수
        private void InitTotalCardCtnDict()
        {
            for (var i = 0; i < TotalCardsCtn; i++)
            {
                //cardList.Add(Convert.ToChar(i + 65).ToString());
                CardCtnDic.Add(key:Convert.ToChar(i + 65).ToString(), value:0);
            }
        }
        
        // // 들어온 매개변수와 서로 일치하는 CardCtnDic의 멤버(key)의 value를 수정해준다(count Up)
        // private void UsedCardsCtn(Transform passenger)
        // {
        //     string keyword = "(Clone)";
        //     string originName;
        //
        //     if (completionCtn <= 1)
        //     {
        //         foreach (var card in CardCtnDic.Keys.ToList())
        //         {
        //             if(passenger != null)
        //             {
        //                 // 클론 카드인지 확인하고 클론이면 오브젝트명에서 clone부분 삭제
        //                 if (RemoveWord.EndsWithWord(passenger.name, keyword))
        //                 {
        //                     originName = passenger.name.Replace("(Clone)", "");
        //                 }
        //                 
        //                 // 오리지널 카드일 경우
        //                 else
        //                 {
        //                     originName = passenger.name;
        //                 }
        //                 
        //                 if (card == originName)
        //                 {
        //                     CardCtnDic[card] += 1;
        //                     Debug.Log(card + " is " +   CardCtnDic[card]);
        //                 }
        //             }                                              
        //         }
        //     }
        //
        //     else
        //     {
        //         foreach (var yelCard in yelCardsArr)
        //         {
        //             if (passenger != null)
        //             {
        //                 if (RemoveWord.EndsWithWord(passenger.name, keyword))
        //                 {
        //                     originName = passenger.name.Replace("(Clone)", "");
        //                 }
        //
        //                 else
        //                 {
        //                     originName = passenger.name;
        //                 }
        //
        //                 if (yelCard == originName)
        //                 {
        //                     // yellow 카드를 사용한 횟수 데이터를 +1 시켜야 한다
        //                 }
        //             }
        //         }
        //     }
        // }

        // private void SetYellowForCards()
        // {
        //     // Dict안에서 value가 큰 순서대로 내림차순 정렬
        //     var sortedDict = from entry in CardCtnDic
        //         orderby entry.Value descending select entry;
        //         
        //     CardCtnDic = sortedDict.ToDictionary<KeyValuePair<string, int>,
        //         string, int>(pair => pair.Key, pair => pair.Value);
        //     
        //     // 제일 많이 사용한 카드 두장을 yelCardArr 배열에 넣는다
        //     for (int i = 0; i < YelCardCtn; i++)
        //     {
        //         yelCardsArr[i] = CardCtnDic.Keys.ElementAt(i);
        //     }
        //
        //     // yelCardArr 배열을 참고로 grpList안에 있는 2개의 카드의 색상을 변경
        //     foreach (var orgCard in grpList)
        //     {
        //         foreach (var entry in yelCardsArr)
        //         {
        //             if (orgCard.name == entry)
        //             {
        //                 orgCard.GetChild(2).GetComponent<MeshRenderer>().material = yellowMat;
        //             }
        //         }
        //     }
        // }
        
        public void DoStartSchedule()
        {
            PlaySoundByTypes(ESoundType02.Click);
            StartCoroutine(StartCntDown());
            if (completionCtn >= 2)
            {
                this.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        
        IEnumerator StartCntDown()
        {
            bgmController.GetComponent<BGMcontroller02>().PlayBGMByTypes("BGM");

            textTitle.text = "<color=#FFFFFF>준비 ~";

            yield return new WaitForSeconds(1f);
            PlaySoundByTypes(ESoundType02.Cnt);
            textTitle.text = "<color=#FFFFFF>3";
            yield return new WaitForSeconds(1);
            PlaySoundByTypes(ESoundType02.Cnt);
            textTitle.text = "<color=#FFFFFF>2";
            yield return new WaitForSeconds(1);
            PlaySoundByTypes(ESoundType02.Cnt);
            textTitle.text = "<color=#FFFFFF>1";
            yield return new WaitForSeconds(1);
            textTitle.text = "<color=#FFFFFF>시작 !";
            yield return new WaitForSeconds(1f);

            startBtn.GetChild(0).GetComponent<Button>().enabled = true;
            mainUi.GetComponent<GraphicRaycaster>().enabled = true;
            
            VisibleStartBtn(false);
            //board.gameObject.SetActive(true);
            board.GetComponent<CanvasGroup>().blocksRaycasts = true;
            beforeStart = false;
            leGogo = true;
            firstSelect = true;

            if (!beforeStartGuideCheck)
            {
                collectData.AddTimeStamp("GUIDE SKIP");
            }

            collectData.AddTimeStamp("MISSION START");
        }
        
        public void ShowFinishPanel()
        {
            PlaySoundByTypes(ESoundType02.Click);
            
            VisibleBoard(false);
            VisibleFinPanel(true);
            btnFinish.gameObject.SetActive(false);
        }
        
        public void FinishPanel_No()
        {
            PlaySoundByTypes(ESoundType02.Click);
         
            VisibleBoard(true);
            VisibleFinPanel(false);
            btnFinish.gameObject.SetActive(true);
            starParticle.GetComponent<ParticleSystem>().Play();

            selectNoCtn += 1;
        }

        public void FinishPanel_Yes(bool isSkip)
        {
            if (!m_bClickOneTime)
            {
                m_bClickOneTime = true;

            
            Debug.Log("isSkip = " + isSkip);
            // 완료한 계획표 횟수
            if (!isSkip)
            {
                completionCtn += 1;
            }

            PlaySoundByTypes(ESoundType02.Click);

            if (completionCtn >= 2)
            {
                SchedulerDict.Clear();
                InitSchedulerDict();
            }

            // n번째로 완성한 계획표 변수로 저장
            SortedCardData(isSkip);

            scoreManager.ScoreCalculator();

            StartCoroutine(hud.GetComponent<HUDSchedule02>().SetReadyShowScore());
            StartCoroutine(hud.GetComponent<HUDSchedule02>().ShowScore());
            }
            // // 몇번째 완료인지 체크 
            //    // 첫번째 계획표를 마쳤는지에 대한 조건문
            // if (completionCtn == 1 && !isSkip) // 첫번째 완료라면 아래의 프로세싱 후 재 시작
            // {
            //     VisibleBoard(true);
            //     mainUi.GetComponent<GraphicRaycaster>().enabled = false;
            //     VisibleFinPanel(false);
            //     ReSetAll();
            //     
            //     StartCoroutine(hud.GetComponent<HUDSchedule02>().HalfInfoSetUiTxt());
            //     StartCoroutine(audioCon.PlaySecInfo());
            //     return;
            // }
            //
            //    // 두번째 계획표를 마쳤을때의 로직
            // collectData.AddTimeStamp("MISSION END");
            //
            // leGogo = false;
            //
            // slot.gameObject.SetActive(false);
            // grp.gameObject.SetActive(false);
            // VisibleFinPanel(false);
            //
            // //scoreManager.ScoreCalculator();
            // StartCoroutine(AskQuestion());
            if(checkTimeOut) StartCoroutine(AskQuestion());
            Debug.Log("NEXT");
        }

        public void ClickConfirmBtn()
        {
            if(!m_bClickOneTime){
                m_bClickOneTime = true;
                PlaySoundByTypes(ESoundType02.Click);


                // 몇번째 완료인지 체크 
                // 첫번째 계획표를 마쳤는지에 대한 조건문
                if (completionCtn == 1) // 첫번째 완료라면 아래의 프로세싱 후 재 시작
                {
                    dataChecker.SetScoreD211();
                    VisibleBoard(true);
                    mainUi.GetComponent<GraphicRaycaster>().enabled = false;
                    VisibleFinPanel(false);
                    ReSetAll();

                    StartCoroutine(hud.GetComponent<HUDSchedule02>().HalfInfoSetUiTxt());
                    StartCoroutine(audioCon.PlaySecInfo());
                    return;
                }

                // 두번째 계획표를 마쳤을때의 로직
                dataChecker.SetScoreD212();
                collectData.AddTimeStamp("MISSION END");

                leGogo = false;

                slot.gameObject.SetActive(false);
                grp.gameObject.SetActive(false);
                VisibleFinPanel(false);

                //scoreManager.ScoreCalculator();

                StartCoroutine(AskQuestion());
            }
        }
        
        private IEnumerator AskQuestion()
        {
            yield return new WaitForSeconds(1f);
            audioCon.PlayBGMByTypes("Question");
            tesEmoji.gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            
            hud.GetComponent<HUDSchedule02>().PopupQuestion(true);
        }
        
        // 마지막 질문에 대한 대답이 Yes일때의 함수
        public void Yes_Question()
        {
            data213 = 1;
            hud.GetComponent<HUDSchedule02>().PopupQuestion(false);
            tesEmoji.gameObject.SetActive(false);
            EndScene();
        }

        // 마지막 질문에 대한 대답이 No일때의 함수
        public void No_Question()
        {
            data213 = 0;
            hud.GetComponent<HUDSchedule02>().PopupQuestion(false);
            tesEmoji.gameObject.SetActive(false);
            EndScene();
        }

        private void EndScene()
        {
            wellDoneAndBye.gameObject.SetActive(true);
            boomParticle.GetComponent<ParticleSystem>().Play();
            voiceRecording.StopRecordingNBehavior();
            
            /*
            Data_201 계획을 완료하는데 걸린 총 시간                               TotalElapsedTimeForCalc
            Data_202 계획을 얼마나 바꾸는지(이동)                                 TotalMovingCnt
            Data_203 계획 초기화(다시하기)를 누른 횟수                            ResetCnt
            Data_204 완료 결정을 못하고 번복한 횟수                               ClickNoCnt
            Data_205 첫번째로 완료된 계획 전송                                   PlanData01
            Data_206 두번째로 완료된 계획 전송                                   PlanData02
            Data_207 중도 포기(스킵)                                              SkipYn
            Data_208 시작버튼 누르기 까지 걸린 시간                               TimerForBeforeStarted
            Data_209 시작하기 누른 후 첫번째 계획 선택까지 걸린 시간              TimerForFirstSelect
            Data_210 미션과 관계없는 생물을 건든 횟수
            Data_211 첫번째 계획표 점수
            Data_212 두번째 계획표 점수
            Data_213 질문 : 계획표 작성에 집중하기 어려웠니?
            */
            
            data210 = dataChecker.scheduleData.data210;
            data211 = dataChecker.scheduleData.data211;
            data212 = dataChecker.scheduleData.data212;
            // 흩어져 있는 데이터들을 배열에 넣어 전달할 준비
            Scene2Arr = new object[] { totalElapsedTimeForCalc, totalMovingCnt, resetCnt, selectNoCtn, _planData01,_planData02, 
                skipYn, timerForBeforeStarted, timerForFirstSelect, data210, data211, data212, data213 };
            gameDataManager.GetComponent<GameDataManager>().SaveCurrentData();
            DataSend.GetSceneData();
            StartCoroutine(GoToNextScene());
        }
        
        private void VisibleBoard(bool isOn)
        {
            
            if (isOn)
            {
                defCards.SetActive(true);
                title.gameObject.SetActive(true);
                arrows.gameObject.SetActive(true);
                cardTitle.gameObject.SetActive(true);
                hud.GetComponent<HUDSchedule02>().FadeInPanel(board, 1f);
                
                board.GetComponent<CanvasGroup>().blocksRaycasts = true;

                foreach (var currCard in grpList)
                {
                    //currCard = currCard.GetComponent<PlanSlotController1>().passenger.transform;
                    currCard.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
                    //currCard.GetComponent<MeshRenderer>().enabled = true;
                }

                foreach (var slot in slotList)
                {
                    if (slot.GetComponent<PlanSlotController2>().passenger == null)
                    {
                        slot.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
            
            else
            {
                defCards.SetActive(false);
                title.gameObject.SetActive(false);
                arrows.gameObject.SetActive(false);
                cardTitle.gameObject.SetActive(false);
                hud.GetComponent<HUDSchedule02>().FadeOutPanel(board, 1f);
                
                board.GetComponent<CanvasGroup>().blocksRaycasts = false;
                
                foreach (var currCard in grpList)
                {
                    //currCard = currCard.GetComponent<PlanSlotController1>().passenger.transform;
                    currCard.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
                    //currCard.GetComponent<MeshRenderer>().enabled = false;
                }
                
                foreach (var slot in slotList)
                {
                    slot.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        
        public void VisibleFinPanel(bool isOn)
        {
            if (isOn)
            {
                finishPanel.GetComponent<CanvasGroup>().alpha = 1;
                finishPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            
            else
            {
                finishPanel.GetComponent<CanvasGroup>().alpha = 0;
                finishPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        public void VisibleStartBtn(bool isOn)
        {
            if (isOn)
            {
                startBtn.GetComponent<CanvasGroup>().alpha = 1;
                startBtn.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            
            else
            {
                startBtn.GetComponent<CanvasGroup>().alpha = 0;
                startBtn.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
        
        // 카드를 정렬한 순서를 변수로 저장
        private void SortedCardData(bool isSkip)
        {
            if (isSkip)
            {
                skipYn = 1;
                VisibleBoard(false);
                VisibleFinPanel(false);
                VisibleStartBtn(false);
            }
            else
            {
                int currKey;
                Transform currCard;
                skipYn = 0;
                var myScheduleForJson = "";

                // 슬롯 리스트에 들어있는 카드를 확인해 타임라인 순서대로 나열해 변수로 저장
                foreach (var slot in slotList)
                {
                    if (slot.GetComponent<PlanSlotController2>().passenger != null)
                    {
                        currCard = slot.GetComponent<PlanSlotController2>().passenger.transform;
                        
                        // slot이름과 일치하는 ScheDict Key일때 passenger이름을 Value에 넣는다
                        int.TryParse(slot.name, out currKey);
                        
                        if (SchedulerDict.ContainsKey(currKey))             
                        {
                            SchedulerDict[currKey] = currCard.name;
                        }

                        if (slot != null)
                        {
                            // string 변수에 카드 순서를 writing 하는 곳 
                            var text = currCard.GetChild(0).GetComponent<Text>().text + " ";
                            myScheduleForJson += currCard.GetChild(1).name;
                        }
                        // 오류로 슬롯이 비어 있다면 0을 추가
                        else
                        {
                            myScheduleForJson += "0";
                        }
                    }
                }
                
                // ScheDict에 키 밸류 페어가 맞게 되었는지 Test  
                foreach (var result in SchedulerDict)
                {
                    Debug.Log(result.Key + " in " + result.Value);
                }

                switch (completionCtn)
                {
                    case 1:
                        _planData01 = float.Parse(myScheduleForJson, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case 2:
                        _planData02 = float.Parse(myScheduleForJson, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    default:
                        Debug.Log("계획표를 완료한 횟수가 유효하지 않습니다");
                        break;
                }
            }
        }

        private IEnumerator GoToNextScene()
        {
            yield return new WaitForSeconds(2);

            finishCntDwn.text = "3";
            yield return new WaitForSeconds(1);

            finishCntDwn.text = "2";
            yield return new WaitForSeconds(1);

            finishCntDwn.text = "1";
            yield return new WaitForSeconds(1);
            SceneLoader.LoadScene(buildScene);
        }

        public void PlaySoundByTypes(ESoundType02 soundType)
        {
            audioSource.clip = soundType switch
            {
                ESoundType02.Click => soundClick,
                ESoundType02.In => soundIn,
                ESoundType02.Cnt => soundCount,
                ESoundType02.Put => soundPut,
                ESoundType02.Pop => soundPop,
                _ => null
            };
            audioSource.Play();
        }
        
        //Test Mode
        
        [ContextMenu("OutPut CartCtn")]
        public void OutPutCardCtnDic()
        {
            foreach (var card in CardCtnDic)
            {
                Debug.Log("key : " + card.Key + " value : " + card.Value);
            }
        }
        
        private IEnumerator WaitSec(float sec)
        {
            yield return new WaitForSeconds(sec);
        }
    }
}