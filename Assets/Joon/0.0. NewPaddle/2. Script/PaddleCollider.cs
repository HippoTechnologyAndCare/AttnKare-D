using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class PaddleCollider : MonoBehaviour
{
    public Grabbable CurrentGrabbable;
    public enum HANDLE { UP, RIGHT, LEFT }
    public HANDLE e_HANDLE;
    private float timePassed; //한 바퀴 돌리는 동안 걸린 시간 체크용
    public int nStage;
    int c;
    public bool bPaddle = true;
    public bool bORDER = false;
    Guide_Paddle Guide;
    private void Start()
    {
        nStage = 0;
        Guide = GameObject.Find("Guide_Paddle").GetComponent<Guide_Paddle>();

    }

    void Update()
    {
        if(bPaddle)
        timePassed += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {

        nStage = Manager_Paddle.intStage;
        if (collision.collider.tag == "HANDLE_MY" && CurrentGrabbable.BeingHeld)
        {
            if (e_HANDLE == HANDLE.UP)
            {
                StageCheck(timePassed);
                timePassed = 0;
            }
            if (e_HANDLE == HANDLE.RIGHT && Manager_Paddle.SDB[nStage].strHANDLE == null)
            { Manager_Paddle.SDB[nStage].strHANDLE = "FORWARD"; }
            if (e_HANDLE == HANDLE.LEFT &&Manager_Paddle.SDB[nStage].strHANDLE == null)
                Manager_Paddle.SDB[nStage].strHANDLE = "BACKWARD";
            Debug.Log("COLLIDED===" + collision.collider.gameObject.name+"????"+this.gameObject.name);
        }
        if(collision.collider.tag == "HANDLE_AUTO")
        {
            if (e_HANDLE == HANDLE.UP)
            {
                Debug.Log("UOP");
                bPaddle = false;

            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "HANDLE_MY") bPaddle = true;
    }

    private void StageCheck(float time)
    {

        float timeMinus = time - Manager_Paddle.SDB[nStage].fTime;
        Debug.Log(nStage + "+" +Manager_Paddle.SDB[nStage].fTime+"+"+ Manager_Paddle.SDB[nStage].strORDER);
        Guide.PaddleCheck(timeMinus);
    }



}


