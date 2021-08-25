using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject timer;
    private Text timerText;
    [HideInInspector] public float secondsCount = 0;
    private int minuteCount;
    private int hourCount;

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
}
