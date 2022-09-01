using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using NAudio.Lame;
using NAudio.Wave.WZT;

public class VoiceRecording : MonoBehaviour
{

    /*using System.Diagnostics;*/
    string FileName;
    string fWAV = ".wav"; // wav Ȯ����
    string fMP3 = ".mp3"; // mp3 Ȯ����

    float timer = 0f;

    bool NowRecording = false;

    AudioClip recording;
    AudioSource audioSource;
    float startRecordingTime;

    int MaxRecordingTime = 300;
    [HideInInspector]
    public bool fin = false;
    void Start()
    {
        FileName = SceneManager.GetActiveScene().buildIndex.ToString(); // SceneManager.GetActiveScene().buildIndex.ToString();
        audioSource = GetComponent<AudioSource>();
        StartRecording();

        //  NowRecording = true;
    }

    void Update()
    {
        /*if (NowRecording)
        {
            timer += Time.deltaTime;

            if (timer > MaxRecordingTime - 1)   //timer > 5
            {
                //5���� �Ǹ� �ڵ� ����
                StopRecordingNBehavior(); 
                // �� �Լ��� ȣ��Ǹ鼭 CollectData�� ������ ��� �Լ��� �ѹ� �� ȣ��˴ϴ�. 300�� �̻� �����ϴ� ��̵��� �����Ͱ� 
                // �ѹ� ����� �ǰ�, delimiter�� �������� ��ϵǾ� ������ �м��� ������ �� �� �����ϴ�. ������ �������ֽø� �����ϰڽ��ϴ�.
            }
        }*/
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
        /*string callingFuncName = new StackFrame(1).GetMethod().Name;
        UnityEngine.Debug.Log("called by: " + callingFuncName);*/
        //    NowRecording = false;
        StartCoroutine(FinishAndMakeClip());

    }

    IEnumerator FinishAndMakeClip()
    {
        Microphone.End("");
        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);


        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        recording = recordingNew;
        audioSource.clip = recording;
        //  EncodeMP3.SaveMp3(audioSource.clip,"Assets / Joon", 128);

        //wav���Ϸ� ����
        SavWavMp3.SaveWav(GUIDE_API.RecordingPath + fWAV, recording);
        byte[] byteWav = File.ReadAllBytes(GUIDE_API.RecordingPath + ".wav");
        File.WriteAllBytes(GUIDE_API.RecordingPath + ".mp3", EncodeMP3.ConvertWavToMp3(byteWav, 128));
        yield return new WaitUntil(() => File.Exists(GUIDE_API.RecordingPath + ".mp3"));
        File.Delete(GUIDE_API.RecordingPath + ".wav");
        Debug.Log("SAVEWAV");
        fin = true;


        //����� wav���� mp3�� ��ȯ
        /// WaveToMP3(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV,
        //  UserData.DataManager.GetInstance().FilePath_Folder + FileName + fMP3);

        //���� ���� �Ϸ���� ���
        //   yield return new WaitUntil(() => File.Exists(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fMP3));  

        // File.Delete(UserData.DataManager.GetInstance().FilePath_Folder + FileName + fWAV);
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV ���� MP3�� ��ȯ
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }
}
