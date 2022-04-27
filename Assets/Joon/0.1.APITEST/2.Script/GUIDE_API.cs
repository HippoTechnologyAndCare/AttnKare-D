using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class GUIDE_API : MonoBehaviour
{
    public static string RecordingPath;
    public static GUIDE_API Guide_instance;
    private string Authorization;
    //    HUD_API UI_Hud;
    DATA_API DATA;
    UserInfo_API Userinfo;
    // string BASE_URL = "https://adhd.hippotnc.kr:444";
    string BASE_URL = "http://jdi.bitzflex.com:4007";
    string SigninURL = "/api/v2/users"; //계정 생성(POST) + 상세조회(GET)
    string LoginURL = "/api/v2/session";
    string ServicesURL = "/api/v2/services?type=2761"; //서비스 목록 조회
    string ServicesSubsURL = "/api/v2/subscriptions"; //사용자 구독 서비스 목록 조회(GET) + 사용자 서비스 구독(POST)
    string JobURL = ""; //servicesubsurl + subs_id + joburl
    string JobBottomURL = "/jobs?sort_order=desc&sort_by=inserted_at&page=1&per_page=5";
    string AddJobURL = "";
    string AddJobBottomURL = "/jobs"; //servicessubsurl + subid + joburl
    string AddJobFinalURL = "";
    string DataSendURL = "/api/v2/data/up";
    string MP3URL = "";
    string JobExecutionURL = "/eval"; //servicessuburl + sub_id + /jobs/jobid_eval
    string PdfListURL = "/attn/uploads?sort_order=desc&sort_by=inserted_at&page=1&per_page=10";
    string PdfListURL_jobid = "/uploads?sort_by=version_number&sort_order=desc";
    string PdfFileURl = "";
    /*
    string LoginURL = "/api/v1/session";
    string RegistrationURL = "/api/v1/operator/services";
    string PlayerRegister = "/api/v1/operator/players";
    string PlayerURL = "/api/v1/operator/players?sort_order=desc&sort_by=inserted_at&page=";
    string PlayerURL_perPage = "&per_page=5";
    string SearchPlayerURL = "/api/v1/operator/search-players?query=";
    string JobURL = "/api/v1/operator/jobs?sort_order=desc&sort_by=inserted_at&page=";
    string JobURL_perPage="&per_page=5&player_id=";
    string AddJobURL = "/api/v1/operator/jobs";
    string DataSendURL = "/api/v1/operator/data/up";
    string MP3URL = "/api/v1/operator/jobs/";
    string JobExecutionURL = "/api/v1/operator/eval";
    string PdfListURL = "/api/v1/operator/jobs/";
    string PdfListURL_jobid= "/uploads?sort_by=version_number&sort_order=desc";
    string PdfFileURl = "/api/v1/operator/jobs/";
    */

    // Start is called before the first frame update


    private void Awake()
    {
        GUIDE_API.RecordingPath = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + "VOICE";

        if (Guide_instance == null)
        {
            Guide_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }
    private void Start()
    {

        SetURL();
        Authorization = "";
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("SCENE LOADED");
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            DATA = FindObjectOfType<DATA_API>();
            Userinfo = FindObjectOfType<UserInfo_API>();
        }
    }

    void SetURL()
    {
        SigninURL = BASE_URL + SigninURL;
        LoginURL = BASE_URL + LoginURL;
        ServicesURL = BASE_URL + ServicesURL;
        ServicesSubsURL = BASE_URL + ServicesSubsURL;
        JobURL = ServicesSubsURL + "/";
        AddJobURL = ServicesSubsURL + "/";
        DataSendURL = BASE_URL + DataSendURL;

     

    }
    public void CoroutineStart(string coroutine)
    {
        StartCoroutine(coroutine);
    }

    public IEnumerator POST_Signin()
    {
        string UserJsonString = DATA.POST_Signin();
        Debug.Log("SEE");
        yield return new WaitUntil(() => UserJsonString != null);
        Debug.Log("SEE3");
        UnityWebRequest webRequest = UnityWebRequest.Post(SigninURL, UserJsonString);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
            Debug.Log(webRequest.downloadHandler.text);
            DATA.PlayerError(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("SIGNIN COMPLETE");
            DATA.SignInComplete();
            Authorization = DATA.GET_Token(webRequest.downloadHandler.text);
        }
    }
    public IEnumerator POST_Login() //로그인
    {
        string UserJsonString = DATA.POST_Login();
        yield return new WaitUntil(() => UserJsonString != null);
        UnityWebRequest webRequest = UnityWebRequest.Post(LoginURL, UserJsonString);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
            if (webRequest.error == "HTTP/1.1 500 Internal Server Error")
            {
                //Warning.text = "Sorry, Could not find your account";
            }
        }
        else
        {
            Debug.Log("LOGIN COMPLETE");
            Authorization = DATA.GET_Token(webRequest.downloadHandler.text);
            StartCoroutine(GET_UserInfo());
        }

    }

    public IEnumerator GET_UserInfo()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(SigninURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("if");
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) //list is empty error (just show empty list)
            {
                Debug.Log("error");
                //StartCoroutine(POST_Signin());
            }
        }
        else
        {
            Debug.Log("USER LOGGED IN");
            Debug.Log(webRequest.downloadHandler.text);
            DATA.GET_UserInfo(webRequest.downloadHandler.text);
            StartCoroutine(GET_ServicesSubs());
        }
    }

    IEnumerator GET_ServicesSubs()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(ServicesSubsURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("if");
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) //list is empty error (just show empty list)
            {
                Debug.Log("error");
                StartCoroutine(GET_Services());
            }
        }
        else
        {
            Debug.Log("USER LOGGED IN");
            Debug.Log(webRequest.downloadHandler.text);
            bool subscribed = DATA.GET_ServicesSubs(webRequest.downloadHandler.text);
            if (subscribed) StartCoroutine(GET_Joblist());
            if (!subscribed) StartCoroutine(GET_Services());

        }
    }
    IEnumerator GET_Services()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(ServicesURL);
        Debug.Log(ServicesURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("ERROR");
            Debug.Log(webRequest.downloadHandler.text);
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Services Listed");
            Debug.Log(webRequest.downloadHandler.text);
            DATA.GET_Services(webRequest.downloadHandler.text);
            StartCoroutine(POST_ServicesSubs());

        }
    }
    IEnumerator POST_ServicesSubs() //서비스 구독
    {
        string UserJsonString = DATA.POST_ServicesSubs();
        UnityWebRequest webRequest = UnityWebRequest.Post(ServicesSubsURL, UserJsonString);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Signin COMPLETE");
            DATA.GET_SubsID(webRequest.downloadHandler.text);
            StartCoroutine(GET_ServicesSubs());


        }
    }







    /*
    IEnumerator POST_Signin() //서비스 가입
    {
        string UserJsonString = DATA.POST_Signin();
        UnityWebRequest webRequest = UnityWebRequest.Post(RegistrationURL, UserJsonString);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Signin COMPLETE");
            Debug.Log(webRequest.downloadHandler.text);
            StartCoroutine(GET_Playerlist(1));

        }
    }

    */

    string JOBFinalURL;
    public IEnumerator GET_Joblist()
    {
        JOBFinalURL = JobURL + UserInfo_API.GetInstance().UserTotalInfo.id + JobBottomURL;
        Debug.Log(JOBFinalURL);
        UnityWebRequest webRequest = UnityWebRequest.Get(JOBFinalURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
            Debug.Log(webRequest.downloadHandler.text);
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) DATA.GET_Joblist(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Get Joblist COMPLETE");
            DATA.GET_Joblist(webRequest.downloadHandler.text);
        }
    }


    public IEnumerator POST_Addjob()
    {
        string UserJsonString = DATA.POST_AddJob();
        yield return new WaitUntil(() => UserJsonString != null);
        Debug.Log(UserJsonString);
        AddJobFinalURL = AddJobURL + UserInfo_API.GetInstance().UserTotalInfo.id + AddJobBottomURL;
        UnityWebRequest webRequest = UnityWebRequest.Post(AddJobFinalURL, UserJsonString); ;
        byte[] jsonToSend = new UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Add Job COMPLETE");
            StartCoroutine(GET_Joblist());
        }
    }

    public IEnumerator POST_NQTT(int datatype, int scene_id, string data)
    {
        Debug.Log("DATA SENDING");
        string UserJsonString = DATA.SendData(datatype, scene_id, data);
        yield return new WaitUntil(() => UserJsonString != null);
        UnityWebRequest webRequest = UnityWebRequest.Post(DataSendURL, UserJsonString); ;
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Data Send COMPLETE");
        }


    }
    public IEnumerator POST_MP3(int scene_id)
    {
        WWWForm formData = new WWWForm();
        MP3URL = AddJobFinalURL + UserInfo_API.GetInstance().jobInfo.id + "/attn/audio-uploads";
        Debug.Log(MP3URL);
        formData.AddBinaryData("upload", File.ReadAllBytes(GUIDE_API.RecordingPath + ".mp3"), "VOICE.mp3", "audio/mpeg");
        Debug.Log(GUIDE_API.RecordingPath + ".mp3");
        formData.AddField("job_id", UserInfo_API.GetInstance().jobInfo.id);
        formData.AddField("scene_id", scene_id);
        UnityWebRequest webRequest = UnityWebRequest.Post(MP3URL, formData);
        //   byte[] boundary = UnityWebRequest.GenerateBoundary();
        //  string contentType = String.Concat("multipart/form-data; boundary=", Encoding.UTF8.GetString(boundary));
        //   webRequest.SetRequestHeader("Content-Type", "multipart/form-data");
        webRequest.SetRequestHeader("Authorization", Authorization);
        webRequest.SetRequestHeader("Content-Type", "multipart/form-data");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Data Voice COMPLETE");
            File.Delete(GUIDE_API.RecordingPath + ".mp3");
        }


    }

    string JobStatusURL = "";
    public IEnumerator PUT_STATUS(int scene_id)
    {
        string UserJsonString = DATA.PUT_Status(scene_id);
        yield return new WaitUntil(() => UserJsonString != "");
        JobStatusURL = AddJobURL + UserInfo_API.GetInstance().UserTotalInfo.id + AddJobBottomURL + "/" + UserInfo_API.GetInstance().jobInfo.id;
        Debug.Log(JobStatusURL + scene_id);
        UnityWebRequest webRequest = UnityWebRequest.Put(JobStatusURL, UserJsonString);
        Debug.Log("send");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Status Change COMPLETE");
            if (scene_id == 999)
            {

                Debug.Log("9999999");
                StartCoroutine(POST_JobExecution());
            }
        }
    }
    string FinalJobExecution = "";
    IEnumerator POST_JobExecution()
    {
        Debug.Log("DATA POSTING");
        string UserJsonString = DATA.POST_JobExecution();
        yield return new WaitUntil(() => UserJsonString != null);
        FinalJobExecution = AddJobURL + UserInfo_API.GetInstance().UserTotalInfo.id + AddJobBottomURL + "/" + UserInfo_API.GetInstance().jobInfo.id + JobExecutionURL;
        Debug.Log(FinalJobExecution);
        UnityWebRequest webRequest = UnityWebRequest.Post(FinalJobExecution, UserJsonString); ;
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Data Post COMPLETE");
            StartCoroutine(GoBacktoJoblist());
        }
    }

    public IEnumerator GoBacktoJoblist()
    {
        DATA_API.LoginInner user = UserInfo_API.GetInstance().LoginInfo;
        //  DATA = null;
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 0);
        //    DATA = FindObjectOfType<DATA_API>();
        Debug.Log("!!!!!!!!!!!!!!!!JOB LIST");
        UserInfo_API.GetInstance().LoginInfo = user;
        yield return StartCoroutine(GET_Joblist());
        UserInfo_API.GetInstance().LoginInfo = user;
        Debug.Log(UserInfo_API.GetInstance().LoginInfo.uid);
        StartCoroutine(GET_Joblist());
    }
    /*
   public void Nullify()
    {
        DATA = null;
        userinfo_instance = null;
        userinfo_instance = UserInfo_API.GetInstance();
        DATA = FindObjectOfType<DATA_API>();
    }
    */


    string PdfListURL_final;
    public IEnumerator GET_PDFList()
    {
        PdfListURL_final = JobURL + UserInfo_API.GetInstance().UserTotalInfo.id +"/" +UserInfo_API.GetInstance().jobInfo.id + PdfListURL;
        Debug.Log(PdfListURL_final +"///"+ UserInfo_API.GetInstance().UserTotalInfo.id);
        UnityWebRequest webRequest = UnityWebRequest.Get(PdfListURL_final);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) DATA.GET_PDFList(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            int pdf_id = DATA.GET_PDFList(webRequest.downloadHandler.text);
            StartCoroutine(GET_PDFFile(pdf_id));
        }
    }
    string PDFFileURL_final;
    public IEnumerator GET_PDFFile(int id)
    {
        PDFFileURL_final = JobURL + "/" + UserInfo_API.GetInstance().UserTotalInfo.id + UserInfo_API.GetInstance().jobInfo.id + "/uploads/" +id;
        Debug.Log(PDFFileURL_final);
        UnityWebRequest webRequest = UnityWebRequest.Get(PDFFileURL_final);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) Debug.Log("EMPTY");
        }
        else
        {
            Debug.Log("Get Joblist COMPLETE");
            Debug.Log("!!URL" + PDFFileURL_final);
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + id + ".pdf", webRequest.downloadHandler.data);
            yield return new WaitUntil(() => File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + id + ".pdf"));
            DATA.POPUP();
        }
    }
    /*
    public IEnumerator POST_NQTT(int datatype, int scene_id, string data)
    {
        Debug.Log("DATA SENDING");
        string UserJsonString = DATA.SendData(datatype, scene_id, data);
        yield return new WaitUntil(() => UserJsonString != null);
        UnityWebRequest webRequest = UnityWebRequest.Post(DataSendURL, UserJsonString); ;
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Data Send COMPLETE");
        }
    }

    string MP3UploadURL = "";
    public IEnumerator POST_MP3(int scene_id)
    {
        WWWForm formData = new WWWForm();
        MP3UploadURL = MP3URL + UserInfo_API.GetInstance().jobInfo.id + "/audio-uploads";
        Debug.Log(MP3UploadURL);
        formData.AddBinaryData("upload", File.ReadAllBytes(GUIDE_API.RecordingPath + ".mp3"), "VOICE.mp3", "audio/mpeg");
        Debug.Log(GUIDE_API.RecordingPath + ".mp3");
        formData.AddField("job_id", UserInfo_API.GetInstance().jobInfo.id);
        formData.AddField("scene_id", scene_id);
        UnityWebRequest webRequest = UnityWebRequest.Post(MP3UploadURL, formData);
     //   byte[] boundary = UnityWebRequest.GenerateBoundary();
      //  string contentType = String.Concat("multipart/form-data; boundary=", Encoding.UTF8.GetString(boundary));
     //   webRequest.SetRequestHeader("Content-Type", "multipart/form-data");
        webRequest.SetRequestHeader("Authorization", Authorization);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Data Voice COMPLETE");
            File.Delete(GUIDE_API.RecordingPath + ".mp3");
        }
    }
    string JobStatusURL = "";
    public IEnumerator PUT_STATUS(int scene_id)
    {
        string UserJsonString = DATA.PUT_Status(scene_id);
        yield return new WaitUntil(() => UserJsonString != "");
        JobStatusURL = AddJobURL+"/" + UserInfo_API.GetInstance().jobInfo.id;
        Debug.Log(JobStatusURL + scene_id);
        UnityWebRequest webRequest = UnityWebRequest.Put(JobStatusURL, UserJsonString);
        Debug.Log("send");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
       webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
       webRequest.downloadHandler = new DownloadHandlerBuffer();
       webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Status Change COMPLETE");
            if (scene_id == 999)
            {

                Debug.Log("9999999");
                StartCoroutine(POST_JobExecution());
            }
        }
    }
    /*
    IEnumerator POST_JobExecution()
    {
        Debug.Log("DATA POSTING");
        string UserJsonString = DATA.POST_JobExecution();
        yield return new WaitUntil(() => UserJsonString != null);
        UnityWebRequest webRequest = UnityWebRequest.Post(JobExecutionURL, UserJsonString); ;
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Data Post COMPLETE");
     //       StartCoroutine(GoBacktoJoblist());
        }
    }
/*
    public IEnumerator GoBacktoJoblist()
    {
        DATA_API.USERINNER user = UserInfo_API.GetInstance().userInfo;
      //  DATA = null;
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 0);
    //    DATA = FindObjectOfType<DATA_API>();
        Debug.Log("!!!!!!!!!!!!!!!!JOB LIST");
        UserInfo_API.GetInstance().userInfo = user;
        yield return StartCoroutine(GET_Playerlist(1));
        UserInfo_API.GetInstance().userInfo = user;
        Debug.Log(UserInfo_API.GetInstance().userInfo.uid);
        StartCoroutine(GET_Joblist(UserInfo_API.GetInstance().playerInfo.id, 1));
        Debug.Log(UserInfo_API.GetInstance().userInfo.uid);
    }

    /*public void Nullify()
    {
        DATA = null;
        userinfo_instance = null;
        userinfo_instance = UserInfo_API.GetInstance();
        DATA = FindObjectOfType<DATA_API>();
    }

    string PdfListURL_final;
    public IEnumerator GET_PDFList(string jobid)
    {
        PdfListURL_final = PdfListURL + jobid + PdfListURL_jobid;
        Debug.Log(PdfListURL_final);
        UnityWebRequest webRequest = UnityWebRequest.Get(PdfListURL_final);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) DATA.GET_PDFList(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            int pdf_id = DATA.GET_PDFList(webRequest.downloadHandler.text);
            StartCoroutine(GET_PDFFile(jobid, pdf_id));
        }
    }
    string PDFFileURL_final;
    public IEnumerator GET_PDFFile(string jobid, int id)
    {
        PDFFileURL_final = PdfFileURl + jobid +"/uploads/" +id;
        Debug.Log(PDFFileURL_final);
        UnityWebRequest webRequest = UnityWebRequest.Get(PDFFileURL_final);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) Debug.Log("EMPTY");
        }
        else
        {
            Debug.Log("Get Joblist COMPLETE");
            Debug.Log("!!URL" + JobFinalURL);
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + id + ".pdf", webRequest.downloadHandler.data);
            yield return new WaitUntil(() => File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + id + ".pdf"));
            DATA.POPUP();
        }
    }
    */
}

