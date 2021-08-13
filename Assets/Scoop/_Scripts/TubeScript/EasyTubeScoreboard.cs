using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EasyTubeScoreboard : MonoBehaviour
{
    [Header("Ball Debugger")]

    [Tooltip("All Ball Objects in Scene")]
    public List<GameObject> Balls = new List<GameObject>();

    [Tooltip("Successfully moved balls")]
    // List of balls that are successfully moved
    public List<GameObject> successBalls1 = new List<GameObject>(); // Yellow
    public List<GameObject> successBalls2 = new List<GameObject>(); // Light Purple
    public List<GameObject> successBalls3 = new List<GameObject>(); // Turqoise

    [Header("Score Board")]
    public GameObject scoreText; // Score Text
    public int totalDrops = 0; // Total number of drops throughout game
    public string clearTime = ""; // Clear Time
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
    [HideInInspector] public int excessBalls = 0; // Number of Excess Balls put into tube
    [HideInInspector] public int wrongColor = 0; // Number of Balls that do not match tube color
    [HideInInspector] public int scoopLost = 0; // **DEPRECATED** Number of Times Scoop was lost
    public int stageBalls = 1; // Number of Balls needed in each tube to move onto next stage

    [Header("Prefabs and Objects")]
    public GameObject timer; // Timer Text
    public GameObject waitMessage; // Wait Message, shown after each stage
    public GameObject pileOfBalls; // Empty Object Containing all balls in scene
    public List<GameObject> activeBalls1 = new List<GameObject>(); // Active Yellow Balls
    public List<GameObject> activeBalls2 = new List<GameObject>(); // Active Light Purple Balls
    public List<GameObject> activeBalls3 = new List<GameObject>(); // Active Turqoise Balls
    public List<GameObject> lostBalls1 = new List<GameObject>(); // Lost Yellow Balls
    public List<GameObject> lostBalls2 = new List<GameObject>(); // Lost Light Purple Balls
    public List<GameObject> lostBalls3 = new List<GameObject>(); // Lost Turqoise Balls
    public Transform returnPoint1; // **DEPRECATED** Yellow Ball Return Point
    public Transform returnPoint2; // **DEPRECATED** Light Purple Ball Return Point
    public Transform returnPoint3; // **DEPRECATED** Turqoise Ball Return Point
    public GameObject Tools; // **DEPRECATED** Empty Object Containing All Tools Available
    public List<GameObject> toolList = new List<GameObject>(); // **DEPRECATED** List of Tools
    public GameObject audioTrigger; // Audio Trigger

    [Header("Debug Panel")]
    public int left1; // Number of Yellow Balls Left Active in Scene
    public int left2; // Number of Light Purple Balls Left Active in Scene
    public int left3; // Number of Turqoise Balls Left Active in Scene
    public GameObject debugText; // In Game Debug Panel

    [Header("Audio Clips")]
    [SerializeField] public AudioClip stage1Audio;
    [SerializeField] public AudioClip stage2Audio;
    [SerializeField] public AudioClip stage3Audio;
    [SerializeField] AudioClip correctBall;
    [SerializeField] public AudioClip wrongBall;
    [SerializeField] AudioClip nextStage;

    [Header("Materials")]
    [SerializeField] Material tubeBall1; // Yellow Material
    [SerializeField] Material tubeBall2; // Light Purple Material
    [SerializeField] Material tubeBall3; // Turqoise Material

    // Temporary Timer Variables
    float delayTimer;
    float startTime = 0;

    // Boolean for Gameplay
    public bool endOfGame = false;
    bool gameFailed = false;
    public bool dataRecorded = false;
    int isSkipped = 0;

    // Boolean to Load Data (Only Used Once after Start Function)
    bool isChecked = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Child Count: " + pileOfBalls.transform.childCount);

        // Add active balls to list
        for (int i = 0; i < pileOfBalls.transform.childCount; i++)
        {
            Balls.Add(pileOfBalls.transform.GetChild(i).gameObject);
        }

        // **DEPRECATED** Add Available Tools to List
        for (int i=0; i < Tools.transform.childCount; i++)
        {
            if (Tools.transform.GetChild(i).gameObject.activeSelf)
            {
                toolList.Add(Tools.transform.GetChild(i).gameObject);
            }
            
        }

        // Initialize In Game Debugger
        InGameDebugger();

        // Initialize Scoreboard Text
        scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString() +
                "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\nWrong Color: " + wrongColor.ToString() + "  Excess Balls: " + excessBalls.ToString() + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        // Load Data on First Frame
        if (!isChecked)
        {
            scoreUpdate();
            isChecked = true;
        }

        // Don't allow grab before audio is finished
        if (audioTrigger.GetComponent<AudioSource>().isPlaying == true)
        {
            foreach (GameObject tool in toolList)
            {
                tool.GetComponent<BNG.Grabbable>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject tool in toolList)
            {
                tool.GetComponent<BNG.Grabbable>().enabled = true;
            }
        }

        // Constantly Update In Game Debug Panel if used
        InGameDebugger();

        // ***TEST FEATURE*** Disable this Script after data is recorded (Used to write data only once)
        /*if (dataRecorded)
        {
            GetComponent<TubeScoreboard>().enabled = false;
            dataRecorded = false;
        }*/

        // Used for Stage Wait Time
        delayTimer += Time.deltaTime;

        // Moves onto next stage
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && delayTimer - startTime > 4.8f && delayTimer - startTime < 5.2f && !endOfGame)
        {
            Debug.Log("Timer Finished: " + delayTimer);
            StartCoroutine(stageClear());
            startTime = 0;
        }
        // End of Game
        else if (endOfGame)
        {
            if (!gameFailed) // Game Finish
            {
                scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString();
                RecordData(endOfGame, gameFailed);
                AddBreakPoint("Game Finish");
                dataRecorded = true;                
            }
            else // Too many balls lost
            {
                scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString();
                RecordData(endOfGame, gameFailed);
                AddBreakPoint("Too many balls lost");
                dataRecorded = true;
            }
        }
    }

    // Debugging Tool 1
    void InGameDebugger()
    {
        debugText.GetComponent<Text>().text = "Number of Balls: " + Balls.Count
            + "\n\nSuccess Balls 1: " + successBalls1.Count
            + "\n\nSuccess Balls 2: " + successBalls2.Count
            + "\n\nSuccess Balls 3: " + successBalls3.Count
            + "\n\nTotal Drops: " + totalDrops
            + "\n\nStage 1 Drops: " + stage1Drops
            + "\n\nStage 2 Drops: " + stage2Drops
            + "\n\nStage 3 Drops: " + stage3Drops
            + "\n\nActive Balls 1: " + activeBalls1.Count
            + "\n\nActive Balls 2: " + activeBalls2.Count
            + "\n\nActive Balls 3: " + activeBalls3.Count
            + "\n\nLost Balls 1: " + lostBalls1.Count
            + "\n\nLost Balls 2: " + lostBalls2.Count
            + "\n\nLost Balls 3: " + lostBalls3.Count
            + "\n\nLeft 1: " + left1
            + "\n\nLeft 2: " + left2
            + "\n\nLeft 3: " + left3
            + "\n\nIs Data Recorded? " + (dataRecorded ? "Yes" : "No")
            + "\n\nIs Game Failed?" + (gameFailed ? "Yes" : "No")
            + "\n\nIs it End of Game? " + (endOfGame ? "Yes" : "No");
    }

    // Debugging Tool 2
    void AddBreakPoint(string message)
    {
        debugText.GetComponent<Text>().text += "\n\n" + message; // Shows which condition was executed
    }

    // Check if Ball is Active
    void CheckBallActive()
    {
        // Checks for number of active balls
        foreach (GameObject ball in Balls)
        {
            // Check for active balls
            if (ball.activeSelf)
            {
                switch (ball.GetComponent<EasyTubeBall>().ballMatID)
                {
                    case 1:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall1 && !activeBalls1.Contains(ball))
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
                switch (ball.GetComponent<EasyTubeBall>().ballMatID)
                {
                    case 1:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall1 && !lostBalls1.Contains(ball))
                        {
                            lostBalls1.Add(ball);
                            if (activeBalls1.Contains(ball))
                            {
                                activeBalls1.Remove(ball);
                            }
                        }
                        break;
                    case 2:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall2 && !lostBalls2.Contains(ball))
                        {
                            lostBalls2.Add(ball);
                            if (activeBalls2.Contains(ball))
                            {
                                activeBalls2.Remove(ball);
                            }
                        }
                        break;
                    case 3:
                        if (ball.GetComponent<Renderer>().sharedMaterial == tubeBall3 && !lostBalls3.Contains(ball))
                        {
                            lostBalls3.Add(ball);
                            if (activeBalls3.Contains(ball))
                            {
                                activeBalls3.Remove(ball);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // Updates score on each ball collision
    void scoreUpdate()
    {
        // First Check if Ball is Active and Update Lists
        CheckBallActive();

        // Keep Track of Number of Active Balls
        left1 = activeBalls1.Count;
        left2 = activeBalls2.Count;
        left3 = activeBalls3.Count;

        if (!endOfGame)
        {
            // Updates Scoreboard Text
            scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString()
                + "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\nWrong Color: " + wrongColor.ToString() + "  Excess Balls: " + excessBalls.ToString() + "\n";

            // Fails if too many balls are lost (Each Condition is for each stage)
            if (stageCounter == 1 && (left1 < 6 || left2 < 6 || left3 < 6))
            {
                endOfGame = true;
                gameFailed = true;
                clearTime = timer.GetComponent<Text>().text;
                RecordStageClearTime(stageCounter);
                RecordStageDrops(stageCounter);
                RecordData(endOfGame, gameFailed);
                scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString() + "\n\n";
                AddBreakPoint("Fail in stage 1");
                dataRecorded = true;
            }
            else if (stageCounter == 2 && (left1 < 5 || left2 < 5 || left3 < 5))
            {
                endOfGame = true;
                gameFailed = true;
                clearTime = timer.GetComponent<Text>().text;
                RecordStageClearTime(stageCounter);
                RecordStageDrops(stageCounter);
                RecordData(endOfGame, gameFailed);
                scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString() + "\n\n";
                AddBreakPoint("Fail in stage 2");
                dataRecorded = true;
            }
            else if (stageCounter == 3 && (left1 < 3 || left2 < 3 || left3 < 3))
            {
                endOfGame = true;
                gameFailed = true;
                clearTime = timer.GetComponent<Text>().text;
                RecordStageClearTime(stageCounter);
                RecordStageDrops(stageCounter);
                RecordData(endOfGame, gameFailed);
                scoreText.GetComponent<Text>().text = "Failed\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "    Excess Balls: " + excessBalls.ToString() + "\n\n";
                AddBreakPoint("Fail in stage 3");
                dataRecorded = true;
            }
        }

        // Used for 5 second delay before moving onto next stage
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls)
        {
            startTime = delayTimer; 
        }
        else
        {
            startTime = 0;
        }

        Debug.Log("ScoreUpdate Function has been called");
    }

    // Update ball status on every collision
    public void ballUpdate(GameObject ball)
    {
        // Checks for
        // 1. Correct Ball Input
        // 2. Excess Balls
        // for each color tube
        switch (ball.GetComponent<EasyTubeBall>().ballMatID)
        {
            case 1:
                if (ball.GetComponent<EasyTubeBall>().ScoreCheck && !successBalls1.Contains(ball) && ball.activeSelf)
                {
                    if (score1 >= stageBalls)
                    {
                        ball.GetComponent<EasyTubeBall>().resetBall();
                        ball.SetActive(false);
                        excessBalls++;
                        PlaySound(wrongBall);
                    }
                    else
                    {
                        successBalls1.Add(ball);
                        score1++;
                        PlaySound(correctBall);
                    }
                }
                break;
            case 2:
                if (ball.GetComponent<EasyTubeBall>().ScoreCheck && !successBalls2.Contains(ball) && ball.activeSelf)
                {
                    if (score2 >= stageBalls)
                    {
                        ball.GetComponent<EasyTubeBall>().resetBall();
                        ball.SetActive(false);
                        excessBalls++;
                        PlaySound(wrongBall);
                    }
                    else
                    {
                        successBalls2.Add(ball);
                        score2++;
                        PlaySound(correctBall);
                    }
                }
                break;
            case 3:
                if (ball.GetComponent<EasyTubeBall>().ScoreCheck && !successBalls3.Contains(ball) && ball.activeSelf)
                {
                    if (score3 >= stageBalls)
                    {
                        ball.GetComponent<EasyTubeBall>().resetBall();
                        ball.SetActive(false);
                        excessBalls++;
                        PlaySound(wrongBall);
                    }
                    else
                    {
                        successBalls3.Add(ball);
                        score3++;
                        PlaySound(correctBall);
                    }
                }
                break;
            default:
                break;
        }

        // Updates Score After Ball State has changed
        scoreUpdate();
    }

    // Wait 5 seconds (Coroutine)
    IEnumerator Wait()
    {
        Debug.Log("Start Wait Coroutine");
        yield return new WaitForSeconds(5f);
        Debug.Log("Wait Coroutine Finished");
    }

    // This function is called when stage is cleared
    IEnumerator stageClear()
    {       
        // If score is 3, end game
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && stageCounter == 3)
        {
            PlaySound(nextStage);
            clearTime = timer.GetComponent<Text>().text;
            timer.SetActive(false);
            endOfGame = true;
            RecordStageClearTime(stageCounter);
            RecordStageDrops(stageCounter);
            RecordData(endOfGame, gameFailed);
            scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n" + WriteStageClearTime() + "\nWrong Color: " + wrongColor.ToString() + "\nExcess Balls: " + excessBalls.ToString() + "\n";
            AddBreakPoint("Successfully finished game");
            dataRecorded = true;

            yield return StartCoroutine(Wait());
            PlaySound(stage3Audio);
        }
        // If score is not 3, move onto next stage
        else if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls)
        {
            // Play Stage Clear Sound
            PlaySound(nextStage);

            scoreText.SetActive(false);
            waitMessage.SetActive(true);

            // Reset successfully moved balls
            foreach (GameObject ball in successBalls1)
            {
                ball.GetComponent<EasyTubeBall>().resetBall();
                ball.GetComponent<EasyTubeBall>().ScoreCheck = false;
                ball.SetActive(false);
            }
            foreach (GameObject ball in successBalls2)
            {
                ball.GetComponent<EasyTubeBall>().resetBall();
                ball.GetComponent<EasyTubeBall>().ScoreCheck = false;
                ball.SetActive(false);
            }
            foreach (GameObject ball in successBalls3)
            {
                ball.GetComponent<EasyTubeBall>().resetBall();
                ball.GetComponent<EasyTubeBall>().ScoreCheck = false;
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

            // Record Stage Data
            RecordStageClearTime(stageCounter);
            RecordStageDrops(stageCounter);

            // Reset variables for next stage
            stageCounter++;
            waitMessage.SetActive(false);
            scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString() + 
                "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\nWrong Color: " + wrongColor.ToString() + "  Excess Balls: " + excessBalls.ToString() + "\n";
            scoreText.SetActive(true);

            if (stageCounter == 2)
            {
                PlaySound(stage1Audio);
            }
            else if (stageCounter == 3)
            {
                PlaySound(stage2Audio);
            }
        }
    }

    void PlaySound(AudioClip sound)
    {
        GetComponent<AudioSource>().clip = sound;
        GetComponent<AudioSource>().Play();
    }

    // Reset all ball transforms
    public void ResetBalls()
    {
        Debug.Log("Reset Balls Function Called");

        foreach (GameObject ball in Balls)
        {
            ball.GetComponent<EasyTubeBall>().resetBall();
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
    void RecordData(bool end, bool failed)
    {
        string results = "";

        if(end)
        {
            if (failed)
            {
                results += "Failed: Y\n\n" + WriteStageDrops() + "Wrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n\nScoop Lost Count: " + scoopLost.ToString() + "\n";
            }
            else if (!failed)
            {
                results += "Failed: N\n\n" + WriteStageDrops() + WriteStageClearTime() + "\n\nWrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n\nScoop Lost Count: " + scoopLost.ToString() + "\n";
            }
        }
        else
        {
            results += "Failed: N\n\n" + WriteStageDrops() + WriteStageClearTime() + "\n\nWrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n\nScoop Lost Count: " + scoopLost.ToString() + 
                "\n\nTerminated(Stage " + stageCounter + ")\n";
        }

        GetComponent<SaveScoopData>().SaveTempSceneData(results); // Change location of this if necessary
    }

    // Record Stage Clear Time for Each Stage
    void RecordStageClearTime(int stage)
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

    // Write Stage Clear Time to File
    string WriteStageClearTime()
    {
        return "Stage 1 Clear Time: " + clearTime1.ToString() + "\nStage 2 Clear Time: " + clearTime2.ToString() + "\nStage 3 Clear time: " + clearTime3.ToString();
    }

    // Record Number of Drops for Each Stage
    void RecordStageDrops(int stage)
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

    // Write Number of Drops for Each Stage to File
    string WriteStageDrops()
    {
        return "Stage 1 Drops: " + stage1Drops.ToString() + "    Stage 2 Drops: " + stage2Drops.ToString() + "    Stage 3 Drops: " + stage3Drops.ToString() + "\n\n";
    }

    // Record Data (When Terminated) Change this when connecting scenes
    private void OnApplicationQuit()
    {
        if (!endOfGame)
        {
            RecordStageClearTime(stageCounter);
            RecordStageDrops(stageCounter);
            RecordData(endOfGame, gameFailed);
        }
    }

    public void SaveAndFinish(bool skipped)
    {
        if (skipped)
        {
            isSkipped = 1;

        }

        // Data variables goes here
        GetComponent<AutoVoiceRecording>().StopRecordingNBehavior();
    }
}
