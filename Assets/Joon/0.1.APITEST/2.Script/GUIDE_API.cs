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
    string BASE_URL = "http://jdi.bitzflex.com:4007";
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

    // Start is called before the first frame update


    private void Awake()
    {
        GUIDE_API.RecordingPath = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + "VOICE";

        if (Guide_instance== null)
        {
            Guide_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }
    public void Start()
    {

        SetURL();
        Authorization = "";
  //      UI_Hud = GameObject.Find("UI").GetComponent<HUD_API>();
        DATA = FindObjectOfType<DATA_API>();
        Userinfo = FindObjectOfType<UserInfo_API>();
    }

    void SetURL()
    {
        LoginURL = BASE_URL + LoginURL;
        RegistrationURL = BASE_URL + RegistrationURL;
        PlayerRegister = BASE_URL + PlayerRegister;
        PlayerURL = BASE_URL + PlayerURL;
        SearchPlayerURL = BASE_URL + SearchPlayerURL;
        JobURL = BASE_URL + JobURL;
        AddJobURL = BASE_URL + AddJobURL;
        DataSendURL = BASE_URL + DataSendURL;
        JobExecutionURL = BASE_URL + JobExecutionURL;
        MP3URL = BASE_URL + MP3URL;
        PdfListURL = BASE_URL + PdfListURL;
        PdfFileURl = BASE_URL + PdfFileURl;


    }
    public void CoroutineStart(string coroutine)
    {
        StartCoroutine(coroutine);
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
            Authorization = DATA.GET_Login(webRequest.downloadHandler.text);
            StartCoroutine(GET_Registration());
        }

    }

    IEnumerator GET_Registration() //서비스 가입 목록 조회
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(RegistrationURL);

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
                StartCoroutine(POST_Signin());
            }
        }
        else
        {
            Debug.Log("Registration COMPLETE");
            Debug.Log(webRequest.downloadHandler.text);
            bool complete = DATA.GET_Registration(webRequest.downloadHandler.text);
            StartCoroutine(GET_Playerlist(1));

        }
    }
    


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


    public IEnumerator GET_Playerlist(int page)
    {
        string FinalPlayerURL = PlayerURL + page + PlayerURL_perPage;
        Debug.Log(FinalPlayerURL);
        UnityWebRequest webRequest = UnityWebRequest.Get(FinalPlayerURL);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) DATA.GET_Playerlist(webRequest.downloadHandler.text,false);
        }
        else
        {
            Debug.Log("Get Playerlist COMPLETE");
            Debug.Log(webRequest.downloadHandler.text);
            DATA.GET_Playerlist(webRequest.downloadHandler.text,true);

        }
    }
  
    public IEnumerator POST_AddPlayer()
    {
        string UserJsonString = DATA.POST_AddPlayer();
        yield return new WaitUntil(() => UserJsonString != null);
        Debug.Log(UserJsonString);
        UnityWebRequest webRequest = UnityWebRequest.Post(PlayerURL, UserJsonString);
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
            Debug.Log("Add Player COMPLETE");
            StartCoroutine(POST_RegisterPlayer(webRequest.downloadHandler.text));
        }

    }

    int m_playerIndex = 0;
    string RegisterPlayerURL;
    IEnumerator POST_RegisterPlayer(string webResult)
    {
        m_playerIndex = DATA.RegisterPlayer(webResult);
        yield return new WaitUntil(() => m_playerIndex != 0);
        Debug.Log(m_playerIndex);
        RegisterPlayerURL = PlayerRegister + "/" + m_playerIndex + "/choose";
        Debug.Log(PlayerURL);
        UnityWebRequest webRequest = UnityWebRequest.Post(RegisterPlayerURL, webResult);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(webResult);
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
            Debug.Log("Post Playerlist COMPLETE");
            StartCoroutine(GET_Playerlist(1));
        }
    }

    string SearchPlayerURL_final;
    public IEnumerator GET_SearchPlayer(string name)
    {
        SearchPlayerURL_final = SearchPlayerURL + name;
        Debug.Log(SearchPlayerURL_final);
        UnityWebRequest webRequest = UnityWebRequest.Get(SearchPlayerURL_final);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) DATA.GET_Playerlist(webRequest.downloadHandler.text, false);
        }
        else
        {
            Debug.Log("Get Playerlist COMPLETE");
            Debug.Log(webRequest.downloadHandler.text);
            DATA.GET_Playerlist(webRequest.downloadHandler.text, false);
        }

    }
    
    string JobFinalURL;
    public IEnumerator GET_Joblist(int id,int page)
    {
        JobFinalURL = JobURL +page + JobURL_perPage + id;
        Debug.Log(JobFinalURL);
        UnityWebRequest webRequest = UnityWebRequest.Get(JobFinalURL);

        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", Authorization);
        Debug.Log(Authorization);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            int m_nError = DATA.ERROR_CONTROLLER(webRequest.downloadHandler.text);
            if (m_nError == 404) DATA.GET_Joblist(webRequest.downloadHandler.text, id);
        }
        else
        {
            Debug.Log("Get Joblist COMPLETE");
            Debug.Log("!!URL" + JobFinalURL);
            DATA.GET_Joblist(webRequest.downloadHandler.text, id);
        }
    }

    public IEnumerator POST_Addjob(int id, int page)
    {
        string UserJsonString = DATA.POST_AddJob(id);
        yield return new WaitUntil(() => UserJsonString != null);
        Debug.Log(UserJsonString);
        UnityWebRequest webRequest = UnityWebRequest.Post(AddJobURL, UserJsonString); ;
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
            StartCoroutine(GET_Joblist(id, page));
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
            StartCoroutine(GoBacktoJoblist());
        }
    }

    public IEnumerator GoBacktoJoblist()
    {
        DATA = null;
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 0);
        DATA = FindObjectOfType<DATA_API>();
        Debug.Log(DATA.gameObject.name);
        Debug.Log("!!!!!!!!!!!!!!!!JOB LIST");
        yield return StartCoroutine(GET_Playerlist(1));
        StartCoroutine(GET_Joblist(UserInfo_API.GetInstance().playerInfo.id, 1));
    }

    /*public void Nullify()
    {
        DATA = null;
        userinfo_instance = null;
        userinfo_instance = UserInfo_API.GetInstance();
        DATA = FindObjectOfType<DATA_API>();
    }
    */
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
}

