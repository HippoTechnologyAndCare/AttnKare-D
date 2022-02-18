using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud_Paddle : MonoBehaviour
{
    public Text txtButton;
    public TextMeshProUGUI txtDISTANCE;
    public TextMeshProUGUI txtERROR;
    public AudioClip[] clipNarration;
    /*
     * [0] : Guide
     * [1] : Time Limit
     * [2] : Time Out
     */
    public AudioClip[] clipEffect;
    /*
     * [0] : CountDown
     * [1] : ButtonClick
     * [2] : Success
     * [3] : Fail
     * [4] : Stage UP
     * [5] : YE(ALL DONE)
     */
    AudioSource m_audioEffect;
    AudioSource m_audioNarration;
    int m_nDISTANCE  = 0;
    int m_nPERCENT;
    public bool bCoroutine;
    // Start is called before the first frame update

    void Awake()
    {
        var audioSources = transform.GetComponents<AudioSource>();
        m_audioEffect = audioSources[0];
        m_audioNarration = audioSources[1];
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDistance(int nStage)
    { 
        m_nPERCENT = Manager_Paddle.SDB[nStage].intPercent;
        m_nDISTANCE += m_nPERCENT;
        txtDISTANCE.text = m_nDISTANCE.ToString();
    }

    public void AudioController(string text)
    {
        txtERROR.text = text;
        if (!bCoroutine)
        {
            switch (text)
            {
                case "guide"      : PlayNarration(clipNarration[0], false); break;
                case "bgm"        : PlayNarration(clipNarration[3], true); break;
                case "button"     : StartCoroutine(TextSpeechWarning(null, clipEffect[1])); break;
                case "correct"    : StartCoroutine(TextSpeechWarning("잘했어!", clipEffect[2])); break;
                case "wrong order": StartCoroutine(TextSpeechWarning("친구 방향에 맞춰 돌려야 해", clipEffect[3])); break;
                case "wrong speed": StartCoroutine(TextSpeechWarning("친구 페달의 속도에 맞춰 돌려줘", clipEffect[3])); break;
                case "time limit" : PlayNarration(clipNarration[1], false); break;
                case "time over"  : PlayNarration(clipNarration[2], false); break;
                case "stage"      : StartCoroutine(TextSpeechWarning("속도와 방향이 바뀌었어!", clipEffect[4])); break;
                case "complete"   : StartCoroutine(TextSpeechWarning("정말 잘했어!", clipEffect[5])); break;
            }
        }
    }

   
    public IEnumerator CountDown() {
        bCoroutine = true;
        m_audioNarration.Stop();
        m_audioEffect.clip = clipEffect[0];
        m_audioEffect.Play();
        txtButton.text = "3";
        yield return new WaitForSeconds(.7f);
        m_audioEffect.Play();
        txtButton.text = "2";
        yield return new WaitForSeconds(.7f);
        m_audioEffect.Play();
        txtButton.text = "1";
        yield return new WaitForSeconds(.7f);
        txtButton.transform.parent.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        bCoroutine = false;


    }
    IEnumerator TextSpeechWarning(string text,AudioClip audioClip )
    {
        bCoroutine = true;
        if (text!=null) txtERROR.text = text;
        m_audioEffect.clip = audioClip;
        m_audioEffect.Play();
        if (audioClip.length < 1f) yield return new WaitForSeconds(2.0f);
        else yield return new WaitForSeconds(audioClip.length);
        txtERROR.text = "";
        bCoroutine = false;
    }

    public void PlayNarration(AudioClip audioClip, bool loop)
    {
        m_audioNarration.clip = audioClip;
        m_audioNarration.Play();
        m_audioNarration.loop = loop;

    }
}
