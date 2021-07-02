using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScheduleManager : MonoBehaviour
{
    public Transform Intro;
    public Transform Schedule;
    public Transform Finish;
    public Transform WellDoneAndBye;
    public TextMeshProUGUI FinishCntDwn;

    public Text Result;

    public Transform Btn_Finish;

    public TextMeshProUGUI TextTitle;

    public Text ToShow;

    public Transform Slot;
    public Transform Grp;

    List<Transform> SlotList;
    List<Transform> GrpList;

    bool LeGogo = false;

    float TimeLimit = 300;              //시간 제한 사용 방향 기획 필요
    float TotalElapsedTime = 0;         //수행한 시간 계산용
    float TotalElapsedTimeForShow = 0;  //수행한 시간 보여주기용

    public bool OnBoard = false;        //보드안에서 집중하고 있는지
    float NotOnBoard = 0;               //집중 못한 시간 계산용
    float NotOnBoardForShow = 0;        //집중 못한 시간 보여주기용

    public int TotalMovingCnt = 0;      //이동 횟수
    public int ResetCnt = 0;            //초기화 누른 횟수
    int ClickNoCnt = 0;                 //아니오 누른 횟수




    //------------- SOUND
    AudioSource audioSource;
    public AudioClip Sound_Count;
    public AudioClip Sound_In;
    public AudioClip Sound_Click;
    public AudioClip Sound_Put;



    //------------- Manager
    public Transform setData_PlayerData;
    public Transform saveData_GameDataMG;


    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        LeGogo = true;
        SlotList = new List<Transform>();
        GrpList = new List<Transform>();

        foreach (Transform s in Slot)
        {
            SlotList.Add(s);
        }

        foreach (Transform g in Grp)
        {
            GrpList.Add(g);
        }
    }

    void Update()
    {
        if (LeGogo)
        {
            TotalElapsedTime += Time.deltaTime;

            if (TotalElapsedTime > 1)
            {
                TotalElapsedTime = 0;
                TotalElapsedTimeForShow += 1;

                ShowUpdate();

                if (TotalElapsedTimeForShow >= TimeLimit)
                {
                    //제한 시간 후 해야할 행동
                    Btn_Finish.gameObject.SetActive(true);
                }
            }

            if (OnBoard)
            {
                NotOnBoard += Time.deltaTime;

                if (NotOnBoard > 1)
                {
                    NotOnBoard = 0;
                    NotOnBoardForShow += 1;

                    ShowUpdate();
                }
            }
        }
    }

    public void LockAllCollision(Transform obj)
    {
        foreach (Transform go in GrpList)
        {
            if (go != obj)
            {
                go.GetComponent<PlanCubeController>().enabled = false;
            }
        }
    }

    public void ReleaseAllCollision()
    {
        foreach (Transform go in GrpList)
        {
            go.GetComponent<PlanCubeController>().enabled = true;
        }
    }

    public void CheckMovingCnt()
    {
        TotalMovingCnt += 1;
        ShowUpdate();
    }

    bool CheckEmptySlot()
    {
        bool check = false;

        foreach (Transform s in SlotList)
        {
            if (s.GetComponent<PlanSlotController>().passenger != null)
            {
                check = true;
            }
        }

        return check;
    }

    public void reSetAll()
    {
        PlaySoundByTypes("CLICK");

        if (CheckEmptySlot())
        {
            for (int s = 0; s < SlotList.Count; s++)
            {
                SlotList[s].GetComponent<PlanSlotController>().resetPlanSlot();
            }

            for (int g = 0; g < GrpList.Count; g++)
            {
                GrpList[g].GetComponent<PlanCubeController>().resetPlanCube();
            }

            Btn_Finish.gameObject.SetActive(false);
            ResetCnt += 1;
            ShowUpdate();
        }
    }

    void ShowUpdate()
    {
        ToShow.text = "시간:" + TotalElapsedTimeForShow.ToString() + " / 이동:" + TotalMovingCnt.ToString() + " / 다시하기:" + ResetCnt.ToString() + " / 집중못한 시간:" + NotOnBoardForShow.ToString() + " / 아니오:" + ClickNoCnt.ToString();
    }

    public void CheckAllScheduleOnSlot()
    {
        bool AllDone = true;

        foreach (Transform aSlot in SlotList)
        {
            if (aSlot.GetComponent<PlanSlotController>().passenger == null)
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
        PlaySoundByTypes("CLICK");
        StartCoroutine(StartCntDown());
    }

    IEnumerator StartCntDown()
    {
        TextTitle.text = "준비 ~";

        yield return new WaitForSeconds(1f);
        PlaySoundByTypes("CNT");
        TextTitle.text = "3";
        yield return new WaitForSeconds(1);
        PlaySoundByTypes("CNT");
        TextTitle.text = "2";
        yield return new WaitForSeconds(1);
        PlaySoundByTypes("CNT");
        TextTitle.text = "1";
        yield return new WaitForSeconds(1);
        TextTitle.text = "시작 !";
        yield return new WaitForSeconds(1f);

        Intro.gameObject.SetActive(false);
        Schedule.gameObject.SetActive(true);
    }

    public void ShowFinishPanel()
    {
        PlaySoundByTypes("CLICK");
        Finish.gameObject.SetActive(true);
    }

    public void FinishPanel_No()
    {
        PlaySoundByTypes("CLICK");
        Finish.gameObject.SetActive(false);

        ClickNoCnt += 1;
        ShowUpdate();
    }

    public void FinishPanel_Yes()
    {
        PlaySoundByTypes("CLICK");

        LeGogo = false;

        Schedule.gameObject.SetActive(false);
        Finish.gameObject.SetActive(false);
        WellDoneAndBye.gameObject.SetActive(true);

        //전송용 데이터 정리

        string MySchedule = "";
        string MyScheduleforJson = "";

        foreach (Transform plan_Slot in SlotList)
        {
            Transform plan_Box = plan_Slot.GetComponent<PlanSlotController>().passenger.transform;

            if (plan_Box != null)
            {
                MySchedule += plan_Box.GetChild(0).GetComponent<Text>().text + " ";
                MyScheduleforJson += plan_Box.GetChild(1).name;
            }
        }

        /*
                Result.text = "총시간 " + TotalElapsedTimeForShow.ToString() + " / 총이동 " + TotalMovingCnt.ToString() 
                    + " / 총초기화 " + ResetCnt.ToString() + " / 시선벗어난시간 " + NotOnBoardForShow.ToString() 
                    + " / 아니오횟수 " + ClickNoCnt.ToString() + "\n최종계획 - " + MySchedule;
        */

        float PlanData = float.Parse(MyScheduleforJson, System.Globalization.CultureInfo.InvariantCulture);

        //JsonManager.GetInstance().LoadPlayerDataFromJson();
        setData_PlayerData.GetComponent<SetPlayerData>().GetSceneIndex2(TotalElapsedTimeForShow, TotalMovingCnt, NotOnBoardForShow, ResetCnt, ClickNoCnt, PlanData);
        saveData_GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();


        string SendStr = "총시간 " + TotalElapsedTimeForShow.ToString() + " / 총이동 " + TotalMovingCnt.ToString()
            + " / 다시하기 " + ResetCnt.ToString() + " / 시선벗어난시간 " + NotOnBoardForShow.ToString()
            + " / 아니오횟수 " + ClickNoCnt.ToString() + "\n최종계획 ::: " + MySchedule;

       //this.transform.GetComponent<SaveTempData>().SaveTempSceneData(SendStr);        //텍스트파일 저장

        Result.text = SendStr;

        StartCoroutine(GoToLobby());
    }

    IEnumerator GoToLobby()
    {
        yield return new WaitForSeconds(2);

        FinishCntDwn.text = "3";
        yield return new WaitForSeconds(1);

        FinishCntDwn.text = "2";
        yield return new WaitForSeconds(1);

        FinishCntDwn.text = "1";
        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync("Lobby");

    }



    public void PlaySoundByTypes(string Type)
    {
        audioSource.clip = null;

        if (Type == "IN")
        {
            audioSource.clip = Sound_In;
        }
        else if (Type == "CLICK")
        {
            audioSource.clip = Sound_Click; 
        }
        else if (Type == "CNT")
        {
            audioSource.clip = Sound_Count;
        }
        else if (Type == "PUT")
        {
            audioSource.clip = Sound_Put;
        }

        audioSource.Play();
    }
}
