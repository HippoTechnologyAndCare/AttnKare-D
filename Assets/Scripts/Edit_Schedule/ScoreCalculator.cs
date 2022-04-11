using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using Scheduler;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalculator : MonoBehaviour
{
    private const int rCardLimit = 3;
    private const int tier2Limit = 3;
    private const int tier3Limit = 2;
    private const int tier4Limit = 3;

    [SerializeField] private ScheduleManager1 scManager;

    [SerializeField] private int bCardCtn;
    [SerializeField] private int rCardCtn;

    [SerializeField] private string[] bCardArr;
    [SerializeField] private string[] rCardArr;

    // 카드 등급
    // Tier 1 = A, G, I   // Study, Read, Work out
    // Tier 2 = C, E      // CleanUp, Conversation with family
    // Tier 3 = H, J      // Two Play Cards
    // Tier 4 = B, D, F   // TV, Game, Sleep
    [SerializeField] private string[] tier1Arr;
    [SerializeField] private string[] tier2Arr;
    [SerializeField] private string[] tier3Arr;
    [SerializeField] private string[] tier4Arr;
    
    private void Awake()
    {
        bCardArr = new [] {"A", "C", "E", "G", "I" };
        rCardArr = new [] {"B", "D", "F", "H", "J" };

        tier1Arr = new[] {"A", "G", "I"};
        tier2Arr = new[] {"C", "E"};
        tier3Arr = new[] {"H", "j"};
        tier4Arr = new[] {"B", "F", "D"};
    }

    private void Scorer()
    {
        // 스케줄을 완료하면서 카드 사용 정보가 모아진 CardCtnDic 사전을 활용해 점수 계산을 해야 한다
        // 
        foreach (var card in scManager.CardCtnDic)
        {
            
        }

    }
    
    
    
}
