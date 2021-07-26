using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

public class JsonManager : MonoBehaviour
{    
    //public static bool isFin;
    public static bool isFirst;    
    private static JsonManager instance; // 싱글턴 인스턴스 생성 (static + 클래스명 문법으로 생성한 변수)
        
    // MonoBehaviour의 상속을 받지 않는 PlayerData 클래스(클래스안에 생성자 선언)의 List<T>를 생성한다.
    public List<PlayerData> dataList = new List<PlayerData>(); //생성자 인스턴스를 선언

    // 싱글턴 인스턴스 함수
    public static JsonManager GetInstance()
    {
        // 예외처리 1 : 인스턴스가 비어있을때 인스턴스에 JsonManager타입의 오브젝트를 찾아서 넣어준다
        if(instance == null)
        {
            instance = FindObjectOfType<JsonManager>();

            // 예외처리 2 : 그래도 비어있을때 JsonManager 게임오브젝트를 생성해 컨테이너에 넣어준다음
            //              컨테이너안에 있는 게임오브젝트에 AddComponent로 JsonManager 클래스를 넣어준다
            if(instance == null)
            {
                GameObject container = new GameObject("JsonManager");

                instance = container.AddComponent<JsonManager>();
            }
        }

        return instance; 
    }
    // ContextMenu필드는 인스펙터상에 표시된 해당 컴포넌트의 설정메뉴에 아래의 해당 함수가 작동하도록 들어간다
    [ContextMenu("To Json Data")]
    
    public void SavePlayerDataToJson(string userInfo) // Json 파일로 저장하는 함수
    {    
        // 날짜와 시간 형식의 수식을 string 포맷으로 만드는 문법 
        //fileName = string.Format("PlayerData({0:yyyy-MM-dd_hh-mm-ss-tt}).json",    
        //System.DateTime.Now);  // System.DateTime.Now는 현재 날짜, 시간을 가져오는 함수
        string fileName = string.Format("data_test.json");
        string jsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);
        //string path = Path.Combine(Application.dataPath, fileName);
        string path = Path.Combine("D:/python_test/", fileName);   
        File.WriteAllText(path, jsonData);
        Debug.Log("save complete");
    }

    [ContextMenu("From Json Data")]
    public void LoadPlayerDataFromJson(string userInfo) // Json 파일을 로드하는 함수
    {
        string fileName = string.Format("data_test.json");
        //string path = Path.Combine(Application.dataPath + "/d:/python_test/", fileName);
        string path = Path.Combine("D:/python_test/", fileName);
        string jsonData = File.ReadAllText(path);
        dataList = JsonConvert.DeserializeObject<List<PlayerData>>(jsonData);
        Debug.Log("load complete");
    }

    private void Awake()
    {
        // Scene 시작시 Safty 처리하는 If문 // 인스턴스는 널이 아님 -> 인스턴스가 자신(this)가 아닐때 셀프 파괴
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

    private void Start()
    {        
          
    }    
}

// Serializable - 인스펙터에 표시 (인스턴스로 사용되어 질때 표시되는 것으로 간주됨 / 테스트해봄)
[System.Serializable]
public class PlayerData // List<T>에 들어갈 클래스 생성
{
    // PlayerData를 다룰 리스트에 들어가 매개변수로 사용될 것들을 미리 변수로 선언해 둔다
    public int ID;
    public string DataName;
    public float Result;

    // 생성자 생성됨
    public PlayerData(int iD, string dataName, float result)
    {
        this.ID = iD;
        this.DataName = dataName;
        this.Result = result;
    }    
}
