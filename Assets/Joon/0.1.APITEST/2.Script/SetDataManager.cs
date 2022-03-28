using System;
using System.IO;
using UserData;
using UnityEngine;
using UnityEngine.UI;


public class SetDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle m_Toggle;
    private int m_nAge;
    private string m_sGrade;
    private string m_sGender;
    private void CreateFolder()
    {
        // Persistant Folder Path C:\Users\<계정이름>\AppData\LocalLow\HippoTnC\Strengthen_Concentration_VR\
        DataManager.GetInstance().FilePath_Root = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";

        DataManager.GetInstance().userInfo.FolderName = DataManager.GetInstance().userInfo.Name + "_" + DataManager.GetInstance().userInfo.PhoneNumber + "_" +
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

    public void SetData()
    {

        DataManager.GetInstance().userInfo.Name = UserInfo_API.GetInstance().playerInfo.player_name;
        DataManager.GetInstance().userInfo.Age = SetAge();
        DataManager.GetInstance().userInfo.PhoneNumber = UserInfo_API.GetInstance().playerInfo.phonenum;
        DataManager.GetInstance().userInfo.Location = UserInfo_API.GetInstance().jobInfo.place;
        DataManager.GetInstance().userInfo.Grade = SetGrade();
        DataManager.GetInstance().userInfo.Gender = SetGender();
        CreateFolder();

        DataManager.GetInstance().SavePlayerDataToJson();

        GetComponent<NetworkManager>().DoSendToTextMsg();
    }
    public void TESTMODE_Toggle()
    {
        //Fetch the Toggle GameObject
        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
        
    }
    void ToggleValueChanged(bool test)
    {
        Debug.Log(test);
        DataManager.GetInstance().isTest = test ? true : false;
    }
    private int SetAge()
    {
        switch (UserInfo_API.GetInstance().playerInfo.grade)
        {
            case 1: m_nAge = 8; break;
            case 2: m_nAge = 9; break;
            case 3: m_nAge = 10; break;
            case 4: m_nAge = 11; break;
            case 5: m_nAge = 12; break;
            case 6: m_nAge = 13; break;
          //  default: age = 13; break;
        }
        return m_nAge;
        
    }
    private string SetGrade()
    {
        if (m_nAge > 10) m_sGrade = "H";
        if (m_nAge < 11) m_sGrade = "L";
        return m_sGrade;
    }
    private string SetGender()
    {
        switch (UserInfo_API.GetInstance().playerInfo.gender)
        {
            case "F": m_sGender = "여"; break;
            case "M": m_sGender = "남"; break;
        }
        return m_sGender;
    }
}
