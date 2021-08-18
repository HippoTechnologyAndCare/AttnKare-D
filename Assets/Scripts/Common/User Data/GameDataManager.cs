﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//using UnityEditor;
using UnityEngine.SceneManagement;
using UserData;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class GameDataManager : MonoBehaviour
{
    public SetPlayerData setPlayerData;
    //public bool isPM;    

    delegate void SaveCurrentSceneData(int SceneIndex);

    SaveCurrentSceneData saveCurrentSceneData;    

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.GetInstance().isPlayed == true)
        {
            DataManager.GetInstance().LoadPlayerDataFromJson();
            Debug.Log("Load Data!");
        }

        else
        {
            setPlayerData.InitialDataSetting();
            DataManager.GetInstance().isPlayed = true;
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
            case 1: //Doorlock
                saveCurrentSceneData = SetData_pm;
                break;
            case 2: //Schedule
                saveCurrentSceneData = SetData;
                break;
            case 3: //BP L
                saveCurrentSceneData = SetData_pm;
                break;
            case 4: //Scoop L
                saveCurrentSceneData = SetData;
                break;
            case 5: //CR
                saveCurrentSceneData = SetData_pm;
                break;
            case 6: //PlayPaddle
                saveCurrentSceneData = SetData;
                break;
            case 7: //bagpacking H
                saveCurrentSceneData = SetData_pm;
                break;
            case 8: //Scoop H
                saveCurrentSceneData = SetData;
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

        DataManager.GetInstance().Invoke("SavePlayerDataToJson", 0.1f);
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