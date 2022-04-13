using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using BNG;



public class DATA_API : MonoBehaviour
{
    public HUD_API UI_Hud;
    public UserInfo_API UserInfoInput;
    private int perPage = 5;
    private void Awake()
    {
    }
    private void Start()
    {
        UserInfoInput = FindObjectOfType<UserInfo_API>();

    }
    /// <summary>
    /// 
    /// </summary>
    /// 

    public class LoginInfo {

        public LoginInfoInner data;
    }
    [Serializable]
    public class LoginInfoInner
    {
        public string access_token;
        public string renewal_token;
    }
    public string GET_Login(string webResult)
    {
        LoginInfo userData = JsonConvert.DeserializeObject<LoginInfo>(webResult);
        Debug.Log(userData.data.access_token);
        UserInfo_API.GetInstance().Authorization = userData.data.access_token;
        return userData.data.access_token;
    }
    public class USERINFO
    {
        public USERINNER user;
    }

    [Serializable]
    public class USERINNER
    {
        public string uid;
        public string password;
    }

    public class DataInfo
    {
        public DataInner data;
    }
    public class DataInner
    {
        public List<ServiceInner> services;
    }
    public class ServiceInner
    {
        public int id;
        public int device_id;
        public int farm_id;
        public int service_type_cd;
        public string inserted_at;
        public FarmInfo farm;
        public DeviceInfo device;
        public string updated_at;
    }

    public class FarmInfo
    {
        public int id;
        public string ip;
        public string name;
        public string inserted_at;
        public string update_at;
    }
    public class DeviceInfo
    {
        public int id;
        public string device_id;
        public string name;
        public string inserted_at;
        public string udpated_at;
    }


    USERINNER userinfo;
    public string POST_Login() //로그인
    {

        Dictionary<string, object> user = UI_Hud.UserLogin();
        if (user != null)
        {
            USERINNER JsonUserClass = new USERINNER
            {
                uid = (string)user["uid"],
                password = (string)user["password"]
            };

            USERINFO JsonLogInClass = new USERINFO
            {
                user = JsonUserClass
            };
            string UserJsonString = JsonUtility.ToJson(JsonLogInClass);
            Debug.Log(UserJsonString);
            userinfo = JsonUserClass;
            return UserJsonString;
        }
        else return " ";

    }

    public int userData_id;
    public bool GET_Registration(string webResult)
    {
        DataInfo userData = JsonConvert.DeserializeObject<DataInfo>(webResult);
        return CompareInfo(userData);

    }

    bool CompareInfo(DataInfo userdata) //가입여부 확인 --> device id와 farm id가 1 인지 확인
    {
        for (int i = 0; i < userdata.data.services.Count; i++)
        {
            if (userdata.data.services[i].device_id == 1 && userdata.data.services[i].farm_id == 1) //일단 device 1, farm 1로 고정
            {
                userData_id = userdata.data.services[i].id;
                Debug.Log(userdata.data.services[i].id);
                return true;
            }
        }
        //만약 없다면
        return false;
    }

    public class ServiceSignIn
    {
        public SignInInner service;
    }
    public class SignInInner
    {
        public int device_id;
        public int farm_id;
        public int service_type_cd;
    }
    public string POST_Signin()
    {
        SignInInner JsonSignIn = new SignInInner
        {
            device_id = 1,
            farm_id = 1,
            service_type_cd = 1
        };
        ServiceSignIn JsonSignInClass = new ServiceSignIn
        {
            service = JsonSignIn
        };
        string UserJsonString = JsonUtility.ToJson(JsonSignInClass);
        return UserJsonString;
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PlayerView
    {
        public int count;
        public PlayersInfo data;
    }
    public class PlayersInfo
    {
        public List<PlayerInner> players;
    }

    [Serializable]
    public class PlayerInner
    {
        public int id;
        public string player_name;
        public string birth;
        public string phonenum;
        public string email;
        public string guardian_name;
        public string gender;
        public int grade;
        public string inserted_at;
        public string updated_at;
    }
    PlayerView PlayersData;
    List<PlayerInner> AllPlayersList = new List<PlayerInner>();
    public void GET_Playerlist(string webResult, bool all)
    {
        UI_Hud.ResetList("PLAYER");
        PlayersData = JsonConvert.DeserializeObject<PlayerView>(webResult);
        Debug.Log("!!!!!총인원수" + PlayersData.count);
        UserInfo_API.GetInstance().userInfo = userinfo; //사용자 정보를 저장
        if (PlayersData.data == null) { UI_Hud.ShowPlayerList(); return; }
        for (int i = 0; i < PlayersData.data.players.Count; i++)
        {
            PlayerInner playerinfo = PlayersData.data.players[i];
            if (all) AllPlayersList.Add(playerinfo);
            Debug.Log(playerinfo.player_name);
            UI_Hud.CreateList(playerinfo, "GOING");
        }
        UI_Hud.ShowPlayerList();
        UI_Hud.PlayerPage(CreatePage(PlayersData.count));
    }
    public int CreatePage(int TotalCnt)
    {
        int division = TotalCnt / perPage;
        int remainder = TotalCnt % perPage;
        if(division == 0) { return 1; }
        if (division >= 1)
        {
            if (remainder == 0)
            {
                return division;
            }
            else { return division + 1; }
        }
        else return 0;
    }
    public string SpecificPlayer()
    {
        string childname = "";
        childname = UI_Hud.SearchbyName();
        Debug.Log(childname);
        return childname;


    }

    /// <summary>
    /// 
    /// </summary>

    public class NewPlayer
    {
        public NewPlayerInner user;
    }
    [Serializable] //요거 안하니까 에러남
    public class NewPlayerInner
    {
        public string player_name;
        public string birth;
        public string phonenum;
        public string email;
        public string guardian_name;
        public string gender;
        public int grade;
    }
    public string POST_AddPlayer()
    {
        Dictionary<string, string> player = UI_Hud.AddNewPlayer();
        if (player != null)
        {
            NewPlayerInner JsonPlayerInner = new NewPlayerInner
            {
                player_name = (string)player["player_name"],
                birth = (string)player["birth"],
                phonenum = (string)player["phonenum"],
                email = (string)player["email"],
                guardian_name = (string)player["guardian_name"],
                gender = (string)player["gender"],
                grade = int.Parse(player["grade"])
            };
            NewPlayer JsonNewPlayer = new NewPlayer
            {
                user = JsonPlayerInner
            };
            Debug.Log(JsonNewPlayer.user.player_name + " " + JsonNewPlayer.user.birth);
            string UserJsonString = JsonUtility.ToJson(JsonNewPlayer);
            Debug.Log(UserJsonString);
            return UserJsonString;
        }
        else return " ";
    }
    Dictionary<string, Dictionary<string, Dictionary<string, string>>> addPlayer;
    public int RegisterPlayer(string webResult)
    {
        addPlayer = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(webResult);
        Debug.Log(addPlayer["data"]["player"]["player_name"]);

        int id = int.Parse(addPlayer["data"]["player"]["id"]);
        Debug.Log(id);
        return id;

    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class JobView
    {
        public int count;
        public JobList data;
    }
    public class JobList
    {
        public List<JobData> jobs;
    }
    [Serializable]
    public class JobData
    {
        public string id;
        public string name;
        public string service_id;
        public int status;
        public List<ResultList> result;
        public string place;
        public string updated_at;
        public string inserted_at;
    }
    [Serializable]
    public class ResultList
    {

    }
    JobView JobsData;
    List<JobData> AllJobsList = new List<JobData>();
    public void GET_Joblist(string webResult, int player_id)
    {
        UI_Hud.ResetList("JOB");
        AllJobsList.Clear();
        PlayerInner SelectedPlayer = AllPlayersList.Find(x => x.id == player_id);
        UserInfo_API.GetInstance().playerInfo = SelectedPlayer; //사용자 정보를 저장
        UI_Hud.job_button.name = player_id.ToString();
        JobsData = JsonConvert.DeserializeObject<JobView>(webResult);
        UI_Hud.GET_JOBLIST(SelectedPlayer);
        Debug.Log("@@@PLAYER ID" + SelectedPlayer.id);
        if (JobsData.data == null) { UI_Hud.ShowJobList(); return; }
        for (int i = 0; i < JobsData.data.jobs.Count; i++)
        {
            JobData jobinfo = JobsData.data.jobs[i];
            AllJobsList.Add(jobinfo);
            UI_Hud.CreateJobList(jobinfo);
            Debug.Log(i);

            //  UI_Hud.CreateList(playerinfo.id, playerinfo.player_name, playerinfo.birth, playerinfo.phonenum, "GOING");
        }
        UI_Hud.JobPage(CreatePage(JobsData.count));
        UI_Hud.ShowJobList();
    }

    public void FindPlayerJob(string id)
    {
        JobData SelectedJob = AllJobsList.Find(x => x.id == id);
        UserInfo_API.GetInstance().jobInfo = SelectedJob;
    }

    public class NewJob
    {
        public NewJobInner job;
    }
    [Serializable]
    public class NewJobInner
    {
        public string name;
        public int player_id;
        public int service_id;
        public string place;
    }
    public string POST_AddJob(int id)
    {
        Dictionary<string, string> job = UI_Hud.AddNewJob();
        if (job != null)
        {
            NewJobInner JsonJobInner = new NewJobInner
            {
                name = (string)job["name"],
                player_id = id,
                service_id = 1,
                place = (string)job["place"]
            };
            NewJob JsonNewJob = new NewJob
            {
                job = JsonJobInner
            };
            Debug.Log(JsonNewJob.job.name + JsonNewJob.job.place);
            string UserJsonString = JsonUtility.ToJson(JsonNewJob);
            Debug.Log(UserJsonString);
            return UserJsonString;
        }
        else return " ";
    }
    public class ERROR
    {
        public ERRORMSG error;
    }
    public class ERRORMSG
    {
        public int code;
        public string message;
    }

    ERROR errorMessage;
    public int ERROR_CONTROLLER(string webResult)
    {
        errorMessage = JsonConvert.DeserializeObject<ERROR>(webResult);
        return errorMessage.error.code;
    }
    public class Status
    {
        public StatusInner job;
    }
    [Serializable]
    public class StatusInner
    {
        public int status;
    }
    public string PUT_Status(int scene_id)
    {
        StatusInner JsonStatusInner = new StatusInner
        {
            status = scene_id
        };
        Status JsonStatus = new Status
        {
            job = JsonStatusInner
        };
        string UserJsonString = JsonUtility.ToJson(JsonStatus);
        return UserJsonString;
    }
    ///// DATA POST
    ///
    public class SceneData
    {
        public string topic;
        public SceneDataInner payload;
    }
    [Serializable]
    public class SceneDataInner
    {
        public int type;
        public string job_id;
        public int scene_id;
        public int player_id;
        public string data; //행동 데이터
    }

    public string SendData(int data_type, int scene_id, string sentdata)
    {
        string m_stopic = "UP." + UserInfo_API.GetInstance().userInfo.uid + "|dtx|" + UserInfo_API.GetInstance().jobInfo.service_id + "|1";
        SceneDataInner JsonDataInner = new SceneDataInner
        {
            type = data_type,
            job_id = UserInfo_API.GetInstance().jobInfo.id,
            scene_id = scene_id,
            player_id = UserInfo_API.GetInstance().playerInfo.id,
            data = sentdata
        };
        SceneData JsonData = new SceneData
        {
            topic = m_stopic,
            payload = JsonDataInner

        };
        Debug.Log(JsonData.payload.player_id + JsonData.payload.scene_id);
        string UserJsonString = JsonUtility.ToJson(JsonData);
        return UserJsonString;
    }
    [Serializable]
    public class JobExecution
    {
        public JobExecutionInner job;
    }
    [Serializable]
    public class JobExecutionInner
    {
        public string job_id;
        public int service_id;
        public JobExecutionCmd body;
    }
    [Serializable]
    public class JobExecutionCmd
    {
        public string cmd;
    }
    public string POST_JobExecution()
    {
        JobExecutionCmd JsonCmd = new JobExecutionCmd
        {
            cmd = "echo $PATH"
        };
        JobExecutionInner JsonDataInner = new JobExecutionInner
        {
            job_id = UserInfo_API.GetInstance().jobInfo.id,
            service_id = UserInfo_API.GetInstance().jobInfo.service_id,
            body = JsonCmd
        };
        JobExecution JsonData = new JobExecution
        {
            job = JsonDataInner
        };
        string UserJsonString = JsonUtility.ToJson(JsonData);
        Debug.Log(UserJsonString);
        return UserJsonString;


    }
    [Serializable]
    public class PDFList
    {
        public PDFListInner data;
    }
    [Serializable]
    public class PDFListInner
    {
        public List<PDFs> uploads;
    }
    [Serializable]
    public class PDFs
    {
        public int id;
        public string content_type;
        public string filename;
        public string hash;
        public int size;
        public string inserted_at;
        public string updated_at;
    }
    PDFList Pdfs_id;
    public int GET_PDFList(string webResult)
    {
        Pdfs_id = JsonConvert.DeserializeObject<PDFList>(webResult);
        Debug.Log(Pdfs_id.data.uploads[0].id);
        int id_pdf = Pdfs_id.data.uploads[0].id;
        return id_pdf;
    }
    public void POPUP()
    {
        StartCoroutine(UI_Hud.PopUP("PDF"));
    }


}
