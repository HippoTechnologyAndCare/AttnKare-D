using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class GUIDE_API : MonoBehaviour
{
    public static string RecordingPath;
    public static GUIDE_API Guide_instance;
    //    HUD_API UI_Hud;
    DATA_API DATA;
    UserInfo_API Userinfo;
    // string BASE_URL = "https://adhd.hippotnc.kr:444";
    public static int service_type = 5944; 
    /*
     * 웅진 : 2761
     * 삼성 : 5944
     */
    string BASE_URL = "http://jdi.bitzflex.com:4007";
    string SigninURL = "/api/v2/users"; //계정 생성(POST) + 상세조회(GET)
    string LoginURL = "/api/v2/session";
    string RenewURL = "/api/v2/session/renew";
    string ServicesURL = "/api/v2/services?type="; //서비스 목록 조회
    string ServicesSubsURL = "/api/v2/subscriptions"; //사용자 구독 서비스 목록 조회(GET) + 사용자 서비스 구독(POST)
    string JobURL = ""; //servicesubsurl + subs_id + joburl
    string JobBottomURL = "/jobs?sort_order=desc&sort_by=inserted_at&page=1&per_page=5";
    string AddJobURL = "";
    string AddJobBottomURL = "/jobs"; //servicessubsurl + subid + joburl
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
        RenewURL = BASE_URL + RenewURL;
        ServicesURL = BASE_URL + ServicesURL + service_type;
        ServicesSubsURL = BASE_URL + ServicesSubsURL;
        JobURL = ServicesSubsURL + "/";
        AddJobURL = ServicesSubsURL + "/";
        DataSendURL = BASE_URL + DataSendURL;

     

    }
    public void CoroutineStart(string coroutine)
    {
        StopAllCoroutines();
        StartCoroutine(coroutine);
    }

  
    public IEnumerator POST_Renew()
    {
        Debug.Log("RENEWed");
        UnityWebRequest webRequest = UnityWebRequest.Post(RenewURL,"");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.renewal_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("RENEW " +webRequest.downloadHandler.text);
            DATA.GET_Token(webRequest.downloadHandler.text);
        }
    }
    public IEnumerator POST_Signin()
    {
        string UserJsonString = DATA.POST_Signin();
        yield return new WaitUntil(() => UserJsonString != null);
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
            DATA.PlayerError(webRequest.downloadHandler.text, true);
        }
        else
        {
            Debug.Log("POST_Signin COMPLETE");
            DATA.SignInComplete();
            DATA.GET_Token(webRequest.downloadHandler.text);
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
            Debug.Log(webRequest.downloadHandler.text);
            if (webRequest.error == "HTTP/1.1 500 Internal Server Error")
            {
                DATA.POPUP("Invalid");
            }
        }
        else
        {
            Debug.Log("POST_Login COMPLETE");
            DATA.GET_Token(webRequest.downloadHandler.text);
            StartCoroutine(GET_UserInfo());
        }

    }

    public IEnumerator DEL_Login()
    {
        UnityWebRequest webRequest = UnityWebRequest.Delete(LoginURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Log_out");
            DATA.LogOut();
        }
    }

    public IEnumerator GET_UserInfo()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(SigninURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        Debug.Log(UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("if");
            string m_sError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_sError == "not_found") //list is empty error (just show empty list)
            {
                Debug.Log("error");
                //StartCoroutine(POST_Signin());
            }
        }
        else
        {
            Debug.Log("GET_UserInfo COMPLETE");
            Debug.Log(webRequest.downloadHandler.text);
            DATA.GET_UserInfo(webRequest.downloadHandler.text);
            StartCoroutine(GET_ServicesSubs());
        }
    }
    public IEnumerator PUT_UserInfo()
    {
        string UserJsonString = DATA.PUT_UserInfo();
        yield return new WaitUntil(() => UserJsonString != "");
        UnityWebRequest webRequest = UnityWebRequest.Put(SigninURL, UserJsonString);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            DATA.PlayerError(webRequest.downloadHandler.text, false);
        }
        else
        {
            Debug.Log("EDIT DONE");
            DATA.LogOut();
        }
    }
    public IEnumerator DEL_UserInfo()
    {
        UnityWebRequest webRequest = UnityWebRequest.Delete(SigninURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("DELETED");
            DATA.LogOut();


        }
    }

    IEnumerator GET_ServicesSubs()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(ServicesSubsURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        Debug.Log(UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("SERVICE SUB ERROR" + webRequest.downloadHandler.text);
            Debug.Log("SERVICE SUB ERROR2" + webRequest.error);
            string m_sError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_sError == "not_found") //list is empty error (just show empty list)
            {
                Debug.Log("error");
                StartCoroutine(GET_Services());
            }
        }
        else
        {
            Debug.Log("GET_ServiceSubs COMPLETE");
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
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        Debug.Log(UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("ERROR");
            Debug.Log(webRequest.downloadHandler.text);
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("GET_Services COMPLETE");
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
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("POST_ServiceSubs COMPLETE");
            DATA.GET_SubsID(webRequest.downloadHandler.text);
            StartCoroutine(GET_ServicesSubs());


        }
    }

 
    public IEnumerator GET_Joblist()
    {
        string JOBFinalURL = JobURL + UserInfo_API.GetInstance().UserTotalInfo.id + JobBottomURL;
        Debug.Log(JOBFinalURL);
        UnityWebRequest webRequest = UnityWebRequest.Get(JOBFinalURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        Debug.Log(UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
            Debug.Log(webRequest.downloadHandler.text);
       //     int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            Dictionary<string, string> m_dictError = JsonConvert.DeserializeObject<Dictionary<string, string>>(webRequest.downloadHandler.text);
            string error_code = m_dictError["code"];
            Debug.Log("ERROR CODE++" + error_code);
            /*
             * 여기 list 비어있으면  not_found 코드로 감
             */
         //   if(error_code == "not_authenticated") { yield return StartCoroutine(POST_Renew()); CoroutineStart("GET_Joblist"); }
            if (error_code == "not_found") DATA.GET_Joblist(webRequest.downloadHandler.text);
        }
        else
        {/*
            Debug.Log(webRequest.downloadHandler.text);
            string[] m_arrRequest = webRequest.downloadHandler.text.Split(new char[] { '{', '}', ',', ':' });
            if (m_arrRequest[0] == "error")
            {
                Dictionary<string, string> m_dictError = JsonConvert.DeserializeObject<Dictionary<string, string>>(webRequest.downloadHandler.text);
                string error_code = m_dictError["code"];
                if (error_code == "not_authenticated") { Debug.Log("RENEW"); yield return StartCoroutine(POST_Renew()); CoroutineStart("POST_Addjob"); }
            }
            */
            Debug.Log("Get Joblist COMPLETE");
            DATA.GET_Joblist(webRequest.downloadHandler.text);
        }
    }


    public IEnumerator POST_Addjob()
    {
        string UserJsonString = DATA.POST_AddJob();
        yield return new WaitUntil(() => UserJsonString != null);
        Debug.Log(UserJsonString);
        string AddJobFinalURL = AddJobURL + UserInfo_API.GetInstance().UserTotalInfo.id + AddJobBottomURL;
        UnityWebRequest webRequest = UnityWebRequest.Post(AddJobFinalURL, UserJsonString); ;
        byte[] jsonToSend = new UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            Debug.Log(webRequest.error);
           
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            string[] m_arrRequest = webRequest.downloadHandler.text.Split(new string[] { "{","}",",",":"," ","\n","\r\n","\""}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < m_arrRequest.Length; i++) { Debug.Log("ERROR " + i + " " + m_arrRequest[i]); }
            if (m_arrRequest[0] == "error")
            {
                Dictionary<string, Dictionary<string, string>> m_dictError = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(webRequest.downloadHandler.text);
                string error_code = m_dictError["error"]["code"];
                if (error_code == "not_authenticated") { Debug.Log("RENEW"); yield return StartCoroutine(POST_Renew()); CoroutineStart("POST_Addjob"); }
            }
            
            Debug.Log("Add Job COMPLETE");
            StartCoroutine(GET_Joblist());
        }
    }

    public IEnumerator POST_NQTT(int datatype, int scene_id, List<List<object>> data)
    {
        Debug.Log("DATA SENDING");
        string UserJsonString = DATA.SendData(datatype, scene_id, data);
        yield return new WaitUntil(() => UserJsonString != null);
        UnityWebRequest webRequest = UnityWebRequest.Post(DataSendURL, UserJsonString); ;
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
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
    public IEnumerator POST_NQTT_JSON(int datatype, int scene_id, Dictionary<string, PlayerJsonData> data)
    {
        Debug.Log("DATA SENDING");
        string UserJsonString = DATA.SendJson(datatype, scene_id, data);
        Debug.Log("DATA AGAIN =" + UserJsonString);
        yield return new WaitUntil(() => UserJsonString != null);
        Debug.Log("DATA  =" + UserJsonString);
        UnityWebRequest webRequest = UnityWebRequest.Post(DataSendURL, UserJsonString); ;
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
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

    public IEnumerator DEL_Joblist()
    {
        string JOBDelURL = JobURL + UserInfo_API.GetInstance().UserTotalInfo.id + "/jobs/" + UserInfo_API.GetInstance().jobInfo.id;
        Debug.Log(JOBDelURL);
        UnityWebRequest webRequest = UnityWebRequest.Delete(JOBDelURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        Debug.Log(UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
            Debug.Log(webRequest.downloadHandler.text);
            //     int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            Dictionary<string, string> m_dictError = JsonConvert.DeserializeObject<Dictionary<string, string>>(webRequest.downloadHandler.text);
            string error_code = m_dictError["code"];
            Debug.Log("ERROR CODE++" + error_code);
            /*
             * 여기 list 비어있으면  not_found 코드로 감
             */
            if (error_code == "not_found") DATA.GET_Joblist(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Delete Joblist COMPLETE");
            StartCoroutine(GET_Joblist());
        }
    }
    public IEnumerator POST_MP3(int scene_id)
    {
        WWWForm formData = new WWWForm();
        MP3URL = AddJobURL + UserInfo_API.GetInstance().UserTotalInfo.id + AddJobBottomURL +"/"+ UserInfo_API.GetInstance().jobInfo.id + "/attn/audio-uploads";
        Debug.Log(MP3URL);
        formData.AddBinaryData("upload", File.ReadAllBytes(GUIDE_API.RecordingPath + ".mp3"), "VOICE.mp3", "audio/mpeg");
        Debug.Log(GUIDE_API.RecordingPath + ".mp3");
        formData.AddField("job_id", UserInfo_API.GetInstance().jobInfo.id);
        formData.AddField("scene_id", scene_id);
        UnityWebRequest webRequest = UnityWebRequest.Post(MP3URL, formData);
        //   byte[] boundary = UnityWebRequest.GenerateBoundary();
        //  string contentType = String.Concat("multipart/form-data; boundary=", Encoding.UTF8.GetString(boundary));
        //   webRequest.SetRequestHeader("Content-Type", "multipart/form-data");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
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
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
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
    IEnumerator POST_JobExecution()
    {
        Debug.Log("DATA POSTING");
        string UserJsonString = DATA.POST_JobExecution();
        yield return new WaitUntil(() => UserJsonString != null);
        string FinalJobExecution = AddJobURL + UserInfo_API.GetInstance().UserTotalInfo.id + AddJobBottomURL + "/" + UserInfo_API.GetInstance().jobInfo.id + JobExecutionURL;
        Debug.Log(FinalJobExecution);
        UnityWebRequest webRequest = UnityWebRequest.Post(FinalJobExecution, UserJsonString); ;
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserJsonString);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
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
    public IEnumerator GET_PDFList()
    {
        string PdfListURL_final = JobURL + UserInfo_API.GetInstance().UserTotalInfo.id +"/jobs/" +UserInfo_API.GetInstance().jobInfo.id + PdfListURL;
        Debug.Log(PdfListURL_final +"///"+ UserInfo_API.GetInstance().UserTotalInfo.id);
        UnityWebRequest webRequest = UnityWebRequest.Get(PdfListURL_final);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        Debug.Log(UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            string m_sError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_sError == "not_found") DATA.GET_PDFList(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            int pdf_id = DATA.GET_PDFList(webRequest.downloadHandler.text);
            StartCoroutine(GET_PDFFile(pdf_id));
        }
    }
    public IEnumerator GET_PDFFile(int id)
    {
        string PDFFileURL_final = JobURL + UserInfo_API.GetInstance().UserTotalInfo.id + "/jobs/" + UserInfo_API.GetInstance().jobInfo.id + "/attn/uploads/" +id;
        Debug.Log(PDFFileURL_final);
        UnityWebRequest webRequest = UnityWebRequest.Get(PDFFileURL_final);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", UserInfo_API.GetInstance().Token.access_token);
        Debug.Log(UserInfo_API.GetInstance().Token.access_token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            string m_sError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_sError == "not_found") Debug.Log("EMPTY");
        }
        else
        {
            Debug.Log("Get Joblist COMPLETE");
            Debug.Log("!!URL" + PDFFileURL_final);
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + id + ".pdf", webRequest.downloadHandler.data);
            yield return new WaitUntil(() => File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + UserInfo_API.GetInstance().UserTotalInfo.user.name + ".pdf"));
            DATA.POPUP("PDF");
        }
    }
   
}

