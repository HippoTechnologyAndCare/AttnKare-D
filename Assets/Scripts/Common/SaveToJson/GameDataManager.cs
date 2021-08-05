using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class GameDataManager : MonoBehaviour
{
    public SetPlayerData setPlayerData;
    //public bool isPM;    

    delegate void SaveCurrentSceneData(int SceneIndex);

    SaveCurrentSceneData saveCurrentSceneData;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (JsonManager.isFirst == true)
        {                        
            JsonManager.GetInstance().LoadPlayerDataFromJson();
            Debug.Log("Load Data!");
        }
        else
        {
            setPlayerData.InitialDataSetting();
            JsonManager.isFirst = true;
        }
        CheckSaveDataType();        
    }

    void CheckSaveDataType()
    {
        int sceneIndex;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
#if UNITY_EDITOR
        sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
#endif

        switch (sceneIndex)
        {
            case 1:
                saveCurrentSceneData = SetData_pm;
                break;
            case 2:
                saveCurrentSceneData = SetData;
                break;
            case 3:
                saveCurrentSceneData = SetData_pm;
                break;
            case 4:
                saveCurrentSceneData = SetData;
                break;
            case 5:
                saveCurrentSceneData = SetData_pm;
                break;
        }        
    }

    public void SaveCurrentData()
    {
        int sceneIndex;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
#if UNITY_EDITOR
        sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
#endif  
        saveCurrentSceneData(sceneIndex);
        
        JsonManager.GetInstance().Invoke("SavePlayerDataToJson", 0.1f);        
    }

    public void SetData(int sceneIndex)
    {
        string convertIndex = sceneIndex.ToString();
        string functionName = "GetSceneIndex" + convertIndex;

        GameObject.Find("SetPlayerData").SendMessage(functionName);
    }

    public void SetData_pm(int sceneIndex)
    {
        string convertIndex = sceneIndex.ToString();
        string functionName = "GetSceneIndex" + convertIndex;
        
        GameObject.Find("SetPlayerData").SendMessage(functionName);        
    }
}