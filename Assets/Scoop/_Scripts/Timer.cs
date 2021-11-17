using System.Collections;
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
                if (scoreboard.GetComponent<EasyTubeScoreboard>().LoHi == 0 && (minuteCount == 10 && secondsCount == 0))
                {
                    // Lo
                    timeLimitPlayed = true;
                }
            }
            if(scoreboard.GetComponent<TubeScoreboard>() != null)
            {
                if (scoreboard.GetComponent<TubeScoreboard>().LoHi == 1 && (minuteCount == 8 && secondsCount == 0))
                {
                    // Hi
                    timeLimitPlayed = true;
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
                if (scoreboard.GetComponent<EasyTubeScoreboard>().LoHi == 0 && (minuteCount == 11 && secondsCount == 0))
                {
                    // Lo
                    timeOutPlayed = true;
                }
            }
            if (scoreboard.GetComponent<TubeScoreboard>() != null)
            {
                if (scoreboard.GetComponent<TubeScoreboard>().LoHi == 1 && (minuteCount == 9 && secondsCount == 0))
                {
                    // Hi
                    timeOutPlayed = true;
                }
            }
        }
    }
}
