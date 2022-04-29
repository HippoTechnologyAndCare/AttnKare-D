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

    [SerializeField] private string[] cardArr01;
    [SerializeField] private string[] cardArr02;

    //cardArr

    private void Awake()
    {
        cardArr01 = new[] {"B", "D"};
        cardArr02 = new[] {"F"};

    }

    public void ScorerCalculator()
    {
        // 스케줄을 완료하면서 카드 사용 정보가 모아진 CardCtnDic 사전을 활용해 점수 계산을 해야 한다
        foreach (var card in scManager.CardCtnDic)
        {
            // if (tier1Arr.Any(tier1 => card.Key == tier1))
            // {
            //     tier1Ctn += card.Value;
            // }
            //
            // if (tier2Arr.Any(tier2 => card.Key == tier2))
            // {
            //     tier2Ctn += card.Value;
            // }
            //
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
