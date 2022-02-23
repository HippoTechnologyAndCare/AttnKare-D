using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using HutongGames.PlayMaker.Actions;

[System.Serializable]
public class SetTxtTiming
{
    [SerializeField]
    private List<float> keys = new List<float>();
    [SerializeField]
    private List<string> values = new List<string>();

    public Dictionary<float, string> TxtDictionary = new Dictionary<float, string>();
}

public class HUDSchedule : MonoBehaviour
{
    private enum Voice {HowTo, Start, HalfInfo, WellDone}

    public SetTxtTiming setTxtTiming;

    /*************************************************************************
    //처음 안내문구 음성과 문구을 전시합니다
    *************************************************************************/
    [SerializeField] private AudioSource audSIntro; //안내 오디오 소스
    [SerializeField] private AudioClip[] audCIntro; // 안내 음성 클립
    [SerializeField] public DOTweenAnimation[] dotAnim; // 텍스트 애니메이션

    public Canvas infoCanvas;

    [SerializeField] private TextMeshProUGUI howToTMP;
    [SerializeField] private string[] howToScriptTxt;

    private bool _isFade;

    private void Start()
    {
        HowToPlay();
        setTxtTiming.TxtDictionary = new Dictionary<string, float>();
    }
    
    public void HowToPlay()
    {
        //StartCoroutine(HowToPlayVoiceText());
        StartCoroutine(HowToPlaySetUiTxt());
    }

    private IEnumerator HowToPlaySetUiTxt()
    {
        yield return new WaitForSeconds(2f);
        
        foreach (var setT in howToScriptTxt)
        {
            if (!_isFade)
            {
                howToTMP.SetText(setT);
                FadeInCanvas(infoCanvas, 1f); // Info Canvas fade In
                _isFade = true;
            }

            yield return new WaitForSeconds(2f);
            howToTMP.SetText(setT);
        }

        yield return new WaitForSeconds(2f);
        
        FadeOutCanvas(infoCanvas, 1f);
    }
    
    private IEnumerator HowToPlayVoiceText()
    {
        yield return new WaitForSeconds(3f);
        
        var wait = PlaySound(audSIntro, audCIntro[(int)Voice.HowTo]);
        yield return new WaitForSeconds(wait);
        foreach (var aC in audCIntro)
        {
            yield return new WaitForSeconds(1f);
            wait = PlaySound(audSIntro, aC);
            
            dotAnim[Array.IndexOf(audCIntro, aC)].DOPlay(); // Start Text Anim
            yield return new WaitForSeconds(wait);
        }

        FadeOutCanvas(infoCanvas, 1f); // Info Canvas fade Out
        yield return new WaitForSeconds(2f);
    }

    private void FadeInCanvas(Canvas canvas, float time)
    {
        if(!canvas) return;  // check valid canvas ?
        StartCoroutine(AnimAlpha(canvas, time,true));
    }

    private void FadeOutCanvas(Canvas canvas, float time)
    {
        if(!canvas) return;  // check valid canvas ?
        StartCoroutine(AnimAlpha(canvas, time,false));
    }
    
    private IEnumerator AnimAlpha(Canvas canvas,float time, bool bIn)
    {
        var cg = canvas.GetComponent<CanvasGroup>();
        cg.alpha = bIn ? 0 : 1f;
        var loop = (int)(time/0.05f);
        var fadeStep = 1f / loop;
        for(var i=0; i< loop; i++) 
        {
            yield return new WaitForSeconds(0.05f);
            cg.alpha += bIn ? fadeStep : (-1f)*fadeStep;
        }
    }

    private float PlaySound(AudioSource aSource, AudioClip aClip)
    {
        if(!aSource || !aClip) return 0f;        
        aSource.clip  = aClip;
        aSource.Play();
        return aClip.length;  
    }

}
