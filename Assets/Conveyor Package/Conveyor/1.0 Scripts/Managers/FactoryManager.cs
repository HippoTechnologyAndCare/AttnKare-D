using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR 
using UnityEditor.SceneManagement;
#endif
using UserData;
using Newtonsoft.Json;

#region DEBUGGER CLASS
[Serializable]
public class Debugger
{
    [Header("Score Debugger")]
    public List<int> stage1Score;
    public int stage1Destroyed;
    public List<int> stage2Score;
    public int stage2Destroyed;
    public List<int> stage3Score;
    public int stage3Destroyed;
    public List<GameObject> m_grabbedList;
    public string gameState;
    public int currentStage;
    public float conveyorSpeed;
    public string stageColor;
}
#endregion

// Debugging & Data Management
public class FactoryManager : MonoBehaviour
{
    [SerializeField] StageManager m_stageManager;                       // StageManager.cs
    [SerializeField] AudioManager m_audioManager;                       // AudioManager.cs
    [SerializeField] UIManager m_UIManager;                             // UIManager.cs
    [SerializeField] BNG.CollectData m_collectData;                     // CollectData.cs
    [SerializeField] bool m_dataIsDictionary;                           // Should json data be in dictionary format?
    [SerializeField] PlayArea m_playArea;                               // PlayArea.cs

    [SerializeField] Text m_debuggerUI;                                 // Debugger UI

    [Header("BoxSpawner Manipulation")]
    [SerializeField] BoxSpawner m_plainBoxSpawner;
    [SerializeField] BoxSpawner m_openBoxSpawner;
    [SerializeField] BoxSpawner m_closedBoxSpawner;

    public static CollectibleData m_gameData;                           // Data to export
    public static List<GameObject> m_grabbedList;                       // number of toys(GameObject) grabbed by user
    public static int m_destroyCount;                                   // Number of Robots destroyed (Not Grabbed)
    static string m_json;                                               // json formatted string
    public Dictionary<int, string> collectibleData;

    [Header("Debugger")]
    [SerializeField] Debugger m_debugger;
    [HideInInspector] public List<List<int>> m_stage1Score;
    [HideInInspector] public List<List<int>> m_stage2Score;
    [HideInInspector] public List<List<int>> m_stage3Score;

    // Start is called before the first frame update
    void Start()
    {
        m_gameData = new CollectibleData();
        m_stage1Score = new List<List<int>>(); m_stage2Score = new List<List<int>>(); m_stage3Score = new List<List<int>>();
        m_grabbedList = new List<GameObject>();

        // Debugger
        m_debugger.stage1Score = new List<int>(); m_debugger.stage2Score = new List<int>(); m_debugger.stage3Score = new List<int>();
        for (int i = 0; i < 5; i++) 
        {
            m_debugger.stage1Score.Add(-1);
            m_debugger.stage2Score.Add(-1);
            m_debugger.stage3Score.Add(-1);
        }
    }

    // Update Debugger per frame
    private void Update() { DebugScore(); }

    // Save current state of CollectibleData on unexpected quit
    private void OnApplicationQuit() { if (!m_gameData.IsDataSaved()) SaveGameData(); }

    // Load Next Scene if:
    //      (i)  User pressed skip button
    //      (ii) User finished game
    public static void LoadNextScene()
    {
        Debug.Log("Load Next Scene Function Called");
        // Load Scene by Scene Index

    }

    // Update Debugger per frame
    void DebugScore()
    {
        for (int i = 0; i < m_stage1Score.Count; i++) m_debugger.stage1Score[i] = m_stage1Score[i][0];
        for (int i = 0; i < m_stage2Score.Count; i++) m_debugger.stage2Score[i] = m_stage2Score[i][0];
        for (int i = 0; i < m_stage3Score.Count; i++) m_debugger.stage3Score[i] = m_stage3Score[i][0];

        m_debugger.gameState = Enum.GetName(typeof(GameState), StageManager.currentGameState);
        m_debugger.stageColor = StageManager.m_currentColor.ToString() + " / " + StageManager.m_currentColor + " / " + Enum.GetName(typeof(BNG.Toy.ToyType), StageManager.m_currentColor);
        m_debugger.m_grabbedList = m_grabbedList;

        m_debugger.stage1Destroyed = StageManager.currentGameState == GameState.Stage1 ? m_destroyCount : m_gameData.m_stage1DestroyCnt;
        m_debugger.stage2Destroyed = StageManager.currentGameState == GameState.Stage2 ? m_destroyCount : m_gameData.m_stage2DestroyCnt;
        m_debugger.stage3Destroyed = StageManager.currentGameState == GameState.Stage3 ? m_destroyCount : m_gameData.m_stage3DestroyCnt;

        m_debugger.currentStage = StageManager.m_currentStage;
        m_debugger.conveyorSpeed = Conveyor.speed;
    }

    // Update debug text canvas
    void UpdateDebugText(List<int> scores)
    {
        string _isSuccessful = scores[0] == 1 ? "Yes" : "No";

        UIManager.AddBoxDebuggerText(m_debuggerUI, "Is Successful? " + _isSuccessful);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "Excess", scores[1]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "WrongColor", scores[2]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "Total", scores[3]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "");
    }

    #region REFERENCED METHODS
    public static void AddToGrabbedList(GameObject toy) { if (!m_grabbedList.Contains(toy)) m_grabbedList.Add(toy); }                                                   // Called in Toy.cs
    public static void RemoveFromGrabbedList(GameObject toy) { if (m_grabbedList.Contains(toy)) m_grabbedList.Remove(toy); }                                            // Called in Box.cs
    public static void CheckMissing() { foreach (GameObject toy in m_grabbedList) if (toy == null) Debug.Log("Missing Object: " + m_grabbedList.IndexOf(toy)); }        // Called in Box.cs
    
    // If Box is in, make Box Spawner Spawn next Box
    // Called in Box.cs
    public void BoxIn(BoxType boxType)
    {
        if (boxType == BoxType.PlainBox) m_openBoxSpawner.SpawnNextBox();
        if (boxType == BoxType.OpenBox) m_closedBoxSpawner.SpawnNextBox();
    }
    #endregion

    // Save destroy count for stage
    static void SetDestroyCnt(int stage)
    {
        switch (stage)
        {
            case 1: m_gameData.m_stage1DestroyCnt = m_destroyCount; break;
            case 2: m_gameData.m_stage2DestroyCnt = m_destroyCount; break;
            case 3: m_gameData.m_stage3DestroyCnt = m_destroyCount; break;
            default: break;
        }
    }

    // Reset destroy count on stage end
    public static void ResetDestroyCount(int stage) { SetDestroyCnt(stage); m_destroyCount = 0; }

    // Receive Data from Box Instance (Called in Box.cs)
    public void GetScore(List<int> scores, int index)
    {
        switch (index)
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

    #region DATA EXPORT METHODS
    // Save CollectibleData data as json
    public void SaveGameData()
    {
        // Save Box Scores per Stage
        /*m_gameData.SetStage1Boxes(m_stage1Score);
        m_gameData.SetStage2Boxes(m_stage2Score);
        m_gameData.SetStage3Boxes(m_stage3Score);*/

        m_gameData.m_stage1 = ParseList(m_stage1Score);
        m_gameData.m_stage2 = ParseList(m_stage2Score);
        m_gameData.m_stage3 = ParseList(m_stage3Score);

        // Save Successful Boxes per Stage
        int boxCount = 0;
        for (int i = 0; i < m_stage1Score.Count; i++) { if (m_stage1Score[i][0] == 1) boxCount++; }
        m_gameData.m_stage1Success = boxCount; boxCount = 0;
        for (int i = 0; i < m_stage2Score.Count; i++) { if (m_stage2Score[i][0] == 1) boxCount++; }
        m_gameData.m_stage2Success = boxCount; boxCount = 0;
        for (int i = 0; i < m_stage3Score.Count; i++) { if (m_stage3Score[i][0] == 1) boxCount++; }
        m_gameData.m_stage3Success = boxCount;

        // Save Play Area Data
        m_gameData.m_escapeCount = m_playArea.GetEscapeCount();
        m_gameData.m_escapeTime  = m_playArea.GetEscapeTime();

        // Save Number of Toys on Floor
        m_gameData.m_toysOnFloor = m_grabbedList.Count;

        // Save Game Skipped Boolean

        // Change Game Data State as Saved
        m_gameData.SetDataSaved(true);
        
        FormatJson();
        /*m_json = JsonConvert.SerializeObject(m_gameData, Formatting.Indented);*/

        ExportAsJson();

        m_collectData.SaveBehaviorData();

        Debug.Log("Game Data has been Saved!");
    }

    // Static Method to return json string
    public static string GetJsonData() { return m_json; }

    // Save json file to directory
    public void ExportAsJson() { File.WriteAllText(DataManager.GetInstance().FilePath_Folder + "Conveyor.json", m_json); }

    // Format Collectible to Json string
    void FormatJson()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;          // Current Scene index

#if UNITY_EDITOR
        sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
#endif

        for (int i = 1; i < 14; i++)
        {
            if(m_dataIsDictionary)
                m_json += "\"" + (sceneIndex * 100 + i).ToString() + "\": ";

            m_json += m_gameData.GetMemberVariableJson(i) + ",\n";
        }

        m_json = m_json.Remove(m_json.Length - 2, 1);

        if (m_dataIsDictionary)
            m_json = "{\n" + m_json + "}";
        else
            m_json = "[\n" + m_json + "]";
    }

    // Convert string to json format
    string ParseList(List<List<int>> listToParse)
    {
        string json = "";

        for (int i = 0; i < listToParse.Count; i++)
        {
            json += "[";
            for (int j = 0; j < listToParse[i].Count; j++)
            {
                json += listToParse[i][j].ToString() + ',';
            }
            if (json.Length > 0) json = json.Remove(json.Length - 1);
            json += "],";
        }

        if(json.Length > 0) json = json.Remove(json.Length - 1);
        json = "[" + json + "]";

        return json;
    }
    #endregion
}

#region JSONDATA COLLECTIBLE
[Serializable]
public class CollectibleData
{
    public int m_stage1Success;
    public int m_stage2Success;
    public int m_stage3Success;

    public int m_stage1DestroyCnt;
    public int m_stage2DestroyCnt;
    public int m_stage3DestroyCnt;

    public string m_stage1;
    public string m_stage2;
    public string m_stage3;

    /*public List<List<int>> m_stage1Score;
    public List<List<int>> m_stage2Score;
    public List<List<int>> m_stage3Score;*/

    public float m_escapeTime;
    public int m_escapeCount;

    public int m_toysOnFloor;

    public bool m_isSkipped = false;

    // Bool to Check if Data is Saved
    bool m_dataSaved;

    // Constructor
    /*public CollectibleData()
    {
        m_stage1Score = new List<List<int>>();
        m_stage2Score = new List<List<int>>();
        m_stage3Score = new List<List<int>>();
    }*/

    public string GetMemberVariableJson(int index)
    {
        switch (index)
        {
            case 1:  return JsonConvert.SerializeObject(ToJsonDataInt(1, m_stage1Success), Formatting.Indented);
            case 2:  return JsonConvert.SerializeObject(ToJsonDataInt(2, m_stage2Success), Formatting.Indented);
            case 3:  return JsonConvert.SerializeObject(ToJsonDataInt(3, m_stage3Success), Formatting.Indented);
            case 4:  return JsonConvert.SerializeObject(ToJsonDataInt(4, m_stage1DestroyCnt), Formatting.Indented);
            case 5:  return JsonConvert.SerializeObject(ToJsonDataInt(5, m_stage2DestroyCnt), Formatting.Indented);
            case 6:  return JsonConvert.SerializeObject(ToJsonDataInt(6, m_stage3DestroyCnt), Formatting.Indented);
            case 7:  return JsonConvert.SerializeObject(ToJsonDataString(7, m_stage1), Formatting.Indented);
            case 8:  return JsonConvert.SerializeObject(ToJsonDataString(8, m_stage2), Formatting.Indented);
            case 9:  return JsonConvert.SerializeObject(ToJsonDataString(9, m_stage3), Formatting.Indented);
            case 10: return JsonConvert.SerializeObject(ToJsonDataFloat(10, m_escapeTime), Formatting.Indented);
            case 11: return JsonConvert.SerializeObject(ToJsonDataInt(11, m_escapeCount), Formatting.Indented);
            case 12: return JsonConvert.SerializeObject(ToJsonDataInt(12, m_toysOnFloor), Formatting.Indented);
            case 13: return JsonConvert.SerializeObject(ToJsonDataInt(13, m_isSkipped ? 1 : 0), Formatting.Indented);
            default: return "";
        }
    }


    // Getters
    /*public int GetStage1Success() { return m_stage1Success; }
    public int GetStage2Success() { return m_stage2Success; }
    public int GetStage3Success() { return m_stage3Success; }
    public int GetStage1DestroyCnt() { return m_stage1DestroyCnt; }
    public int GetStage2DestroyCnt() { return m_stage2DestroyCnt; }
    public int GetStage3DestroyCnt() { return m_stage3DestroyCnt; }
    public string GetStage1() { return m_stage1; }
    public string GetStage2() { return m_stage2; }
    public string GetStage3() { return m_stage3; }
    public float GetEscapeTime() { return m_escapeTime; }
    public int GetEscapeCount() { return m_escapeCount; }
    public int GetToysOnFloor() { return m_toysOnFloor; }
    public bool GetSkipped() { return m_isSkipped; }*/

    // Setters
    /*public void SetStage1Success(int val) { m_stage1Success = val; }
    public void SetStage2Success(int val) { m_stage2Success = val; }
    public void SetStage3Success(int val) { m_stage3Success = val; }
    public void SetStage1DestroyCnt(int val) { m_stage1DestroyCnt = val; }
    public void SetStage2DestroyCnt(int val) { m_stage2DestroyCnt = val; }
    public void SetStage3DestroyCnt(int val) { m_stage3DestroyCnt = val; }
    public void SetStage1(string str) { m_stage1 = str; }
    public void SetStage2(string str) { m_stage2 = str; }
    public void SetStage3(string str) { m_stage3 = str; }
    public void SetEscapeTime(float val) { m_escapeTime = val; }
    public void SetEscapeCount(int val) { m_escapeCount = val; }
    public void SetToysOnFloor(int val) { m_toysOnFloor = val; }
    public void SetSkipped(bool val) { m_isSkipped = val; }*/

    public bool IsDataSaved() { return m_dataSaved; }
    public void SetDataSaved(bool input) { m_dataSaved = input; }

    public JsonDataInt ToJsonDataInt(int dataNum, int var)
    {
        JsonDataInt jsonDataField = new JsonDataInt();

        int sceneIndex;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
#if UNITY_EDITOR
        sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
#endif

        jsonDataField.ID = sceneIndex * 100 + dataNum;
        jsonDataField.Result = var;

        return jsonDataField;
    }

    public JsonDataFloat ToJsonDataFloat(int dataNum, float var)
    {
        JsonDataFloat jsonDataField = new JsonDataFloat();

        int sceneIndex;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
#if UNITY_EDITOR
        sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
#endif

        jsonDataField.ID = sceneIndex * 100 + dataNum;
        jsonDataField.Result = var;

        return jsonDataField;
    }

    public JsonDataString ToJsonDataString(int dataNum, string var)
    {
        JsonDataString jsonDataField = new JsonDataString();

        int sceneIndex;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
#if UNITY_EDITOR
        sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
#endif

        jsonDataField.ID = sceneIndex * 100 + dataNum;
        jsonDataField.Result = var;

        return jsonDataField;
    }
}
#endregion

#region JSON_INT CLASS
[Serializable]
public class JsonDataInt
{
    public int ID;
    public int Result;
}
#endregion

#region JSON_FLOAT CLASS
[Serializable]
public class JsonDataFloat
{
    public int ID;
    public float Result;
}
#endregion

#region JSON_STRING CLASS
[Serializable]
public class JsonDataString
{
    public int ID;
    public string Result;
}
#endregion