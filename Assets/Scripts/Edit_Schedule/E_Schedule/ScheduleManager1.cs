using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KetosGames.SceneTransition;
using BNG;
using UnityEngine.Serialization;

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
        //public Transform Behavior;
        public CollectData collectData;
        public AutoVoiceRecording voiceRecording;
        //Behavior.GetComponent<BNG.CollectData>().AddTimeStamp("delimiter name");

        public Transform intro;
        //public Transform Schedule;
        public GameObject board;
        public Transform Finish;
        [FormerlySerializedAs("WellDoneAndBye")] public Transform wellDoneAndBye;
        [FormerlySerializedAs("FinishCntDwn")] public TextMeshProUGUI finishCntDwn;

        public Transform BGM_Controller;

        public Text Result;

        public Transform Btn_Finish;

        public TextMeshProUGUI TextTitle;

        public Text ToShow;

        public Transform Slot;
        public Transform Grp;

        public Text TimerText;

        List<Transform> SlotList;
        [SerializeField] List<Transform> GrpList;

        public float[] scene2arr;

        public bool isReset = false;

        bool leGogo = false;
        bool beforeStart = false;
        bool firstSelect = false;
        bool beforeStartGuideCheck = false;

        bool checkTimeLimit = false;
        bool checkTimeOut = false;

        float guide_Length = 25;

        int timerMin = 1;
        int timerSec = 30;

        float timeLimit = 900;              //시간 제한 사용 방향 기획 필요
        float timeLimitForFinish = 1200;      //강제종료시간
        float totalElapsedTime = 0;         //수행한 시간 계산용
        float totalElapsedTimeForCalc = 0;  //수행한 시간 보여주기용
        float timerForBeforeStarted = 0;    //시작하기 누르기까지 시간
        float timerForFirstSelect = 0;      //시작하기 누르고 첫 선택까지 시간

        //float TotalScenePlayingTime = 0;    //컨텐츠 시작부터 끝까지 총 시간 TimerForBeforeStarted + TotalElapsedTimeForCalc

        public int totalMovingCnt = 0;      //이동 횟수
        public int resetCnt = 0;            //초기화 누른 횟수
        int selectNoCtn = 0;                 //아니오 누른 횟수

        int skipYn = 0;

        //------------- SOUND    
        public AudioClip sound_Count;
        public AudioClip sound_In;
        public AudioClip sound_Click;
        public AudioClip sound_Put;
        AudioSource audioSource;

        //------------- Manager
        public Transform SetPlayerData;
        public Transform GameDataManager;

        void Start()
        {
            audioSource = this.GetComponent<AudioSource>();

            beforeStart = true;

            SlotList = new List<Transform>();
            GrpList = new List<Transform>();

            InitSlotList();
            InitGrpList();

            collectData.AddTimeStamp("GUIDE START");
        }
       
        void Update()
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

                        string TextSec = "";

                        if (timerSec < 10)
                        {
                            TextSec = "0" + timerSec.ToString();
                        }
                        else
                        {
                            TextSec = timerSec.ToString();
                        }


                        TimerText.text = "0" + timerMin.ToString() + ":" + TextSec;
                    }

                    if (!checkTimeLimit && totalElapsedTimeForCalc >= timeLimit)
                    {
                        //시간제한
                        checkTimeLimit = true;
                        StartCoroutine(TimeLimitAndKeepGoing());
                    }

                    if (!checkTimeOut && totalElapsedTimeForCalc >= timeLimitForFinish)
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
            BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("LIMIT");

            timerSec = 30;

            yield return new WaitForSeconds(6);

            BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");
        }

        IEnumerator TimeOutAndFinish()
        {
            collectData.AddTimeStamp("TIME OUT");
            BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("OUT");

            yield return new WaitForSeconds(6);

            FinishPanel_Yes(true);
        }      

        IEnumerator ResetDelay()
        {
            isReset = true;
            yield return new WaitForSeconds(0.05f);
            isReset = false;           
        }

        List<Transform> InitSlotList()
        {
            
            foreach (Transform s in Slot)
            {
                SlotList.Add(s);
            }
            return SlotList;
        }

        List<Transform> InitGrpList()
        {                       
            foreach (Transform g in Grp)
            {
                GrpList.Add(g);
            }
            Debug.Log("grpList 초기화");
            return GrpList;
        }

        List<Transform> ResetGrpList()
        {            
            string keyword = "(Clone)";
            
            foreach (Transform g in Grp)
            {
                if(g != null)
                {
                    if (RemoveWord.EndsWithWord(g.name, keyword))
                    {
                        GrpList.Remove(g);
                    }
                }                                              
            }            
            return GrpList;
        }

        public void LockAllCollision(Transform obj)
        {
            foreach (Transform go in GrpList)
            {
                if (go != obj)
                {
                    go.GetComponent<PlanCubeController1>().enabled = false;
                }
            }
        }

        public void ReleaseAllCollision()
        {
            foreach (Transform go in GrpList)
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

            foreach (Transform s in SlotList)
            {
                if (s.GetComponent<PlanSlotController1>().passenger != null)
                {
                    check = true;
                }
            }

            return check;
        }

        public void reSetAll()
        {
            PlaySoundByTypes(ESoundType.Click);

            if (CheckEmptySlot())
            {
                StartCoroutine(ResetDelay());

                foreach (var t in SlotList)
                {
                    t.GetComponent<PlanSlotController1>().resetPlanSlot();
                }
                
                foreach(var t in SlotList)
                {
                    StartCoroutine(t.GetComponent<PlanCubeController1>().resetPlanCube());
                }
              
                ResetGrpList();

                Btn_Finish.gameObject.SetActive(false);
                resetCnt += 1;
            }
        }

        public void CheckAllScheduleOnSlot()
        {
            bool AllDone = true;

            foreach (Transform aSlot in SlotList)
            {
                if (aSlot.GetComponent<PlanSlotController1>().passenger == null)
                {
                    AllDone = false;
                }
            }

            if (AllDone)
            {
                Btn_Finish.gameObject.SetActive(true);
            }
        }


        public void DoStartSchedule()
        {
            PlaySoundByTypes(ESoundType.Click);
            StartCoroutine(StartCntDown());
        }

        IEnumerator StartCntDown()
        {
            BGM_Controller.GetComponent<BGMcontroller>().PlayBGMByTypes("BGM");

            TextTitle.text = "준비 ~";

            yield return new WaitForSeconds(1f);
            PlaySoundByTypes(ESoundType.Cnt);
            TextTitle.text = "3";
            yield return new WaitForSeconds(1);
            PlaySoundByTypes(ESoundType.Cnt);
            TextTitle.text = "2";
            yield return new WaitForSeconds(1);
            PlaySoundByTypes(ESoundType.Cnt);
            TextTitle.text = "1";
            yield return new WaitForSeconds(1);
            TextTitle.text = "시작 !";
            yield return new WaitForSeconds(1f);

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
            Finish.gameObject.SetActive(true);
        }

        public void FinishPanel_No()
        {
            PlaySoundByTypes(ESoundType.Click);
            Finish.gameObject.SetActive(false);

            selectNoCtn += 1;
        }

        public void FinishPanel_Yes(bool Skipped)
        {
            PlaySoundByTypes(ESoundType.Click);
            collectData.AddTimeStamp("MISSION END");

            leGogo = false;
            float PlanData = 0;

            board.gameObject.SetActive(false);
            Finish.gameObject.SetActive(false);
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
                string MySchedule = "";
                string MyScheduleForJson = "";

                foreach (Transform plan_Slot in SlotList)
                {
                    Transform plan_Box = plan_Slot.GetComponent<PlanSlotController>().passenger.transform;

                    if (plan_Box != null)
                    {
                        MySchedule += plan_Box.GetChild(0).GetComponent<Text>().text + " ";
                        MyScheduleForJson += plan_Box.GetChild(1).name;
                    }
                    else
                    {
                        MySchedule += "0 ";
                        MyScheduleForJson += "0";
                    }
                }

                PlanData = float.Parse(MyScheduleForJson, System.Globalization.CultureInfo.InvariantCulture);
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
            scene2arr = new float[] { totalElapsedTimeForCalc, totalMovingCnt, resetCnt, selectNoCtn, PlanData, skipYn, timerForBeforeStarted, timerForFirstSelect };
            GameDataManager.GetComponent<GameDataManager>().SaveCurrentData();
            StartCoroutine(GoToNextScene());
        }

        IEnumerator GoToNextScene()
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
            audioSource.clip = null;

            switch (soundType)
            {
                case ESoundType.Click:
                    audioSource.clip = sound_Click;
                    break;
                case ESoundType.In:
                    audioSource.clip = sound_In;
                    break;
                case ESoundType.Cnt:
                    audioSource.clip = sound_Count;
                    break;
                case ESoundType.Put:
                    audioSource.clip = sound_Put;
                    break;
            }
            audioSource.Play();            
        }
    }

}

