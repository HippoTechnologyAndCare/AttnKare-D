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
    string FilePath;

    AudioClip recording;
    AudioSource audioSource;
    float startRecordingTime;

    int MaxRecordingTime = 600;

    void Start()
    {
        FolderName = DateTime.Now.ToString("yyyyMMddHHmmss"); ; // UserData.DataManager.GetInstance().userInfo.Name + "_" + UserData.DataManager.GetInstance().userInfo.PhoneNumer;
        FileName = "TEST"; // SceneManager.GetActiveScene().buildIndex.ToString();
        FilePath = Application.streamingAssetsPath + "/" + FolderName + "/";       //아이마다 저장

        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
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

            if (timer > 3)   //timer > MaxRecordingTime - 1
            {
                Debug.Log("3 stop");
                //10분이 되면 자동 종료
                StopRecording();
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

    public void StopRecording()     // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 이거 호출하면 종료 및 저장
    {
        NowRecording = false;
        StartCoroutine(FinishAndMakeClip());
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

        SavWav.Save(FilePath + FileName + fWAV, recording);     //wav파일로 저장
        WaveToMP3(FilePath + FileName + fWAV, FilePath + FileName + fMP3);      //저장된 wav파일 mp3로 변환
        yield return new WaitUntil(() => File.Exists(FilePath + FileName + fMP3));  //파일 저장 완료까지 대기

        File.Delete(FilePath + FileName + fWAV);
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV 파일 MP3로 변환
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }
}
