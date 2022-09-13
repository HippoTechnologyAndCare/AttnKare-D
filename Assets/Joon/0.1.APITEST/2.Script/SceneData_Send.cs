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
            case 9: scene_id = 1070; StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //������
            case 8: scene_id = 1080; GetBehaviorData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //Ʃ�丮��
            case 1: scene_id = 1001; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //å����
            case 5: scene_id = 1005; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //������
            case 2: scene_id = 1002; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //���ű��
            case 6: scene_id = 1006; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //���
            case 3: scene_id = 1003; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //���ڸ��߱�
            case 4: scene_id = 1004; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //��û��
            case 7: scene_id = 1007; GetBehaviorData(); GetJsonData(); StartCoroutine(GetVoiceData()); StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; // �����̾�
            case 11: scene_id = 999; GetBehaviorData(); StartCoroutine(GetVoiceData());  StartCoroutine(APICONNECT.PUT_STATUS(scene_id)); break; //����   
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
        SoftwareTest.CreateTXT("JSON �Ľ�", true);
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
