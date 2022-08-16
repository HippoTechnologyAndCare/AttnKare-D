using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UserData;
using BNG;
public class SceneData_Send : MonoBehaviour
{
    public GameObject KEYINPUT;
    GUIDE_API APICONNECT;
    PlayerMovementData BehaviorDataCollection;
    JsonDataManager JsonDataCollection;
    VoiceRecording VoiceRecording;

    // Start is called before the first frame update
    int buildindex;
    int scene_id;
    void Start()
    {
        buildindex = SceneManager.GetActiveScene().buildIndex;
        APICONNECT = FindObjectOfType<GUIDE_API>();
        if(buildindex != 9)
        {
            VoiceRecording = FindObjectOfType<VoiceRecording>();
            BehaviorDataCollection = FindObjectOfType<PlayerMovementData>();
            if (buildindex != 11){
                JsonDataCollection = FindObjectOfType<JsonDataManager>();}

        }
        if (!FindObjectOfType<KeyInput>())
        {
            Instantiate(KEYINPUT);
        }
    }

    public void GetSceneData()
    {
        switch (buildindex)
        {
            case 9: scene_id = 1070; StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //오프닝
            case 8: scene_id = 1080; GetBehaviorData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //튜토리얼
            case 1: scene_id = 1001; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //책가방
            case 5: scene_id = 1005; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //스케줄
            case 2: scene_id = 1002; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //공옮기기
            case 6: scene_id = 1006; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //페달
            case 3: scene_id = 1003; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //숫자맞추기
            case 4: scene_id = 1004; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //방청소
            case 7: scene_id = 1007; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; // 컨베이어
            case 11: scene_id = 999; GetBehaviorData(); StartCoroutine(GetVoiceData());  StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //엔딩   
        }

    }
    /*
    public void ConveyorDataSend(string json)
    {
        scene_id = 1007;
        Debug.Log(json);
        StartCoroutine(APICONNECT.POST_NQTT(2, scene_id, json));
        GetBehaviorData();
        StartCoroutine(GetVoiceData());
        StartCoroutine(APICONNECT.PUT_STATUS(scene_id));
    }
    */
    void GetBehaviorData()
    {
        List<List<object>> behavior = BehaviorDataCollection.GetBehaviorData();
        StartCoroutine(APICONNECT.POST_NQTT(1, scene_id, behavior));
    }
    /*OLD VERSION
     * void GetJsonData()
    {
        string JsonData = JsonDataCollection.SaveCurrentData();
        StartCoroutine( APICONNECT.POST_NQTT(0, scene_id, JsonData));
    }
        */
    public void TutorialJson()
    {
        Dictionary<string, PlayerJsonData> JsonData = JsonDataCollection.SaveCurrentData();
        StartCoroutine(APICONNECT.POST_NQTT_JSON(0, scene_id, JsonData));
    }

    void GetJsonData()
    {
        int a;
        Debug.Log("JSON DATA SENDING");
        Dictionary<string, PlayerJsonData> JsonData = JsonDataCollection.SaveCurrentData();
        StartCoroutine(APICONNECT.POST_NQTT_JSON(0, scene_id, JsonData));
    }

        IEnumerator GetVoiceData()
    {
        Debug.Log("VOICE" + VoiceRecording.gameObject.name);
        VoiceRecording.StopRecordingNBehavior();
        yield return new WaitUntil(() => VoiceRecording.fin == true);
        StartCoroutine(APICONNECT.POST_MP3(scene_id));
    }
   
}
