using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;
[System.Serializable]
public class HUD_API : MonoBehaviour
{
    [SerializeField] private InputField inputID;
    [SerializeField] private InputField inputPW;
    [SerializeField] private Text Warning;
    [System.Serializable]
    public struct CanvasPack
    {
        public GameObject Canvas_LOGIN;
        public GameObject Canvas_Player;
        public GameObject Canvas_NewPlayer;
        public GameObject Canvas_Details;
        public GameObject Canvas_AddJob;
        public GameObject Canvas_Joblist;
        public GameObject Canvas_EditPlayer;
        public GameObject Canvas_DelPlayer;
    }
    [System.Serializable]
    public struct PlayerPack
    {
        public InputField player_name;
        public Text player_birth;
        public InputField player_phone;
        public InputField player_uid;
        public InputField player_PWD;
        public InputField player_PWDConfirm;
        public Toggle player_Male;
        public Toggle player_Female;
        public string player_Gender;
        public InputField player_Grade;
        public int m_nGrade;
        public GameObject Errors_Parent;
        public List<Sprite> PlayerStatus;
    }
    [System.Serializable]
    public struct EditPlayerPack
    {
        public InputField player_name;
        public Text player_birth;
        public InputField player_phone;
        public InputField player_uid;
        public InputField player_currentPWD;
        public InputField player_PWD;
        public InputField player_PWDConfirm;
        public InputField player_HospitalCode;
        public Toggle player_Male;
        public Toggle player_Female;
        public string player_Gender;
        public InputField player_Grade;
        public int m_nGrade;
        public GameObject Errors_Parent;
    }

    [System.Serializable]
    public struct JobPack
    {
        public Transform JobContent;
        public GameObject Joblist_Prefab;
        public Text JOB_CrntPg;
        public Text JOB_TtlPg;
        public int nCrntPg_Job;
        public int nTtlPg_Job;
        public Text JOB_Name;
        public Text JOB_Birth;
        public Text JOB_Phone;
        public Text JOB_Email;
        public Text JOB_Guardian;
        public Text JOB_Gender;
        public Text JOB_Grade;
        public GameObject JOB_Refresh;
        public List<Sprite> JobStatus;
        public Toggle toggle_Testmode;

        public InputField job_Name;
        public InputField job_Place;
        public GameObject job_button;
    }

    public CanvasPack login_pack = new CanvasPack();
    public PlayerPack player_pack = new PlayerPack();
    public JobPack job_pack = new JobPack();
    public EditPlayerPack editPlayer_pack = new EditPlayerPack();
    public GameObject prefab_Error;
    public GameObject PageList_Prefab;
    public GameObject PageSkipPrev;
    public GameObject PageSkipNext;
    private List<Vector3> m_nV3pagePos = new List<Vector3>() {
        new Vector3(-150,-409,0),new Vector3(-60,-409,0),new Vector3(30,-409,0),
        new Vector3(120,-409,0), new Vector3(210,-409,0)  };
    private List<GameObject> m_goPageList;
    private int m_nPageN =5;
    private int[] m_pageArr;
    private int m_nCurrentPage = 1;
    int m_nlastPage; //마지막 페이지에 있는 페이지 갯수
    int m_nPageCnt;

    public GameObject go_PopUp;
    string m_sPopup;

    public GUIDE_API APIConnet;
    private void Awake()
    {
        ManualXRControl.GetInstance().XR_AutoStarter();
    }
    private void Start()
    {
        
    }
    public Dictionary<string, object> UserLogin()
    {
        Dictionary<string, object> User = new Dictionary<string, object>();
        if (inputID.text != "" && inputPW.text != "")
        {
            User.Add("uid", inputID.text);
            User.Add("password", inputPW.text);
            return User;

        }
        else
        {
            Warning.text = "Please Insert Both ID & PW";
            return null;
        }
    }

    public void PlayerError(string errors, List<string> list_errors)
    {
        string m_sError = "";
        float m_fPos = 0.0f;
        foreach (string error in list_errors) m_sError= error;
        switch (errors)
        {
            case "password": m_fPos = player_pack.player_PWD.GetComponent<RectTransform>().localPosition.y; break;
            case "phonenum": m_fPos = player_pack.player_phone.GetComponent<RectTransform>().localPosition.y; break;
            case "password_confirmation": m_fPos = player_pack.player_PWDConfirm.GetComponent<RectTransform>().localPosition.y; break;
            case "uid": m_fPos = player_pack.player_uid.GetComponent<RectTransform>().localPosition.y; break;
        }
        GameObject go_Error = Instantiate(prefab_Error);
        go_Error.transform.parent = player_pack.Errors_Parent.transform;
        go_Error.GetComponent<RectTransform>().localPosition = new Vector3(221f, m_fPos, prefab_Error.GetComponent<RectTransform>().localPosition.z);
        go_Error.GetComponent<Text>().text = m_sError;

    }
    public void EditPlayerError(string errors, List<string> list_errors)
    {
        string m_sError = "";
        float m_fPos = 0.0f;
        foreach (string error in list_errors) m_sError += error;
        switch (errors)
        {
            case "password": m_fPos = editPlayer_pack.player_PWD.GetComponent<RectTransform>().localPosition.y; break;
            case "phonenum": m_fPos = editPlayer_pack.player_phone.GetComponent<RectTransform>().localPosition.y; break;
            case "current_password": m_fPos = editPlayer_pack.player_currentPWD.GetComponent<RectTransform>().localPosition.y; break;
            case "password_confirmation": m_fPos = editPlayer_pack.player_PWDConfirm.GetComponent<RectTransform>().localPosition.y; break;
            case "uid": m_fPos = editPlayer_pack.player_uid.GetComponent<RectTransform>().localPosition.y; break;
        }
        GameObject go_Error = Instantiate(prefab_Error);
        go_Error.transform.parent = editPlayer_pack.Errors_Parent.transform;
        go_Error.GetComponent<RectTransform>().localPosition= new Vector3(221f, m_fPos, prefab_Error.GetComponent<RectTransform>().localPosition.z);
        go_Error.GetComponent<Text>().text = m_sError;

    }
    public void SignInComplete()
    {
        PopUP("PlayerAdd");
        ShowLogin();


    }
    public void ResetList(string content)
    {
        Transform parentTransform = null;
        switch (content) {
            case "JOB": parentTransform = job_pack.JobContent; break;
        }
        Debug.Log(parentTransform);
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }
    
    
    /*
    void PageListCreate() //처음 시작하면 항상 1에서 시작
    {
        int m_nPage = m_nCurrentPage -1;
        Debug.Log("현재 페이지" + m_nCurrentPage);
        int m_firstPage = (m_nPageN * m_nCurrentPage) + 1;
        for(int i =0; i< m_pageArr[m_nPage]; i++) //해당 리스트[i]에 있는 페이지 갯수
        {
            CreatePageList(m_nV3pagePos[i], m_firstPage);
            m_firstPage++;
        }
        GetPage();
    }
    void GetPage()
    {
        bool prev;
        bool next;
        if(m_nCurrentPage == 1)prev = false;
        else prev = true;
        if ( m_nCurrentPage == m_nPageCnt) next = false; //m_nlastPage == 0 ||
        else next = true;
        PageSkipActive(prev, next);
    }
    void PageSkipActive(bool prev, bool next)
    {
        PageSkipPrev.SetActive(prev);
        PageSkipNext.SetActive(next);
    }

    void CreatePageList(Vector3 v3Pos, int pageN)
    {
        GameObject m_goPage = Instantiate(PageList_Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        m_goPage.transform.SetParent(PlayerContent, false);
        m_goPage.transform.GetComponent<RectTransform>().position = v3Pos;
        m_goPage.name = pageN.ToString();
        m_goPage.GetComponentInChildren<Text>().text = pageN.ToString();
        m_goPageList.Add(m_goPage);


    }
    public void NextPage()
    {
        m_nCurrentPage += 1;
        foreach (GameObject go in m_goPageList) Destroy(go);
        PageListCreate();
    }
    public void PrevPage()
    {
        m_nCurrentPage -= 1;
        foreach (GameObject go in m_goPageList) Destroy(go);
        PageListCreate();
    }
    */

    public void GET_JOBLIST()//create detailed user info in job list screen
    {
        DATA_API.UserInner playerinfo = UserInfo_API.GetInstance().UserTotalInfo.user;
        Debug.Log("+++HUD" + job_pack.JOB_Name.gameObject.name + "::::" + playerinfo.id);
        job_pack.JOB_Name.text= playerinfo.name;
        job_pack.JOB_Birth.text = playerinfo.birth;
        job_pack.JOB_Phone.text = playerinfo.phonenum;
        job_pack.JOB_Email.text = playerinfo.uid;
        Debug.Log(playerinfo.gender);
        job_pack.JOB_Gender.text = playerinfo.gender == 1 ? "M" : "F";
        var birthYear = playerinfo.birth.Split("-"[0]);
        Debug.Log(birthYear[0]);
        string currentYear = System.DateTime.Now.ToString("yyyy");
        int grade = int.Parse(currentYear)- int.Parse(birthYear[0]);
        string grade_s = "NO";
        if (grade > 6 && grade < 10) grade_s = "L";
        if (grade > 9 && grade < 13) grade_s = "H";
        job_pack.JOB_Grade.text = grade_s;
        job_pack.JOB_Refresh.name = playerinfo.id.ToString();

    }
    public void CreateJobList(DATA_API.JobData JobInfo)
    {
        GameObject job = Instantiate(job_pack.Joblist_Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        job.transform.SetParent(job_pack.JobContent, false);
        job.GetComponent<Toggle>().group = job_pack.JobContent.GetComponent<ToggleGroup>();
        Debug.Log("TOGGLE");
        job.name = JobInfo.id;
        job.transform.Find("PLACE").GetComponent<Text>().text = JobInfo.place;
        Debug.Log("PLACE");
        job.transform.Find("DATE").GetComponent<Text>().text = JobInfo.updated_at;
        Debug.Log("DATE");
        Image StatusImage = job.transform.Find("STATE").GetComponent<Image>();
        Text JobStatus = job.transform.Find("Text").GetComponent<Text>();
        switch (JobInfo.status)
        {
            case 0: StatusImage.sprite = player_pack.PlayerStatus[0]; break;
            case 1070: StatusImage.sprite = player_pack.PlayerStatus[1]; JobStatus.text = "1"; break;
            case 1080: StatusImage.sprite = player_pack.PlayerStatus[1]; JobStatus.text = "2"; break;
            case 1001: StatusImage.sprite = player_pack.PlayerStatus[1]; JobStatus.text = "3"; break;
            case 1002: StatusImage.sprite = player_pack.PlayerStatus[1]; JobStatus.text = "4"; break;
            case 1003: StatusImage.sprite = player_pack.PlayerStatus[1]; JobStatus.text = "5"; break;
            case 1004: StatusImage.sprite = player_pack.PlayerStatus[1]; JobStatus.text = "6"; break;
            case 999: StatusImage.sprite = player_pack.PlayerStatus[2]; break;
            case 98: StatusImage.sprite = player_pack.PlayerStatus[3]; 
                StatusImage.rectTransform.sizeDelta = new Vector2(45, StatusImage.rectTransform.sizeDelta.y);  break;
            case 99: StatusImage.sprite = player_pack.PlayerStatus[4];
                StatusImage.rectTransform.sizeDelta = new Vector2(45, StatusImage.rectTransform.sizeDelta.y);
                StatusImage.GetComponent<Button>().enabled = true;
                break;
            
        }
        Debug.Log("STATe");
        /*
        job.transform.Find("Move").name = id.ToString();
        //  player.transform.Find("Move").GetComponent<Player_API>().APIMANAGER = APIConnet;
        Image StateImg = job.transform.Find("STATE").GetComponent<Image>();
        switch (state)
        {
            case "GOING": StateImg.sprite = StateSprite[0]; break;
            case "FINISH": StateImg.sprite = StateSprite[1]; break;
        }
        */
    }
    public void JobPage(int page) //DATA 
    {
        job_pack.nTtlPg_Job = page;
        job_pack.JOB_CrntPg.text = job_pack.nCrntPg_Job.ToString();
        job_pack.JOB_TtlPg.text = page.ToString();
    }

    void OnClick() //버튼 누르면 자동으로 세부 정보 조회로 넘어가게
    {
        APIConnet.CoroutineStart("GET_Joblist");
    }
    public void ShowLogin()
    {
        login_pack.Canvas_LOGIN.SetActive(true);
        login_pack.Canvas_Player.SetActive(false);
        login_pack.Canvas_NewPlayer.SetActive(false);
        login_pack.Canvas_Details.SetActive(false);
        login_pack.Canvas_AddJob.SetActive(false);
        login_pack.Canvas_Joblist.SetActive(false);
        login_pack.Canvas_EditPlayer.SetActive(false);
    }
    public void ShowPlayerList()
    {
        login_pack.Canvas_LOGIN.SetActive(false);
        login_pack.Canvas_NewPlayer.SetActive(false);
        login_pack.Canvas_Player.SetActive(true);
        login_pack.Canvas_Details.SetActive(false);
        login_pack.Canvas_EditPlayer.SetActive(false);

    }
    public void ShowAddPlayer()
    {
        login_pack.Canvas_LOGIN.SetActive(false);
        login_pack.Canvas_Player.SetActive(false);
        login_pack.Canvas_NewPlayer.SetActive(true);
        login_pack.Canvas_Details.SetActive(false);
        login_pack.Canvas_EditPlayer.SetActive(false);
    }

    public void ShowJobList()
    {
        login_pack.Canvas_LOGIN.SetActive(false);
        login_pack.Canvas_Player.SetActive(false);
        login_pack.Canvas_NewPlayer.SetActive(false);
        login_pack.Canvas_Details.SetActive(true);
        login_pack.Canvas_AddJob.SetActive(false);
        login_pack.Canvas_Joblist.SetActive(true);
        login_pack.Canvas_EditPlayer.SetActive(false);
        login_pack.Canvas_DelPlayer.SetActive(false);
    }
    public void ShowEditPlayer()
    {
        login_pack.Canvas_LOGIN.SetActive(false);
        login_pack.Canvas_Player.SetActive(false);
        login_pack.Canvas_NewPlayer.SetActive(false);
        login_pack.Canvas_Details.SetActive(false);
        login_pack.Canvas_AddJob.SetActive(false);
        login_pack.Canvas_Joblist.SetActive(false);
        login_pack.Canvas_EditPlayer.SetActive(true);
    }

    public Dictionary<string, string> AddNewPlayer()
    {
        if (player_pack.player_Female != null || player_pack.player_Male != null)
        {
            player_pack.player_Gender = player_pack.player_Female ? "1" : "0"; //남아면 1 여아면 2
        }
        Dictionary<string, string> NewPlayer = new Dictionary<string, string>();
        NewPlayer.Add("player_name", player_pack.player_name.text);
        NewPlayer.Add("birth", player_pack.player_birth.text);
        NewPlayer.Add("phonenum", player_pack.player_phone.text);
        string uid = player_pack.player_uid.text;
        NewPlayer.Add("uid", uid);
        NewPlayer.Add("password", player_pack.player_PWD.text);
        NewPlayer.Add("password_confirmation", player_pack.player_PWDConfirm.text);
        NewPlayer.Add("gender", player_pack.player_Gender);
        NewPlayer.Add("grade", player_pack.player_Grade.text);
        return NewPlayer;

    }
    public void EdtiPlayerOpen()
    {
        DATA_API.UserInner user = UserInfo_API.GetInstance().UserTotalInfo.user;
        editPlayer_pack.player_name.text = user.name;
        editPlayer_pack.player_birth.text = user.birth;
        editPlayer_pack.player_phone.text = user.phonenum;
        editPlayer_pack.player_uid.text = user.uid;
        editPlayer_pack.player_currentPWD.text = "";
        editPlayer_pack.player_PWD.text = "";
        editPlayer_pack.player_PWDConfirm.text = "";
        editPlayer_pack.player_HospitalCode.text = "";


    }
    
    public Dictionary<string, string> EditPlayer()
    {
        if (editPlayer_pack.player_Female != null || editPlayer_pack.player_Male != null)
        {
            editPlayer_pack.player_Gender = editPlayer_pack.player_Female ? "1" : "0"; //남아면 1 여아면 2
        }
        Dictionary<string, string> EditPlayer = new Dictionary<string, string>();
        EditPlayer.Add("player_name", editPlayer_pack.player_name.text);
        EditPlayer.Add("birth", editPlayer_pack.player_birth.text);
        EditPlayer.Add("phonenum", editPlayer_pack.player_phone.text);
        EditPlayer.Add("uid", editPlayer_pack.player_uid.text);
        EditPlayer.Add("current_password", editPlayer_pack.player_currentPWD.text);
        EditPlayer.Add("password", editPlayer_pack.player_PWD.text);
        EditPlayer.Add("password_confirmation", editPlayer_pack.player_PWDConfirm.text);
        EditPlayer.Add("gender", editPlayer_pack.player_Gender);
        EditPlayer.Add("grade", editPlayer_pack.player_Grade.text);
        EditPlayer.Add("hospital_code", editPlayer_pack.player_HospitalCode.text);
        return EditPlayer;
    }
    
    public Dictionary<string, string> AddNewJob()
    {
        Dictionary<string, string> NewJob = new Dictionary<string, string>();
        NewJob.Add("name", job_pack.job_Name.text);
        //삼성용
        if (job_pack.job_Place.text == "") { job_pack.job_Place.text = "웅진 플레이도시"; }
        NewJob.Add("place", job_pack.job_Place.text);
        NewJob.Add("player_id", job_pack.job_button.name);
        return NewJob;
    }
    public void PlusGrade()
    {
        player_pack.m_nGrade = int.Parse(player_pack.player_Grade.text);
        if (player_pack.m_nGrade >= 6) return;
        player_pack.m_nGrade = int.Parse(player_pack.player_Grade.text);
        player_pack.m_nGrade += 1;
        player_pack.player_Grade.text = player_pack.m_nGrade.ToString();
    }
    public void MinusGrade()
    {
        player_pack.m_nGrade = int.Parse(player_pack.player_Grade.text);
        if (player_pack.m_nGrade <= 1) return;
        player_pack.m_nGrade -= 1;
        player_pack.player_Grade.text = player_pack.m_nGrade.ToString();
    }

    public IEnumerator PopUP(string popup)
    {

        m_sPopup = "";
        switch (popup)
        {
            case "PDF": m_sPopup = "PDF가 생성되었습니다."; break;
            case "Playerlist": m_sPopup = "아동 목록을 불러왔습니다."; break;
            case "Joblist": m_sPopup = "진단 내역을 불러왔습니다."; break;
            case "PlayerAdd": m_sPopup = "아동이 등록되었습니다"; break;
        }
        go_PopUp.GetComponentInChildren<Text>().text = m_sPopup;
        go_PopUp.SetActive(true);
        RectTransform rect_PopUp = go_PopUp.GetComponent<RectTransform>();
     //   float f_originalY = rect_PopUp.anchoredPosition.y;
        rect_PopUp.DOLocalMoveY(440, .8f);
        yield return new WaitForSeconds(2);
        rect_PopUp.DOLocalMoveY(730, .8f);

    }
}
class PageSize
{
    public int[,] pageSizeArr;

    public PageSize(int x, int y)
    {
        this.pageSizeArr = new int[x, y];
    }
}
