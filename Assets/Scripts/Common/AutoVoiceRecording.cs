using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using NAudio.Lame;
using NAudio.Wave.WZT;

public class AutoVoiceRecording : MonoBehaviour
{
    string fWAV = ".wav"; // wav 확장자
    string fMP3 = ".mp3"; // mp3 확장자

    float timer = 0f;

    bool NowRecording = false;

    string FileName;
    string FolderName;
    string FilePath_Root;
    string FilePath_Folder;

    AudioClip recording;
    AudioSource audioSource;
    float startRecordingTime;

    int MaxRecordingTime = 600;

    void Start()
    {
        FolderName = "NAME" + DateTime.Now.ToString("yyyyMMddHHdd");        // UserData.DataManager.GetInstance().userInfo.Name + "_" + UserData.DataManager.GetInstance().userInfo.Gender;
        FileName = SceneManager.GetActiveScene().buildIndex.ToString(); // SceneManager.GetActiveScene().buildIndex.ToString();
        FilePath_Root = Application.streamingAssetsPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";       //아이마다 저장
        FilePath_Folder = FilePath_Root + FolderName + "/";

        if (!Directory.Exists(FilePath_Root))
        {
            Directory.CreateDirectory(FilePath_Root);
        }

        if (!Directory.Exists(FilePath_Folder))
        {
            Directory.CreateDirectory(FilePath_Folder);
        }

        audioSource = GetComponent<AudioSource>();

        StartRecording();
        NowRecording = true;
    }

    void Update()
    {
        if (NowRecording)
        {
            timer += Time.deltaTime;

            if (timer > MaxRecordingTime - 1)   //
            {
                //10분이 되면 자동 종료
                StopRecordingNBehavior();
            }
        }
    }

    public void StartRecording()
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            Destroy(audioSource.clip);
        }

        startRecordingTime = Time.time;
        recording = Microphone.Start("", false, MaxRecordingTime, 44100);
    }

    public void StopRecordingNBehavior()     // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 이거 호출하면 종료 및 저장
    {
        NowRecording = false;
        StartCoroutine(FinishAndMakeClip());
        transform.GetComponent<BNG.CollectData>().SaveBehaviorData();
    }

    IEnumerator FinishAndMakeClip()
    {
        //바로 종료하면 마지막 소리가 짤릴 수 있으므로 딜레이 주고 종료
        yield return new WaitForSeconds(1f);

        Microphone.End("");

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);

        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        recording = recordingNew;
        audioSource.clip = recording;

        SavWav.Save(FilePath_Folder + FileName + fWAV, recording);     //wav파일로 저장
        WaveToMP3(FilePath_Folder + FileName + fWAV, FilePath_Folder + FileName + fMP3);      //저장된 wav파일 mp3로 변환
        yield return new WaitUntil(() => File.Exists(FilePath_Folder + FileName + fMP3));  //파일 저장 완료까지 대기

        File.Delete(FilePath_Folder + FileName + fWAV);
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV 파일 MP3로 변환
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }
}
