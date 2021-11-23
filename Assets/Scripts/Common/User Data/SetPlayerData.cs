﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using UserData;

public class SetPlayerData : MonoBehaviour
{
    public GameDataManager gameDataManager;
    public PlayMakerFSM fsm;

    //private void Awake()
    //{
    //    if (!DataManager.GetInstance().isPlayed)
    //    {
    //        DataManager.GetInstance().isPlayed = true;

    //        InitialDataSetting();
    //        DataManager.GetInstance().SavePlayerDataToJson();
    //        Debug.Log("Data Creat!");
    //    }
    //}
    public void ClearDataSetting()
    {
        DataManager.GetInstance().dataList.Clear();
    }


    public void InitialDataSetting()
    {        
        // Doorlock Data
        DataManager.GetInstance().dataList.Add(new PlayerData(0, "empty_data", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(101, "data_101", 0)); //1
        DataManager.GetInstance().dataList.Add(new PlayerData(102, "data_102", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(103, "data_103", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(104, "data_104", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(105, "data_105", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(106, "data_106", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(107, "data_107", 0)); //7
        // Schedule Data
        DataManager.GetInstance().dataList.Add(new PlayerData(201, "data_201", 0)); //8
        DataManager.GetInstance().dataList.Add(new PlayerData(202, "data_202", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(203, "data_203", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(204, "data_204", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(205, "data_205", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(206, "data_206", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(207, "data_207", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(208, "data_208", 0)); //15
        // CleanUpMyRoom Data
        DataManager.GetInstance().dataList.Add(new PlayerData(301, "data_301", 0)); //16
        DataManager.GetInstance().dataList.Add(new PlayerData(302, "data_302", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(303, "data_303", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(304, "data_304", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(305, "data_305", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(306, "data_306", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(307, "data_307", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(308, "data_308", 0)); //23
        // PlayPaddle Data
        DataManager.GetInstance().dataList.Add(new PlayerData(401, "data_401", 0)); //24
        DataManager.GetInstance().dataList.Add(new PlayerData(402, "data_402", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(403, "data_403", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(404, "data_404", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(405, "data_405", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(406, "data_406", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(407, "data_407", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(408, "data_408", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(409, "data_409", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(410, "data_410", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(411, "data_411", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(412, "data_412", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(413, "data_413", 0)); //36
        // BackPacking Data
        DataManager.GetInstance().dataList.Add(new PlayerData(501, "data_501", 0)); //37
        DataManager.GetInstance().dataList.Add(new PlayerData(502, "data_502", 0)); 
        DataManager.GetInstance().dataList.Add(new PlayerData(503, "data_503", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(504, "data_504", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(505, "data_505", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(506, "data_506", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(507, "data_507", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(508, "data_508", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(509, "data_509", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(510, "data_510", 0)); //46
        // Scoop Data
        DataManager.GetInstance().dataList.Add(new PlayerData(601, "data_601", 0)); //47
        DataManager.GetInstance().dataList.Add(new PlayerData(602, "data_602", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(603, "data_603", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(604, "data_604", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(605, "data_605", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(606, "data_606", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(607, "data_607", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(608, "data_608", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(609, "data_609", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(610, "data_610", 0));
        DataManager.GetInstance().dataList.Add(new PlayerData(611, "data_611", 0)); //57
    }
    
    // Set Doorlock //
    public void GetSceneIndex1()
    {
        FsmFloat data101 = fsm.FsmVariables.GetFsmFloat("psFocusT");
        FsmFloat data102 = fsm.FsmVariables.GetFsmFloat("psUnfocusT");
        FsmFloat data103 = fsm.FsmVariables.GetFsmFloat("psWrNumC");
        FsmFloat data104 = fsm.FsmVariables.GetFsmFloat("psFailC");
        FsmFloat data105 = fsm.FsmVariables.GetFsmFloat("psRpL");
        FsmFloat data106 = fsm.FsmVariables.GetFsmFloat("psCmplT");
        FsmFloat data107 = fsm.FsmVariables.GetFsmFloat("psSkip");

        DataManager.GetInstance().dataList[1].Result = data101.Value;
        DataManager.GetInstance().dataList[2].Result = data102.Value;
        DataManager.GetInstance().dataList[3].Result = data103.Value;
        DataManager.GetInstance().dataList[4].Result = data104.Value;
        DataManager.GetInstance().dataList[5].Result = data105.Value;
        DataManager.GetInstance().dataList[6].Result = data106.Value;
        DataManager.GetInstance().dataList[7].Result = data107.Value;
    }

    // Set Schecule //
    public void GetSceneIndex2(params float[] myVal)
    {
        float data201 = myVal[0];
        float data202 = myVal[1];
        float data203 = myVal[2];
        float data204 = myVal[3];
        float data205 = myVal[4];
        float data206 = myVal[5];
        float data207 = myVal[6];
        float data208 = myVal[7];

        DataManager.GetInstance().dataList[8].Result = data201;
        DataManager.GetInstance().dataList[9].Result = data202;
        DataManager.GetInstance().dataList[10].Result = data203;
        DataManager.GetInstance().dataList[11].Result = data204;
        DataManager.GetInstance().dataList[12].Result = data205;
        DataManager.GetInstance().dataList[13].Result = data206;
        DataManager.GetInstance().dataList[14].Result = data207;
        DataManager.GetInstance().dataList[15].Result = data208;
    }

    // Set Back Packing L //
    public void GetSceneIndex3()
    {
        //책가방
        FsmFloat data501 = fsm.FsmVariables.GetFsmFloat("data_501");
        FsmFloat data502 = fsm.FsmVariables.GetFsmFloat("data_502");
        FsmFloat data503 = fsm.FsmVariables.GetFsmFloat("data_503");
        FsmFloat data504 = fsm.FsmVariables.GetFsmFloat("data_504");
        FsmFloat data505 = fsm.FsmVariables.GetFsmFloat("data_505");
        FsmFloat data506 = fsm.FsmVariables.GetFsmFloat("data_506");
        FsmFloat data507 = fsm.FsmVariables.GetFsmFloat("data_507");
        FsmFloat data508 = fsm.FsmVariables.GetFsmFloat("data_508");
        FsmFloat data509 = fsm.FsmVariables.GetFsmFloat("data_509");
        FsmFloat data510 = fsm.FsmVariables.GetFsmFloat("data_510");

        DataManager.GetInstance().dataList[37].Result = data501.Value;
        DataManager.GetInstance().dataList[38].Result = data502.Value;
        DataManager.GetInstance().dataList[39].Result = data503.Value;
        DataManager.GetInstance().dataList[40].Result = data504.Value;
        DataManager.GetInstance().dataList[41].Result = data505.Value;
        DataManager.GetInstance().dataList[42].Result = data506.Value;
        DataManager.GetInstance().dataList[43].Result = data507.Value;
        DataManager.GetInstance().dataList[44].Result = data508.Value;
        DataManager.GetInstance().dataList[45].Result = data509.Value;
        DataManager.GetInstance().dataList[46].Result = data510.Value;
    }

    // Scoop L //
    public void GetSceneIndex4()
    {        
        //공옮기기
        FsmFloat data601 = fsm.FsmVariables.GetFsmFloat("data_601");
        FsmFloat data602 = fsm.FsmVariables.GetFsmFloat("data_602");
        FsmFloat data603 = fsm.FsmVariables.GetFsmFloat("data_603");
        FsmFloat data604 = fsm.FsmVariables.GetFsmFloat("data_604");
        FsmFloat data605 = fsm.FsmVariables.GetFsmFloat("data_605");
        FsmFloat data606 = fsm.FsmVariables.GetFsmFloat("data_606");
        FsmFloat data607 = fsm.FsmVariables.GetFsmFloat("data_607");
        FsmFloat data608 = fsm.FsmVariables.GetFsmFloat("data_608");
        FsmFloat data609 = fsm.FsmVariables.GetFsmFloat("data_609");
        FsmFloat data610 = fsm.FsmVariables.GetFsmFloat("data_610");
        FsmFloat data611 = fsm.FsmVariables.GetFsmFloat("data_611");

        DataManager.GetInstance().dataList[47].Result = data601.Value;
        DataManager.GetInstance().dataList[48].Result = data602.Value;
        DataManager.GetInstance().dataList[49].Result = data603.Value;
        DataManager.GetInstance().dataList[50].Result = data604.Value;
        DataManager.GetInstance().dataList[51].Result = data605.Value;
        DataManager.GetInstance().dataList[52].Result = data606.Value;
        DataManager.GetInstance().dataList[53].Result = data607.Value;
        DataManager.GetInstance().dataList[54].Result = data608.Value;
        DataManager.GetInstance().dataList[55].Result = data609.Value;
        DataManager.GetInstance().dataList[56].Result = data610.Value;
        DataManager.GetInstance().dataList[57].Result = data611.Value;
    }

    // Clean Up My Room //
    public void GetSceneIndex5()
    {
        //방정리
        FsmFloat data301 = fsm.FsmVariables.GetFsmFloat("crSucessL");
        FsmFloat data302 = fsm.FsmVariables.GetFsmFloat("crUnfocusT");
        FsmFloat data303 = fsm.FsmVariables.GetFsmFloat("crCmplT");
        FsmFloat data304 = fsm.FsmVariables.GetFsmFloat("crGrbC");
        FsmFloat data305 = fsm.FsmVariables.GetFsmFloat("crMovM");
        FsmFloat data306 = fsm.FsmVariables.GetFsmFloat("crFocusT");
        FsmFloat data307 = fsm.FsmVariables.GetFsmFloat("crSkip");
        FsmFloat data308 = fsm.FsmVariables.GetFsmFloat("data_308");

        DataManager.GetInstance().dataList[16].Result = data301.Value;
        DataManager.GetInstance().dataList[17].Result = data302.Value;
        DataManager.GetInstance().dataList[18].Result = data303.Value;
        DataManager.GetInstance().dataList[19].Result = data304.Value;
        DataManager.GetInstance().dataList[20].Result = data305.Value;
        DataManager.GetInstance().dataList[21].Result = data306.Value;
        DataManager.GetInstance().dataList[22].Result = data307.Value;
        DataManager.GetInstance().dataList[23].Result = data308.Value;
    }

    // Play Paddle //
    public void GetSceneIndex6()
    {        
        //노젓기
        FsmFloat data401 = fsm.FsmVariables.GetFsmFloat("Data_401");
        FsmFloat data402 = fsm.FsmVariables.GetFsmFloat("Data_402");
        FsmFloat data403 = fsm.FsmVariables.GetFsmFloat("Data_403");
        FsmFloat data404 = fsm.FsmVariables.GetFsmFloat("Data_404");
        FsmFloat data405 = fsm.FsmVariables.GetFsmFloat("Data_405");
        FsmFloat data406 = fsm.FsmVariables.GetFsmFloat("Data_406");
        FsmFloat data407 = fsm.FsmVariables.GetFsmFloat("Data_407");
        FsmFloat data408 = fsm.FsmVariables.GetFsmFloat("Data_408");
        FsmFloat data409 = fsm.FsmVariables.GetFsmFloat("Data_409");
        FsmFloat data410 = fsm.FsmVariables.GetFsmFloat("Data_410");
        FsmFloat data411 = fsm.FsmVariables.GetFsmFloat("Data_411");
        FsmFloat data412 = fsm.FsmVariables.GetFsmFloat("Data_412");
        FsmFloat data413 = fsm.FsmVariables.GetFsmFloat("Data_413");

        DataManager.GetInstance().dataList[24].Result = data401.Value;
        DataManager.GetInstance().dataList[25].Result = data402.Value;
        DataManager.GetInstance().dataList[26].Result = data403.Value;
        DataManager.GetInstance().dataList[27].Result = data404.Value;
        DataManager.GetInstance().dataList[28].Result = data405.Value;
        DataManager.GetInstance().dataList[29].Result = data406.Value;
        DataManager.GetInstance().dataList[30].Result = data407.Value;
        DataManager.GetInstance().dataList[31].Result = data408.Value;
        DataManager.GetInstance().dataList[32].Result = data409.Value;
        DataManager.GetInstance().dataList[33].Result = data410.Value;
        DataManager.GetInstance().dataList[34].Result = data411.Value;
        DataManager.GetInstance().dataList[35].Result = data412.Value;
        DataManager.GetInstance().dataList[36].Result = data413.Value;
    }

    // Back Packing H //
    public void GetSceneIndex7()
    {
        //책가방
        FsmFloat data501 = fsm.FsmVariables.GetFsmFloat("data_501");
        FsmFloat data502 = fsm.FsmVariables.GetFsmFloat("data_502");
        FsmFloat data503 = fsm.FsmVariables.GetFsmFloat("data_503");
        FsmFloat data504 = fsm.FsmVariables.GetFsmFloat("data_504");
        FsmFloat data505 = fsm.FsmVariables.GetFsmFloat("data_505");
        FsmFloat data506 = fsm.FsmVariables.GetFsmFloat("data_506");
        FsmFloat data507 = fsm.FsmVariables.GetFsmFloat("data_507");
        FsmFloat data508 = fsm.FsmVariables.GetFsmFloat("data_508");
        FsmFloat data509 = fsm.FsmVariables.GetFsmFloat("data_509");
        FsmFloat data510 = fsm.FsmVariables.GetFsmFloat("data_510");

        DataManager.GetInstance().dataList[37].Result = data501.Value;
        DataManager.GetInstance().dataList[38].Result = data502.Value;
        DataManager.GetInstance().dataList[39].Result = data503.Value;
        DataManager.GetInstance().dataList[40].Result = data504.Value;
        DataManager.GetInstance().dataList[41].Result = data505.Value;
        DataManager.GetInstance().dataList[42].Result = data506.Value;
        DataManager.GetInstance().dataList[43].Result = data507.Value;
        DataManager.GetInstance().dataList[44].Result = data508.Value;
        DataManager.GetInstance().dataList[45].Result = data509.Value;
        DataManager.GetInstance().dataList[46].Result = data510.Value;
    }

    // Scoop H //
    public void GetSceneIndex8()
    {
        //공옮기기
        FsmFloat data601 = fsm.FsmVariables.GetFsmFloat("data_601");
        FsmFloat data602 = fsm.FsmVariables.GetFsmFloat("data_602");
        FsmFloat data603 = fsm.FsmVariables.GetFsmFloat("data_603");
        FsmFloat data604 = fsm.FsmVariables.GetFsmFloat("data_604");
        FsmFloat data605 = fsm.FsmVariables.GetFsmFloat("data_605");
        FsmFloat data606 = fsm.FsmVariables.GetFsmFloat("data_606");
        FsmFloat data607 = fsm.FsmVariables.GetFsmFloat("data_607");
        FsmFloat data608 = fsm.FsmVariables.GetFsmFloat("data_608");
        FsmFloat data609 = fsm.FsmVariables.GetFsmFloat("data_609");
        FsmFloat data610 = fsm.FsmVariables.GetFsmFloat("data_610");
        FsmFloat data611 = fsm.FsmVariables.GetFsmFloat("data_611");

        DataManager.GetInstance().dataList[47].Result = data601.Value;
        DataManager.GetInstance().dataList[48].Result = data602.Value;
        DataManager.GetInstance().dataList[49].Result = data603.Value;
        DataManager.GetInstance().dataList[50].Result = data604.Value;
        DataManager.GetInstance().dataList[51].Result = data605.Value;
        DataManager.GetInstance().dataList[52].Result = data606.Value;
        DataManager.GetInstance().dataList[53].Result = data607.Value;
        DataManager.GetInstance().dataList[54].Result = data608.Value;
        DataManager.GetInstance().dataList[55].Result = data609.Value;
        DataManager.GetInstance().dataList[56].Result = data610.Value;
        DataManager.GetInstance().dataList[57].Result = data611.Value;
    }
}

