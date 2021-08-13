using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using UserData;

public class SetPlayerData : MonoBehaviour
{
    public GameDataManager gameDataManager;
    public PlayMakerFSM fsm;

    private void Awake()
    {
        if (!DataManager.GetInstance().isPlayed)
        {
            DataManager.GetInstance().isPlayed = true;

            InitialDataSetting();
            DataManager.GetInstance().SavePlayerDataToJson();
            Debug.Log("Data Creat!");
        }
    }

    public void InitialDataSetting()
    {
        DataManager.GetInstance().dataList.Add(new PlayerData(1, "cmMoveN", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(2, "cmVoiceN", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(3, "cmGenderN", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(4, "psFocusT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(5, "psUnfocusT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(6, "psWrNumC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(7, "psFailC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(8, "psRpL", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(9, "psCmplT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(10, "ptCmplT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(11, "ptChangeC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(12, "ptUnfocusT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(13, "ptReplayC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(14, "prChangeC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(15, "ptPlanN", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(16, "crSucessL", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(17, "crUnfocusT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(18, "crCmplT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(19, "crGrbC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(20, "crMovM", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(21, "crFocusT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(22, "ppCmplT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(23, "ppFailC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(24, "ppNthC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(25, "ppDsrptC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(26, "ppCmplS", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(27, "ppCoopC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(28, "ppFocusT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(29, "ppUnfocusT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(30, "NoName", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(31, "bpCmplT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(32, "bpNtcC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(33, "bpTmtT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(34, "bpUnpkT", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(35, "bpUnpC", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(36, "bpPcmpT", 0));
    }

    public void GetSceneIndex1()
    {
        FsmFloat data04 = fsm.FsmVariables.GetFsmFloat("psFocusT");
        FsmFloat data05 = fsm.FsmVariables.GetFsmFloat("psUnfocusT");
        FsmFloat data06 = fsm.FsmVariables.GetFsmFloat("psWrNumC");
        FsmFloat data07 = fsm.FsmVariables.GetFsmFloat("psFailC");
        FsmFloat data08 = fsm.FsmVariables.GetFsmFloat("psRpL");
        FsmFloat data09 = fsm.FsmVariables.GetFsmFloat("psCmplT");

        DataManager.GetInstance().dataList[3].Result = data04.Value;
        DataManager.GetInstance().dataList[4].Result = data05.Value;
        DataManager.GetInstance().dataList[5].Result = data06.Value;
        DataManager.GetInstance().dataList[6].Result = data07.Value;
        DataManager.GetInstance().dataList[7].Result = data08.Value;
        DataManager.GetInstance().dataList[8].Result = data09.Value;
    }

    public void GetSceneIndex2(float ptCmplT, float ptChangeC, float ptUnfocusT, float ptReplayC, float prChangeC, float ptPlanN)
    {//setData_PlayerData.GetComponent<SetPlayerData>().GetSceneIndex2(TotalElapsedTimeForShow, TotalMovingCnt, NotOnBoardForShow, ResetCnt, ClickNoCnt, PlanData);
        float data10 = ptCmplT;
        float data11 = ptChangeC;
        float data12 = ptUnfocusT;
        float data13 = ptReplayC;
        float data14 = prChangeC;
        float data15 = ptPlanN;

        DataManager.GetInstance().dataList[9].Result = data10;
        DataManager.GetInstance().dataList[10].Result = data11;
        DataManager.GetInstance().dataList[11].Result = data12;
        DataManager.GetInstance().dataList[12].Result = data13;
        DataManager.GetInstance().dataList[13].Result = data14;
        DataManager.GetInstance().dataList[14].Result = data15;
    }

    public void GetSceneIndex3()
    {
        FsmFloat data16 = fsm.FsmVariables.GetFsmFloat("crSucessL");
        FsmFloat data17 = fsm.FsmVariables.GetFsmFloat("crUnfocusT");
        FsmFloat data18 = fsm.FsmVariables.GetFsmFloat("crCmplT");
        FsmFloat data19 = fsm.FsmVariables.GetFsmFloat("crGrbC");
        FsmFloat data20 = fsm.FsmVariables.GetFsmFloat("crMovM");
        FsmFloat data21 = fsm.FsmVariables.GetFsmFloat("crFocusT");

        DataManager.GetInstance().dataList[15].Result = data16.Value;
        DataManager.GetInstance().dataList[16].Result = data17.Value;
        DataManager.GetInstance().dataList[17].Result = data18.Value;
        DataManager.GetInstance().dataList[18].Result = data19.Value;
        DataManager.GetInstance().dataList[19].Result = data20.Value;
        DataManager.GetInstance().dataList[20].Result = data21.Value;
    }

    public void GetSceneIndex4(float ppCmplT, float ppFailC, float ppNthC, float ppDsrptC, float ppCmplS, float ppCoopC, float ppFocusT, float ppUnfocusT, float noName)
    {//setData_PlayerData.GetComponent<SetPlayerData>().GetSceneIndex4(TimeElapsedShow, FailureCnt_Nth, FailureTimerShow, Failure_DistbT, StageLvl, PaddleFailCnt, 0, DNC_TimerShow, 0);.
        float data22 = ppCmplT;
        float data23 = ppFailC;
        float data24 = ppNthC;
        float data25 = ppDsrptC;
        float data26 = ppCmplS;
        float data27 = ppCoopC;
        float data28 = ppFocusT;
        float data29 = ppUnfocusT;
        float data30 = noName;

        DataManager.GetInstance().dataList[21].Result = data22;
        DataManager.GetInstance().dataList[22].Result = data23;
        DataManager.GetInstance().dataList[23].Result = data24;
        DataManager.GetInstance().dataList[24].Result = data25;
        DataManager.GetInstance().dataList[25].Result = data26;
        DataManager.GetInstance().dataList[26].Result = data27;
        DataManager.GetInstance().dataList[27].Result = data28;
        DataManager.GetInstance().dataList[28].Result = data29;
        DataManager.GetInstance().dataList[29].Result = data30;
    }
    public void GetSceneIndex5()
    {
        FsmFloat data31 = fsm.FsmVariables.GetFsmFloat("bpCmplT");
        FsmFloat data32 = fsm.FsmVariables.GetFsmFloat("bpNtcC");
        FsmFloat data33 = fsm.FsmVariables.GetFsmFloat("bpTmtT");
        FsmFloat data34 = fsm.FsmVariables.GetFsmFloat("bpUnpkT");
        FsmFloat data35 = fsm.FsmVariables.GetFsmFloat("bpUnpC");
        FsmFloat data36 = fsm.FsmVariables.GetFsmFloat("bpPcmpT");

        DataManager.GetInstance().dataList[30].Result = data31.Value;
        DataManager.GetInstance().dataList[31].Result = data32.Value;
        DataManager.GetInstance().dataList[32].Result = data33.Value;
        DataManager.GetInstance().dataList[33].Result = data34.Value;
        DataManager.GetInstance().dataList[34].Result = data35.Value;
        DataManager.GetInstance().dataList[35].Result = data36.Value;
    }
}

