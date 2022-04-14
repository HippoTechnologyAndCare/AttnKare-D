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
    public CleanUp.Guide m_Guide;
    public Canvas       QACanvas;
    [HideInInspector] public static bool onlyFirstTime = false;
    [HideInInspector] public static int m_nResult = 0;
    public enum m_Value
    {
        NONE,
        apple,
        orange,
        grape,
        banana
        
    }
    public m_Value m_RV;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectSomthing()
    {
        m_Hud.playClipBell();
        CanvasFadeOut(QACanvas, 1f);
        m_Guide.Make_End();
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
    }
    IEnumerator AnimAlpah(Canvas canvas, float time, bool bIn)
    {
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        cg.alpha = bIn ? 0 : 1f;
        int loop = (int)(time / 0.05f);
        float fadeStep = 1f / loop;
        for (int i = 0; i < loop; i++)
        {
            yield return new WaitForSeconds(0.05f);
            cg.alpha += bIn ? fadeStep : (-1f) * fadeStep;
        }
        returnSelected();
        if (!bIn) canvas.gameObject.SetActive(false);
    }
    void returnSelected()
    {
        //Debug.Log("result" + (int)m_RV);
        m_nResult = (int)m_RV;
        //return (int)m_RV;
    }
    
}
