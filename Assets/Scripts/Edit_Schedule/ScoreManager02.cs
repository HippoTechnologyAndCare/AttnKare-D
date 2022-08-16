using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker.Actions;
using Scheduler;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager02 : MonoBehaviour
{
    [SerializeField] private ScheduleManager2 scManager;
    //[SerializeField] private PlanCubeController2 planCubeCon;

    [SerializeField] private int printScore;
    [SerializeField] public int scoreI;
    [SerializeField] public string scoreS;

    [SerializeField] private string[] mornCardArr;
    [SerializeField] private string[] optCardArr;
    [SerializeField] private string[] sleepCardArr;
    
    //test
    [SerializeField] private string[] tier1Arr;
    private int tier1Ctn;
    
    private void Awake()
    {
        mornCardArr = new[] {"A", "C", "E"};
        optCardArr = new[] {"B", "D"};
        sleepCardArr = new[] {"F"};
    }

    public void ScoreCalculator()
    {
        scoreS = "";
        printScore = 0;
        scoreI = 100;
        
        MorningScoring();
        OptionalScoring();
        SleepScoring();

        printScore = scoreI;

        // 0점 이하의 점수라면 0점으로 표시하기 위한 조건문
        if (printScore < 0)
        {
            printScore = 0;
        }
        scoreS = printScore.ToString();
    }

    private void MorningScoring()
    {
        int i = 0;
        
        // 스케줄을 완료하면서 카드 사용 정보가 모아진 SchedulerDict 사전을 활용해 점수 계산을 해야 한다
        foreach (var seq in scManager.SchedulerDict)
        {
            if (i < 3)
            {
                // 1번 슬롯에 "일어나기"카드가 들어있지 않다면
                if (seq.Key == 1 && seq.Value != "C")
                {
                    scoreI += -10;
                    Debug.Log("슬롯1에 일어나기 카드가 없어서 -10점");
                    
                    // 1번 슬롯에 "잠자기" 카드가 들어 있다면
                    if (seq.Value == "F")
                    {
                        scoreI += -10;
                        Debug.Log("슬롯1에 잠자기카드가 있어서 -10점");
                    }
                }
                // 2번 슬롯에 "밥먹기" 카드가 들어있지 않다면
                if (seq.Key == 2 && seq.Value != "A")
                {
                    scoreI += -5;
                    Debug.Log("슬롯2에 밥먹기 카드가 없어서 -5점");
                }
                // 3번 슬롯에 "학교가기" 카드가 들어있지 않다면
                if (seq.Key == 3 && seq.Value != "E")
                {
                    scoreI += -10;
                    Debug.Log("슬롯3에 학교가기 카드가 없어서 -10점");
                }
                // 모닝 슬롯(1~3)에 모닝카드(A,C,E)가 들어있지 않다면
                if (!mornCardArr.Contains(seq.Value))
                {
                    scoreI += -10;
                    Debug.Log("모닝슬롯 " + seq.Key + " 에 모닝카드가 없어서 -10점");
                }
            }
            
            i++;
        }
    }
    
    private void OptionalScoring()
    {
        var i = 0;
        
        foreach (var seq in scManager.SchedulerDict)
        {
            if (i >=3 && i <= 4)
            {
                // 옵션 슬롯에->
                switch (seq.Value)
                {
                    // -> 밥먹기 카드가 들어 있을때
                    case "A" :
                        scoreI += -10;
                        Debug.Log("옵션슬롯" + seq.Key + "에 밥먹기 카드가 있어서 -10점");
                        break;
                    // -> 학교가기 카드가 들어 있을때
                    case "E" :
                        scoreI += -5;
                        Debug.Log("옵션슬롯" + seq.Key + "에 학교가기 카드가 있어서 -5점");
                        break;
                    // -> 잠자기 카드가 들어 있을때
                    case "F" :
                        scoreI += -5;
                        Debug.Log("옵션슬롯" + seq.Key + "에 잠자기 카드가 있어서 -5점");
                        break;
                }

                // 옵션슬롯에 옵션카드가 없다면
                if (!optCardArr.Contains(seq.Value))
                {
                    scoreI += -5;
                    Debug.Log("옵션슬롯 " + seq.Key + "에 옵션카드가 없어서 -5점");
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
                // 슬립 슬롯에 ->
                switch (seq.Value)
                {
                    // -> 일어나기 카드가 있을때
                    case "C" :
                        scoreI += -10;
                        Debug.Log("슬립슬롯에 일어나기 카드가 있어서 -10점");
                        break;
                    // -> 학교가기 카드가 있을때
                    case "E" :
                        scoreI += -10;
                        Debug.Log("슬립슬롯에 학교가기 카드가 있어서 -10점");
                        break;
                }
            }
            
            i++;
        }
    }

}
