﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject timer;
    private Text timerText;
    [HideInInspector] public float secondsCount = 0;
    [HideInInspector] public int minuteCount;
    [HideInInspector] public int hourCount;

    public GameObject voice;            // Sounds/Voice
    public GameObject scoreboard;       // ScoopGame/Room/Scoreboard

    [SerializeField] AudioClip timeLimitAudio;
    [SerializeField] AudioClip timeOutAudio;

    bool timeLimitPlayed = false;
    bool timeOutPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        timerText = timer.GetComponent<Text>();
        timerText.text = 0 + "초";
    }

    // Update is called once per frame
    void Update()
    {
        secondsCount += Time.deltaTime;
        UpdateTimerUI();

        // Check Time Limit or Time Out
        PlayTimeLimitAudio();
        PlayTimeOutAudio();
    }

    // Timer starts running when game starts
    public void UpdateTimerUI()
    {
        //set timer UI
        if(hourCount == 0 && minuteCount == 0)
        {
            timerText.text = (int)secondsCount + "초";
        }
        else if (hourCount == 0 && minuteCount != 0)
        {
            timerText.text = minuteCount + "분 " + (int)secondsCount + "초";
        }
        else
        {
            timerText.text = hourCount + "시간 " + minuteCount + "분 " + (int)secondsCount + "초";
        }
        
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount %= 60;
            if (minuteCount >= 60)
            {
                hourCount++;
                minuteCount %= 60;
            }
        }
    }

    public void PlayTimeLimitAudio()
    {
        if (!timeLimitPlayed)
        {
            // Play Time Limit Audio
            if(scoreboard.GetComponent<EasyTubeScoreboard>() != null)
            {
                if (minuteCount == 10)
                {
                    Debug.Log("Easy");
                    // Lo
                    GetComponent<AudioSource>().clip = timeLimitAudio;
                    GetComponent<AudioSource>().Play();
                    Debug.Log("Playing Easy Time Limit Audio");
                    timeLimitPlayed = true;
                    scoreboard.GetComponent<EasyTubeScoreboard>().timeLimit = 1;
                    scoreboard.GetComponent<BNG.CollectData>().AddTimeStamp("TIME LIMIT");
                }
            }
            if(scoreboard.GetComponent<TubeScoreboard>() != null)
            {
                if (minuteCount == 8)
                {
                    Debug.Log("Normal");
                    // Hi
                    GetComponent<AudioSource>().clip = timeLimitAudio;
                    GetComponent<AudioSource>().Play();
                    Debug.Log("Playing Normal Time Limit Audio");
                    timeLimitPlayed = true;
                    scoreboard.GetComponent<TubeScoreboard>().timeLimit = 1;
                    scoreboard.GetComponent<BNG.CollectData>().AddTimeStamp("TIME LIMIT");
                }
            }
        }
    }

    public void PlayTimeOutAudio()
    {
        if (!timeOutPlayed)
        {
            // Play Time Out Audio
            if (scoreboard.GetComponent<EasyTubeScoreboard>() != null)
            {
                if (minuteCount == 11)
                {
                    // Lo
                    GetComponent<AudioSource>().clip = timeOutAudio;
                    GetComponent<AudioSource>().Play();
                    Debug.Log("Playing Easy Time Out Audio");
                    timeOutPlayed = true;
                    scoreboard.GetComponent<BNG.CollectData>().AddTimeStamp("TIME OUT");
                    // Call Force End Function from EasyTubeScoreboard
                    scoreboard.GetComponent<EasyTubeScoreboard>().timeOutCheck = true;
                }
            }
            if (scoreboard.GetComponent<TubeScoreboard>() != null)
            {
                if (minuteCount == 9)
                {
                    // Hi
                    GetComponent<AudioSource>().clip = timeOutAudio;
                    GetComponent<AudioSource>().Play();
                    Debug.Log("Playing Normal Time Out Audio");
                    timeOutPlayed = true;
                    scoreboard.GetComponent<BNG.CollectData>().AddTimeStamp("TIME OUT");
                    // Call Force End Function from TubeScoreboard
                    scoreboard.GetComponent<TubeScoreboard>().timeOutCheck = true;
                }
            }
        }
    }
}
