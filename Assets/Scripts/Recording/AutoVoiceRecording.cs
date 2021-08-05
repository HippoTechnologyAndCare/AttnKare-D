using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NAudio.Lame;
using NAudio.Wave.WZT;

public class AutoVoiceRecording : MonoBehaviour
{
    string fWAV = ".wav"; // wav Ȯ����
    string fMP3 = ".mp3"; // mp3 Ȯ����

    float timer = 0f;

    bool NowRecording = false;

    string FileName;
    string FilePath;

    AudioClip recording;
    AudioSource audioSource;
    float startRecordingTime;

    int MaxRecordingTime = 600;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        FilePath = Application.streamingAssetsPath + "/Hippo/";

        StartRecording();

        NowRecording = true;
    }

    void Update()
    {
        if (NowRecording)
        {
            //���� �� �ð� �����ֱ�� + MAX �ð��� �Ǹ� �ڵ� ����
            timer += Time.deltaTime;

            if (timer > MaxRecordingTime - 1)
            {
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

    public void StopRecording()
    {
        StartCoroutine(FinishAndMakeClip());
    }

    IEnumerator FinishAndMakeClip()
    {
        //���� ���� �� ���μ���
        //�ٷ� �����ϸ� ������ �Ҹ��� ©�� �� �����Ƿ� ������ �ְ� ����
        yield return new WaitForSeconds(1f);

        Microphone.End("");

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);

        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        recording = recordingNew;
        audioSource.clip = recording;

        FileName = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();  // ------------------ ���ϸ� �������� + SceneNum���� �����ұ� ������

        //wav���Ϸ� ����
        SavWav.Save(FilePath + FileName + fWAV, recording);
        //����� wav���� mp3�� ��ȯ
        WaveToMP3(FilePath + FileName + fWAV, FilePath + FileName + fMP3);
        //���� ���� �Ϸ���� ���
        yield return new WaitUntil(() => File.Exists(FilePath + FileName + fMP3));


        File.Delete(FilePath + FileName + fWAV);

        //���� �Ϸ� �� �ʿ��, ����� mp3 ���� ����
        //File.Delete(FilePath + FileName + fMP3);
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV ���� MP3�� ��ȯ
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }
}
