using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UserData;

public class NetworkManager : MonoBehaviour
{
    string ServerURL_feedback = "http://jdi.bitzflex.com:4005/feedback";   //feedback
  //  string ServerURL_upload = "http://jdi.bitzflex.com:4005/upload_all_files_v1_withloca";   //upload
    string ServerURL_BnB = "http://jdi.bitzflex.com:4005/upload_all_files_bnb_smc";

    string AudioFormat = "";

    public void DoSendToTextMsg()
    {
        StartCoroutine(Request_Feedback());
    }

    public void DoSendToFinishData()
    {
        StartCoroutine(Request_ResultPDF());
    }

    IEnumerator Request_Feedback()
    {
        WWWForm formData = new WWWForm();

        formData.AddField("name", DataManager.GetInstance().userInfo.Name);
        formData.AddField("phone", DataManager.GetInstance().userInfo.PhoneNumber);

        UnityWebRequest webRequest = UnityWebRequest.Post(ServerURL_feedback, formData);

        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.DataProcessingError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
    }

    IEnumerator Request_ResultPDF()
    {
        WWWForm formData = new WWWForm();

        var resPath = DataManager.GetInstance().FilePath_Folder + "/";

        formData.AddField("name", DataManager.GetInstance().userInfo.Name);
        formData.AddField("phone", DataManager.GetInstance().userInfo.PhoneNumber);
        formData.AddField("gender", DataManager.GetInstance().userInfo.Gender);
        formData.AddField("age", DataManager.GetInstance().userInfo.Age);
        formData.AddField("location", DataManager.GetInstance().userInfo.Location);

        formData.AddBinaryData("json", File.ReadAllBytes(resPath + "UserData.json"), "interaction.json", "application/octet-stream");


        AudioFormat = ".wav";
/*
        // 라이브러리 변경 전까지는 wav 파일을 전송 mp3 -> wav
#if UNITY_EDITOR
        AudioFormat = ".mp3";
#else
        AudioFormat = ".wav";
#endif
*/
        formData.AddBinaryData("tutorial_txt", File.ReadAllBytes(resPath + "8_Behavior.txt"), "tutorial_txt.txt", "application/octet-stream");
        formData.AddBinaryData("tutorial_mp3", File.ReadAllBytes(resPath + "8" + AudioFormat), "tutorial_mp3.mp3", "application/octet-stream");
        if (DataManager.GetInstance().userInfo.Grade == "L")
        {
            formData.AddBinaryData("bagpacking_txt", File.ReadAllBytes(resPath + "1_Behavior.txt"), "bagpacking_txt.txt", "application/octet-stream");
            formData.AddBinaryData("bagpacking_mp3", File.ReadAllBytes(resPath + "1" + AudioFormat), "bagpacking_mp3.mp3", "application/octet-stream");
            formData.AddBinaryData("scoop_txt", File.ReadAllBytes(resPath + "2_Behavior.txt"), "scoop_txt.txt", "application/octet-stream");
            formData.AddBinaryData("scoop_mp3", File.ReadAllBytes(resPath + "2" + AudioFormat), "scoop_mp3.mp3", "application/octet-stream");
            formData.AddBinaryData("cleanupmyroom_txt", File.ReadAllBytes(resPath + "4_Behavior.txt"), "cleanupmyroom_txt.txt", "application/octet-stream");
            formData.AddBinaryData("cleanupmyroom_mp3", File.ReadAllBytes(resPath + "4" + AudioFormat), "cleanupmyroom_mp3.mp3", "application/octet-stream");
        }
        if (DataManager.GetInstance().userInfo.Grade == "H")
        {
            formData.AddBinaryData("schedule_txt", File.ReadAllBytes(resPath + "5_Behavior.txt"), "schedule_txt.txt", "application/octet-stream");
            formData.AddBinaryData("schedule_mp3", File.ReadAllBytes(resPath + "5" + AudioFormat), "schedule_mp3.mp3", "application/octet-stream");
            formData.AddBinaryData("playpaddle_txt", File.ReadAllBytes(resPath + "6_Behavior.txt"), "playpaddle_txt.txt", "application/octet-stream");
            formData.AddBinaryData("playpaddle_mp3", File.ReadAllBytes(resPath + "6" + AudioFormat), "playpaddle_mp3.mp3", "application/octet-stream");
            formData.AddBinaryData("conveyor_txt", File.ReadAllBytes(resPath + "7_Behavior.txt"), "conveyor_txt.txt", "application/octet-stream");
            formData.AddBinaryData("conveyor_mp3", File.ReadAllBytes(resPath + "7" + AudioFormat), "conveyor_mp3.mp3", "application/octet-stream");
            formData.AddBinaryData("conveyor_json", File.ReadAllBytes(resPath + "Conveyor.json"), "conveyor_json.json", "application/octet-stream");
        }
        formData.AddBinaryData("numbermatching_txt", File.ReadAllBytes(resPath + "3_Behavior.txt"), "numbermatching_txt.txt", "application/octet-stream");
        formData.AddBinaryData("numbermatching_mp3", File.ReadAllBytes(resPath + "3" + AudioFormat), "numbermatching_mp3.mp3", "application/octet-stream");

        formData.AddBinaryData("ending_txt", File.ReadAllBytes(resPath + "11_Behavior.txt"), "ending_txt.txt", "application/octet-stream");
        formData.AddBinaryData("ending_mp3", File.ReadAllBytes(resPath + "11" + AudioFormat), "ending_mp3.wav", "application/octet-stream");

        UnityWebRequest webRequest = UnityWebRequest.Post(ServerURL_BnB, formData);

        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(webRequest.error);
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text + " FINISH ");
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + DataManager.GetInstance().userInfo.Name + ".pdf", webRequest.downloadHandler.data);
        }
       
    }
    /*

       if (DataManager.GetInstance().userInfo.Grade == "L")
       {
           formData.AddBinaryData("doorlock_txt", File.ReadAllBytes(resPath + "1_Behavior.txt"), "doorlock_txt.txt", "application/octet-stream");
           formData.AddBinaryData("doorlock_mp3", File.ReadAllBytes(resPath + "1" + AudioFormat), "doorlock_mp3.mp3", "application/octet-stream");
           formData.AddBinaryData("schedule_txt", File.ReadAllBytes(resPath + "2_Behavior.txt"), "schedule_txt.txt", "application/octet-stream");
           formData.AddBinaryData("schedule_mp3", File.ReadAllBytes(resPath + "2" + AudioFormat), "schedule_mp3.mp3", "application/octet-stream");

       formData.AddBinaryData("bagpacking_txt", File.ReadAllBytes(resPath + "3_Behavior.txt"), "bagpacking_txt.txt", "application/octet-stream");
           formData.AddBinaryData("bagpacking_mp3", File.ReadAllBytes(resPath + "3" + AudioFormat), "bagpacking_mp3.mp3", "application/octet-stream");
           formData.AddBinaryData("scoop_txt", File.ReadAllBytes(resPath + "4_Behavior.txt"), "scoop_txt.txt", "application/octet-stream");
           formData.AddBinaryData("scoop_mp3", File.ReadAllBytes(resPath + "4" + AudioFormat), "scoop_mp3.mp3", "application/octet-stream");
       }
       else
       {
           formData.AddBinaryData("cleanupmyroom_txt", File.ReadAllBytes(resPath + "5_Behavior.txt"), "cleanupmyroom_txt.txt", "application/octet-stream");
           formData.AddBinaryData("cleanupmyroom_mp3", File.ReadAllBytes(resPath + "5" + AudioFormat), "cleanupmyroom_mp3.mp3", "application/octet-stream");
               formData.AddBinaryData("scoop_txt", File.ReadAllBytes(resPath + "8_Behavior.txt"), "scoop_txt.txt", "application/octet-stream");
       formData.AddBinaryData("scoop_mp3", File.ReadAllBytes(resPath + "8" + AudioFormat), "scoop_mp3.mp3", "application/octet-stream");

           formData.AddBinaryData("bagpacking_txt", File.ReadAllBytes(resPath + "7_Behavior.txt"), "bagpacking_txt.txt", "application/octet-stream");
           formData.AddBinaryData("bagpacking_mp3", File.ReadAllBytes(resPath + "7" + AudioFormat), "bagpacking_mp3.mp3", "application/octet-stream");
       formData.AddBinaryData("playpaddle_txt", File.ReadAllBytes(resPath + "6_Behavior.txt"), "playpaddle_txt.txt", "application/octet-stream");
       formData.AddBinaryData("playpaddle_mp3", File.ReadAllBytes(resPath + "6" + AudioFormat), "playpaddle_mp3.mp3", "application/octet-stream");


       }
       */

}
