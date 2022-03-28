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
            
            case "GET_Joblist":     playerID = int.Parse(transform.name); 
                                    StartCoroutine(APIMANAGER.GET_Joblist(int.Parse(transform.name), UI_HUD.nCrntPg_Job)); break;
            case "GET_JobRefresh":  playerID = int.Parse(transform.name);
                                    StartCoroutine(UI_HUD.PopUP("Joblist"));
                                    StartCoroutine(APIMANAGER.GET_Joblist(int.Parse(transform.name), UI_HUD.nCrntPg_Job)); break;
            case "POST_AddJob":     StartCoroutine(APIMANAGER.POST_Addjob(int.Parse(transform.name), UI_HUD.nCrntPg_Job)); break;
            case "FIND_JOB":        APIDATA.FindPlayerJob(transform.name); break;
            case "POST_Login":      APIMANAGER.CoroutineStart(POST_case); break;
            case "GET_Playerlist":  StartCoroutine(APIMANAGER.GET_Playerlist(UI_HUD.nCrntPgeN)); break;
            case "POST_AddPlayer":  APIMANAGER.CoroutineStart(POST_case); break;
            case "GET_SearchPlayer":StartCoroutine(APIMANAGER.GET_SearchPlayer(APIDATA.SpecificPlayer())); break;
            case "GET_Refreshplayer":StartCoroutine(UI_HUD.PopUP("Playerlist"));
                                     StartCoroutine((APIMANAGER.GET_Playerlist(UI_HUD.nCrntPgeN))); break;
            case "ResetPlayerlist":  UI_HUD.ResetSearchbyName(); APIMANAGER.CoroutineStart("GET_Playerlist"); break;
            case "GET_PDFList":     StartCoroutine(APIMANAGER.GET_PDFList(transform.parent.name)); break;
        }
    }

    public void PlayerListPageUP()
    {
        if(UI_HUD.nCrntPgeN != UI_HUD.nTtlPgeN)
        {
            UI_HUD.nCrntPgeN += 1;
            Debug.Log(UI_HUD.nCrntPgeN);
            StartCoroutine(APIMANAGER.GET_Playerlist(UI_HUD.nCrntPgeN));
        }
    }
    public void PlayerListPageDOWN()
    {
        if (UI_HUD.nCrntPgeN != 1)
        {
            UI_HUD.nCrntPgeN -= 1;
            Debug.Log(UI_HUD.nCrntPgeN);
            StartCoroutine(APIMANAGER.GET_Playerlist(UI_HUD.nCrntPgeN));
        }
    }
    public void JobListPageUP()
    {
        if (UI_HUD.nCrntPg_Job != UI_HUD.nTtlPg_Job)
        {
            UI_HUD.nCrntPg_Job += 1;
            Debug.Log(UI_HUD.nCrntPg_Job);
            StartCoroutine(APIMANAGER.GET_Joblist(int.Parse(UI_HUD.JOB_Refresh.name), UI_HUD.nCrntPg_Job));
        }
    }
    public void JobListPageDOWN()
    {
        if (UI_HUD.nCrntPg_Job != 1)
        {
            UI_HUD.nCrntPg_Job -= 1;
            Debug.Log(UI_HUD.nCrntPg_Job);
            StartCoroutine(APIMANAGER.GET_Joblist(int.Parse(UI_HUD.JOB_Refresh.name), UI_HUD.nCrntPg_Job));
        }
    }

    public void Confirm_n_DataExistenceCheck()
    {
        int status = UserInfo_API.GetInstance().jobInfo.status;
        int scene_index = 0;
        switch (status)
        {
            case 0: scene_index = 11; break;
            case 1070: scene_index = 10; break;
            case 1080: scene_index = 3; break;
            case 1001: scene_index = 14; break;
            case 1002: scene_index = 4; break;
            case 1003: scene_index = 9; break;
            case 1004: scene_index = 13; break;
            case 98: Debug.Log(status); return;
            case 99: Debug.Log(status); return;
            case 999: Debug.Log(status); return;
        }
        SceneLoader.LoadScene(scene_index);
    }

}
