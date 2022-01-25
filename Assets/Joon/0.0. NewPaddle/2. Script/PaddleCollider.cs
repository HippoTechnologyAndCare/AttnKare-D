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


