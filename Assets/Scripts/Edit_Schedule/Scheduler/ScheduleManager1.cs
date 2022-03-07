using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KetosGames.SceneTransition;
using BNG;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;

public enum ESoundType
{
    In,
    Click,
    Cnt,
    Put
}

namespace Scheduler
{
    [RequireComponent(typeof(Transform))]
    public class ScheduleManager1 : MonoBehaviour
    {
        
        public CollectData collectData;
        public AutoVoiceRecording voiceRecording;
        
        private const float TimeLimit = 120; //시간 제한 사용 방향 기획 필요
        private const float TimeLimitForFinish = 180; //강제종료시간

        [SerializeField] private Transform hud;
        public Transform mainUi;
        public Transform intro;
        public GameObject board;
        public Transform finishPanel;
        public Transform wellDoneAndBye;
        public TextMeshProUGUI finishCntDwn;

        public Transform bgmController;

        public Text result;

        public Transform btnFinish;

        public TextMeshProUGUI textTitle;

        public Text toShow;

        public Transform slot;
        public Transform grp;
        public Transform originPos;

        public Text timerText;

        private List<Transform> slotList; 
        public List<Transform> grpList;
        private List<Transform> oPosList;

        public float[] Scene2Arr { get; set; }

        public bool isReset;
        public bool pointerLock;

        private bool leGogo = false;
        private bool beforeStart = false;
        private bool firstSelect = false;
        private bool beforeStartGuideCheck = false;

        private bool checkTimeLimit = false;
        private bool checkTimeOut = false;

        private float guide_Length = 25;

        [SerializeField] private int _completionCtn;
        private int timerMin = 2;
        private int timerSec = 30;
        
        private float totalElapsedTime = 0;         //수행한 시간 계산용
        private float totalElapsedTimeForCalc = 0;  //수행한 시간 보여주기용
        private float timerForBeforeStarted = 0;    //시작하기 누르기까지 시간
        private float timerForFirstSelect = 0;      //시작하기 누르고 첫 선택까지 시간

        //float TotalScenePlayingTime = 0;    //컨텐츠 시작부터 끝까지 총 시간 TimerForBeforeStarted + TotalElapsedTimeForCalc

        public int totalMovingCnt = 0;      //이동 횟수
        public int resetCnt = 0;            //초기화 누른 횟수
        private int selectNoCtn = 0;                 //아니오 누른 횟수

        private int skipYn = 0;

        //------------- SOUND    
        [FormerlySerializedAs("sound_Count")] public AudioClip soundCount;
        [FormerlySerializedAs("sound_In")] public AudioClip soundIn;
        [FormerlySerializedAs("sound_Click")] public AudioClip soundClick;
        [FormerlySerializedAs("sound_Put")] public AudioClip soundPut;
        private AudioSource audioSource;

        //------------- Manager
        [FormerlySerializedAs("SetPlayerData")] public Transform setPlayerData;
        [FormerlySerializedAs("GameDataManager")] public Transform gameDataManager;

        private void Awake()
        {
            _completionCtn = 0;

            pointerLock = false;
            beforeStart = true;
            isReset = false;

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
                            timerMin = 0;
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
                        //시간제한
                        checkTimeLimit = true;
                        StartCoroutine(TimeLimitAndKeepGoing());
                    }

                    if (!checkTimeOut && totalElapsedTimeForCalc >= TimeLimitForFinish)
                    {
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
            bgmController.GetComponent<BGMcontroller>().PlayBGMByTypes("LIMIT");

            timerSec = 30;

            yield return new WaitForSeconds(6);

            bgmController.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");
        }

        private IEnumerator TimeOutAndFinish()
        {
            collectData.AddTimeStamp("TIME OUT");
            bgmController.GetComponent<BGMcontroller>().PlayBGMByTypes("OUT");

            yield return new WaitForSeconds(6);

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
                    go.GetComponent<PlanCubeController1>().enabled = false;
                }
            }
        }

        public void ReleaseAllCollision()
        {
            foreach (Transform go in grpList)
            {
                go.GetComponent<PlanCubeController1>().enabled = true;
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
                if (s.GetComponent<PlanSlotController1>().passenger != null)
                {
                    check = true;
                }
            }

            return check;
        }

        public void ReSetAll()
        {
            PlaySoundByTypes(ESoundType.Click);
            foreach (var oP in oPosList)
            {
                oP.GetComponent<OriginPosController>().ResetOriginPos();
            }
            
            foreach(var t in grpList)
            {
                StartCoroutine(t.GetComponent<PlanCubeController1>().ResetPlanCube(0.07f));
            }

            if (CheckEmptySlot())
            {
                StartCoroutine(ResetDelay(0.05f));

                foreach (var t in slotList)
                {
                    t.GetComponent<PlanSlotController1>().ResetPlanSlot();
                }
                
                // foreach(var t in grpList)
                // {
                //     StartCoroutine(t.GetComponent<PlanCubeController1>().resetPlanCube(0.07f));
                // }
              
                foreach (var t in slotList)
                {
                    StartCoroutine(t.GetComponent<PlanSlotController1>().ResetSlotMesh(0.1f));
                }
                ResetGrpList();

                btnFinish.gameObject.SetActive(false);
                resetCnt += 1;
            }
        }

        public void CheckAllScheduleOnSlot()
        {
            var allDone = true;

            foreach (var aSlot in slotList.Where(aSlot => aSlot.GetComponent<PlanSlotController1>().passenger == null))
            {
                allDone = false;
            }

            if (allDone)
            {
                btnFinish.gameObject.SetActive(true);
            }
        }


        public void DoStartSchedule()
        {
            PlaySoundByTypes(ESoundType.Click);
            StartCoroutine(StartCntDown());
            if (_completionCtn >= 2)
            {
                this.gameObject.GetComponent<Button>().interactable = false;
            }
        }

        IEnumerator StartCntDown()
        {
            bgmController.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");

            textTitle.text = "<color=#FFFFFF>준비 ~";

            yield return new WaitForSeconds(1f);
            PlaySoundByTypes(ESoundType.Cnt);
            textTitle.text = "<color=#FFFFFF>3";
            yield return new WaitForSeconds(1);
            PlaySoundByTypes(ESoundType.Cnt);
            textTitle.text = "<color=#FFFFFF>2";
            yield return new WaitForSeconds(1);
            PlaySoundByTypes(ESoundType.Cnt);
            textTitle.text = "<color=#FFFFFF>1";
            yield return new WaitForSeconds(1);
            textTitle.text = "<color=#FFFFFF>시작 !";
            yield return new WaitForSeconds(1f);

            mainUi.GetComponent<GraphicRaycaster>().enabled = true;
            intro.gameObject.SetActive(false);
            board.gameObject.SetActive(true);
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
            PlaySoundByTypes(ESoundType.Click);
            finishPanel.gameObject.SetActive(true);
        }

        public void FinishPanel_No()
        {
            PlaySoundByTypes(ESoundType.Click);
            finishPanel.gameObject.SetActive(false);

            selectNoCtn += 1;
        }

        public void FinishPanel_Yes(bool Skipped)
        {
            if (_completionCtn < 2)
            {
                // 두번째 안내 팝업이 필요
                _completionCtn += 1;
                board.gameObject.SetActive(false);
                finishPanel.gameObject.SetActive(false);
                StartCoroutine(hud.GetComponent<HUDSchedule>().HalfInfoSetUiTxt());
            }
            
            PlaySoundByTypes(ESoundType.Click);
            collectData.AddTimeStamp("MISSION END");

            leGogo = false;
            float PlanData = 0;

            board.gameObject.SetActive(false);
            finishPanel.gameObject.SetActive(false);
            wellDoneAndBye.gameObject.SetActive(true);

            voiceRecording.StopRecordingNBehavior();

            if (Skipped)
            {
                skipYn = 1;
                intro.gameObject.SetActive(false);
                //Schedule.gameObject.SetActive(true);
            }
            else
            {
                skipYn = 0;
                var myScheduleForJson = "";

                foreach (var planBox in slotList.Select(planSlot => planSlot.GetComponent<PlanSlotController>().passenger.transform))
                {
                    if (planBox != null)
                    {
                        var text = planBox.GetChild(0).GetComponent<Text>().text + " ";
                        myScheduleForJson += planBox.GetChild(1).name;
                    }
                    else
                    {
                        myScheduleForJson += "0";
                    }
                }

                PlanData = float.Parse(myScheduleForJson, System.Globalization.CultureInfo.InvariantCulture);
            }


            /*
            Data_201 계획을 완료하는데 걸린 총 시간                               TotalElapsedTimeForCalc
            Data_202 계획을 얼마나 바꾸는지(이동)                                 TotalMovingCnt
            Data_203 계획 초기화(다시하기)를 누른 횟수                            ResetCnt
            Data_204 완료 결정을 못하고 번복한 횟수                               ClickNoCnt
            Data_205 완료된 계획 전송                                             PlanData
            Data_206 중도 포기(스킵)                                              SkipYn
            Data_207 시작버튼 누르기 까지 걸린 시간                               TimerForBeforeStarted
            Data_208 시작하기 누른 후 첫번째 계획 선택까지 걸린 시간              TimerForFirstSelect
            */


            // 흩어져 있는 데이터들을 배열에 넣어 전달할 준비
            Scene2Arr = new[] { totalElapsedTimeForCalc, totalMovingCnt, resetCnt, selectNoCtn, PlanData, skipYn, timerForBeforeStarted, timerForFirstSelect };
            gameDataManager.GetComponent<GameDataManager>().SaveCurrentData();
            StartCoroutine(GoToNextScene());
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

            KetosGames.SceneTransition.SceneLoader.LoadScene(3);
        }

        public void PlaySoundByTypes(ESoundType soundType)
        {
            audioSource.clip = soundType switch
            {
                ESoundType.Click => soundClick,
                ESoundType.In => soundIn,
                ESoundType.Cnt => soundCount,
                ESoundType.Put => soundPut,
                _ => null
            };
            audioSource.Play();
        }
    }
}