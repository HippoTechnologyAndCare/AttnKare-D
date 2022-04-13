using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo_API : MonoBehaviour
{
    public string Authorization;
    public DATA_API.USERINNER userInfo;
    public DATA_API.PlayerInner playerInfo;
    public int service_id;
    public DATA_API.JobData jobInfo;
    private UserInfo_API() { }
    private static UserInfo_API instance = null;
    public static UserInfo_API GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<UserInfo_API>();
            if (instance == null)
            {
                GameObject container = new GameObject("UserInfo");
                instance = container.AddComponent<UserInfo_API>();
            }
        }
        return instance;
    }




    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    public void ResetPlayerInfo()
    {
        playerInfo = null;
        jobInfo = null;
    }
    /*
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
    */
}


