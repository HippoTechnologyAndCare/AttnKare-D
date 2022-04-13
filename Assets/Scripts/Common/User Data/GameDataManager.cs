using Scheduler;
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
            case 1: // BagPacking
                objToFind = FindObjectOfType<Object_BP>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 2: //Scoop
                objToFind = FindObjectOfType<EasyTubeScoreboard>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 3: //Nummatch
                objToFind = FindObjectOfType<Guide_Paddle>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 4: //Cleanup
                objToFind = FindObjectOfType<CleanUp.Guide>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 5: //스케줄
                objToFind = FindObjectOfType<Scheduler.ScheduleManager1>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            case 6: //PlayPaddle
                objToFind = FindObjectOfType<Guide_Paddle>().gameObject;
                saveCurrentSceneData = SetData;
                break;
            default:
                Debug.Log("Scene Index가 유효하지 않습니다");
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
                setPlayerData.SetSceneData(objToFind.GetComponent<Scheduler.ScheduleManager1>().Scene2Arr);
                break;
            case 6: //PlayPaddle
                setPlayerData.SetSceneData(objToFind.GetComponent<Guide_Paddle>().arrData);
                break;
            default:
                Debug.Log("Scene Index가 유효하지 않습니다");
                break;
        }      
    }

    private void SetData_pm(int sceneIndex)
    {
        //setPlayerData.SetSceneData(objToFind.GetComponent<CleanUp.Guide>().m_dataReportFloat);
        //GameObject.Find("SetPlayerData").SendMessage("SetSceneData");
    }    
}