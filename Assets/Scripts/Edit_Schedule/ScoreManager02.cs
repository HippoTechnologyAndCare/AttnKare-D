using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker.Actions;
using Scheduler;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager02 : MonoBehaviour
{
    [SerializeField] private ScheduleManager2 scManager;
    //[SerializeField] private PlanCubeController2 planCubeCon;

    [SerializeField] private int scoreI;
    [SerializeField] private string scoreS;
    [SerializeField] private Text scoreTxt;

    [SerializeField] private string[] mornCardArr;
    [SerializeField] private string[] optCardArr;
    [SerializeField] private string[] sleepCardArr;
    
    //test
    [SerializeField] private string[] tier1Arr;
    private int tier1Ctn;
    
    private void Awake()
    {
        scoreI = 100;
        mornCardArr = new[] {"A", "C", "E"};
        optCardArr = new[] {"B", "D"};
        sleepCardArr = new[] {"F"};
    }

    public void ScorerCalculator()
    {
        MorningScoring();
        OptionalScoring();
        SleepScoring();
        
        scoreS = scoreI.ToString();
        scoreTxt.text = scoreS;
    }

    private void MorningScoring()
    {
        int i = 0;
        
        // 스케줄을 완료하면서 카드 사용 정보가 모아진 SchedulerDict 사전을 활용해 점수 계산을 해야 한다
        foreach (var seq in scManager.SchedulerDict)
        {
            if (i < 3)
            {
                if (!mornCardArr.Contains(seq.Value))
                {
                    scoreI += -10;
                    Debug.Log("모닝카드때문에 감점됨");
                }
            }
            
            i++;

            // if (tier3Arr.Any(tier3 => card.Key == tier3))
            // {
            //     tier3Ctn += card.Value;
            // }
            //
            // if (tier4Arr.Any(tier4 => card.Key == tier4))
            // {
            //     tier3Ctn += card.Value;
            // }
        }
    }

    private void OptionalScoring()
    {
        var i = 0;
        
        foreach (var seq in scManager.SchedulerDict)
        {
            if (i >=3 && i <= 4)
            {
                if (!optCardArr.Contains(seq.Value))
                {
                    scoreI += -10;
                    Debug.Log("옵션카드때문에 감점됨");
                }
            }

            i++;
        }
    }

    private void SleepScoring()
    {
        var i = 0;

        foreach (var seq in scManager.SchedulerDict)
        {
            if (i == 5)
            {
                if ((!sleepCardArr.Contains(seq.Value)))
                {
                    scoreI += -10;
                    Debug.Log("슬립카드때문에 감점됨");
                }
            }
            
            i++;
        }
    }

}
