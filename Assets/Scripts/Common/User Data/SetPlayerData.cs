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
        DataManager.GetInstance().dataList.Add(new PlayerData(1, "data_01", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(2, "data_02", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(3, "data_03", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(4, "data_04", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(5, "data_05", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(6, "data_06", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(7, "data_07", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(8, "data_08", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(9, "data_09", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(10, "data_10", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(11, "data_11", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(12, "data_12", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(13, "data_13", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(14, "data_14", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(15, "data_15", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(16, "data_16", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(17, "data_17", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(18, "data_18", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(19, "data_19", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(20, "data_20", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(21, "data_21", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(22, "data_22", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(23, "data_23", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(24, "data_24", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(25, "data_25", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(26, "data_26", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(27, "data_27", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(28, "data_28", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(29, "data_29", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(30, "data_30", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(31, "data_31", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(32, "data_32", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(33, "data_33", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(34, "data_34", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(35, "data_35", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(36, "data_36", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(37, "data_37", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(38, "data_38", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(39, "data_39", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(40, "data_40", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(41, "data_41", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(42, "data_42", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(43, "data_43", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(44, "data_44", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(45, "data_45", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(46, "data_46", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(47, "data_47", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(48, "data_48", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(49, "data_49", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(50, "data_50", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(51, "data_51", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(52, "data_52", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(53, "data_53", 0));
    }

    public void GetSceneIndex1()
    {
        FsmFloat data01 = fsm.FsmVariables.GetFsmFloat("psFocusT");
        FsmFloat data02 = fsm.FsmVariables.GetFsmFloat("psUnfocusT");
        FsmFloat data03 = fsm.FsmVariables.GetFsmFloat("psWrNumC");
        FsmFloat data04 = fsm.FsmVariables.GetFsmFloat("psFailC");
        FsmFloat data05 = fsm.FsmVariables.GetFsmFloat("psRpL");
        FsmFloat data06 = fsm.FsmVariables.GetFsmFloat("psCmplT");
        FsmFloat data07 = fsm.FsmVariables.GetFsmFloat("giveUp");

        DataManager.GetInstance().dataList[1].Result = data01.Value;
        DataManager.GetInstance().dataList[2].Result = data02.Value;
        DataManager.GetInstance().dataList[3].Result = data03.Value;
        DataManager.GetInstance().dataList[4].Result = data04.Value;
        DataManager.GetInstance().dataList[5].Result = data05.Value;
        DataManager.GetInstance().dataList[6].Result = data06.Value;
    }

    public void GetSceneIndex2(float ptCmplT, float ptChangeC, float ptUnfocusT, float ptReplayC, float prChangeC, float ptPlanN)
    {//setData_PlayerData.GetComponent<SetPlayerData>().GetSceneIndex2(TotalElapsedTimeForShow, TotalMovingCnt, NotOnBoardForShow, ResetCnt, ClickNoCnt, PlanData);
        float data08 = ptCmplT;
        float data09 = ptChangeC;
        float data10 = ptUnfocusT;
        float data11 = ptReplayC;
        float data12 = prChangeC;
        float data13 = ptPlanN;

        DataManager.GetInstance().dataList[8].Result = data08;
        DataManager.GetInstance().dataList[9].Result = data09;
        DataManager.GetInstance().dataList[10].Result = data10;
        DataManager.GetInstance().dataList[11].Result = data11;
        DataManager.GetInstance().dataList[12].Result = data12;
        DataManager.GetInstance().dataList[13].Result = data13;
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

