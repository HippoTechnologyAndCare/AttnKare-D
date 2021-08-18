using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using KetosGames.SceneTransition;

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
    public GameObject sceneText;
    public int totalDrops = 0; // Total number of drops throughout game
    public string clearTime = ""; // Clear Time
    [HideInInspector] public int stage1Drops = 0; // Stage 1 Drops
    [HideInInspector] public int stage2Drops = 0; // Stage 2 Drops
    [HideInInspector] public int stage3Drops = 0; // Stage 3 Drops
    [HideInInspector] public string clearTime1 = ""; // Stage 1 Clear Time
    [HideInInspector] public string clearTime2 = ""; // Stage 2 Clear Time
    [HideInInspector] public string clearTime3 = ""; // Stage 3 Clear Time
    [HideInInspector] public float time1 = -1f; // Stage 1 Clear Time (float)
    [HideInInspector] public float time2 = -1f; // Stage 2 Clear Time (float)
    [HideInInspector] public float time3 = -1f; // Stage 3 Clear Time (float)
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
    [SerializeField] public GameObject voices;
    [SerializeField] public GameObject soundEffects;
    [SerializeField] public GameObject stageAudio;

    [Header("Materials")]
    [SerializeField] Material tubeBall1; // Yellow Material
    [SerializeField] Material tubeBall2; // Light Purple Material
    [SerializeField] Material tubeBall3; // Turqoise Material

    [Header("Data")]
    [SerializeField] Transform setData_PlayerData;
    [SerializeField] Transform saveData_GameDataMG;

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
        /*Debug.Log("Child Count: " + pileOfBalls.transform.childCount);*/

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
        scoreText.GetComponent<Text>().text = stageCounter + " 단계\n\n노란색: " + score1.ToString() + "    연보라색: " + score2.ToString() + "    청록색: " + score3.ToString() +
                "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\n";
    }

    // Update is called once per frame
    void Update()
    {
        // Load Data on First Frame
        if (!isChecked)
        {
            foreach (GameObject ball in Balls)
            {
                CheckBallActive(ball);
            }

            left1 = activeBalls1.Count;
            left2 = activeBalls2.Count;
            left3 = activeBalls3.Count;

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
        if (dataRecorded)
        {
            StartCoroutine(GoToLobby(false));
            dataRecorded = false;
            endOfGame = false;
        }

        // Used for Stage Wait Time
        delayTimer += Time.deltaTime;

        // Moves onto next stage
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && delayTimer - startTime > 4.8f && delayTimer - startTime < 5.2f && !endOfGame)
        {
            /*Debug.Log("Timer Finished: " + delayTimer);*/
            StartCoroutine(stageClear());
            startTime = 0;
        }
        // End of Game
        else if (endOfGame)
        {
            if (!gameFailed) // Game Finish
            {
                scoreText.GetComponent<Text>().text = "성공!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n";
                RecordData(endOfGame, gameFailed);
                AddBreakPoint("Game Finish");
                dataRecorded = true;                
            }
            else // Too many balls lost
            {
                scoreText.GetComponent<Text>().text = "실패!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n";
                soundEffects.GetComponent<SoundEffects>().WrongBall();
                RecordData(endOfGame, gameFailed);
                AddBreakPoint("Too many balls lost");                
                dataRecorded = true;
            }
        }
    }

    IEnumerator GoToLobby(bool isSkipped)
    {
        yield return new WaitForSeconds(7);

        scoreText.GetComponent<Text>().enabled = false;

        sceneText.GetComponent<Text>().text = "이동합니다";
        yield return new WaitForSeconds(2);

        sceneText.GetComponent<Text>().text = "3";
        yield return new WaitForSeconds(1);

        sceneText.GetComponent<Text>().text = "2";
        yield return new WaitForSeconds(1);

        sceneText.GetComponent<Text>().text = "1";
        yield return new WaitForSeconds(1);

        SaveAndFinish(isSkipped);

        SceneLoader.LoadScene(10);
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
    void CheckBallActive(GameObject ball)
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

    // Updates score on each ball collision
    void scoreUpdate(GameObject ball)
    {
        // First Check if Ball is Active and Update Lists
        CheckBallActive(ball);

        // Keep Track of Number of Active Balls
        left1 = activeBalls1.Count;
        left2 = activeBalls2.Count;
        left3 = activeBalls3.Count;

        if (!endOfGame)
        {
            // Updates Scoreboard Text
            scoreText.GetComponent<Text>().text = stageCounter + " 단계\n\n노란색: " + score1.ToString() + "    연보라색: " + score2.ToString() + "    청록색: " + score3.ToString()
                + "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\n";

            // Fails if too many balls are lost (Each Condition is for each stage)
            if (stageCounter == 1 && (left1 < 6 || left2 < 6 || left3 < 6))
            {
                endOfGame = true;
                gameFailed = true;
                clearTime = timer.GetComponent<Text>().text;
                soundEffects.GetComponent<SoundEffects>().WrongBall();
                RecordStageClearTime(stageCounter);
                RecordStageDrops(stageCounter);
                RecordData(endOfGame, gameFailed);
                scoreText.GetComponent<Text>().text = "실패!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n";
                AddBreakPoint("Fail in stage 1");                
                dataRecorded = true;
            }
            else if (stageCounter == 2 && (left1 < 5 || left2 < 5 || left3 < 5))
            {
                endOfGame = true;
                gameFailed = true;
                clearTime = timer.GetComponent<Text>().text;
                soundEffects.GetComponent<SoundEffects>().WrongBall();
                RecordStageClearTime(stageCounter);
                RecordStageDrops(stageCounter);
                RecordData(endOfGame, gameFailed);
                scoreText.GetComponent<Text>().text = "실패!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n";
                AddBreakPoint("Fail in stage 2");
                dataRecorded = true;
            }
            else if (stageCounter == 3 && (left1 < 3 || left2 < 3 || left3 < 3))
            {
                endOfGame = true;
                gameFailed = true;
                clearTime = timer.GetComponent<Text>().text;
                soundEffects.GetComponent<SoundEffects>().WrongBall();
                RecordStageClearTime(stageCounter);
                RecordStageDrops(stageCounter);
                RecordData(endOfGame, gameFailed);
                scoreText.GetComponent<Text>().text = "실패!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\n";
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
                        soundEffects.GetComponent<SoundEffects>().WrongBall();
                    }
                    else
                    {
                        successBalls1.Add(ball);
                        score1++;
                        soundEffects.GetComponent<SoundEffects>().CorrectBall();
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
                        soundEffects.GetComponent<SoundEffects>().WrongBall();
                    }
                    else
                    {
                        successBalls2.Add(ball);
                        score2++;
                        soundEffects.GetComponent<SoundEffects>().CorrectBall();
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
                        soundEffects.GetComponent<SoundEffects>().WrongBall();
                    }
                    else
                    {
                        successBalls3.Add(ball);
                        score3++;
                        soundEffects.GetComponent<SoundEffects>().CorrectBall();
                    }
                }
                break;
            default:
                break;
        }

        // Updates Score After Ball State has changed
        scoreUpdate(ball);
    }

    // Wait 5 seconds (Coroutine)
    IEnumerator Wait()
    {
        /*Debug.Log("Start Wait Coroutine");*/
        yield return new WaitForSeconds(5f);
        /*Debug.Log("Wait Coroutine Finished");*/
    }

    // This function is called when stage is cleared
    IEnumerator stageClear()
    {       
        // If score is 3, end game
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && stageCounter == 3)
        {
            stageAudio.GetComponent<StageAudio>().NextStage();
            clearTime = timer.GetComponent<Text>().text;
            timer.SetActive(false);
            endOfGame = true;
            RecordStageClearTime(stageCounter);
            RecordStageDrops(stageCounter);
            RecordData(endOfGame, gameFailed);
            scoreText.GetComponent<Text>().text = "성공!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n" + WriteStageClearTime() + "\n";
            AddBreakPoint("Successfully finished game");
            dataRecorded = true;

            yield return StartCoroutine(Wait());
            voices.GetComponent<Voice>().Stage3Finish();
        }
        // If score is not 3, move onto next stage
        else if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls)
        {
            // Play Stage Clear Sound
            stageAudio.GetComponent<StageAudio>().NextStage();

            scoreText.SetActive(false);
            waitMessage.SetActive(true);

            // Reset successfully moved balls
            foreach (GameObject ball in successBalls1)
            {
                ball.GetComponent<EasyTubeBall>().resetBall();
                ball.GetComponent<EasyTubeBall>().ScoreCheck = false;
                ball.SetActive(false);
                ballUpdate(ball);
            }
            foreach (GameObject ball in successBalls2)
            {
                ball.GetComponent<EasyTubeBall>().resetBall();
                ball.GetComponent<EasyTubeBall>().ScoreCheck = false;
                ball.SetActive(false);
                ballUpdate(ball);
            }
            foreach (GameObject ball in successBalls3)
            {
                ball.GetComponent<EasyTubeBall>().resetBall();
                ball.GetComponent<EasyTubeBall>().ScoreCheck = false;
                ball.SetActive(false);
                ballUpdate(ball);
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
            scoreText.GetComponent<Text>().text = stageCounter + " 단계\n\n노란색: " + score1.ToString() + "    연보라색: " + score2.ToString() + "    청록색: " + score3.ToString() +
                "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n";
            scoreText.SetActive(true);

            if (stageCounter == 2)
            {
                voices.GetComponent<Voice>().Stage1Finish();
            }
            else if (stageCounter == 3)
            {
                voices.GetComponent<Voice>().Stage2Finish();
            }
        }
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

    // Record Game Score
    void RecordData(bool end, bool failed)
    {
        string results = "";

        if(end)
        {
            if (failed)
            {
                results += "Failed: Y\n\n" + WriteStageDrops() + "Wrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n";
            }
            else if (!failed)
            {
                results += "Failed: N\n\n" + WriteStageDrops() + WriteStageClearTime() + "\n\nWrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n";
            }
        }
        else
        {
            results += "Failed: N\n\n" + WriteStageDrops() + WriteStageClearTime() + "\n\nWrong Color: " + wrongColor.ToString() + "\n\nExcess Balls: " + excessBalls.ToString() + "\n\nTerminated(Stage " + stageCounter + ")\n";
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
                time1 = timer.GetComponent<Timer>().secondsCount;
                break;
            case 2:
                clearTime2 = timer.GetComponent<Text>().text;
                time2 = timer.GetComponent<Timer>().secondsCount;
                break;
            case 3:
                clearTime3 = timer.GetComponent<Text>().text;
                time3 = timer.GetComponent<Timer>().secondsCount;
                break;
            default:
                break;
        }
    }

    // Write Stage Clear Time to File
    string WriteStageClearTime()
    {
        return "1 단계 완료시간: " + clearTime1.ToString() + "\n2 단계 완료시간: " + clearTime2.ToString() + "\n3 단계 완료시간: " + clearTime3.ToString() + "\n\n";
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
        return "1 단계에서 떨어뜨린 횟수: " + stage1Drops.ToString() + "\n2 단계에서 떨어뜨린 횟수: " + stage2Drops.ToString() + "\n3 단계에서 떨어뜨린 횟수: " + stage3Drops.ToString() + "\n\n";
    }

    // Record Data (When Terminated) Change this when connecting scenes
    private void OnApplicationQuit()
    {
        if (!endOfGame)
        {
            RecordStageClearTime(stageCounter);
            RecordStageDrops(stageCounter);
            RecordData(endOfGame, gameFailed);
            SaveAndFinish(false);
        }
    }

    // Change to json
    public void SaveAndFinish(bool skipped)
    {
        if (skipped)
        {
            isSkipped = 1;

        }

        // Save Data to local 
        setData_PlayerData.GetComponent<SetPlayerData>().GetSceneIndex4(time1, time2, time3, stage1Drops, stage2Drops, stage3Drops, wrongColor, excessBalls, gameFailed ? 1f : 0f, isSkipped);
        saveData_GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();

        // Data variables go here
        GetComponent<AutoVoiceRecording>().StopRecordingNBehavior();
    }
}
