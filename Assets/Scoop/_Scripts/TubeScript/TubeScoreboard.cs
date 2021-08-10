﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TubeScoreboard : MonoBehaviour
{
    [Header("Ball Debugger")]

    [Tooltip("Instantiated Balls")]
    // List of balls instantiated
    public List<GameObject> clonedBalls = new List<GameObject>(); // delete this
    public List<GameObject> Balls = new List<GameObject>();
    [Tooltip("Successfully moved balls")]
    // List of balls that are successfully moved
    public List<GameObject> successBalls1 = new List<GameObject>();
    public List<GameObject> successBalls2 = new List<GameObject>();
    public List<GameObject> successBalls3 = new List<GameObject>();

    [Header("Score Board")]
    /*public TextMesh scoreBoard; // Score Text*/
    public GameObject scoreText;
    public int totalDrops = 0; // Total number of drops throughout game
    public string clearTime = ""; // Clear Time, shown after game finishes
    [HideInInspector] public int stage1Drops = 0; // Stage 1 Drops
    [HideInInspector] public int stage2Drops = 0; // Stage 2 Drops
    [HideInInspector] public int stage3Drops = 0; // Stage 3 Drops
    [HideInInspector] public string clearTime1 = ""; // Stage 1 Clear Time
    [HideInInspector] public string clearTime2 = ""; // Stage 2 Clear Time
    [HideInInspector] public string clearTime3 = ""; // Stage 3 Clear Time
    [HideInInspector] public int score1 = 0; // Yellow Ball
    [HideInInspector] public int score2 = 0; // Light Purple Ball
    [HideInInspector] public int score3 = 0; // Turqoise Ball
    private int stageCounter = 1; // Stage number
    [HideInInspector] public int excessBalls = 0;
    [HideInInspector] public int wrongColor = 0;
    [HideInInspector] public int scoopLost = 0;
    public int stageBalls = 1;

    [Header("Prefabs and Objects")]
    public GameObject timer; // Timer Text
    public GameObject waitMessage;
    public GameObject pileOfBalls;
    public List<GameObject> activeBalls1 = new List<GameObject>();
    public List<GameObject> activeBalls2 = new List<GameObject>();
    public List<GameObject> activeBalls3 = new List<GameObject>();
    public List<GameObject> lostBalls1 = new List<GameObject>();
    public List<GameObject> lostBalls2 = new List<GameObject>();
    public List<GameObject> lostBalls3 = new List<GameObject>();
    public GameObject Tools;
    public List<GameObject> toolList = new List<GameObject>();

    [Header("Debug Panel")]
    public int left1;
    public int left2;
    public int left3;

    [Header("Materials")]
    [SerializeField] Material tubeBall1;
    [SerializeField] Material tubeBall2;
    [SerializeField] Material tubeBall3;

    float delayTimer;
    float startTime = 0;
    float temp; // save stage clear time data temporarily
    public bool endOfGame = false;
    bool gameFailed = false;
    bool dataRecorded = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Child Count: " + pileOfBalls.transform.childCount);

        // Add active balls to list
        for(int i = 0; i < pileOfBalls.transform.childCount; i++)
        {
            Balls.Add(pileOfBalls.transform.GetChild(i).gameObject);
        }

        for(int i=0; i < Tools.transform.childCount; i++)
        {
            if (Tools.transform.GetChild(i).gameObject.activeSelf)
            {
                toolList.Add(Tools.transform.GetChild(i).gameObject);
            }
            
        }

        // Initialize Scoreboard
        scoreUpdate();

        // Initialize Scoreboard Text
        scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString() +
                "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\nWrong Color: " + wrongColor.ToString() + "  Excess Balls: " + excessBalls.ToString() + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        if (dataRecorded)
        {
            GetComponent<TubeScoreboard>().enabled = false;
        }

        // For Stage Wait Time
        delayTimer += Time.deltaTime;

        /*// Moves onto next stage
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && delayTimer - startTime > 4.8f && delayTimer - startTime < 5.2f && !endOfGame)
        {
            Debug.Log("Timer Finished: " + delayTimer);
            StartCoroutine(stageClear());
            startTime = 0;
        }
        // End of Game
        else if (endOfGame && !gameFailed)
        {
            scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString();
            RecordData(endOfGame, gameFailed);
            dataRecorded = true;
        }
        // Too many balls lost
        else if (endOfGame && gameFailed)
        {
            scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString();
            RecordData(endOfGame, gameFailed);
            dataRecorded = true;
        }*/
    }

    private void FixedUpdate()
    {
        // Moves onto next stage
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && delayTimer - startTime > 4.8f && delayTimer - startTime < 5.2f && !endOfGame)
        {
            Debug.Log("Timer Finished: " + delayTimer);
            StartCoroutine(stageClear());
            startTime = 0;
        }
        // End of Game
        else if (endOfGame && !gameFailed)
        {
            scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString();
            RecordData(endOfGame, gameFailed);
            dataRecorded = true;
        }
        // Too many balls lost
        else if (endOfGame && gameFailed)
        {
            scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString();
            RecordData(endOfGame, gameFailed);
            dataRecorded = true;
        }
    }

    // Updates score on each ball collision
    public void scoreUpdate()
    {
        // Checks for number of active balls
        foreach (GameObject ball in Balls)
        {
            ballUpdate(ball);

            if (ball.activeSelf)
            {
                switch (ball.GetComponent<TubeBall>().ballMatID)
                {
                    case 1:
                        if(ball.GetComponent<Renderer>().sharedMaterial == tubeBall1 && !activeBalls1.Contains(ball))
                        {
                            activeBalls1.Add(ball);
                        }
                        break;
                    case 2:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall2 && !activeBalls2.Contains(ball))
                        {
                            activeBalls2.Add(ball);
                        }
                        break;
                    case 3:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall3 && !activeBalls3.Contains(ball))
                        {
                            activeBalls3.Add(ball);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (ball.GetComponent<TubeBall>().ballMatID)
                {
                    case 1:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall1 && !lostBalls1.Contains(ball))
                        {
                            lostBalls1.Add(ball);
                            activeBalls1.Remove(ball);
                        }
                        break;
                    case 2:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall2 && !lostBalls2.Contains(ball))
                        {
                            lostBalls2.Add(ball);
                            activeBalls2.Remove(ball);
                        }
                        break;
                    case 3:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall3 && !lostBalls3.Contains(ball))
                        {
                            lostBalls3.Add(ball);
                            activeBalls3.Remove(ball);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        left1 = activeBalls1.Count;
        left2 = activeBalls2.Count;
        left3 = activeBalls3.Count;

        // Updates Scoreboard Text
        if (!endOfGame)
        {
            scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString()
                + "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\nWrong Color: " + wrongColor.ToString() + "  Excess Balls: " + excessBalls.ToString() + "\n";
        }

        // Fails if too many balls are lost
        if (!endOfGame && stageBalls == 1 && (left1 < 6 || left2 < 6 || left3 < 6))
        {
            scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString() + "\n\n";
            endOfGame = true;
            gameFailed = true;
            clearTime = timer.GetComponent<Text>().text;
            RecordData(endOfGame, gameFailed);
            dataRecorded = true;
        }
        else if (!endOfGame && stageBalls == 2 && (left1 < 5 || left2 < 5 || left3 < 5))
        {
            scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString() + "\n\n";
            endOfGame = true;
            gameFailed = true;
            clearTime = timer.GetComponent<Text>().text;
            RecordData(endOfGame, gameFailed);
            dataRecorded = true;
        }
        else if (!endOfGame && stageBalls == 3 && (left1 < 3 || left2 < 3 || left3 < 3))
        {
            scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString() + "\n\n";
            endOfGame = true;
            gameFailed = true;
            clearTime = timer.GetComponent<Text>().text;
            RecordData(endOfGame, gameFailed);
            dataRecorded = true;
        }

        // Move onto next stage
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls)
        {
            startTime = delayTimer;
            Debug.Log("Timer Started: " + startTime);
            /*if(clonedBalls.Count == score && delayTimer-startTime > 4.8f && delayTimer-startTime < 5.2f)
            {
                StartCoroutine(stageClear());
            }*/
            
        }
        else
        {
            startTime = 0;
            /*StopAllCoroutines();*/
        }

        Debug.Log("ScoreUpdate Function has been called");
    }

    // Update ball status on every collision
    void ballUpdate(GameObject ball)
    {
        switch (ball.GetComponent<TubeBall>().ballMatID)
        {
            case 1:
                if (ball.GetComponent<TubeBall>().ScoreCheck && !successBalls1.Contains(ball) && ball.activeSelf)
                {
                    if (score1 >= stageBalls)
                    {
                        ball.GetComponent<TubeBall>().resetBall();
                        ball.SetActive(false);
                        excessBalls++;
                    }
                    else
                    {
                        successBalls1.Add(ball);
                        score1++;
                    }
                }
                break;
            case 2:
                if (ball.GetComponent<TubeBall>().ScoreCheck && !successBalls2.Contains(ball) && ball.activeSelf)
                {
                    if (score2 >= stageBalls)
                    {
                        ball.GetComponent<TubeBall>().resetBall();
                        ball.SetActive(false);
                        excessBalls++;
                    }
                    else
                    {
                        successBalls2.Add(ball);
                        score2++;
                    }
                }
                break;
            case 3:
                if (ball.GetComponent<TubeBall>().ScoreCheck && !successBalls3.Contains(ball) && ball.activeSelf)
                {
                    if (score3 >= stageBalls)
                    {
                        ball.GetComponent<TubeBall>().resetBall();
                        ball.SetActive(false);
                        excessBalls++;
                    }
                    else
                    {
                        successBalls3.Add(ball);
                        score3++;
                    }
                }
                break;
            default:
                break;
        }
    }

    // Wait 5 seconds
    IEnumerator Wait()
    {
        Debug.Log("Start Wait Coroutine");
        yield return new WaitForSeconds(5f);
        Debug.Log("Wait Coroutine Finished");
    }

    // Function called when stage is cleared
    IEnumerator stageClear()
    {       
        // If score is 3, end game
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && stageBalls == 3)
        {
            clearTime = timer.GetComponent<Text>().text;
            scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n\n";
            timer.SetActive(false);
            endOfGame = true;
            RecordStageClearTime(stageCounter);
            RecordStageDrops(stageCounter);
            RecordData(endOfGame, gameFailed);
            dataRecorded = true;
        }
        // If score is not 3, move onto next stage
        else if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls)
        {
            scoreText.SetActive(false);
            waitMessage.SetActive(true);

            // Reset successfully moved balls
            foreach (GameObject ball in successBalls1)
            {
                ball.GetComponent<TubeBall>().resetBall();
                ball.GetComponent<TubeBall>().ScoreCheck = false;
                ball.SetActive(false);
            }
            foreach (GameObject ball in successBalls2)
            {
                ball.GetComponent<TubeBall>().resetBall();
                ball.GetComponent<TubeBall>().ScoreCheck = false;
                ball.SetActive(false);
            }
            foreach (GameObject ball in successBalls3)
            {
                ball.GetComponent<TubeBall>().resetBall();
                ball.GetComponent<TubeBall>().ScoreCheck = false;
                ball.SetActive(false);
            }

            successBalls1.Clear();
            successBalls2.Clear();
            successBalls3.Clear();

            score1 = 0;
            score2 = 0;
            score3 = 0;

            // Wait 5 seconds to move onto the next stage
            yield return StartCoroutine(Wait());

            RecordStageClearTime(stageCounter);
            RecordStageDrops(stageCounter);

            // Reset variables for next stage
            stageBalls++;
            stageCounter++;
            waitMessage.SetActive(false);
            scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString() + 
                "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\nWrong Color: " + wrongColor.ToString() + "  Excess Balls: " + excessBalls.ToString() + "\n";
            scoreText.SetActive(true);
        }
    }

    // Reset all ball transforms
    public void ResetBalls()
    {
        Debug.Log("Reset Balls Function Called");

        foreach (GameObject ball in Balls)
        {
            ball.GetComponent<TubeBall>().resetBall();
        }
    }

    // FUNCTIONS FOR Admin.cs SCRIPT
    /*    public void FreezeBalls()
        {
            foreach (GameObject ball in Balls)
            {
                ball.GetComponent<Rigidbody>().useGravity = false;
                ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }

        public void MeltBalls()
        {
            foreach (GameObject ball in Balls)
            {
                ball.GetComponent<Rigidbody>().useGravity = true;
            }
        }

        public void FinishGameManually()
        {
            clearTime = timer.GetComponent<Text>().text;
            foreach (GameObject ball in Balls)
            {
                Destroy(ball);
            }
            scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 횟수: " + totalDrops.ToString();
            *//*scoreBoard.text = "Finish!\n\nDrops: " + totalDrops.ToString() + "\n\nClear Time: " + clearTime;*//*
            timer.SetActive(false);
            endOfGame = true;
        }*/

    // Record Game Score (Change to json variables here)
    public void RecordData(bool end, bool failed)
    {
        string results = "";

        if(end && failed)
        {
            results += "Failed: Y\n\n" + WriteStageDrops() + "\n\nWrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n";
        }
        else if (end && !failed)
        {
            results += "Failed: N\n\n" + WriteStageDrops() + "\n\n" + WriteStageClearTime() + "\n\nWrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n";
        }
        else
        {
            results += "Failed: N\n\n" + WriteStageDrops() + "\n\n" + WriteStageClearTime() + "\n\nWrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n\nTerminated(Stage " + stageCounter + ")\n";
        }

        results += "Scoop Lost Count: " + scoopLost.ToString() + "\n";
        GetComponent<SaveScoopData>().SaveTempSceneData(results); // Change location of this if necessary
    }

    public void RecordStageClearTime(int stage)
    {
        switch (stage)
        {
            case 1:
                clearTime1 = timer.GetComponent<Text>().text;
                break;
            case 2:
                clearTime2 = timer.GetComponent<Text>().text;
                break;
            case 3:
                clearTime3 = timer.GetComponent<Text>().text;
                break;
            default:
                break;
        }
    }

    public string WriteStageClearTime()
    {
        return "Stage 1 Clear Time: " + clearTime1.ToString() + "\nStage 2 Clear Time: " + clearTime2.ToString() + "\nStage 3 Clear time: " + clearTime3.ToString() + "\n";
    }

    public void RecordStageDrops(int stage)
    {
        switch (stage)
        {
            case 1:
                stage1Drops = totalDrops;
                break;
            case 2:
                stage2Drops = totalDrops - stage1Drops;
                break;
            case 3:
                stage3Drops = totalDrops - stage2Drops - stage1Drops;
                break;
            default:
                break;
        }
    }

    public string WriteStageDrops()
    {
        return "Stage 1 Drops: " + stage1Drops.ToString() + "    Stage 2 Drops: " + stage2Drops.ToString() + "    Stage 3 Drops: " + stage3Drops.ToString() + "\n";
    }

    private void OnApplicationQuit()
    {
        if (!endOfGame)
        {
            RecordStageClearTime(stageCounter);
            RecordStageDrops(stageCounter);
            RecordData(endOfGame, gameFailed);
        }
    }
}
