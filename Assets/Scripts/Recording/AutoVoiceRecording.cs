using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NAudio.Lame;
using NAudio.Wave.WZT;

public class AutoVoiceRecording : MonoBehaviour
{
    string fWAV = ".wav"; // wav 확장자
    string fMP3 = ".mp3"; // mp3 확장자

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
            //녹음 중 시간 보여주기용 + MAX 시간이 되면 자동 종료
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
        //녹음 종료 후 프로세스
        //바로 종료하면 마지막 소리가 짤릴 수 있으므로 딜레이 주고 종료
        yield return new WaitForSeconds(1f);

        Microphone.End("");

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);

        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        recording = recordingNew;
        audioSource.clip = recording;

        FileName = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();  // ------------------ 파일명 아이정보 + SceneNum으로 설정할까 생각중

        //wav파일로 저장
        SavWav.Save(FilePath + FileName + fWAV, recording);
        //저장된 wav파일 mp3로 변환
        WaveToMP3(FilePath + FileName + fWAV, FilePath + FileName + fMP3);
        //파일 저장 완료까지 대기
        yield return new WaitUntil(() => File.Exists(FilePath + FileName + fMP3));


        File.Delete(FilePath + FileName + fWAV);

        //전송 완료 후 필요시, 저장된 mp3 원본 삭제
        //File.Delete(FilePath + FileName + fMP3);
    }


    public static void WaveToMP3(string waveFile, string mp3File)
    {
        //WAV 파일 MP3로 변환
        using (var reader = new WaveFileReader(waveFile))
        using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, LAMEPreset.ABR_128))
            reader.CopyTo(writer);
    }
}
