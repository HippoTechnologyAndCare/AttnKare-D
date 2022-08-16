using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using HutongGames.PlayMaker.Actions;
using Scheduler;
using UnityEngine.EventSystems;

public class HUDSchedule02 : MonoBehaviour
{
    private enum Voice {HowTo, Start, HalfInfo, WellDone}

    public DictionaryScript dicScript01;
    public DictionaryScript dicScript02;
    [SerializeField] DictionaryScriptableObject dicData01;
    [SerializeField] DictionaryScriptableObject dicData02;

    [SerializeField] private ScheduleManager2 schManager;
    [SerializeField] private ScoreManager02 scoreManager;
    
    [SerializeField] private Transform questionPanel;
    
    [SerializeField] private AudioSource audSIntro; //안내 오디오 소스
    [SerializeField] private AudioClip[] audCIntro; // 안내 음성 클립
    [SerializeField] private DOTweenAnimation[] dotAnim; // 텍스트 애니메이션

    [SerializeField] private Dictionary<string, float> txtNTimingDic01;
    [SerializeField] private Dictionary<string, float> txtNTimingDic02;
    
    public Transform infoPanel;

    [SerializeField] private TextMeshProUGUI howToTMP;
    [SerializeField] private string[] howToScriptTxt;
    [SerializeField] private TextMeshProUGUI finPanelTxt;
    [SerializeField] private Transform finYnNbtns;
    [SerializeField] private GameObject confirmBtn;

    [SerializeField] private TextMeshProUGUI finHeadTxt;
    [SerializeField] private TextMeshProUGUI scoreTxt01;
    [SerializeField] private TextMeshProUGUI scoreTxt02;
    [SerializeField] private TextMeshProUGUI scoreTxt03;

    private bool _isFade;

    private void Awake()
    {
        txtNTimingDic01 = dicScript01.TxtDictionary;
        for (int i = 0; i < Mathf.Min(dicData01.Keys.Count, dicData01.Values.Count); i++)
        {
            txtNTimingDic01.Add(dicData01.Keys[i], dicData01.Values[i]);
        }
        
        txtNTimingDic02 = dicScript02.TxtDictionary;
        for (int i = 0; i < Mathf.Min(dicData02.Keys.Count, dicData02.Values.Count); i++)
        {
            txtNTimingDic02.Add(dicData02.Keys[i], dicData02.Values[i]);
        }
    }

    private void Start()
    {
        _isFade = false;
        HowToPlay();
    }
    
    public void HowToPlay()
    {
        //StartCoroutine(HowToPlayVoiceText());
        StartCoroutine(HowToPlaySetUiTxt());
    }

    /*************************************************************************
    //처음 안내문구 음성과 문구을 전시합니다
    *************************************************************************/
    
    private IEnumerator HowToPlaySetUiTxt()
    {
        if (!schManager.is1stInfoSkip)
        {
            yield return new WaitForSeconds(2f);
        
            foreach (var item in dicScript01.TxtDictionary)
            {
                var index = 0;
                if (!_isFade)
                {
                    howToTMP.SetText(item.Key);
                    FadeInPanel(infoPanel, 1f); // Info Canvas fade In
                    _isFade = true;
                    index = dicScript01.TxtDictionary.Values.ToList().IndexOf(item.Value);
                    Debug.Log(index + " 번째값");
                    Debug.Log(item.Value);
                    yield return new WaitForSeconds(item.Value);
                    continue;
                }

                index = dicScript01.TxtDictionary.Values.ToList().IndexOf(item.Value);
                Debug.Log(index + " 번째값");
                Debug.Log(item.Value);
                howToTMP.SetText(item.Key);
                yield return new WaitForSeconds(item.Value);
            }

            _isFade = false;
            FadeOutPanel(infoPanel, 1f);
            yield return new WaitForSeconds(1f);
        }
        
        schManager.VisibleStartBtn(true);
    }

    public IEnumerator HalfInfoSetUiTxt()
    {
        yield return new WaitForSeconds(2f);
        
        foreach (var item in dicScript02.TxtDictionary)
        {
            var index = 0;
            if (!_isFade)
            {
                howToTMP.SetText(item.Key);
                FadeInPanel(infoPanel, 1f); // Info Canvas fade In
                _isFade = true;
                index = dicScript02.TxtDictionary.Values.ToList().IndexOf(item.Value);
                Debug.Log(index + " 번째값");
                Debug.Log(item.Value);
                yield return new WaitForSeconds(item.Value);
                continue;
            }

            index = dicScript02.TxtDictionary.Values.ToList().IndexOf(item.Value);
            Debug.Log(index + " 번째값");
            Debug.Log(item.Value);
            howToTMP.SetText(item.Key);
            yield return new WaitForSeconds(item.Value);
        }

        _isFade = false;
        FadeOutPanel(infoPanel, 1f);
        yield return new WaitForSeconds(1f);
        //schManager.subUi.gameObject.SetActive(true);
        schManager.VisibleStartBtn(true);
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

        FadeOutPanel(infoPanel, 1f); // Info Canvas fade Out
        yield return new WaitForSeconds(2f);
    }

    public void PopupQuestion(bool isOn)
    {
        switch (isOn)
        {
             case true:
                 FadeInPanel(questionPanel, 1f);
                 questionPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
                 break;
             case false:
                 FadeOutPanel(questionPanel, 0.5f);
                 //questionPanel.GetComponent<CanvasGroup>().alpha = 0;
                 questionPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                 break;
             default:
                 Debug.Log("마지막 질문 버튼 동작안함");
                 break;
        }
    }
    /*************************************************************************
    //Fin Panel에 Score Anim을 output 하는 과정
    *************************************************************************/
    public IEnumerator ShowScore()
    {
        int length = scoreManager.scoreS.Length;
        yield return new WaitForSeconds(2f);
        
        if (length > 2)
        {
            var first = "1";
            var second = "00";

            scoreTxt02.DOText(second, 4, scrambleMode: ScrambleMode.Custom,
                scrambleChars: "0123456789").SetDelay(2).SetRelative(true);
            scoreTxt01.DOText(first, 1, scrambleMode: ScrambleMode.Custom,
                scrambleChars: "0123456789").SetDelay(4.5f).SetRelative(true);
            scoreTxt03.DOText("점", 0).SetDelay(5f);

            yield return new WaitForSeconds(5f);
            confirmBtn.SetActive(true);
        }

        else
        {
            var charArr = scoreManager.scoreS.ToCharArray();
            var firstCh = charArr[0];
            var secondCh = charArr[1];

            var first = firstCh.ToString();
            var second = secondCh.ToString();
            
            scoreTxt02.DOText(second, 1.5f, scrambleMode: ScrambleMode.Custom,
                scrambleChars: "0123456789").SetDelay(2).SetRelative(true);
            scoreTxt01.DOText(first, 6, scrambleMode: ScrambleMode.Custom,
                scrambleChars: "0123456789").SetDelay(2).SetRelative(true);
            scoreTxt03.DOText("점", 0).SetDelay(4f);
            
            yield return new WaitForSeconds(4f);
            confirmBtn.SetActive(true);
        }
    }

    public IEnumerator SetReadyShowScore()
    {
        finHeadTxt.DOFade(0, 1f);
        FadeOutPanel(finYnNbtns, 1f);
        yield return new WaitForSeconds(1f);
        
        finYnNbtns.gameObject.SetActive(false);
        finHeadTxt.text = "<color=#03045A>나의 계획표 점수는 몇점일까?";
        finHeadTxt.DOFade(1, 1f);
        schManager.m_bClickOneTime = false;
        //yield return new WaitForSeconds(1f);
    }
    
    /*************************************************************************
    //Fin Panel 초기화
    *************************************************************************/
    public void InitFinPanel()
    {
        scoreTxt01.text = "";
        scoreTxt02.text = "";
        scoreTxt03.text = "";
        finHeadTxt.text = "<color=#03045A>계획표가 마음에 드니 ?<br>그럼 이제 저장하고 종료할까 ?";
        confirmBtn.SetActive(false);
        finYnNbtns.GetComponent<CanvasGroup>().alpha = 1;
        finYnNbtns.gameObject.SetActive(true);
    }
    
    /*************************************************************************
    //Canvas Fade
    *************************************************************************/
    public void FadeInPanel(Transform panel, float time)
    {
        if(!panel) return;  // check valid canvas ?
        StartCoroutine(AnimAlpha(panel, time,true));
    }

    public void FadeOutPanel(Transform panel, float time)
    {
        if(!panel) return;  // check valid canvas ?
        StartCoroutine(AnimAlpha(panel, time,false));
    }
    
    private IEnumerator AnimAlpha(Transform panel,float time, bool bIn)
    {
        var cg = panel.GetComponent<CanvasGroup>();
        cg.alpha = bIn ? 0 : 1f;
        var loop = (int)(time/0.05f);
        var fadeStep = 1f / loop;
        for(var i=0; i< loop; i++) 
        {
            yield return new WaitForSeconds(0.05f);
            cg.alpha += bIn ? fadeStep : (-1f)*fadeStep;
        }
    }

    /*************************************************************************
    //Control Sound Clip
    *************************************************************************/
    private float PlaySound(AudioSource aSource, AudioClip aClip)
    {
        if(!aSource || !aClip) return 0f;        
        aSource.clip  = aClip;
        aSource.Play();
        return aClip.length;  
    }
    
    /*************************************************************************
    //Wait
    *************************************************************************/

    private IEnumerator WaitSec(float sec)
    {
        yield return new WaitForSeconds(sec);
    }

}
