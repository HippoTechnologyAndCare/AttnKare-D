using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public class Debugger
{
    [Header("Score Debugger")]
    public List<int> stage1Score;
    public List<int> stage2Score;
    public List<int> stage3Score;
    public List<GameObject> m_grabbedList;
    public string gameState;
    public string stageColor;
}

public class FactoryManager : MonoBehaviour
{
    [SerializeField] StageManager m_stageManager;
    [SerializeField] AudioManager m_audioManager;
    [SerializeField] UIManager    m_UIManager;
    [SerializeField] PlayArea     m_playArea;

    [SerializeField] Text m_debuggerUI;                               // Debugger UI

    [SerializeField] BoxSpawner m_plainBoxSpawner;
    [SerializeField] BoxSpawner m_openBoxSpawner;
    [SerializeField] BoxSpawner m_closedBoxSpawner;

    [HideInInspector] public List<List<int>> m_stage1Score;
    [HideInInspector] public List<List<int>> m_stage2Score;
    [HideInInspector] public List<List<int>> m_stage3Score;

    public CollectibleData m_gameData;
    public static List<GameObject> m_grabbedList;

    [SerializeField] Debugger m_debugger;

    void DebugScore()
    {
        for (int i = 0; i < m_stage1Score.Count; i++) m_debugger.stage1Score[i] = m_stage1Score[i][0];
        for (int i = 0; i < m_stage2Score.Count; i++) m_debugger.stage2Score[i] = m_stage2Score[i][0];
        for (int i = 0; i < m_stage3Score.Count; i++) m_debugger.stage3Score[i] = m_stage3Score[i][0];

        m_debugger.gameState = Enum.GetName(typeof(GameState), StageManager.currentGameState);
        m_debugger.stageColor = StageManager.m_currentColor.ToString() + " / " + StageManager.m_currentColor + " / " + Enum.GetName(typeof(BNG.Toy.ToyType), StageManager.m_currentColor);
        m_debugger.m_grabbedList = m_grabbedList;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_gameData = new CollectibleData();
        m_stage1Score = new List<List<int>>(); m_stage2Score = new List<List<int>>(); m_stage3Score = new List<List<int>>();
        m_grabbedList = new List<GameObject>();

        // Debugger
        m_debugger.stage1Score = new List<int>(); m_debugger.stage2Score = new List<int>(); m_debugger.stage3Score = new List<int>();
        for (int i = 0; i < 5; i++)  { m_debugger.stage1Score.Add(-1); }
        for (int i = 0; i < 10; i++) { m_debugger.stage2Score.Add(-1); m_debugger.stage3Score.Add(-1); }
    }

    private void Update() { DebugScore(); }

    public static void AddToGrabbedList(GameObject toy) { if(!m_grabbedList.Contains(toy)) m_grabbedList.Add(toy); }                                                    // Called in Toy.cs
    public static void RemoveFromGrabbedList(GameObject toy) { if (m_grabbedList.Contains(toy)) m_grabbedList.Remove(toy); }                                            // Called in Box.cs
    public static void CheckMissing() { foreach (GameObject toy in m_grabbedList) if (toy == null) Debug.Log("Missing Object: " + m_grabbedList.IndexOf(toy)); }        // Called in Box.cs

    // If Box is in, make Box Spawner Spawn next Box
    // Called in Box.cs
    public void BoxIn(BoxType boxType)
    {
        if (boxType == BoxType.PlainBox) m_openBoxSpawner.SpawnNextBox();
        if (boxType == BoxType.OpenBox)  m_closedBoxSpawner.SpawnNextBox();
    }

    // Get Score of Box Instance (Called in Box.cs)
    public void GetScore(List<int> scores,int index)
    {
        switch(index)
        {
            case 1: m_stage1Score.Add(scores); break;
            case 2: m_stage2Score.Add(scores); break;
            case 3: m_stage3Score.Add(scores); break;
        }

        // Show Individual Box Scores on Debugger UI
        UpdateDebugText(scores);

        // Decrement Box Count
        if (StageManager.m_boxCount > 0) StageManager.BoxCountDec();
    }

    // Call this Function after Game Ends
    public void SaveGameData()
    {
        // Save Box Scores per Stage
        m_gameData.SetStage1Boxes(m_stage1Score);
        m_gameData.SetStage2Boxes(m_stage2Score);
        m_gameData.SetStage3Boxes(m_stage3Score);

        // Save Successful Boxes per Stage
        int boxCount = 0;
        for(int i = 0; i < m_stage1Score.Count; i++)  { if (m_stage1Score[i][0] == 1) boxCount++; }
        m_gameData.SetStage1Success(boxCount);  boxCount = 0;
        for (int i = 0; i < m_stage2Score.Count; i++) { if (m_stage2Score[i][0] == 1) boxCount++; }
        m_gameData.SetStage2Success(boxCount);  boxCount = 0;
        for (int i = 0; i < m_stage3Score.Count; i++) { if (m_stage3Score[i][0] == 1) boxCount++; }
        m_gameData.SetStage3Success(boxCount);

        // Save Play Area Data
        m_gameData.SetEscapeCount(m_playArea.GetEscapeCount());
        m_gameData.SetEscapeTime(m_playArea.GetEscapeTime());

        // Save Number of Toys on Floor
        m_gameData.SetToysOnFloor(m_grabbedList.Count);

        // Save Game Skipped Boolean

        // Change Game Data State as Saved
        m_gameData.SetDataSaved(true);

        string jsonOutput = JsonUtility.ToJson(m_gameData);
        Debug.Log(jsonOutput);
    }

    void UpdateDebugText(List<int> scores)
    {
        string _isSuccessful = scores[0] == 1 ? "Yes" : "No";

        UIManager.AddBoxDebuggerText(m_debuggerUI, "Is Successful? " + _isSuccessful);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "Excess",       scores[1]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "WrongColor",   scores[2]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "Total",        scores[3]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "");
    }
}

[Serializable]
public class CollectibleData
{
    public int m_stage1Success;
    public int m_stage2Success;
    public int m_stage3Success;

    public List<List<int>> m_stage1Score;
    public List<List<int>> m_stage2Score;
    public List<List<int>> m_stage3Score;

    public float m_escapeTime;
    public int m_escapeCount;

    public int m_toysOnFloor;

    public bool m_isSkipped;

    // Bool to Check if Data is Saved
    bool m_dataSaved;

    // Constructor
    public CollectibleData()
    {
        m_stage1Score = new List<List<int>>();
        m_stage2Score = new List<List<int>>();
        m_stage3Score = new List<List<int>>();
    }

    // Getters
    public int GetStage1Success() { return m_stage1Success; }
    public int GetStage2Success() { return m_stage2Success; }
    public int GetStage3Success() { return m_stage3Success; }
    public List<List<int>> GetStage1Boxes() { return m_stage1Score; }
    public List<List<int>> GetStage2Boxes() { return m_stage2Score; }
    public List<List<int>> GetStage3Boxes() { return m_stage3Score; }
    public float GetEscapeTime() { return m_escapeTime; }
    public int GetEscapeCount() { return m_escapeCount; }
    public int GetToysOnFloor() { return m_toysOnFloor; }
    public bool GetSkipped() { return m_isSkipped; }

    // Setters
    public void SetStage1Success(int val) { m_stage1Success = val; }
    public void SetStage2Success(int val) { m_stage2Success = val; }
    public void SetStage3Success(int val) { m_stage3Success = val; }
    public void SetStage1Boxes(List<List<int>> dataPacket) { m_stage1Score = dataPacket; }
    public void SetStage2Boxes(List<List<int>> dataPacket) { m_stage2Score = dataPacket; }
    public void SetStage3Boxes(List<List<int>> dataPacket) { m_stage3Score = dataPacket; }
    public void SetEscapeTime(float val) { m_escapeTime = val; }
    public void SetEscapeCount(int val) { m_escapeCount = val; }
    public void SetToysOnFloor(int val) { m_toysOnFloor = val; }
    public void SetSkipped(bool val) { m_isSkipped = val; }

    public bool IsDataSaved() { return m_dataSaved; }
    public void SetDataSaved(bool input) { m_dataSaved = input; }
}