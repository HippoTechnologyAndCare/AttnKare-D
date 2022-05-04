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
    [SerializeField] private PlanCubeController2 planCubeCon;

    [SerializeField] private int scoreI;
    [SerializeField] private string scoreS;
    [SerializeField] private Text scoreTxt;

    [SerializeField] private string[] mornCardArr;
    [SerializeField] private string[] optionCardArr;
    [SerializeField] private string[] cardArr02;
    
    //test
    [SerializeField] private string[] tier1Arr;
    private int tier1Ctn;
    

    //cardArr

    private void Awake()
    {
        mornCardArr = new[] {"A", "C", "E"};
        optionCardArr = new[] {"B", "D"};
        cardArr02 = new[] {"F"};
        
    }

    public void ScorerCalculator()
    {
        // 스케줄을 완료하면서 카드 사용 정보가 모아진 SchedulerDict 사전을 활용해 점수 계산을 해야 한다
        foreach (var seq in scManager.SchedulerDict)
        {
            if (mornCardArr.Any()) ;
            
            if (mornCardArr.Any(morning => seq.Value == morning))
            {
                tier1Ctn += seq.Key;
            }
            
            if (tier1Arr.Any(tier1 => seq.Value == tier1))
            {
                tier1Ctn += seq.Key;
            }
            
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
    
    
    
}
