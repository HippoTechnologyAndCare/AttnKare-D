using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UserData;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameObject pController;

    public int sceneIndex;
    [FormerlySerializedAs("key_rowIndex")] public int keyRowIndex;
    public GameObject objToFind;

    public SetPlayerData setPlayerData;

    private delegate void SaveCurrentSceneData(int SceneIndex);

    private SaveCurrentSceneData saveCurrentSceneData;

    private void Awake()
    {                
        var sceneName = SceneManager.GetActiveScene().name;
        if (!DataManager.GetInstance().isPlayed && sceneName != "Tutorial")
        {
            DataManager.GetInstance().isPlayed = true;

            setPlayerData.InitialDataSetting();
            DataManager.GetInstance().SavePlayerDataToJson();
            Debug.Log("Data Creat!");
        }

        if (DataManager.GetInstance().isTest) return;
        pController = GameObject.Find("PlayerController");
        
        if (pController == null) return;
        pController.GetComponent<BNG.SmoothLocomotion>().AllowInput = false;
        pController.GetComponent<BNG.PlayerRotation>().AllowInput = false;
    }

    private void Start()
    {
        var sceneName = SceneManager.GetActiveScene().name;

        if (DataManager.GetInstance().isPlayed == true)
        {
            DataManager.GetInstance().LoadPlayerDataFromJson();
            Debug.Log("Load Data!");
        }

        else if (sceneName != "Tutorial")
        {
            setPlayerData.InitialDataSetting();
            DataManager.GetInstance().isPlayed = true;
        }

        CheckSaveDataTypes();        
    }

    private void CheckSaveDataTypes()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;        

        switch (sceneIndex)
        {
            case 1: //Doorlock                
                saveCurrentSceneData = SetData_pm;                            
                break;
            case 2: //Schedule
                objToFind = FindObjectOfType<ScheduleManager>().gameObject;
                saveCurrentSceneData = SetData;                       
                break;
            case 3: //BP L
                objToFind = FindObjectOfType<TempScriptJason>().gameObject;
                saveCurrentSceneData = SetData;                      
                break;
            case 4: //Scoop L
                objToFind = FindObjectOfType<EasyTubeScoreboard>().gameObject;
                saveCurrentSceneData = SetData;                            
                break;
            case 5: //CRUM
                saveCurrentSceneData = SetData_pm;                            
                break;
            case 6: //PlayPaddle
                saveCurrentSceneData = SetData_pm;                                
                break;
            case 7: //Bagpacking H
                objToFind = FindObjectOfType<TempScriptJason>().gameObject;
                saveCurrentSceneData = SetData;                               
                break;
            case 8: //Scoop H
                objToFind = FindObjectOfType<TubeScoreboard>().gameObject;
                saveCurrentSceneData = SetData;                                
                break;
            case 9: //NUMMATCH
                saveCurrentSceneData = SetData_pm;
                break;
        }        
    }

    public void SaveCurrentData()
    {
        
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        saveCurrentSceneData(sceneIndex);

        DataManager.GetInstance().Invoke("SavePlayerDataToJson", 0.1f);
    }

    private void SetData(int sceneIndex)
    {        
        switch (sceneIndex)
        {            
            case 2: //Schedule                
                setPlayerData.SetSceneData(objToFind.GetComponent<Scheduler.ScheduleManager1>().Scene2Arr);
                break;
            case 3: //BP L
                setPlayerData.SetSceneData(objToFind.GetComponent<TempScriptJason>().arrFloat);
                break;
            case 4: //Scoop L
                setPlayerData.SetSceneData(objToFind.GetComponent<EasyTubeScoreboard>().scene2arr);
                break;
            case 5: //CRUM
                
                break;
            case 6: //PlayPaddle
                
                break;
            case 7: //bagpacking H
                setPlayerData.SetSceneData(objToFind.GetComponent<TempScriptJason>().arrFloat);
                break;
            case 8: //Scoop H
                setPlayerData.SetSceneData(objToFind.GetComponent<TubeScoreboard>().scene2arr);                
                break;
            case 9: //NUMMATCH
                
                break;
        }      
    }

    private void SetData_pm(int sceneIndex)
    {        
        GameObject.Find("SetPlayerData").SendMessage("SetSceneData");
    }    
}