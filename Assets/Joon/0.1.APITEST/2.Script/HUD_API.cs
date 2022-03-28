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

    public GameObject Canvas_LOGIN;
    public GameObject Canvas_Player;
    public GameObject Canvas_NewPlayer;
    public GameObject Canvas_Joblist;
    public GameObject Canvas_AddJob;
    public GameObject Canvas_JobLIst;
    public InputField player_name;
    public Text txtCrntpage;
    public int nCrntPgeN =1;
    public Text txtTtlPgeN;
    public int nTtlPgeN;
    public Text player_birth;
    public InputField player_phone;
    public InputField player_email;
    public InputField player_EmailDomain;
    public InputField player_guardian;
    public Toggle player_Male;
    public Toggle player_Female;
    string player_Gender;
    public InputField player_Grade;
    private int m_nGrade;
    public InputField searchbyName;
    public GameObject Error_NameOverlap;
    public Transform PlayerContent;
    public GameObject PlayerList_Prefab;

    public List<Sprite> PlayerStatus;

    public Transform JobContent;
    public GameObject Joblist_Prefab;
    public Text JOB_CrntPg;
    public Text JOB_TtlPg;
    public int nCrntPg_Job =1;
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


    public InputField job_Name;
    public InputField job_Place;
    public GameObject job_button;

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

    public void OverlapPlayer()
    {
        Error_NameOverlap.SetActive(true);
    }
    public string SearchbyName()
    {
        if (searchbyName.text != "")
        {
            Debug.Log(searchbyName.text);
            return searchbyName.text;
        }
        else return null;
    }
    public void ResetSearchbyName()
    {
        searchbyName.text = "";
    }
    public void ResetList(string content)
    {
        Transform parentTransform = null;
        switch (content) {
            case "PLAYER": parentTransform = PlayerContent; break;
            case "JOB": parentTransform = JobContent; break;
        }

        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }
    public void CreateList(DATA_API.PlayerInner playerinfo , string state)
    {
        GameObject player = Instantiate(PlayerList_Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        player.transform.SetParent(PlayerContent, false);
        player.transform.Find("NAME").GetComponent<Text>().text = playerinfo.player_name;
        player.transform.Find("GENDER").GetComponent<Text>().text = playerinfo.gender;
        player.transform.Find("GRADE").GetComponent<Text>().text = playerinfo.grade + "학년";
        player.transform.Find("PHONE").GetComponent<Text>().text = playerinfo.phonenum;

        player.transform.Find("Move").name = playerinfo.id.ToString();
      //  player.transform.Find("Move").GetComponent<Player_API>().APIMANAGER = APIConnet;
      //  Image StateImg = player.transform.Find("STATE").GetComponent<Image>();
      //  switch (state)
       // {
       //     case "GOING":  StateImg.sprite = PlayerStatus[0]; break;
      //      case "FINISH": StateImg.sprite = PlayerStatus[1]; break;
       // }
    }
    public void PlayerPage(int page) //DATA 
    {
        /*
        Debug.Log("총 인원수 " + page);
        m_nPageCnt = (page / m_nPageN)+1; //5개씩 나오는 페이지 리스트 갯수
        Debug.Log("페이지 리스트 갯수" + m_nPageCnt);
        m_nlastPage = page % m_nPageN;
        Debug.Log("마지막 리스트 페이지 갯수" + m_nlastPage);
        for(int i = 0; i < m_nPageCnt; i++)
        {
            if(i== m_nPageCnt - 1) { m_pageArr[i] = m_nlastPage; }
            else { m_pageArr[i] = m_nPageN; }
        }
        PageListCreate();
        */
        nTtlPgeN = page;
        txtCrntpage.text = nCrntPgeN.ToString();
        txtTtlPgeN.text = page.ToString();
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

    public void GET_JOBLIST(DATA_API.PlayerInner playerinfo)//create detailed user info in job list screen
    {
        Debug.Log("+++HUD" + JOB_Name.gameObject.name + "::::" + playerinfo.id);
        JOB_Name.text= playerinfo.player_name;
        JOB_Birth.text = playerinfo.birth;
        JOB_Phone.text = playerinfo.phonenum;
        JOB_Email.text = playerinfo.email;
        JOB_Guardian.text = playerinfo.guardian_name;
        JOB_Gender.text = playerinfo.gender;
        JOB_Grade.text = playerinfo.grade.ToString();
        JOB_Refresh.name = playerinfo.id.ToString();

    }
    public void CreateJobList(DATA_API.JobData JobInfo)
    {
        GameObject job = Instantiate(Joblist_Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        job.transform.SetParent(JobContent, false);
        job.GetComponent<Toggle>().group = JobContent.GetComponent<ToggleGroup>();
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
            case 0: StatusImage.sprite = PlayerStatus[0]; break;
            case 1070: StatusImage.sprite = PlayerStatus[1]; JobStatus.text = "1"; break;
            case 1080: StatusImage.sprite = PlayerStatus[1]; JobStatus.text = "2"; break;
            case 1001: StatusImage.sprite = PlayerStatus[1]; JobStatus.text = "3"; break;
            case 1002: StatusImage.sprite = PlayerStatus[1]; JobStatus.text = "4"; break;
            case 1003: StatusImage.sprite = PlayerStatus[1]; JobStatus.text = "5"; break;
            case 1004: StatusImage.sprite = PlayerStatus[1]; JobStatus.text = "6"; break;
            case 999: StatusImage.sprite = PlayerStatus[2]; break;
            case 98: StatusImage.sprite = PlayerStatus[3]; 
                StatusImage.rectTransform.sizeDelta = new Vector2(45, StatusImage.rectTransform.sizeDelta.y);  break;
            case 99: StatusImage.sprite = PlayerStatus[4];
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
        nTtlPg_Job = page;
        JOB_CrntPg.text = nCrntPg_Job.ToString();
        JOB_TtlPg.text = page.ToString();
    }

    void OnClick() //버튼 누르면 자동으로 세부 정보 조회로 넘어가게
    {
        APIConnet.CoroutineStart("GET_Joblist");
    }
    public void ShowPlayerList()
    {
        Canvas_LOGIN.SetActive(false);
        Canvas_NewPlayer.SetActive(false);
        Canvas_Player.SetActive(true);
        Canvas_Joblist.SetActive(false);

    }

    public void ShowAddPlayer()
    {
        Canvas_LOGIN.SetActive(false);
        Canvas_Player.SetActive(false);
        Canvas_NewPlayer.SetActive(true);
        Canvas_Joblist.SetActive(false);
    }

    public void ShowJobList()
    {
        Canvas_LOGIN.SetActive(false);
        Canvas_Player.SetActive(false);
        Canvas_NewPlayer.SetActive(false);
        Canvas_Joblist.SetActive(true);
        Canvas_AddJob.SetActive(false);
        Canvas_JobLIst.SetActive(true);

    }
    public Dictionary<string, string> AddNewPlayer()
    {
        Error_NameOverlap.SetActive(false);
        if (player_Female != null || player_Male != null)
        {
            player_Gender = player_Female ? "F" : "M";
        }
        Dictionary<string, string> NewPlayer = new Dictionary<string, string>();
        NewPlayer.Add("player_name", player_name.text);
        NewPlayer.Add("birth", player_birth.text);
        Debug.Log(player_birth.text);
        NewPlayer.Add("phonenum", player_phone.text);
        string email = player_email.text + "@" + player_EmailDomain.text;
        NewPlayer.Add("email", email);
        NewPlayer.Add("guardian_name", player_guardian.text);
        NewPlayer.Add("gender", player_Gender);
        NewPlayer.Add("grade", player_Grade.text);
        return NewPlayer;

    }
    public Dictionary<string, string> AddNewJob()
    {
        Dictionary<string, string> NewJob = new Dictionary<string, string>();
        NewJob.Add("name", job_Name.text);
        NewJob.Add("place", job_Place.text);
        NewJob.Add("player_id", job_button.name);
        return NewJob;
    }
    public void PlusGrade()
    {
        m_nGrade = int.Parse(player_Grade.text);
        if (m_nGrade >= 6) return;
        m_nGrade = int.Parse(player_Grade.text);
        m_nGrade += 1;
        player_Grade.text = m_nGrade.ToString();
    }
    public void MinusGrade()
    {
        m_nGrade = int.Parse(player_Grade.text);
        if (m_nGrade <= 1) return;
        m_nGrade -= 1;
        player_Grade.text = m_nGrade.ToString();
    }

    public IEnumerator PopUP(string popup)
    {

        m_sPopup = "";
        switch (popup)
        {
            case "PDF": m_sPopup = "PDF가 생성되었습니다."; break;
            case "Playerlist": m_sPopup = "아동 목록을 불러왔습니다."; break;
            case "Joblist": m_sPopup = "진단 내역을 불러왔습니다."; break;
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
