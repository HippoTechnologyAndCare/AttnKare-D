using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class ManualXRControl : MonoBehaviour
{

    static public ManualXRControl instance;
    
    public IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }

    void StopXR()
    {
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
    }    
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
    }

    private void OnApplicationFocus(bool isApplicationHasFocus)
    {
        Debug.Log("isApplicationHasFocus" + isApplicationHasFocus);
    }

    private void Awake()
    {
        AutoStartXR();

        {
            // Scene 시작시 Safty 처리하는 If문 // 인스턴스는 널이 아닌 경우에 한해 -> 인스턴스가 자신(this)이 아닐때 셀프 파괴
            //if (instance != null)
            //{
            //    if (instance != this)
            //    {
            //        Destroy(gameObject);
            //        return;
            //    }
            //}
            //DontDestroyOnLoad(gameObject);
        }


    }

    private void AutoStartXR()
    {
        int sceneIndex;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
#if UNITY_EDITOR
        sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
#endif

        switch (sceneIndex)
        {
            case 0:
                StopXR();
                break;
            case 1:
                StartCoroutine("StartXR");
                break;
            case 2:
                StartCoroutine("StartXR");
                break;
            case 3:
                StartCoroutine("StartXR");
                break;
            case 4:
                StartCoroutine("StartXR");
                break;
            case 5:
                StartCoroutine("StartXR");
                break;
            case 6:
                StartCoroutine("StartXR");
                break;
            case 7:
                StartCoroutine("StartXR");
                break;
            case 8:
                StartCoroutine("StartXR");
                break;
            case 9:
                StartCoroutine("StartXR");
                break;
            case 10:
                StartCoroutine("StartXR");
                break;
            case 11:
                StartCoroutine("StartXR");
                break;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}

