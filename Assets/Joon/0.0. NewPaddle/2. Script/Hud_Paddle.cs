using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud_Paddle : MonoBehaviour
{
    [SerializeField] Button buttonStart;
    [SerializeField] Text txtButton;
    [SerializeField] Canvas canvasGuide;
    [SerializeField] GameObject canvasDistance;
    [SerializeField] TextMeshProUGUI txtDISTANCE;
    [SerializeField] Slider image_CableCar;
    [SerializeField] TextMeshProUGUI txtERROR;
    [SerializeField] Canvas canvasFinish;
    [SerializeField] Canvas canvasQuestion;
    [SerializeField] Text TimeText;
    TextMeshProUGUI txtFinish;
    [SerializeField] AudioClip[] clipNarration;
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
    AudioSource m_audioBGM;
    int m_nDISTANCE  = 0;
    int m_nPERCENT;
    public bool bCoroutine;
    // Start is called before the first frame update
    [SerializeField]
    public bool bTimeStart = false;
    public float m_Time;
    TimeSpan m_TimeSpan;

    void Awake()
    {
        var audioSources = transform.GetComponents<AudioSource>();
        m_audioEffect = audioSources[0];
        m_audioNarration = audioSources[1];
        m_audioBGM = audioSources[2];
    }
    void Start()
    {
        m_audioBGM.clip = clipNarration[3];
    }

    // Update is called once per frame
    void Update()
    {
        if (bTimeStart)
        {
            m_Time -= Time.deltaTime;
            m_TimeSpan = TimeSpan.FromSeconds(m_Time);
            TimeText.text = m_TimeSpan.ToString(@"mm\:ss");
            if (m_Time < 0) bTimeStart = false;
        }
    }

    public void SetDistance(int nStage)
    { 
        m_nPERCENT = Manager_Paddle.SDB[nStage].intPercent;
        m_nDISTANCE += m_nPERCENT;
        image_CableCar.value = (float)m_nDISTANCE / 100;
        Debug.Log(m_nDISTANCE +"SSSSS"+ image_CableCar.value);
        txtDISTANCE.text = m_nDISTANCE.ToString();
    }

    public void AudioController(string text)
    {
        switch (text)
            {
                case "guide"      : StartCoroutine(PlayNarration(clipNarration[0], false)); break;
                case "button"     : StartCoroutine(TextSpeechWarning(null, clipEffect[1])); break;
                case "correct"    : StartCoroutine(TextSpeechWarning("���߾�!", clipEffect[2])); break;
                case "wrong order": StartCoroutine(TextSpeechWarning("ģ�� ���⿡ ���� ������ ��", clipEffect[3])); break;
                case "wrong speed": StartCoroutine(TextSpeechWarning("ģ�� ����� �ӵ��� ���� ������", clipEffect[3])); break;
                case "time limit" : StartCoroutine(PlayNarration(clipNarration[1], false)); break;
                case "time over"  : StartCoroutine(PlayNarration(clipNarration[2], false)); break;
                case "stage"      : StartCoroutine(TextSpeechWarning("�ӵ��� ������ �ٲ����!", clipEffect[4])); break;
                case "complete"   : StartCoroutine(TextSpeechWarning("���� ���߾�!", clipEffect[5])); break;
                case "question"   : StartCoroutine(Question()); break;
        }
    }
   
    public void BGMplay(bool play)
    {
        if (play) m_audioBGM.Play();
        if (!play) m_audioBGM.Pause();
    }
    public IEnumerator CountDown() {
        bCoroutine = true;
        buttonStart.enabled = false;
        m_audioNarration.Stop();
        m_audioEffect.clip = clipEffect[0];
        m_audioEffect.Play();
        txtButton.text = "3";
        yield return new WaitForSeconds(.9f);
        m_audioEffect.Play();
        txtButton.text = "2";
        yield return new WaitForSeconds(.9f);
        m_audioEffect.Play();
        txtButton.text = "1";
        yield return new WaitForSeconds(.9f);
        canvasGuide.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        canvasDistance.SetActive(true);
        bCoroutine = false;
    }
    IEnumerator TextSpeechWarning(string text,AudioClip audioClip )
    {
        bCoroutine = true;
        if (text!=null) txtERROR.text = text;
        Debug.Log(text);
        m_audioEffect.clip = audioClip;
        m_audioEffect.Play();
        if (audioClip.length < 1f) yield return new WaitForSeconds(2.0f);
        else yield return new WaitForSeconds(audioClip.length);
        txtERROR.text = "";
        bCoroutine = false;
    }

    public IEnumerator PlayNarration(AudioClip audioClip, bool loop)
    {
        bCoroutine = true;
        m_audioNarration.clip = audioClip;
        m_audioNarration.Play();
        m_audioNarration.loop = loop;
        yield return new WaitForSeconds(audioClip.length);
        bCoroutine = false;

    }

    IEnumerator Question()
    {
        yield return new WaitForSeconds(2.5f);
        m_audioNarration.Stop();
        canvasGuide.gameObject.SetActive(false);
        canvasDistance.SetActive(false);
        canvasQuestion.gameObject.SetActive(true);
        StartCoroutine(PlayNarration(clipNarration[4], false));
    }
    int a;
    public IEnumerator NextScene()
    {
        bCoroutine = true;
        canvasQuestion.gameObject.SetActive(false);
        canvasFinish.gameObject.SetActive(true);
        yield return StartCoroutine(PlayNarration(clipNarration[5], false));
        txtFinish = canvasFinish.transform.GetComponentInChildren<TextMeshProUGUI>();
        txtFinish.text = "�̵��մϴ�";
        yield return new WaitForSeconds(2f);
        txtFinish.text = "3";
        yield return new WaitForSeconds(0.9f);
        txtFinish.text = "2";
        yield return new WaitForSeconds(0.9f);
        txtFinish.text = "1";
        yield return new WaitForSeconds(1f);
        bCoroutine = false;
    }
}
