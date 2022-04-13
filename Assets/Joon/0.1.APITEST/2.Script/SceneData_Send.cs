using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UserData;
using BNG;
public class SceneData_Send : MonoBehaviour
{
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
        if(buildindex != 9 && buildindex != 11)
        {
            BehaviorDataCollection = FindObjectOfType<PlayerMovementData>();
            JsonDataCollection = FindObjectOfType<JsonDataManager>();
            VoiceRecording = FindObjectOfType<VoiceRecording>();
        }
    }
    public void GetSceneData()
    {
        switch (buildindex)
        {
            case 9: scene_id = 1070; StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //������
            case 8: scene_id = 1080; GetBehaviorData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //Ʃ�丮��
            case 1: scene_id = 1001; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //å����
            case 5: scene_id = 1001; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //������
            case 2: scene_id = 1002; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //���ű��
            case 6: scene_id = 1002; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //���
            case 3: scene_id = 1003; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //���ڸ��߱�
            case 4: scene_id = 1004; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //��û��
            case 11: scene_id = 999; StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //����
        }

    }
    public void ConveyorDataSend(string json)
    {
        scene_id = 1004;
        StartCoroutine(APICONNECT.POST_NQTT(1, scene_id, json));
        GetBehaviorData();
        StartCoroutine(GetVoiceData());
    }
    void GetBehaviorData()
    {
        string behavior = BehaviorDataCollection.GetBehaviorData();
        StartCoroutine(APICONNECT.POST_NQTT(1, scene_id, behavior));
    }
    void GetJsonData()
    {
        string JsonData = JsonDataCollection.SaveCurrentData();
        StartCoroutine( APICONNECT.POST_NQTT(0, scene_id, JsonData));
    }
    IEnumerator GetVoiceData()
    {
        VoiceRecording.StopRecordingNBehavior();
        yield return new WaitUntil(() => VoiceRecording.fin == true);
        StartCoroutine(APICONNECT.POST_MP3(scene_id));
    }
}
