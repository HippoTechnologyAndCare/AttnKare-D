using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UserData;

public class NetworkManager : MonoBehaviour
{
    string ServerURL_feedback = "http://jdi.bitzflex.com:4005/feedback";   //feedback

    string ServerURL_upload = "http://jdi.bitzflex.com:4005/upload_all_files";   //upload

    public string outputpath = @"C:\Report.pdf";


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
        formData.AddField("phone", DataManager.GetInstance().userInfo.PhoneNumer);

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

        var resPath = DataManager.GetInstance().FilePath_Folder;

        formData.AddField("name", DataManager.GetInstance().userInfo.Name);
        formData.AddField("phone", DataManager.GetInstance().userInfo.PhoneNumer);
        formData.AddField("gender", DataManager.GetInstance().userInfo.Gender);
        formData.AddField("age", DataManager.GetInstance().userInfo.Age);

        formData.AddBinaryData("json", File.ReadAllBytes(resPath + "/UserData.json"), "data.json", "application/octet-stream");

        formData.AddBinaryData("tutorial_txt", File.ReadAllBytes(resPath + "/9.txt"), "tutorial.txt", "application/octet-stream");
        formData.AddBinaryData("tutorial_mp3", File.ReadAllBytes(resPath + "/9.mp3"), "tutorial.mp3", "application/octet-stream");

        if (DataManager.GetInstance().userInfo.Grade == "L")
        {
            formData.AddBinaryData("doorlock_txt", File.ReadAllBytes(resPath + "/1.txt"), "doorlock.txt", "application/octet-stream");
            formData.AddBinaryData("doorlock_mp3", File.ReadAllBytes(resPath + "/1.mp3"), "doorlock.mp3", "application/octet-stream");
            formData.AddBinaryData("schedule_txt", File.ReadAllBytes(resPath + "/2.txt"), "schedule.txt", "application/octet-stream");
            formData.AddBinaryData("schedule_mp3", File.ReadAllBytes(resPath + "/2.mp3"), "schedule.mp3", "application/octet-stream");

            formData.AddBinaryData("bagpacking_txt", File.ReadAllBytes(resPath + "/3.txt"), "bagpacking.txt", "application/octet-stream");
            formData.AddBinaryData("bagpacking_mp3", File.ReadAllBytes(resPath + "/3.mp3"), "bagpacking.mp3", "application/octet-stream");
            formData.AddBinaryData("scoop_txt", File.ReadAllBytes(resPath + "/4.txt"), "scoop.txt", "application/octet-stream");
            formData.AddBinaryData("scoop_mp3", File.ReadAllBytes(resPath + "/4.mp3"), "scoop.mp3", "application/octet-stream");
        }
        else
        {
            formData.AddBinaryData("clueanupmyroom_txt", File.ReadAllBytes(resPath + "/5.txt"), "clueanup.txt", "application/octet-stream");
            formData.AddBinaryData("clueanupmyroom_mp3", File.ReadAllBytes(resPath + "/5.mp3"), "clueanup.mp3", "application/octet-stream");
            formData.AddBinaryData("playpaddle_txt", File.ReadAllBytes(resPath + "/6.txt"), "playpaddle.txt", "application/octet-stream");
            formData.AddBinaryData("playpaddle_mp3", File.ReadAllBytes(resPath + "/6.mp3"), "playpaddle.mp3", "application/octet-stream");

            formData.AddBinaryData("bagpacking_txt", File.ReadAllBytes(resPath + "/7.txt"), "bagpacking.txt", "application/octet-stream");
            formData.AddBinaryData("bagpacking_mp3", File.ReadAllBytes(resPath + "/7.mp3"), "bagpacking.mp3", "application/octet-stream");

            formData.AddBinaryData("scoop_txt", File.ReadAllBytes(resPath + "/8.txt"), "scoop.txt", "application/octet-stream");
            formData.AddBinaryData("scoop_mp3", File.ReadAllBytes(resPath + "/8.mp3"), "scoop.mp3", "application/octet-stream");
        }

        UnityWebRequest webRequest = UnityWebRequest.Post(ServerURL_upload, formData);

        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text + " /////////// Form upload complete");
            File.WriteAllBytes(outputpath, webRequest.downloadHandler.data);
        }
    }
}
