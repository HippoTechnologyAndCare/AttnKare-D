﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace UserData
{
    public class DataManager : MonoBehaviour
    {
        public bool isPlayed;
        //public string userInformation;
        //public string userGrade;
        public bool isError_DN;
        public bool isError_UP;

        private string es3APIKey;
        private string folderPath;

        private static DataManager instance; // 싱글턴 인스턴스 생성 (static + 클래스명 문법으로 생성한 변수)

        //PlayerData 클래스(클래스안에 생성자 선언)의 List<T>를 생성한다.
        public List<PlayerData> dataList = new List<PlayerData>(); //생성자 인스턴스를 선언

        //UserInfo 클래스의 생성자를 인스턴스화
        public UserInfo userInfo = new UserInfo();

        // 싱글턴 인스턴스 함수
        public static DataManager GetInstance()
        {
            // 예외처리 1 : 인스턴스가 비어있을때 인스턴스에 DataManager타입의 오브젝트를 찾아서 넣어준다
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();

                // 예외처리 2 : 그래도 비어있을때 DataManager 게임오브젝트를 생성해 컨테이너에 넣어준다음
                //              컨테이너안에 있는 게임오브젝트에 AddComponent로 DataManager 클래스를 넣어준다
                if (instance == null)
                {
                    GameObject container = new GameObject("DataManager");

                    instance = container.AddComponent<DataManager>();
                }
            }

            return instance;
        }
        // ContextMenu필드는 인스펙터상에 표시된 해당 컴포넌트의 설정메뉴에 아래의 해당 함수가 작동하도록 들어간다
        [ContextMenu("To Json Data")]

        public void SavePlayerDataToJson() // Json 파일로 저장하는 함수
        {
            // 날짜와 시간 형식의 수식을 string 포맷으로 만드는 문법 
            //fileName = string.Format("PlayerData({0:yyyy-MM-dd_hh-mm-ss-tt}).json",    
            //System.DateTime.Now);  // System.DateTime.Now는 현재 날짜, 시간을 가져오는 함수

            if (userInfo.CollectAllInfo == "")
            {
                userInfo.CollectAllInfo = "data_test";
            }
            string fileName = userInfo.CollectAllInfo + ".json";
            string jsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, jsonData);
            //File.WriteAllText(Application.persistentDataPath + path, jsonData);
            Debug.Log("save complete");
        }

        [ContextMenu("From Json Data")]
        public void LoadPlayerDataFromJson() // Json 파일을 로드하는 함수
        {
            string fileName = userInfo.CollectAllInfo + ".json";
            //string path = Path.Combine(Application.dataPath + "/d:/python_test/", fileName);
            string path = Path.Combine("D:/python_test/", fileName);
            string jsonData = File.ReadAllText(path);
            dataList = JsonConvert.DeserializeObject<List<PlayerData>>(jsonData);
            Debug.Log("load complete");
        }

        public IEnumerator UploadRoutine()
        {
            // Create a new ES3Cloud object with the URL to our ES3.php file.
            var cloud = new ES3Cloud("https://hippotnc.synology.me:6001/ES3Cloud.php", es3APIKey);

            // Upload another local file, but make it global for all users.                
            yield return StartCoroutine(cloud.UploadFile(userInfo.CollectAllInfo + ".json"));
            Debug.Log("userInfo : " + userInfo.CollectAllInfo);

            if (cloud.isError)
            {
                isError_UP = true;
                Debug.LogError(cloud.error);
                Debug.Log("Upload Failed");
            }

            else
                Debug.Log("Uploaded");
        }

        public IEnumerator DownloadRoutine()
        {
            // Create a new ES3Cloud object with the URL to our ES3.php file.
            var cloud = new ES3Cloud("https://hippotnc.synology.me:6001/ES3Cloud.php", es3APIKey);

            // Upload another local file, but make it global for all users.                
            yield return StartCoroutine(cloud.DownloadFile(userInfo.CollectAllInfo + ".json"));
            Debug.Log("userInfo : " + userInfo.CollectAllInfo);

            if (cloud.isError)
            {
                isError_DN = true;
                DN_ErrorCheck(isError_DN);
                Debug.Log(isError_DN);
                Debug.LogError(cloud.error);
                Debug.Log("Download Failed");
            }

            else
                Debug.Log("Downloaded");
        }

        public bool DN_ErrorCheck(bool isError_DN)
        {
            if (isError_DN)
            {
                Debug.Log("다운로드에 실패했습니다...업로드 시도중...");
            }
            return isError_DN;
        }

        private void Awake()
        {
            // Scene 시작시 Safty 처리하는 If문 // 인스턴스는 널이 아닌 경우에 한해 -> 인스턴스가 자신(this)이 아닐때 셀프 파괴
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
            es3APIKey = "13de814c5d55";
            //folderPath = "/json";        
        }
    }

    // Serializable - 인스펙터에 표시 (직렬화)
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

    [System.Serializable]
    public class UserInfo
    {
        public int _Age;
        public string Name;
        public string _Gender;
        public string Grade;
        public string PhoneNumer;       // <<<< ---------------------- 오타났어요 Number

        private string _CollectAllInfo;

        private bool _IsHigh;
        private bool _IsLow;
        private bool _IsBoy;
        private bool _IsGirl;

        //public UserInfo(string name, int age, string phoneNumber, string gender, string grade)
        //{
        //    this.Name = name;
        //    this._Age = age;
        //    this.PhoneNumer = phoneNumber;
        //    this._Gender = gender;
        //    this._Grade = grade;
        //}

        public int Age
        {
            get { return _Age; }
            set
            {
                _Age = value;
                _IsHigh = value >= 11;
                _IsLow = value <= 10;
            }
        }

        //그레이드 선택이 수동이므로 현재는 쓰이지 않는 프로퍼티 변수
        private bool IsHigh
        {
            get { return _IsHigh; }
            set
            {
                _IsHigh = value;
                Grade = value ? "H" : "L";
            }
        }

        //그레이드 선택이 수동이므로 현재는 쓰이지 않는 프로퍼티 변수
        private bool IsLow
        {
            get { return _IsLow; }
            set
            {
                _IsLow = value;
                Grade = value ? "L" : "H";
            }
        }      

        public string Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        public bool IsBoy
        {
            get { return _IsBoy; }
            set
            {
                _IsBoy = value;
                _Gender = value ? "남" : "여";                
            }
        }

        public bool IsGirl
        {
            get { return _IsGirl; }
            set
            {
                _IsGirl = value;
                _Gender = value ? "여" : "남";                
            }
        }

        public string CollectAllInfo
        {
            get { return _CollectAllInfo; }
            set
            {
                _CollectAllInfo = value;
            }
        }
    }
}
