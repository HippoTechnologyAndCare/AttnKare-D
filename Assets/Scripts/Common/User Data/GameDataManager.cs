using UnityEngine;
using UnityEngine.SceneManagement;
using UserData;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] GameObject pController;

    public int sceneIndex;
    public int key_rowIndex;
    public GameObject objToFind;

    public SetPlayerData setPlayerData;      

    delegate void SaveCurrentSceneData(int SceneIndex);
    SaveCurrentSceneData saveCurrentSceneData;

    void Awake()
    {                
        string sceneName = SceneManager.GetActiveScene().name;
        if (!DataManager.GetInstance().isPlayed && sceneName != "Tutorial")
        {
            DataManager.GetInstance().isPlayed = true;

            setPlayerData.InitialDataSetting();
            DataManager.GetInstance().SavePlayerDataToJson();
            Debug.Log("Data Creat!");
        }

        if (!DataManager.GetInstance().isTest)
        {
            pController = GameObject.Find("PlayerController");
            if (pController != null)
            {
                pController.GetComponent<BNG.SmoothLocomotion>().AllowInput = false;
                pController.GetComponent<BNG.PlayerRotation>().AllowInput = false;
            }
        }
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

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
       
    void CheckSaveDataTypes()
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
                saveCurrentSceneData = SetData_pm;                      
                break;
            case 4: //Scoop L
                saveCurrentSceneData = SetData_pm;                            
                break;
            case 5: //CRUM
                saveCurrentSceneData = SetData_pm;                            
                break;
            case 6: //PlayPaddle
                saveCurrentSceneData = SetData_pm;                                
                break;
            case 7: //bagpacking H
                saveCurrentSceneData = SetData_pm;                               
                break;
            case 8: //Scoop H
                saveCurrentSceneData = SetData_pm;                                
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

    public void SetData(int sceneIndex)
    {        
        switch (sceneIndex)
        {            
            case 2: //Schedule                
                setPlayerData.SetSceneData(objToFind.GetComponent<ScheduleManager>().scene2arr);
                break;
        }      
    }

    public void SetData_pm(int sceneIndex)
    {        
        GameObject.Find("SetPlayerData").SendMessage("SetSceneData");
    }    
}