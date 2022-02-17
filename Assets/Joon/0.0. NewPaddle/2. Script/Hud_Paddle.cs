using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hud_Paddle : MonoBehaviour
{
    public TextMeshProUGUI txtButton;
    public TextMeshProUGUI txtDISTANCE;
    public TextMeshProUGUI txtERROR;
    public AudioClip[] audioNarration;
    /*
     * [0] : Guide
     * [1] : Time Limit
     * [2] : Time Out
     */
    public AudioClip[] audioEffect;
    /*
     * [0] : CountDown
     * [1] : ButtonClick
     * [2] : Success
     * [3] : Fail
     * [4] : Stage UP
     * [5] : YE(ALL DONE)
     */
    AudioSource m_audioEffect;
    int m_nDISTANCE  = 0;
    int m_nPERCENT;
    bool m_bCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        m_audioEffect = this.transform.GetComponent<AudioSource>();
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

    public void ErrorMessage(string text)
    {
        txtERROR.text = text;
        if (!m_bCoroutine)
        {
            switch (text)
            {
                case "button"     : StartCoroutine(TextSpeechWarning(null, audioEffect[1])); break;
                case "correct"    : StartCoroutine(TextSpeechWarning("잘했어!", audioEffect[2])); break;
                case "wrong order": StartCoroutine(TextSpeechWarning("친구 방향에 맞춰 돌려야 해", audioEffect[3])); break;
                case "wrong speed": StartCoroutine(TextSpeechWarning("친구 페달의 속도에 맞춰 돌려줘", audioEffect[3])); break;
                case "time limit" : StartCoroutine(TextSpeechWarning("제한 시간이 다 됐어\n조금만 더 해볼까?", audioNarration[1])); break;
                case "time over"  : StartCoroutine(TextSpeechWarning("여기까지 해보자!", audioNarration[2])); break;
                case "stage"      : StartCoroutine(TextSpeechWarning("속도와 방향이 바뀌었어!", audioEffect[4])); break;
                case "complete"   : StartCoroutine(TextSpeechWarning("정말 잘했어!", audioEffect[5])); break;
            }
        }
    }

   
    public IEnumerator CountDown() {
        m_bCoroutine = true;
        m_audioEffect.clip = audioEffect[0];
        m_audioEffect.Play();
        txtButton.text = "3";
        yield return new WaitForSeconds(.7f);
        m_audioEffect.Play();
        txtButton.text = "2";
        yield return new WaitForSeconds(.7f);
        m_audioEffect.Play();
        txtButton.text = "1";
        yield return new WaitForSeconds(.7f);
        m_bCoroutine = false;


    }
    IEnumerator TextSpeechWarning(string text,AudioClip audioClip )
    {
        m_bCoroutine = true;
        if (text!=null) txtERROR.text = text;
        if (audioClip.length < 1f) yield return new WaitForSeconds(2.0f);
        else yield return new WaitForSeconds(audioClip.length);
        txtERROR.text = "";
        m_bCoroutine = false;
    }
}
