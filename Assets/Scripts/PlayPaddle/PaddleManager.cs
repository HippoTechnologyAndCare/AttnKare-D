using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KetosGames.SceneTransition;


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
    
    float Failure_DistbT = 0;         //친구 손잡이를 잡고 있는 시간

    bool SceneStart = false;             //Scene 시작
    bool PaddleStart = false;           //시작
    float TimeElapsed = 0;
    int TimeElapsedShow = 0;

    bool MyPaddleOn = false;            //핸들을 잡았는지
    bool PaddleTimerSwitch = false;     //바르게 돌리고 있을때 시간 체크용
    float PaddleSpeedTimer = 0;         //핸들 속도 체크용
    int PaddleFailCnt = 0;              //친구와 속도 맞지 않은 횟수

    float DoNothingTimeElapsed = 0;             //안돌리는 시간



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



    float Data_21 = 0;      //시작버튼 누르기 까지 걸린 시간
    float Data_22 = 0;      //완료까지 걸린 총 시간
    float Data_23 = 0;      //스테이지1 걸린 시간
    float Data_24 = 0;      //스테이지2 걸린 시간
    float Data_25 = 0;      //스테이지3 걸린 시간
    float Data_26 = 0;      //스테이지1에서 협동을 지키지 않은 횟수
    float Data_27 = 0;      //스테이지2에서 협동을 지키지 않은 횟수
    float Data_28 = 0;      //스테이지3에서 협동을 지키지 않은 횟수
    float Data_29 = 0;      //친구의 페달을 건드린 횟수
    float Data_30 = 0;      //아무 행동도 하지 않은 총 시간
    float Data_31 = 0;      //중도 포기(스킵)



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

            if (TimeElapsedShow > 300)
            {
                //5분 초과시 강제 종료 ?
                GameFinish();
            }

            if (PaddleStart)
            {
                if (Wheel_Parent_My.childCount != 0)
                {
                    MyPaddleOn = true;
                    Debug.Log("잡 " + MyPaddleOn);
                }
                else
                {
                    MyPaddleOn = false;
                    PaddleTimerSwitch = false;
                    PaddleSpeedTimer = 0;
                    Debug.Log("놓 " + MyPaddleOn);
                }

                if (Wheel_Parent_Bot.childCount != 0)
                {
                    //친구 핸들 잡고 있는 시간
                    Failure_DistbT += Time.deltaTime;
                }

                if (MyPaddleOn && PaddleTimerSwitch)
                {
                    //핸들 속도 계산용
                    PaddleSpeedTimer += Time.deltaTime;
                }

                if (!MyPaddleOn)
                {
                    DoNothingTimeElapsed += Time.deltaTime;
                }

                if (Boat.GetComponent<BoatController>().Distance > 100)
                {
                    GameFinish();
                }
            }
        }
    }

    public void DoStartPaddle()
    {
        Data_21 = TimeElapsed;

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

        ShowIntroText.text = "출발 !";
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
        Debug.Log("ENTER " + col.collider.tag);

        if (PaddleStart && col.collider.tag == "HANDLE_MY")
        {
            PaddleTimerSwitch = false;
            MyPaddleSpeedForCal();
        }
    }

    private void OnCollisionExit(Collision col)
    {
        Debug.Log("OUT " + col.collider.tag);

        if (PaddleStart && col.collider.tag == "HANDLE_MY")
        {
            PaddleTimerSwitch = true;
        }
    }

    void MyPaddleSpeedForCal()
    {
        if (StageLvl == 1)
        {
            if (PaddleSpeedTimer > 3.5f && PaddleSpeedTimer < 4.5f)
            {
                SuccessToGo();

                if (Boat.GetComponent<BoatController>().Distance > 40)
                {
                    Data_23 = TimeElapsed - Data_21;
                    Data_26 = PaddleFailCnt;
                    PaddleFailCnt = 0;
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
            if (PaddleSpeedTimer > 2.5f && PaddleSpeedTimer < 3.5f)
            {
                SuccessToGo();

                if (Boat.GetComponent<BoatController>().Distance > 70)
                {
                    Data_24 = TimeElapsed - Data_21 - Data_23;
                    Data_27 = PaddleFailCnt;
                    PaddleFailCnt = 0;
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
                if (PaddleSpeedTimer > 1.5f && PaddleSpeedTimer < 2.5f)
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

    public void GameFinish()
    {
        Data_22 = TimeElapsed;
        Data_25 = TimeElapsed - Data_21 - Data_23 - Data_24;
        Data_28 = PaddleFailCnt;
        Data_29 = Failure_DistbT;
        Data_30 = DoNothingTimeElapsed;

        SceneStart = false;
        PaddleStart = false;
        transform.GetComponent<BoxCollider>().enabled = false;

        AutoController.GetComponent<AutoController>().GameFinish();
        Boat.GetComponent<BoatController>().GameFinish();

        FinishLand.SetActive(true);
        ShowPopUpInfo("보물섬에 도착했어요!\n잘했어요! 짝짝짝 ~");


        Debug.Log("21 : " + Data_21.ToString()
                + "\n22 : " + Data_22.ToString()
                + "\n23 : " + Data_23.ToString()
                + "\n24 : " + Data_24.ToString()
                + "\n25 : " + Data_25.ToString()
                + "\n26 : " + Data_26.ToString()
                + "\n27 : " + Data_27.ToString()
                + "\n28 : " + Data_28.ToString()
                + "\n29 : " + Data_29.ToString()
                + "\n30 : " + Data_30.ToString());

        //setData_PlayerData.GetComponent<SetPlayerData>().GetSceneIndex4(0, 0, 0, 0, 0, 0, 0, 0, 0);
        //saveData_GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();

        //StartCoroutine(GoToLobby());
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

        SceneLoader.LoadScene(0);       /// -------------------------- 다음컨텐츠 번호 넣어야 함
    }

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
