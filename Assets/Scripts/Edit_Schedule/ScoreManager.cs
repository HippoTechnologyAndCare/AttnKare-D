using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker.Actions;
using Scheduler;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private const int rCardLimit = 3;
    private const int tier2Limit = 3;
    private const int tier3Limit = 2;
    private const int tier4Limit = 3;

    [SerializeField] private ScheduleManager1 scManager;

    [SerializeField] private int bCardCtn;
    [SerializeField] private int rCardCtn;
    [SerializeField] private int tier1Ctn;
    [SerializeField] private int tier2Ctn;
    [SerializeField] private int tier3Ctn;
    [SerializeField] private int tier4Ctn;
    
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

    public void ScorerCalculator()
    {
        // 스케줄을 완료하면서 카드 사용 정보가 모아진 CardCtnDic 사전을 활용해 점수 계산을 해야 한다
        foreach (var card in scManager.CardCtnDic)
        {
            if (tier1Arr.Any(tier1 => card.Key == tier1))
            {
                tier1Ctn += card.Value;
            }

            if (tier2Arr.Any(tier2 => card.Key == tier2))
            {
                tier2Ctn += card.Value;
            }

            if (tier3Arr.Any(tier3 => card.Key == tier3))
            {
                tier3Ctn += card.Value;
            }
            
            if (tier4Arr.Any(tier4 => card.Key == tier4))
            {
                tier3Ctn += card.Value;
            }
        }
        
        Debug.Log("tier1Ctn = " + tier1Ctn);
        Debug.Log("tier2Ctn = " + tier2Ctn);
        Debug.Log("tier3Ctn = " + tier3Ctn);
        Debug.Log("tier4Ctn = " + tier4Ctn);
    }
    
    
    
}
