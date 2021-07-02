using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PaddleManager : MonoBehaviour
{
    public GameObject Canvas_Intro;
    public TextMeshProUGUI ShowIntroText;
    public GameObject Canvas_Display;
    public GameObject Canvas_Finish;
    public TextMeshProUGUI ShowCntDwnText;

    public TextMeshProUGUI ShowDisplayText;
    public Transform ShowPopUpText;

    public Transform Boat;

    public GameObject FinishLand;

    public Transform Wheel_Parent_My;   //어떤 핸들을 잡았는지 식별용 - 내거
    public Transform Wheel_Parent_Bot;  //어떤 핸들을 잡았는지 식별용 - 친구거 실패

    public Transform AutoController;
    public Transform AutoGrabScript;      //시작 전에 잡는건 막아야할듯

    int StageLvl = 1;                   //스테이지 단계 1 ~ 3 속도가 점점 올라감
    
    int FailureCnt_Nth = 0;              //아무것도 하지 않아서 실패 (실패 이후 방법 필요)
    float Failure_DistbT = 0;         //친구 손잡이를 잡고 있는 시간

    bool SceneStart = false;             //Scene 시작
    bool PaddleStart = false;           //시작
    float TimeElapsed = 0;
    int TimeElapsedShow = 0;

    bool MyPaddleOn = false;            //핸들을 잡았는지
    bool PaddleTimerSwitch = false;     //바르게 돌리고 있을때 시간 체크용
    float PaddleSpeedTimer = 0;         //핸들 속도 체크용
    int PaddleFailCnt = 0;              //친구와 속도 맞지 않은 횟수

    float FailureTimer = 0;             //안돌리고 있거나 속도에 맞지 않게 돌리는지 체크용 - 정해진 시간(60초) 지나면 1회 실패로 간주
    int FailureTimerShow = 0;

    public bool DoNotConcentrate = false;
    float DNC_Timer = 0;
    int DNC_TimerShow = 0;

    float ppFocusT = 0;
    float noName = 0;

    //------------- SOUND
    AudioSource audioSource;
    public AudioClip Sound_Count;
    public AudioClip Sound_Click;
    public AudioClip Sound_Success;
    public AudioClip Sound_Fail;
    public AudioClip Sound_StageUp;

    //------------- Manager
    public Transform setData_PlayerData;
    public Transform saveData_GameDataMG;


    private void Start()
    {
        SceneStart = true;
        audioSource = transform.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SceneStart)
        {
            TimeElapsed += Time.deltaTime;

            if (TimeElapsed > 1)
            {
                TimeElapsed = 0;
                TimeElapsedShow += 1;

                ShowDataInfo();

                if (TimeElapsedShow > 300)  
                {
                    //5분 초과시 강제 종료 ? 정책필요
                    //GameFinish();
                }
            }

            if (DoNotConcentrate)
            {
                DNC_Timer += Time.deltaTime;

                if (DNC_Timer > 1)
                {
                    DNC_Timer = 0;
                    DNC_TimerShow += 1;

                    ShowDataInfo();
                }
            }

            if (PaddleStart)
            {
                if (Wheel_Parent_My.childCount != 0)
                {
                    MyPaddleOn = true;
                    FailureTimerShow = 0;
                }
                else
                {
                    MyPaddleOn = false;
                    PaddleTimerSwitch = false;
                    PaddleSpeedTimer = 0;
                }

                if (Wheel_Parent_Bot.childCount != 0)
                {
                    //친구 핸들 잡으면 바로 실패
                    //StartCoroutine(ResetAllGame());

                    //친구 핸들 잡고 있는 시간
                    Failure_DistbT += Time.deltaTime;
                }

                if (MyPaddleOn && PaddleTimerSwitch)
                {
                    PaddleSpeedTimer += Time.deltaTime;
                }
                if (!MyPaddleOn) //아무것도 안하면 시간체크 30초뒤 1실패 (누적)
                {
                    FailureTimer += Time.deltaTime;

                    if (FailureTimer > 1)
                    {
                        FailureTimer = 0;
                        FailureTimerShow += 1;

                        if (FailureTimerShow > 30)
                        {
                            FailureCnt_Nth += 1;
                            FailureTimerShow = 0;
                            //StartCoroutine(ResetAllGame());
                        }
                    }
                }

                if (Boat.GetComponent<BoatController>().Distance > 100)
                {
                    GameFinish();
                }
            }
        }
    }

    void ShowDataInfo()
    {
        ShowDisplayText.text = "TimeElap : " + TimeElapsedShow.ToString() + " _ NotConcent : " + DNC_TimerShow.ToString() + " _ STAGE : " + StageLvl.ToString() + " _ DIST : " + Boat.GetComponent<BoatController>().Distance.ToString() + "/100"
            + "\n NotSame : " + PaddleFailCnt.ToString() + " _ DoNth : " + FailureTimerShow.ToString() + " _ F_Nth : " + FailureCnt_Nth.ToString() + " _ F_DistbT : " + Failure_DistbT.ToString();
    }

    public void DoStartPaddle()
    {
        PlaySoundByType("CLICK");
        StartCoroutine(StartPaddle());
    }

    IEnumerator StartPaddle()
    {
        ShowIntroText.text = "준비 ~";
        yield return new WaitForSeconds(1);

        PlaySoundByType("CNT");
        ShowIntroText.text = "3";
        yield return new WaitForSeconds(1);

        PlaySoundByType("CNT");
        ShowIntroText.text = "2";
        yield return new WaitForSeconds(1);

        PlaySoundByType("CNT");
        ShowIntroText.text = "1";
        yield return new WaitForSeconds(1);

        ShowIntroText.text = "시작 !";
        yield return new WaitForSeconds(1);

        Canvas_Intro.SetActive(false);
        Canvas_Display.SetActive(true);

        PaddleStart = true;
        AutoGrabScript.GetComponent<BNG.Grabbable>().enabled = true;

        AutoController.GetComponent<AutoController>().SetPaddleAnimSpeed(StageLvl);
    }

    void ShowPopUpInfo(string info)
    {
        ShowPopUpText.GetComponent<PopUpController>().DoAvtivatePopUp(info);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (PaddleStart && col.collider.tag == "HANDLE_MY")
        {
            PaddleTimerSwitch = false;
            MyPaddleSpeedForCal();
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (PaddleStart && col.collider.tag == "HANDLE_MY")
        {
            PaddleTimerSwitch = true;
        }
    }

    void MyPaddleSpeedForCal()
    {
        if (StageLvl == 1)
        {
            if (PaddleSpeedTimer > 3f && PaddleSpeedTimer < 4.5f)
            {
                SuccessToGo();

                if (Boat.GetComponent<BoatController>().Distance > 40)
                {
                    StageLvl += 1;
                    ShowPopUpInfo("2 단계로 난이도가 변경됩니다.\n조금 더 빠르게 돌리세요 !");

                    PlaySoundByType("STAGE");
                    AutoController.GetComponent<AutoController>().SetPaddleAnimSpeed(StageLvl);
                }
            }
            else
            {
                FailToPause();
            }
        }
        else if (StageLvl == 2)
        {
            if (PaddleSpeedTimer > 2f && PaddleSpeedTimer < 3.5f)
            {
                SuccessToGo();

                if (Boat.GetComponent<BoatController>().Distance > 70)
                {
                    StageLvl += 1;
                    ShowPopUpInfo("3 단계로 난이도가 변경됩니다.\n빠르게 돌리세요 !");

                    PlaySoundByType("STAGE");
                    AutoController.GetComponent<AutoController>().SetPaddleAnimSpeed(StageLvl);
                }
            }
            else
            {
                FailToPause();
            }
        }
        else if (StageLvl == 3)
        {
            if (Boat.GetComponent<BoatController>().Distance < 100)
            {
                if (PaddleSpeedTimer > 1f && PaddleSpeedTimer < 2.5f)
                {
                    SuccessToGo();
                }
                else
                {
                    FailToPause();
                }
            }
        }

        PaddleSpeedTimer = 0;
    }

    void SuccessToGo()
    {
        if (PaddleStart)
        {
            PlaySoundByType("SUCCESS");

            ShowPopUpInfo("성공 ~ !!");
            Boat.GetComponent<BoatController>().PlusBoatDistance();
            FailureTimerShow = 0;
        }
    }

    void FailToPause()
    {
        if (PaddleStart)
        {
            PlaySoundByType("FAIL");

            PaddleFailCnt += 1;
            ShowPopUpInfo("친구와 속도를 맞춰서 돌리세요");
        }
    }

    void GameFinish()
    {
        SceneStart = false;
        PaddleStart = false;
        transform.GetComponent<BoxCollider>().enabled = false;

        AutoController.GetComponent<AutoController>().GameFinish();
        Boat.GetComponent<BoatController>().GameFinish();

        FinishLand.SetActive(true);
        ShowPopUpInfo("보물섬에 도착했어요!\n잘했어요! 짝짝짝 ~");

        //JsonManager.GetInstance().LoadPlayerDataFromJson();
        setData_PlayerData.GetComponent<SetPlayerData>().GetSceneIndex4(TimeElapsedShow, FailureCnt_Nth, FailureTimerShow, Failure_DistbT, StageLvl, PaddleFailCnt, ppFocusT, DNC_TimerShow, noName);
        saveData_GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();

        StartCoroutine(GoToLobby());
    }

    IEnumerator GoToLobby()
    {
        yield return new WaitForSeconds(3);
        Canvas_Finish.SetActive(true);

        yield return new WaitForSeconds(1);
        ShowCntDwnText.text = "3";

        yield return new WaitForSeconds(1);
        ShowCntDwnText.text = "2";

        yield return new WaitForSeconds(1);
        ShowCntDwnText.text = "1";

        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("Lobby");
    }


/*    IEnumerator ResetAllGame()
    {
        //필요하면 실패데이터 따로 저장

        PaddleStart = false;
        ShowPopUpInfo("처음부터 다시 시작합니다.");

        FailureCnt_Nth += 1;

        StageLvl = 1;

        PaddleSpeedTimer = 0;
        FailureTimer = 0;
        FailureTimerShow = 0;

        Boat.GetComponent<BoatController>().ResetBoat();
        AutoController.GetComponent<AutoController>().SetPaddleAnimSpeed(0);

        yield return new WaitForSeconds(2);

        Canvas_Intro.SetActive(true);
        Canvas_Display.SetActive(false);

        DoStartPaddle();
    }*/

    void PlaySoundByType(string sType)
    {
        audioSource.clip = null;

        if (sType == "SUCCESS")
        {
            audioSource.clip = Sound_Success;
        }
        else if (sType == "CLICK")
        {
            audioSource.clip = Sound_Click;
        }
        else if (sType == "CNT")
        {
            audioSource.clip = Sound_Count;
        }
        else if (sType == "FAIL")
        {
            audioSource.clip = Sound_Fail;
        }
        else if (sType == "STAGE")
        {
            audioSource.clip = Sound_StageUp;
        }

        audioSource.Play();
    }




}
