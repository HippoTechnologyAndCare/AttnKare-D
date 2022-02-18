using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UserData;

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
    public string stageColor;
}
// Debugging & Data Management
public class FactoryManager : MonoBehaviour
{
    [SerializeField] StageManager m_stageManager;
    [SerializeField] AudioManager m_audioManager;
    [SerializeField] UIManager m_UIManager;
    [SerializeField] PlayArea m_playArea;

    [SerializeField] Text m_debuggerUI;                               // Debugger UI

    [SerializeField] BoxSpawner m_plainBoxSpawner;
    [SerializeField] BoxSpawner m_openBoxSpawner;
    [SerializeField] BoxSpawner m_closedBoxSpawner;

    /*[SerializeField] Conveyor m_mainConveyor;
    [SerializeField] Transform m_spawnedToys;*/

    [HideInInspector] public List<List<int>> m_stage1Score;
    [HideInInspector] public List<List<int>> m_stage2Score;
    [HideInInspector] public List<List<int>> m_stage3Score;

    public static CollectibleData m_gameData;
    public static List<GameObject> m_grabbedList;
    public static int m_destroyCount;
    static string m_json;

    [SerializeField] Debugger m_debugger;

    public static string GetJsonData() { return m_json; }

    // Set Path for Json Export
    public void ExportAsJson() { File.WriteAllText(DataManager.GetInstance().FilePath_Folder + "Conveyor.json", m_json); }

    void DebugScore()
    {
        for (int i = 0; i < m_stage1Score.Count; i++) m_debugger.stage1Score[i] = m_stage1Score[i][0];
        for (int i = 0; i < m_stage2Score.Count; i++) m_debugger.stage2Score[i] = m_stage2Score[i][0];
        for (int i = 0; i < m_stage3Score.Count; i++) m_debugger.stage3Score[i] = m_stage3Score[i][0];

        m_debugger.gameState = Enum.GetName(typeof(GameState), StageManager.currentGameState);
        m_debugger.stageColor = StageManager.m_currentColor.ToString() + " / " + StageManager.m_currentColor + " / " + Enum.GetName(typeof(BNG.Toy.ToyType), StageManager.m_currentColor);
        m_debugger.m_grabbedList = m_grabbedList;

        m_debugger.stage1Destroyed = StageManager.currentGameState == GameState.Stage1 ? m_destroyCount : m_gameData.GetStage1DestroyCnt();
        m_debugger.stage2Destroyed = StageManager.currentGameState == GameState.Stage2 ? m_destroyCount : m_gameData.GetStage2DestroyCnt();
        m_debugger.stage3Destroyed = StageManager.currentGameState == GameState.Stage3 ? m_destroyCount : m_gameData.GetStage3DestroyCnt();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_gameData = new CollectibleData();
        m_stage1Score = new List<List<int>>(); m_stage2Score = new List<List<int>>(); m_stage3Score = new List<List<int>>();
        m_grabbedList = new List<GameObject>();

        // Debugger
        m_debugger.stage1Score = new List<int>(); m_debugger.stage2Score = new List<int>(); m_debugger.stage3Score = new List<int>();
        for (int i = 0; i < 5; i++) { m_debugger.stage1Score.Add(-1); }
        for (int i = 0; i < 10; i++) { m_debugger.stage2Score.Add(-1); m_debugger.stage3Score.Add(-1); }
    }

    private void Update() { DebugScore(); }
    private void OnApplicationQuit() { if(!m_gameData.IsDataSaved()) SaveGameData(); }

    public static void AddToGrabbedList(GameObject toy) { if (!m_grabbedList.Contains(toy)) m_grabbedList.Add(toy); }                                                    // Called in Toy.cs
    public static void RemoveFromGrabbedList(GameObject toy) { if (m_grabbedList.Contains(toy)) m_grabbedList.Remove(toy); }                                            // Called in Box.cs
    public static void CheckMissing() { foreach (GameObject toy in m_grabbedList) if (toy == null) Debug.Log("Missing Object: " + m_grabbedList.IndexOf(toy)); }        // Called in Box.cs
    public static void ResetDestroyCount(int stage) { SetDestroyCnt(stage); m_destroyCount = 0; }

    static void SetDestroyCnt(int stage)
    {
        switch (stage)
        {
            case 1: m_gameData.SetStage1DestroyCnt(m_destroyCount); break;
            case 2: m_gameData.SetStage2DestroyCnt(m_destroyCount); break;
            case 3: m_gameData.SetStage3DestroyCnt(m_destroyCount); break;
            default: break;
        }
    }

    /*public void RestartFactory()
    {
        m_mainConveyor.enabled = true;

        for (int i = 0; i < m_spawnedToys.childCount; i++)
        {
            m_spawnedToys.GetChild(i).GetComponent<BNG.Toy>().OnEnter();
        }
    }
    public void StopFactory()
    {
        m_mainConveyor.enabled = false;

        for(int i = 0; i < m_spawnedToys.childCount; i++)
        {
            m_spawnedToys.GetChild(i).GetComponent<BNG.Toy>().OnEscape();
        }
    }*/


    // If Box is in, make Box Spawner Spawn next Box
    // Called in Box.cs
    public void BoxIn(BoxType boxType)
    {
        if (boxType == BoxType.PlainBox) m_openBoxSpawner.SpawnNextBox();
        if (boxType == BoxType.OpenBox) m_closedBoxSpawner.SpawnNextBox();
    }

    // Get Score of Box Instance (Called in Box.cs)
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

    // Call this Function after Game Ends
    public void SaveGameData()
    {
        // Save Box Scores per Stage
        /*m_gameData.SetStage1Boxes(m_stage1Score);
        m_gameData.SetStage2Boxes(m_stage2Score);
        m_gameData.SetStage3Boxes(m_stage3Score);*/

        m_gameData.SetStage1(ParseList(m_stage1Score));
        m_gameData.SetStage2(ParseList(m_stage2Score));
        m_gameData.SetStage3(ParseList(m_stage3Score));

        // Save Successful Boxes per Stage
        int boxCount = 0;
        for (int i = 0; i < m_stage1Score.Count; i++) { if (m_stage1Score[i][0] == 1) boxCount++; }
        m_gameData.SetStage1Success(boxCount); boxCount = 0;
        for (int i = 0; i < m_stage2Score.Count; i++) { if (m_stage2Score[i][0] == 1) boxCount++; }
        m_gameData.SetStage2Success(boxCount); boxCount = 0;
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

        FormatJson();

        ExportAsJson();
    }

    void FormatJson()
    {
        m_json += "[\n";

        for (int i = 1; i < 14; i++)
        {
            m_json += m_gameData.GetMemberVariableJson(i) + ",\n";
        }

        m_json = m_json.Remove(m_json.Length - 2, 1);
        m_json += "]";
    }
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

    void UpdateDebugText(List<int> scores)
    {
        string _isSuccessful = scores[0] == 1 ? "Yes" : "No";

        UIManager.AddBoxDebuggerText(m_debuggerUI, "Is Successful? " + _isSuccessful);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "Excess", scores[1]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "WrongColor", scores[2]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "Total", scores[3]);
        UIManager.AddBoxDebuggerText(m_debuggerUI, "");
    }
}

[Serializable]
public class CollectibleData
{
    int m_stage1Success;
    int m_stage2Success;
    int m_stage3Success;

    int m_stage1DestroyCnt;
    int m_stage2DestroyCnt;
    int m_stage3DestroyCnt;

    string m_stage1;
    string m_stage2;
    string m_stage3;

    /*public List<List<int>> m_stage1Score;
    public List<List<int>> m_stage2Score;
    public List<List<int>> m_stage3Score;*/

    float m_escapeTime;
    int m_escapeCount;

    int m_toysOnFloor;

    bool m_isSkipped;

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
            case 1:  return JsonUtility.ToJson(ToJsonDataInt(1, m_stage1Success), true);
            case 2:  return JsonUtility.ToJson(ToJsonDataInt(2, m_stage2Success), true);
            case 3:  return JsonUtility.ToJson(ToJsonDataInt(3, m_stage3Success), true);
            case 4:  return JsonUtility.ToJson(ToJsonDataInt(4, m_stage1DestroyCnt), true);
            case 5:  return JsonUtility.ToJson(ToJsonDataInt(5, m_stage2DestroyCnt), true);
            case 6:  return JsonUtility.ToJson(ToJsonDataInt(6, m_stage3DestroyCnt), true);
            case 7:  return JsonUtility.ToJson(ToJsonDataString(7, m_stage1), true);
            case 8:  return JsonUtility.ToJson(ToJsonDataString(8, m_stage2), true);
            case 9:  return JsonUtility.ToJson(ToJsonDataString(9, m_stage3), true);
            case 10: return JsonUtility.ToJson(ToJsonDataFloat(10, m_escapeTime), true);
            case 11: return JsonUtility.ToJson(ToJsonDataInt(11, m_escapeCount), true);
            case 12: return JsonUtility.ToJson(ToJsonDataInt(12, m_toysOnFloor), true);
            case 13: return JsonUtility.ToJson(ToJsonDataInt(13, m_isSkipped ? 1 : 0), true);
            default: return "";
        }
    }


    // Getters
    public int GetStage1Success() { return m_stage1Success; }
    public int GetStage2Success() { return m_stage2Success; }
    public int GetStage3Success() { return m_stage3Success; }
    public int GetStage1DestroyCnt() { return m_stage1DestroyCnt; }
    public int GetStage2DestroyCnt() { return m_stage2DestroyCnt; }
    public int GetStage3DestroyCnt() { return m_stage3DestroyCnt; }
    public string GetStage1() { return m_stage1; }
    public string GetStage2() { return m_stage2; }
    public string GetStage3() { return m_stage3; }

    /*public List<List<int>> GetStage1Boxes() { return m_stage1Score; }
    public List<List<int>> GetStage2Boxes() { return m_stage2Score; }
    public List<List<int>> GetStage3Boxes() { return m_stage3Score; }*/
    public float GetEscapeTime() { return m_escapeTime; }
    public int GetEscapeCount() { return m_escapeCount; }
    public int GetToysOnFloor() { return m_toysOnFloor; }
    public bool GetSkipped() { return m_isSkipped; }

    // Setters
    public void SetStage1Success(int val) { m_stage1Success = val; }
    public void SetStage2Success(int val) { m_stage2Success = val; }
    public void SetStage3Success(int val) { m_stage3Success = val; }
    public void SetStage1DestroyCnt(int val) { m_stage1DestroyCnt = val; }
    public void SetStage2DestroyCnt(int val) { m_stage2DestroyCnt = val; }
    public void SetStage3DestroyCnt(int val) { m_stage3DestroyCnt = val; }
    public void SetStage1(string str) { m_stage1 = str; }
    public void SetStage2(string str) { m_stage2 = str; }
    public void SetStage3(string str) { m_stage3 = str; }

    /*public void SetStage1Boxes(List<List<int>> dataPacket) { m_stage1Score = dataPacket; }
    public void SetStage2Boxes(List<List<int>> dataPacket) { m_stage2Score = dataPacket; }
    public void SetStage3Boxes(List<List<int>> dataPacket) { m_stage3Score = dataPacket; }*/
    public void SetEscapeTime(float val) { m_escapeTime = val; }
    public void SetEscapeCount(int val) { m_escapeCount = val; }
    public void SetToysOnFloor(int val) { m_toysOnFloor = val; }
    public void SetSkipped(bool val) { m_isSkipped = val; }

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

[Serializable]
public class JsonDataInt
{
    public int ID;
    public int Result;
}

[Serializable]
public class JsonDataFloat
{
    public int ID;
    public float Result;
}
[Serializable]
public class JsonDataString
{
    public int ID;
    public string Result;
}