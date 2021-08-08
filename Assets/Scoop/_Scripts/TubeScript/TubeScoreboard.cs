using System.Collections;
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
    private int score1 = 0; // Yellow Ball
    private int score2 = 0; // Light Purple Ball
    private int score3 = 0; // Turqoise Ball
    private float stageCounter = 1; // Stage number
    private int stageDrops = 0; // Number of Drops after stage is cleared, updated after each stage finishes
    [SerializeField] private int stageBalls = 1;

    [Header("Prefabs and Objects")]
    public Transform clone; // Ball prefab // delete this
    public GameObject timer; // Timer Text
    public GameObject waitMessage;
    public GameObject pileOfBalls;

    [Header("Tubes")]
    [SerializeField] GameObject glassTube1;
    [SerializeField] GameObject glassTube2;
    [SerializeField] GameObject glassTube3;

    [Header("Materials")]
    [SerializeField] Material tubeBall1;
    [SerializeField] Material tubeBall2;
    [SerializeField] Material tubeBall3;

    float delayTimer;
    float startTime = 0;
    public bool endOfGame = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Child Count: " + pileOfBalls.transform.childCount);
        for(int i = 0; i < pileOfBalls.transform.childCount; i++)
        {
            Balls.Add(pileOfBalls.transform.GetChild(i).gameObject);
        }
        scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString() + "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\n";
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;

        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && delayTimer - startTime > 4.8f && delayTimer - startTime < 5.2f && !endOfGame)
        {
            Debug.Log("Timer Finished: " + delayTimer);
            StartCoroutine(stageClear());
            startTime = 0;
        }
    }

    public void setBallsVisible(bool isVisible)
    {
        foreach (GameObject ball in Balls)
        {
            ball.GetComponent<Renderer>().enabled = isVisible;
        }
    }

    // Updates score on each ball collision
    public void scoreUpdate()
    {
        // Calls Update function every time any ball collides with environment
        foreach (GameObject ball in Balls)
        {
            ballUpdate(ball);
        }

        // Updates Scoreboard Text
        scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString() + "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\n";
        /*scoreBoard.text = "Stage " + stageCounter + "\n\n남은 공: " + (clonedBalls.Count - score).ToString() + " 개\n\nDrops: " + totalDrops.ToString();*/

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

    // Update ball status
    void ballUpdate(GameObject ball)
    {
        switch (ball.GetComponent<TubeBall>().ballMatID)
        {
            case 1:
                if (ball.GetComponent<TubeBall>().ScoreCheck && !successBalls1.Contains(ball))
                {
                    if(score1 == stageBalls)
                    {
                        ball.GetComponent<TubeBall>().resetBall();
                        ball.SetActive(false);
                    }
                    else
                    {
                        successBalls1.Add(ball);
                        score1++;
                    }
                    
                }
                else if (!ball.GetComponent<TubeBall>().ScoreCheck && successBalls1.Contains(ball) && score1 > 0)
                {
                    successBalls1.Remove(ball);
                    score1--;
                }
                break;
            case 2:
                if (ball.GetComponent<TubeBall>().ScoreCheck && !successBalls2.Contains(ball))
                {
                    if (score2 == stageBalls)
                    {
                        ball.GetComponent<TubeBall>().resetBall();
                        ball.SetActive(false);
                    }
                    else
                    {
                        successBalls2.Add(ball);
                        score2++;
                    }
                }
                else if (!ball.GetComponent<TubeBall>().ScoreCheck && successBalls2.Contains(ball) && score2 > 0)
                {
                    successBalls2.Remove(ball);
                    score2--;
                }
                break;
            case 3:
                if (ball.GetComponent<TubeBall>().ScoreCheck && !successBalls3.Contains(ball))
                {
                    if (score3 == stageBalls)
                    {
                        ball.GetComponent<TubeBall>().resetBall();
                        ball.SetActive(false);
                    }
                    else
                    {
                        successBalls3.Add(ball);
                        score3++;
                    }
                }
                else if (!ball.GetComponent<TubeBall>().ScoreCheck && successBalls3.Contains(ball) && score3 > 0)
                {
                    successBalls3.Remove(ball);
                    score3--;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator Wait()
    {
        Debug.Log("Start Wait Coroutine");
        yield return new WaitForSeconds(5f);
        Debug.Log("Wait Coroutine Finished");
    }

    // Function called when stage is cleared
    IEnumerator stageClear()
    {
        stageDrops = totalDrops;
        
        // Wait 5 seconds after successfully moving all balls
        /*yield return StartCoroutine(Wait5());*/

        // If any ball escapes container before 5 second countdown, break out of this function
        /*if (clonedBalls.Count != score)
        {
            yield break;
        }*/
        
        // If score is 3, end game
        if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls && stageBalls == 3)
        {
            clearTime = timer.GetComponent<Text>().text;
            /*foreach (GameObject ball in Balls)
            {
                Destroy(ball);
            }*/
            scoreText.GetComponent<Text>().text = "Finish!\n\n떨어뜨린 공: " + totalDrops.ToString() + "\n\nClear Time: " + clearTime;
            /*scoreBoard.text = "Finish!\n\nDrops: " + totalDrops.ToString() + "\n\nClear Time: " + clearTime;*/
            timer.SetActive(false);
            endOfGame = true;
        }
        // If score is not 10, move onto next stage
        else if (successBalls1.Count == stageBalls && successBalls2.Count == stageBalls && successBalls3.Count == stageBalls)
        {
            scoreText.SetActive(false);
            waitMessage.SetActive(true);

            foreach (GameObject ball in successBalls1)
            {
                ball.GetComponent<TubeBall>().resetBall();
                ball.GetComponent<TubeBall>().ScoreCheck = false;
            }
            foreach (GameObject ball in successBalls2)
            {
                ball.GetComponent<TubeBall>().resetBall();
                ball.GetComponent<TubeBall>().ScoreCheck = false;
            }
            foreach (GameObject ball in successBalls3)
            {
                ball.GetComponent<TubeBall>().resetBall();
                ball.GetComponent<TubeBall>().ScoreCheck = false;
            }

            successBalls1.Clear();
            successBalls2.Clear();
            successBalls3.Clear();

            score1 = 0;
            score2 = 0;
            score3 = 0;

            yield return StartCoroutine(Wait());

            stageBalls++;
            stageCounter++;
            waitMessage.SetActive(false);
            scoreText.GetComponent<Text>().text = "Stage " + stageCounter + "\n\nYellow: " + score1.ToString() + "    Light Purple: " + score2.ToString() + "    Turqoise: " + score3.ToString() + "\n\n떨어뜨린 공: " + totalDrops.ToString() + "개\n\n";
            scoreText.SetActive(true);
        }
    }

    // Function to reset stage, when unintentional overlapping of ball objects occur and drop count is incremented
    /*public void errorCheck()
    {
        Debug.Log("Error Check Function Called");
        ResetBalls();
    }*/

    // Reset all balls to random position in container
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

    // When game is terminated, record data
    private void OnApplicationQuit()
    {
        if(clearTime != "")
        {
            GetComponent<SaveScoopData>().SaveTempSceneData("Drops: " + totalDrops.ToString() + "\n\nClear Time: " + clearTime + "\n");
        }
        else
        {
            GetComponent<SaveScoopData>().SaveTempSceneData("Drops: " + totalDrops.ToString() + "\n\nTerminated(Stage " + stageCounter + ")\n");
        }
        
    }
}
