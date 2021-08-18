using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using KetosGames.SceneTransition;
//using HutongGames.PlayMaker.Actions;

namespace UserData
{
    [System.Serializable]
    public class InputText : MonoBehaviour
    {
        [SerializeField] private Text txt_Name;
        [SerializeField] private Text txt_Age;
        [SerializeField] private Text txt_Fon;

        [SerializeField] private InputField inputTxt_Name;
        [SerializeField] private InputField inputTxt_Age;
        [SerializeField] private InputField inputTxt_Fon;

        [SerializeField] private Toggle genderTg_M;
        [SerializeField] private Toggle genderTg_W;
        [SerializeField] private Toggle gradeTg_L;
        [SerializeField] private Toggle gradeTg_H;

        [SerializeField] private PlayMakerFSM warningFSM;        

        //private int minLength;
        //private int maxLength;

        private bool isProb;

        private void Start()
        {
            //minLength = 1;
            //maxLength = 99;
            //inputTxt_Name.characterLimit = 5;
            //inputTxt_Age.characterLimit = 2;

            inputTxt_Name.onValueChanged.AddListener(
                (word) => inputTxt_Name.text = Regex.Replace(word, @"[^가-힣]", "")
                );
        }

        private void Update()
        {
            //다운로드 시도 실패를 감지하면 로컬에 파일을 저장하고 서버에 업로드 실행
            //if (JsonManager.GetInstance().isError_DN)
            //{
            //    Debug.Log("isErrorDN는 참");
            //    JsonManager.GetInstance().isError_DN = false;
            //    JsonManager.GetInstance().SavePlayerDataToJson();
            //    UploadData();
            //}
        }

        public void InputAgeOnEnd()
        {
            string str = inputTxt_Age.GetComponent<InputField>().text;
            if (str.Length == 1 && str != "0")
            {
                str = "0" + inputTxt_Age.GetComponent<InputField>().text;
                inputTxt_Age.GetComponent<InputField>().text = str;
            }
        }

        public void InputAgeOnValueChanged()
        {
            if (inputTxt_Age.GetComponent<InputField>().text == "")
                return;

            string str = inputTxt_Age.GetComponent<InputField>().text;

            int result = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (!(int.TryParse(str, out result)))
                {
                    inputTxt_Age.GetComponent<InputField>().text = "";
                    return;
                }
            }
        }

        private void CreatFolder()
        {
            // Persistant Folder Path C:\Users\<계정이름>\AppData\LocalLow\HippoTnC\Strengthen_Concentration_VR\
            DataManager.GetInstance().FilePath_Root = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            
            DataManager.GetInstance().userInfo.FolderName = txt_Name.text + "_" + txt_Fon.text + "_" + 
            DataManager.GetInstance().userInfo.Grade;

            DataManager.GetInstance().FilePath_Root = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            DataManager.GetInstance().FilePath_Folder = DataManager.GetInstance().FilePath_Root +
            DataManager.GetInstance().userInfo.FolderName + "/";

            if (!Directory.Exists(DataManager.GetInstance().FilePath_Root))
            {
                Directory.CreateDirectory(DataManager.GetInstance().FilePath_Root);
            }
            if (!Directory.Exists(DataManager.GetInstance().FilePath_Folder))
            {
                Directory.CreateDirectory(DataManager.GetInstance().FilePath_Folder);
            }

        }

        private void Collect_UserInfo()
        {
            DataManager.GetInstance().userInfo.FolderName = txt_Name.text + "_" + txt_Age.text + "_" +
            DataManager.GetInstance().userInfo.Gender + "_" + DataManager.GetInstance().userInfo.Grade + "_" + txt_Fon.text;
        }

        public int AgeToInt(string age)
        {
            DataManager.GetInstance().userInfo.Age = int.Parse(age);
            return DataManager.GetInstance().userInfo.Age;
        }

        private void Check_Gender()
        {
            if (genderTg_M != null || genderTg_W == null)
            {                
                if (genderTg_M.isOn == true)
                {
                    DataManager.GetInstance().userInfo.IsBoy = true;                                        
                }

                else
                {
                    DataManager.GetInstance().userInfo.IsGirl = true;
                }
            }
        }

        private void Check_Grade()
        {
            DataManager.GetInstance().userInfo.Grade = gradeTg_L.isOn ? "L" : "H";
        }

        private bool ExceptionHandling_Check()
        {
            string u_N = txt_Name.text;
            string u_A = txt_Age.text;
            string u_P = txt_Fon.text;

            // 각 InputField에 미입력이 있는지 검사
            if (u_N == "" || u_A == "" || u_P == "")
            {
                if (u_N == "")
                {
                    SendEvent("PL Input Name");
                    Debug.Log("이름을 입력하세요!");
                }

                if (u_A == "")
                {
                    SendEvent("PL Input Age");
                    Debug.Log("나이를 입력하세요!");
                }

                if (u_P == "")
                {
                    SendEvent("PL Input Pon");
                    Debug.Log("전화번호를 입력하세요!");
                }
                isProb = true;
            }

            // 나이 입력을 올바르게 했는지 검사
            //if (u_A != "")
            //{
            //    AgeToInt(u_A);

            //    if (currentAge < minAge || currentAge > maxAge)
            //    {
            //        //SendEvent();
            //        Debug.Log("나이를 다시 입력하세요!(1~99)");
            //        isProb = true;
            //    }            
            //}
            return isProb;
        }

        public void Confirm_n_DataExistenceCheck()
        {
            SendEvent("TurnOff Messages");

            ExceptionHandling_Check();

            if (!isProb)
            {
                DataManager.GetInstance().userInfo.Name = txt_Name.text;
                DataManager.GetInstance().userInfo.Age = int.Parse(txt_Age.text);
                DataManager.GetInstance().userInfo.PhoneNumer = txt_Fon.text;

                Check_Gender();
                Check_Grade();

                CreatFolder();

                DataManager.GetInstance().SavePlayerDataToJson();

                GetComponent<NetworkManager>().DoSendToTextMsg();       // <<<< ---------------- 문자전송 추가

                //manualXRControl.StartCoroutine("StartXR");

                SceneLoader.LoadScene("OPENEND");
            }

            Reset_BoolData();
        }

        private void UploadData()
        {
            DataManager.GetInstance().StartCoroutine("UploadRoutine");
        }

        private void Reset_BoolData()
        {
            isProb = false;
            DataManager.GetInstance().isError_DN = false;
            DataManager.GetInstance().isError_UP = false;
        }

        private void SendEvent(string eventName)
        {
            warningFSM.SendEvent(eventName);
        }
    }
}

