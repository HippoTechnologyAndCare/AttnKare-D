using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Waiting,
    Stage1Start,
    Stage1,
    Stage1End,
    Stage2Start,
    Stage2,
    Stage2End,
    Stage3Start,
    Stage3,
    Stage3End,
    GameEnd
}

public class StageManager : MonoBehaviour
{
    public static GameState currentGameState = GameState.Waiting;
    public static int m_currentStage = 1;
    public static BNG.Toy.ToyType m_currentColor;
    public static int m_boxCount = 5;

    [SerializeField] FactoryManager m_factoryManager;
    [SerializeField] UIManager      m_UIManager;

    static List<int> m_stageColors;

    public delegate void StartStage(int stage);
    public static event StartStage stage;

    private void Start()
    {
        m_stageColors = new List<int>();
    }

    // Update is called once per frame
    void Update() => TrackGameState();

    void TrackGameState()
    {
        if (currentGameState == GameState.Stage1Start)
        {
            stage?.Invoke(1);
            GenerateColor();
            currentGameState = GameState.Stage1;
        }
        if (currentGameState == GameState.Stage2Start)
        {
            NextStage();
            stage?.Invoke(2);
            GenerateColor();
            currentGameState = GameState.Stage2;
            FactoryManager.ResetDestroyCount(1);
        }
        if (currentGameState == GameState.Stage3Start)
        {
            NextStage();
            stage?.Invoke(3);
            GenerateColor();
            currentGameState = GameState.Stage3;
            FactoryManager.ResetDestroyCount(2);
        }

        if (m_boxCount == 0)
        {
            if (currentGameState == GameState.Stage1) currentGameState = GameState.Stage1End;
            if (currentGameState == GameState.Stage2) currentGameState = GameState.Stage2End;
            if (currentGameState == GameState.Stage3) currentGameState = GameState.Stage3End;
        }

        if (!FactoryManager.m_gameData.IsDataSaved() && currentGameState == GameState.GameEnd)
        {
            NextStage();
            FactoryManager.ResetDestroyCount(3);
            m_factoryManager.SaveGameData();

            if (!FactoryManager.m_gameData.m_isSkipped)
                FactoryManager.LoadNextScene();
        }
    }

    public void GenerateColor()
    {
        int retVal;

        do{
            retVal = Random.Range(0, 3);
        } while (m_stageColors.Contains(retVal));

        if (retVal == 0)      m_currentColor = BNG.Toy.ToyType.Yellow;
        else if (retVal == 1) m_currentColor = BNG.Toy.ToyType.Blue;
        else if (retVal == 2) m_currentColor = BNG.Toy.ToyType.Green;

        m_stageColors.Add(retVal);

        UIManager.SetMainUIImage((int)m_currentColor);
        UIManager.EnableMainUIImage();
    }

    public static void ChangeGameState(GameState gameState) { currentGameState = gameState; }
    public static void BoxCountDec() { m_boxCount--; }
    public static void NextStage()   { m_currentStage++; m_boxCount = 5; }
    public static void AudioEnd(int index)
    {
        switch (index)
        {
            case 0: currentGameState = GameState.Stage1Start; break;
            case 1: currentGameState = GameState.Stage2Start; break;
            case 2: currentGameState = GameState.Stage3Start; break;
            case 3: currentGameState = GameState.GameEnd;     break;
        }
    }
}
