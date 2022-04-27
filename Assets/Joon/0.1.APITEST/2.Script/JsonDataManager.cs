using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Newtonsoft.Json;
using UserData;

public class JsonDataManager : MonoBehaviour
{

    public int sceneIndex;
    [FormerlySerializedAs("key_rowIndex")] public int keyRowIndex;
    public GameObject objToFind;

    public SetDataType setPlayerData;

    private delegate void SaveCurrentSceneData(int SceneIndex);

    private SaveCurrentSceneData saveCurrentSceneData;

    public Dictionary<int, PlayerJsonData> dataList = new Dictionary<int, PlayerJsonData>();
    
    private void Start()
    {
        setPlayerData.InitialDataSetting();
        CheckSaveDataTypes();
    }

    private void CheckSaveDataTypes()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (sceneIndex)
        {
            case 1: // BagPacking
                objToFind = FindObjectOfType<Object_BP>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 2: //Scoop
                objToFind = FindObjectOfType<EasyTubeScoreboard>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 3: //Nummatch
                objToFind = FindObjectOfType<CheckData_NumCheck>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 4: //Cleanup
                objToFind = FindObjectOfType<CleanUp.Guide>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 5: //스케줄
                objToFind = FindObjectOfType<Scheduler.ScheduleManager2>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 6: //PlayPaddle
                objToFind = FindObjectOfType<Guide_Paddle>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            default:
                Debug.Log("Scene Index가 유효하지 않습니다");
                break;

                /*
                case 2: //Schedule
                    objToFind = FindObjectOfType<ScheduleManager>().gameObject;
                    saveCurrentSceneData = SetData;
                    break;
                case 3: //BP L
                    objToFind = FindObjectOfType<Object_BP>().gameObject;
                    saveCurrentSceneData = SetData;
                    break;
                case 4: //Scoop L
                    objToFind = FindObjectOfType<EasyTubeScoreboard>().gameObject;
                    saveCurrentSceneData = SetData;
                    break;

                case 6: //PlayPaddle
                    objToFind = FindObjectOfType<Guide_Paddle>().gameObject;
                    saveCurrentSceneData = SetData;
                    break;
                case 7: //Bagpacking H
                    objToFind = FindObjectOfType<Object_BP>().gameObject;
                    saveCurrentSceneData = SetData;
                    break;
                case 8: //Scoop H
                    objToFind = FindObjectOfType<TubeScoreboard>().gameObject;
                    saveCurrentSceneData = SetData;
                    break;
                case 9: //NUMMATCH
                    objToFind = FindObjectOfType<CheckData_NumCheck>().gameObject;
                    saveCurrentSceneData = SetData;
                    break;
                case 14: //NEWPaddle
                    objToFind = FindObjectOfType<Guide_Paddle>().gameObject;
                    saveCurrentSceneData = SetData;
                    break;
                */
        }
    }

    public string SaveCurrentData()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        saveCurrentSceneData(sceneIndex);
       // string jsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);
        string newJsonData = "";
        foreach(KeyValuePair<int, PlayerJsonData> item in dataList)
        {
            newJsonData += item.Key + "(" + "DataName:" + item.Value.DataName + "," + "Result:" + item.Value.Result +")";
        }
        Debug.Log(newJsonData);
        return newJsonData;
    }


    private void SetData(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 1: //방청소하기
                setPlayerData.SetSceneData(objToFind.GetComponent<Object_BP>().arrFloat);
                break;

            case 2: //Scoop
                setPlayerData.SetSceneData(objToFind.GetComponent<EasyTubeScoreboard>().scene2arr);
                break;
            case 3: //NumMatch
                setPlayerData.SetSceneData(objToFind.GetComponent<CheckData_NumCheck>().arrData);
                break;
            case 4: //CleanUp
                setPlayerData.SetSceneData(objToFind.GetComponent<CleanUp.Guide>().m_dataReportFloat);
                break;
            case 5: //Scehedule
                setPlayerData.SetSceneData(objToFind.GetComponent<Scheduler.ScheduleManager2>().Scene2Arr);
                break;
            case 6: //PlayPaddle
                setPlayerData.SetSceneData(objToFind.GetComponent<Guide_Paddle>().arrData);
                break;
            default:
                Debug.Log("Scene Index가 유효하지 않습니다");
                break;
                /*
                case 1: //방청소하기
                    setPlayerData.SetSceneData(objToFind.GetComponent<Object_BP>().arrFloat);
                    break;

                case 2: //Schedule                
                    setPlayerData.SetSceneData(objToFind.GetComponent<Scheduler.ScheduleManager1>().Scene2Arr);
                    break;
                case 3: //BP L
                    setPlayerData.SetSceneData(objToFind.GetComponent<Object_BP>().arrFloat);
                    break;
                case 4: //Scoop L
                    setPlayerData.SetSceneData(objToFind.GetComponent<EasyTubeScoreboard>().scene2arr);
                    break;
                case 5: //CRUM

                    break;
                case 6: //PlayPaddle
                    setPlayerData.SetSceneData(objToFind.GetComponent<Guide_Paddle>().arrData);
                    break;
                case 7: //bagpacking H
                    setPlayerData.SetSceneData(objToFind.GetComponent<Object_BP>().arrFloat);
                    break;
                case 8: //Scoop H
                    setPlayerData.SetSceneData(objToFind.GetComponent<TubeScoreboard>().scene2arr);
                    break;
                case 9: //NUMMATCH
                    setPlayerData.SetSceneData(objToFind.GetComponent<CheckData_NumCheck>().arrData);
                    break;
                */

        }
    }
}

