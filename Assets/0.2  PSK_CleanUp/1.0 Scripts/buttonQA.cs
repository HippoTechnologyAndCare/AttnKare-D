using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KetosGames.SceneTransition; //scene transition
using BNG;

public class buttonQA : MonoBehaviour
{

    // Start is called before the first frame update
    public CleanUp.HUD  m_Hud;
    public Canvas       QACanvas;
    public GameObject reticle;
    public bool targeted;
    //bool onceExe = false;
    bool isTimeover = false;
    //bool deciedSomething = false;
    [HideInInspector] public static bool isFirstTime = false;
    [HideInInspector] public static int m_nResult = 0;
    public enum m_Value
    {
        NONE,
        YES,
        NOPE,
        NOTPUT,
        banana
        
    }
    public m_Value m_RV;
    

    void Start()
    {
        isFirstTime = false;
        m_nResult = 0;
}

    // Update is called once per frame
    void Update()
    {
        if (targeted == true)
        {
            if (Input.GetButtonDown("XRI_Right_TriggerButton"))
            {
                if (isFirstTime == false)
                {
                    buttonQA.isFirstTime = true;
                    selectSomthing();
                }
                
            }
            else targeted = false;
        }
    }

    public void selectSomthing()
    {
        m_Hud.playClipBell();
        //isFirstTime = true;
        //deciedSomething = true;
        CanvasFadeOut(QACanvas, 1f);
        m_Hud.PlayMakeEnd();
    }
    public void startSomthing()
    {
        
        CanvasFadeIn(QACanvas, 1f);
    }
    void CanvasFadeIn(Canvas canvas, float time)
    {
        if (!canvas) return;  // check valid canvas ?
        
        canvas.gameObject.SetActive(true);  
        StartCoroutine(AnimAlpah(canvas, time, true));
    }
    void CanvasFadeOut(Canvas canvas, float time)
    {
        if (!canvas) return;  // check valid canvas ?                
        StartCoroutine(AnimAlpah(canvas, time, false));
        reticle.SetActive(false);
    }
    IEnumerator AnimAlpah(Canvas canvas, float time, bool bIn)
    {
        if (isFirstTime) {
            if (isTimeover == true) m_nResult = 0; // 설문 시간 제한 종료 시
            else m_nResult = (int)m_RV; // 설문 완료 시
        }
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        cg.alpha = bIn ? 0 : 1f;
        int loop = (int)(time / 0.05f);
        float fadeStep = 1f / loop;
        for (int i = 0; i < loop; i++)
        {
            yield return new WaitForSeconds(0.05f);
            cg.alpha += bIn ? fadeStep : (-1f) * fadeStep;
        }
        if (!bIn) canvas.gameObject.SetActive(false);
        
    }
    void returnSelected()
    {
        //Debug.Log("result" + (int)m_RV);
        m_nResult = (int)m_RV;
        //return (int)m_RV;
    }
    public void timeover()
    {
        if (isFirstTime == false) {       
            //deciedSomething = true;
            buttonQA.isFirstTime = true;
            isTimeover = true;
            m_Hud.playClipBell();
            CanvasFadeOut(QACanvas, 1f);
            m_Hud.PlayMakeEnd();
        }
        
    }
    
}
