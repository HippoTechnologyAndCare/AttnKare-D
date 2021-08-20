using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class ManualXRControl : MonoBehaviour
{

    static public ManualXRControl MXC_Instance;
    
    public static ManualXRControl Instance
    {
        get
        {
            if (MXC_Instance == null)
            {
                ManualXRControl manualXRControl = (ManualXRControl)GameObject.FindObjectOfType(typeof(ManualXRControl));
                if (manualXRControl != null)
                {
                    MXC_Instance = manualXRControl;
                }
                else
                {
                    GameObject MXCPrefab = Resources.Load<GameObject>("Prefabs/CommonPrefabs/ManualXRController");
                    MXC_Instance = (GameObject.Instantiate(MXCPrefab)).GetComponent<ManualXRControl>();
                }
            }
            return MXC_Instance;
        }
    }

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
        StopXR();
    }

    private void OnApplicationFocus(bool isApplicationHasFocus)
    {
        Debug.Log("isApplicationHasFocus" + isApplicationHasFocus);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);


        if (MXC_Instance != null && MXC_Instance != this)
        {
            Destroy(MXC_Instance.gameObject);
            MXC_Instance = this;
        }
    }

    private void Start()
    {
        AutoStartXR();
    }

    private void AutoStartXR()
    {
        int sceneIndex;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
#if UNITY_EDITOR
        sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
#endif

        if (sceneIndex == 0)
        {
            StopXR();
        }
        else
        {
            StartCoroutine("StartXR");
        }                        
    }    
}

