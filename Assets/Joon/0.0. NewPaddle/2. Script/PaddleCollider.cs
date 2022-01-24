using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleCollider : MonoBehaviour
{
    public enum HANDLE { UP, RIGHT, LEFT }
    public HANDLE e_HANDLE;
    private float timePassed; //한 바퀴 돌리는 동안 걸린 시간 체크용
    public int intStage;

    public bool bPaddle = true;
    public bool bORDER = false;




    Guide_Paddle Guide;
    private void Start()
    {
        intStage = 0;
        Guide = GameObject.Find("Guide_Paddle").GetComponent<Guide_Paddle>();

    }

    void Update()
    {
        if(bPaddle)
        timePassed += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "HANDLE_MY")
        {
            intStage = Manager_Paddle.intStage;
            if (e_HANDLE == HANDLE.UP)
            {
                StageCheck(timePassed);
                bPaddle = false;
                timePassed = 0;
                
            }
            if (e_HANDLE == HANDLE.RIGHT && Manager_Paddle.SDB[intStage].strHANDLE == null)
                    Manager_Paddle.SDB[intStage].strHANDLE = "FORWARD";
            if (e_HANDLE == HANDLE.LEFT &&Manager_Paddle.SDB[intStage].strHANDLE == null)
                Manager_Paddle.SDB[intStage].strHANDLE = "BACKWARD";
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "HANDLE_MY") bPaddle = true;
    }

    private void StageCheck(float time)
    {
        float timeMinus = time - Manager_Paddle.SDB[intStage].fTime;
        Guide.PaddleCheck(timeMinus);

        //first check if speed is right
    }



}






/*
    Manager_Paddle.HANDLE_INFO m_eHandleInfo;
    public Manager_Paddle.HANDLE m_eHandle;
    public bool bAuto;
    GameObject go_Handle;
    GameObject go_AutoHandle;

    public static float fAUTO;
    public static float fHANDLE;
    

    bool bUP;
    bool set = true;
    float timePassed;
    float timeMinus;
    private void Start()
    {
        m_eHandleInfo = Manager_Paddle.HDB[(int)m_eHandle];
        go_AutoHandle = GameObject.FindGameObjectWithTag("HANDLE_AUTO");
        go_Handle = GameObject.FindGameObjectWithTag("HANDLE_MY");
        if (m_eHandleInfo.strPaddle == "UP") bUP = true;

    }
    private void Update()
    {
        timePassed += Time.deltaTime;
        
    }
    

    public void OnCollisionEnter(Collision other)
    {
        if (m_eHandle == Manager_Paddle.HANDLE.UP)
        {
            if (other.gameObject.tag == "HANDLE_MY")
            {
                Debug.Log("up");
                fHANDLE = timePassed;
            }
            if(other.gameObject.tag == "HANDLE_AUTO")
            {
                fAUTO = timePassed;
            }
            CheckRound();
        }
    }
   
     private void CheckRound()
    {
       
            timeMinus = fAUTO - fHANDLE;
            if (timeMinus > 0.6f || timeMinus < -0.6f)
            {
                Debug.Log(timeMinus);
                StartCoroutine(DEBUG("SPEED"));
            }
            ResetHandle();
        
    }

    public IEnumerator DEBUG(string text)
    {
        Debug.Log(text);
        yield return null;

    }

    private void ResetHandle()
    {
        fAUTO = fHANDLE = timeMinus = timePassed= 0;
        set = false;

    }
    */
