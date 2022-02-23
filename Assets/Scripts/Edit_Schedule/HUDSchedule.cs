using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

[System.Serializable]
public class TxtNTimeScript : ISerializationCallbackReceiver
{
    [SerializeField] 
    private DictionaryScriptableObject dictionaryData;

    [SerializeField]
    private List<string> keys = new List<string>();
    [SerializeField]
    private List<float> values = new List<float>();

    public Dictionary<string, float> TxtDictionary = new Dictionary<string, float>();

    public bool modifyValues;
    
    public void OnBeforeSerialize()
    {
        if (modifyValues) return;
        keys.Clear();
        values.Clear();
    
        for (var i = 0; i != Math.Min(dictionaryData.Keys.Count, dictionaryData.Values.Count); i++)
        {
            keys.Add((dictionaryData.Keys[i]));
            values.Add(dictionaryData.Values[i]);
        }
    }

    public void OnAfterDeserialize()
    {
        
    }
    
    public void DeserializeDictionary()
    {
        Debug.Log("Deserialization");
        TxtDictionary = new Dictionary<string, float>();
        dictionaryData.Keys.Clear();
        dictionaryData.Values.Clear();

        for (var i = 0; i < Math.Min(keys.Count, values.Count); i++)
        {
            dictionaryData.Keys.Add(keys[i]);
            dictionaryData.Values.Add(values[i]);
            TxtDictionary.Add(keys[i], values[i]);
        }

        modifyValues = false;
    }
}

public class HUDSchedule : MonoBehaviour
{
    private enum Voice {HowTo, Start, HalfInfo, WellDone}

    public TxtNTimeScript dicScript;
    [SerializeField] DictionaryScriptableObject dicData;

    /*************************************************************************
    //처음 안내문구 음성과 문구을 전시합니다
    *************************************************************************/
    [SerializeField] private AudioSource audSIntro; //안내 오디오 소스
    [SerializeField] private AudioClip[] audCIntro; // 안내 음성 클립
    [SerializeField] private DOTweenAnimation[] dotAnim; // 텍스트 애니메이션

    [SerializeField] private Dictionary<string, float> txtNTimingDic;
    
    public Canvas infoCanvas;

    [SerializeField] private TextMeshProUGUI howToTMP;
    [SerializeField] private string[] howToScriptTxt;

    private bool _isFade;

    private void Awake()
    {
        txtNTimingDic = dicScript.TxtDictionary;
        for (int i = 0; i < Mathf.Min(dicData.Keys.Count, dicData.Values.Count); i++)
        {
            txtNTimingDic.Add(dicData.Keys[i], dicData.Values[i]);
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

    private IEnumerator HowToPlaySetUiTxt()
    {
        yield return new WaitForSeconds(2f);
        
        foreach (var item in dicScript.TxtDictionary)
        {
            var index = 0;
            if (!_isFade)
            {
                howToTMP.SetText(item.Key);
                FadeInCanvas(infoCanvas, 1f); // Info Canvas fade In
                _isFade = true;
                index = dicScript.TxtDictionary.Values.ToList().IndexOf(item.Value);
                Debug.Log(index + " 번째값");
                Debug.Log(item.Value);
                yield return new WaitForSeconds(item.Value);
                continue;
            }

            index = dicScript.TxtDictionary.Values.ToList().IndexOf(item.Value);
            Debug.Log(index + " 번째값");
            Debug.Log(item.Value);
            howToTMP.SetText(item.Key);
            yield return new WaitForSeconds(item.Value);
        }
        
        // foreach (var setT in howToScriptTxt)
        // {
        //     if (!_isFade)
        //     {
        //         howToTMP.SetText(setT);
        //         FadeInCanvas(infoCanvas, 1f); // Info Canvas fade In
        //         _isFade = true;
        //     }
        //
        //     yield return new WaitForSeconds(2f);
        //     howToTMP.SetText(setT);
        // }

        //yield return new WaitForSeconds(3f);
        
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
