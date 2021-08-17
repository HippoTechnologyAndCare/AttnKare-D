using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    string ServerURL_feedback = "http://115.145.179.57:5000/feedback";   //feedback

    string ServerURL_upload = "http://115.145.179.57:5000/upload_all_files";   //upload

    public string outputpath = @"D:\hipo2.pdf";


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

        formData.AddField("name", "아이이름"); //UserData.DataManager.GetInstance().userInfo.Name
        formData.AddField("phone", "01076877191"); //UserData.DataManager.GetInstance().userInfo.PhoneNumer

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

        var resPath = Application.streamingAssetsPath;

        formData.AddField("name", "");  ////UserData.DataManager.GetInstance().userInfo.Name
        formData.AddField("phone", ""); ////UserData.DataManager.GetInstance().userInfo.PhoneNumer
        formData.AddField("gender", "");    ////UserData.DataManager.GetInstance().userInfo.Gender
        formData.AddField("age", "");   ////UserData.DataManager.GetInstance().userInfo.Age
        formData.AddBinaryData("json", System.IO.File.ReadAllBytes(resPath + "/data.json"), "data.json", "application/octet-stream");

        formData.AddBinaryData("tutorial_txt", System.IO.File.ReadAllBytes(resPath + "/tutorial_text.txt"), "tutorial_text.txt", "application/octet-stream");
        formData.AddBinaryData("tutorial_mp3", System.IO.File.ReadAllBytes(resPath + "/tutorial_mp3.mp3"), "tutorial_mp3.mp3", "application/octet-stream");

        formData.AddBinaryData("bagpacking_txt", System.IO.File.ReadAllBytes(resPath + "/bagpacking_txt.txt"), "bagpacking_txt.txt", "application/octet-stream");
        formData.AddBinaryData("bagpacking_mp3", System.IO.File.ReadAllBytes(resPath + "/bagpacking_mp3.mp3"), "bagpacking_mp3.mp3", "application/octet-stream");

        formData.AddBinaryData("scoop_txt", System.IO.File.ReadAllBytes(resPath + "/scoop_txt.txt"), "scoop_txt.txt", "application/octet-stream");
        formData.AddBinaryData("scoop_mp3", System.IO.File.ReadAllBytes(resPath + "/scoop_mp3.mp3"), "scoop_mp3.mp3", "application/octet-stream");

        formData.AddBinaryData("doorlock_txt", System.IO.File.ReadAllBytes(resPath + "/doorlock_txt.txt"), "doorlock_txt.txt", "application/octet-stream");
        formData.AddBinaryData("doorlock_mp3", System.IO.File.ReadAllBytes(resPath + "/doorlock_mp3.mp3"), "doorlock_mp3.mp3", "application/octet-stream");

        formData.AddBinaryData("schedule_txt", System.IO.File.ReadAllBytes(resPath + "/schedule_txt.txt"), "schedule_txt.txt", "application/octet-stream");
        formData.AddBinaryData("schedule_mp3", System.IO.File.ReadAllBytes(resPath + "/schedule_mp3.mp3"), "schedule_mp3.mp3", "application/octet-stream");

        //formData.AddBinaryData("clueanupmyroom_txt", System.IO.File.ReadAllBytes(resPath + "/clueanupmyroom_txt.txt"), "bagpacking_txt.txt", "application/octet-stream");
        //formData.AddBinaryData("clueanupmyroom_mp3", System.IO.File.ReadAllBytes(resPath + "/clueanupmyroom_mp3.mp3"), "bagpacking_mp3.mp3", "application/octet-stream");

        //formData.AddBinaryData("playpaddle_txt", System.IO.File.ReadAllBytes(resPath + "/playpaddle_txt.txt"), "playpaddle_txt.txt", "application/octet-stream");
        //formData.AddBinaryData("playpaddle_mp3", System.IO.File.ReadAllBytes(resPath + "/playpaddle_mp3.mp3"), "playpaddle_mp3.mp3", "application/octet-stream");

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
            //UnityEngine.Windows.File.WriteAllBytes(outputpath, webRequest.downloadHandler.data);
        }
    }
}
