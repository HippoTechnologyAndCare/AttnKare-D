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
    string fWAV = ".wav"; // wav Ȯ����
    string fMP3 = ".mp3"; // mp3 Ȯ����

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
        FilePath_Root = Application.streamingAssetsPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";       //���̸��� ����
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
                //10���� �Ǹ� �ڵ� ����
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

    public void StopRecordingNBehavior()     // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< �̰� ȣ���ϸ� ���� �� ����
    {
        NowRecording = false;
        StartCoroutine(FinishAndMakeClip());
        transform.GetComponent<BNG.CollectData>().SaveBehaviorData();
    }

    IEnumerator FinishAndMakeClip()
    {
        //�ٷ� �����ϸ� ������ �Ҹ��� ©�� �� �����Ƿ� ������ �ְ� ����
        yield return new WaitForSeconds(1f);

        Microphone.End("");

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);

        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        recording = recordingNew;
        audioSource.clip = recording;

        SavWav.Save(FilePath_Folder + FileName + fWAV, recording);     //wav���Ϸ� ����
        WaveToMP3(FilePath_Folder + FileName + fWAV, FilePath_Folder + FileName + fMP3);      //����� wav���� mp3�� ��ȯ
        yield return new WaitUntil(() => File.Exists(FilePath_Folder + FileName + fMP3));  //���� ���� �Ϸ���� ���

        File.Delete(FilePath_Folder + FileName + fWAV);
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV ���� MP3�� ��ȯ
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }
}
