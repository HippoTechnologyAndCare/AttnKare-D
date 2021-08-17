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
        DataManager.GetInstance().dataList.Add(new PlayerData(0, "empty_data", 0));
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
        
    }
    // Doorlock //
    public void GetSceneIndex1()
    {
        FsmFloat data01 = fsm.FsmVariables.GetFsmFloat("psFocusT");
        FsmFloat data02 = fsm.FsmVariables.GetFsmFloat("psUnfocusT");
        FsmFloat data03 = fsm.FsmVariables.GetFsmFloat("psWrNumC");
        FsmFloat data04 = fsm.FsmVariables.GetFsmFloat("psFailC");
        FsmFloat data05 = fsm.FsmVariables.GetFsmFloat("psRpL");
        FsmFloat data06 = fsm.FsmVariables.GetFsmFloat("psCmplT");
        FsmFloat data07 = fsm.FsmVariables.GetFsmFloat("psSkip");

        DataManager.GetInstance().dataList[1].Result = data01.Value;
        DataManager.GetInstance().dataList[2].Result = data02.Value;
        DataManager.GetInstance().dataList[3].Result = data03.Value;
        DataManager.GetInstance().dataList[4].Result = data04.Value;
        DataManager.GetInstance().dataList[5].Result = data05.Value;
        DataManager.GetInstance().dataList[6].Result = data06.Value;
        DataManager.GetInstance().dataList[7].Result = data07.Value;
    }
    // Schecule //
    public void GetSceneIndex2(float ptCmplT, float ptChangeC, float ptReplayC, float prChangeC, float ptPlanN, float ptSkip)
    {//setData_PlayerData.GetComponent<SetPlayerData>().GetSceneIndex2(TotalElapsedTimeForShow, TotalMovingCnt, NotOnBoardForShow, ResetCnt, ClickNoCnt, PlanData);
        float data08 = ptCmplT;
        float data09 = ptChangeC;
        float data10 = ptReplayC;
        float data11 = prChangeC;
        float data12 = ptPlanN;
        float data13 = ptSkip;

        DataManager.GetInstance().dataList[8].Result = data08;
        DataManager.GetInstance().dataList[9].Result = data09;
        DataManager.GetInstance().dataList[10].Result = data10;
        DataManager.GetInstance().dataList[11].Result = data11;
        DataManager.GetInstance().dataList[12].Result = data12;
        DataManager.GetInstance().dataList[13].Result = data13;
    }
    // Back Packing L //
    public void GetSceneIndex3()
    {
        //책가방
        FsmFloat data32 = fsm.FsmVariables.GetFsmFloat("bpCmplT");
        FsmFloat data33 = fsm.FsmVariables.GetFsmFloat("bpNtcC");
        FsmFloat data34 = fsm.FsmVariables.GetFsmFloat("bpTmtT");
        FsmFloat data35 = fsm.FsmVariables.GetFsmFloat("bpUnpkT");
        FsmFloat data36 = fsm.FsmVariables.GetFsmFloat("bpUnpC");
        FsmFloat data37 = fsm.FsmVariables.GetFsmFloat("bpStg1C");
        FsmFloat data38 = fsm.FsmVariables.GetFsmFloat("bpStgChngT");
        FsmFloat data39 = fsm.FsmVariables.GetFsmFloat("bpUnwT");
        FsmFloat data40 = fsm.FsmVariables.GetFsmFloat("bpSkip");

        DataManager.GetInstance().dataList[32].Result = data32.Value;
        DataManager.GetInstance().dataList[33].Result = data33.Value;
        DataManager.GetInstance().dataList[34].Result = data34.Value;
        DataManager.GetInstance().dataList[35].Result = data35.Value;
        DataManager.GetInstance().dataList[36].Result = data36.Value;
        DataManager.GetInstance().dataList[37].Result = data37.Value;
        DataManager.GetInstance().dataList[38].Result = data38.Value;
        DataManager.GetInstance().dataList[39].Result = data39.Value;
        DataManager.GetInstance().dataList[40].Result = data40.Value;
    }
    // Scoop L //
    public void GetSceneIndex4(float sgCmplT_1, float sgCmplT_2, float sgCmplT_3, float sgDrL_1, float sgDrL_2, float sgDrL_3, float sgWrColL, float sgUnptL, float sgFailY, float sgQuitY)
    {
        float data41 = sgCmplT_1;
        float data42 = sgCmplT_2;
        float data43 = sgCmplT_3;
        float data44 = sgDrL_1;
        float data45 = sgDrL_2;
        float data46 = sgDrL_3;
        float data47 = sgWrColL;
        float data48 = sgUnptL;
        float data49 = sgFailY;
        float data50 = sgQuitY;

        DataManager.GetInstance().dataList[41].Result = data41;
        DataManager.GetInstance().dataList[42].Result = data42;
        DataManager.GetInstance().dataList[43].Result = data43;
        DataManager.GetInstance().dataList[44].Result = data44;
        DataManager.GetInstance().dataList[45].Result = data45;
        DataManager.GetInstance().dataList[46].Result = data46;
        DataManager.GetInstance().dataList[47].Result = data47;
        DataManager.GetInstance().dataList[48].Result = data48;
        DataManager.GetInstance().dataList[49].Result = data49;
        DataManager.GetInstance().dataList[50].Result = data50;
    }
    // Clean Up My Room //
    public void GetSceneIndex5()
    {
        //방정리
        FsmFloat data14 = fsm.FsmVariables.GetFsmFloat("crSucessL");
        FsmFloat data15 = fsm.FsmVariables.GetFsmFloat("crUnfocusT");
        FsmFloat data16 = fsm.FsmVariables.GetFsmFloat("crCmplT");
        FsmFloat data17 = fsm.FsmVariables.GetFsmFloat("crGrbC");
        FsmFloat data18 = fsm.FsmVariables.GetFsmFloat("crMovM");
        FsmFloat data19 = fsm.FsmVariables.GetFsmFloat("crFocusT");
        FsmFloat data20 = fsm.FsmVariables.GetFsmFloat("crSkip");

        DataManager.GetInstance().dataList[14].Result = data14.Value;
        DataManager.GetInstance().dataList[15].Result = data15.Value;
        DataManager.GetInstance().dataList[16].Result = data16.Value;
        DataManager.GetInstance().dataList[17].Result = data17.Value;
        DataManager.GetInstance().dataList[18].Result = data18.Value;
        DataManager.GetInstance().dataList[19].Result = data19.Value;
        DataManager.GetInstance().dataList[20].Result = data20.Value;
    }
    // Play Paddle //
    public void GetSceneIndex6(float data_21, float data_22, float data_23, float data_24, float data_25, float data_26, float data_27, float data_28, float data_29, float data_30, float data_31)
    {
        float data21 = data_21;
        float data22 = data_22;
        float data23 = data_23;
        float data24 = data_24;
        float data25 = data_25;
        float data26 = data_26;
        float data27 = data_27;
        float data28 = data_28;
        float data29 = data_29;
        float data30 = data_30;

        DataManager.GetInstance().dataList[21].Result = data21;
        DataManager.GetInstance().dataList[22].Result = data22;
        DataManager.GetInstance().dataList[23].Result = data23;
        DataManager.GetInstance().dataList[24].Result = data24;
        DataManager.GetInstance().dataList[25].Result = data25;
        DataManager.GetInstance().dataList[26].Result = data26;
        DataManager.GetInstance().dataList[27].Result = data27;
        DataManager.GetInstance().dataList[28].Result = data28;
        DataManager.GetInstance().dataList[29].Result = data29;
        DataManager.GetInstance().dataList[30].Result = data30;
    }
    // Back Packing H //
    public void GetSceneIndex7()
    {        
        //책가방
        FsmFloat data32 = fsm.FsmVariables.GetFsmFloat("bpCmplT");
        FsmFloat data33 = fsm.FsmVariables.GetFsmFloat("bpNtcC");
        FsmFloat data34 = fsm.FsmVariables.GetFsmFloat("bpTmtT");
        FsmFloat data35 = fsm.FsmVariables.GetFsmFloat("bpUnpkT");
        FsmFloat data36 = fsm.FsmVariables.GetFsmFloat("bpUnpC");
        FsmFloat data37 = fsm.FsmVariables.GetFsmFloat("bpStg1C");
        FsmFloat data38 = fsm.FsmVariables.GetFsmFloat("bpStgChngT");
        FsmFloat data39 = fsm.FsmVariables.GetFsmFloat("bpUnwT");
        FsmFloat data40 = fsm.FsmVariables.GetFsmFloat("bpSkip");

        DataManager.GetInstance().dataList[32].Result = data32.Value;
        DataManager.GetInstance().dataList[33].Result = data33.Value;
        DataManager.GetInstance().dataList[34].Result = data34.Value;
        DataManager.GetInstance().dataList[35].Result = data35.Value;
        DataManager.GetInstance().dataList[36].Result = data36.Value;
        DataManager.GetInstance().dataList[37].Result = data37.Value;
        DataManager.GetInstance().dataList[38].Result = data38.Value;
        DataManager.GetInstance().dataList[39].Result = data39.Value;
        DataManager.GetInstance().dataList[40].Result = data40.Value;
    }
    // Scoop H //
    public void GetSceneIndex8(float sgCmplT_1, float sgCmplT_2, float sgCmplT_3, float sgDrL_1, float sgDrL_2, float sgDrL_3, float sgWrColL, float sgUnptL, float sgFailY, float sgQuitY)
    {        
        float data41 = sgCmplT_1;
        float data42 = sgCmplT_2;
        float data43 = sgCmplT_3;
        float data44 = sgDrL_1;
        float data45 = sgDrL_2;
        float data46 = sgDrL_3;
        float data47 = sgWrColL;
        float data48 = sgUnptL;
        float data49 = sgFailY;
        float data50 = sgQuitY;

        DataManager.GetInstance().dataList[41].Result = data41;
        DataManager.GetInstance().dataList[42].Result = data42;
        DataManager.GetInstance().dataList[43].Result = data43;
        DataManager.GetInstance().dataList[44].Result = data44;
        DataManager.GetInstance().dataList[45].Result = data45;
        DataManager.GetInstance().dataList[46].Result = data46;
        DataManager.GetInstance().dataList[47].Result = data47;
        DataManager.GetInstance().dataList[48].Result = data48;
        DataManager.GetInstance().dataList[49].Result = data49;
        DataManager.GetInstance().dataList[50].Result = data50;
    }
}

