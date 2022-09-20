using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using KetosGames.SceneTransition;


public class Player_API : MonoBehaviour
{
    // Start is called before the first frame update
    public GUIDE_API APIMANAGER;
    public DATA_API APIDATA;
    public HUD_API UI_HUD;
    public string POST_case;
    int playerID;
    private void Start()
    {
        APIMANAGER = GameObject.FindObjectOfType<GUIDE_API>();
        APIDATA = GameObject.FindObjectOfType<DATA_API>();
        UI_HUD = FindObjectOfType<HUD_API>();
    }
 
    public void SetClick()
    {
        int a;
        Debug.Log(POST_case);
        Debug.Log(transform.name);
        switch (POST_case)
        {
            
            case "GET_Joblist":     playerID = int.Parse(transform.name); break;
                                 //   StartCoroutine(APIMANAGER.GET_Joblist(int.Parse(transform.name), UI_HUD.nCrntPg_Job)); break;
            case "GET_JobRefresh":  playerID = int.Parse(transform.name); StartCoroutine(UI_HUD.PopUP("Joblist")); break;
                                  //  StartCoroutine(APIMANAGER.GET_Joblist(int.Parse(transform.name), UI_HUD.nCrntPg_Job)); break;
            case "POST_AddJob":   StartCoroutine(APIMANAGER.POST_Addjob()); break;//, UI_HUD.nCrntPg_Job)); break;
            case "FIND_JOB":        APIDATA.FindPlayerJob(transform.name); break;
            case "POST_Login":      APIMANAGER.CoroutineStart(POST_case); break;
            case "DEL_Login":       APIMANAGER.CoroutineStart(POST_case); break;
            case "POST_Signin": APIMANAGER.CoroutineStart(POST_case); break;
            case "DEL_UserInfo": APIMANAGER.CoroutineStart(POST_case); break;
            case "POST_AddPlayer":  APIMANAGER.CoroutineStart("POST_Signin"); break;
            case "PUT_EditPlayer": APIMANAGER.CoroutineStart("PUT_UserInfo"); break;
            case "GET_PDFList":   StartCoroutine(APIMANAGER.GET_PDFList()); break;
            case "DEL_Joblist": APIMANAGER.CoroutineStart("DEL_Joblist"); break;
        }
    }

   
    public void JobListPageUP()
    {
        if (UI_HUD.job_pack.nCrntPg_Job != UI_HUD.job_pack.nTtlPg_Job)
        {
            UI_HUD.job_pack.nCrntPg_Job += 1;
            Debug.Log(UI_HUD.job_pack.nCrntPg_Job);
            StartCoroutine(APIMANAGER.GET_Joblist());
        }
    }
    public void JobListPageDOWN()
    {
        if (UI_HUD.job_pack.nCrntPg_Job != 1)
        {
            UI_HUD.job_pack.nCrntPg_Job -= 1;
            Debug.Log(UI_HUD.job_pack.nCrntPg_Job);
            StartCoroutine(APIMANAGER.GET_Joblist());
        }
    }

    public void Confirm_n_DataExistenceCheck()
    {
        int grade = UserInfo_API.GetInstance().UserTotalInfo.user.grade;
        if (grade > 3) { StatusHigh(); }
        if(grade < 4) { StatusLow(); }
    }
       
    void StatusLow()
    {
        int status = UserInfo_API.GetInstance().jobInfo.status;
        string scene_index = "";
        switch (status)
        {
            case 0: scene_index = "OPENEND"; break;
            case 1070: scene_index = "Tutorial"; break;
            case 1080: scene_index = "BagPacking2X2_Young"; break;
            case 1001: scene_index = "Scoop_tube_easy"; break;
            case 1002: scene_index = "NumMatch"; break;
            case 1003: scene_index = "CleanUp"; break;
            case 1004: scene_index = "Ending"; break;
            case 98: Debug.Log(status); return;
            case 99: Debug.Log(status); return;
            case 999: Debug.Log(status); return;
        }
        SceneLoader.LoadScene(scene_index);
    }
    void StatusHigh() {
        int status = UserInfo_API.GetInstance().jobInfo.status;
        string scene_index = "";
        switch (status)
        {
            case 0: scene_index = "OPENEND"; break;
            case 1070: scene_index = "Tutorial"; break;
            case 1080: scene_index = "E_Schedule03"; break;
            case 1001: scene_index = "NewPaddle"; break;
            case 1002: scene_index = "NumMatch"; break;
            case 1003: scene_index = "Conveyor"; break;
            case 1004: scene_index = "Ending"; break;
            case 98: Debug.Log(status); return;
            case 99: Debug.Log(status); return;
            case 999: Debug.Log(status); return;
        }
        SceneLoader.LoadScene(scene_index);

    }

}
