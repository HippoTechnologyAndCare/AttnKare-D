using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hud_Paddle : MonoBehaviour
{

    public TextMeshProUGUI m_textDISTANCE;
    public TextMeshProUGUI m_textERROR;
    public AudioClip[] audioClips;
    int m_nDISTANCE  = 0;
    int m_nPERCENT;
    bool m_bCoroutine;
    // Start is called before the first frame update
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
        m_textDISTANCE.text = m_nDISTANCE.ToString();
    }

    public void ErrorMessage(string text)
    {
        m_textERROR.text = text;
        if (!m_bCoroutine)
        {
            switch (text)
            {
                case "wrong order": StartCoroutine(TextSpeechWarning("ģ�� ���⿡ ���� ������ ��", audioClips[0])); break;
                case "wrong speed": StartCoroutine(TextSpeechWarning("ģ�� ����� �ӵ��� ���� ������", audioClips[0])); break;
                case "time limit" : StartCoroutine(TextSpeechWarning("���� �ð��� �� �ƾ�\n���ݸ� �� �غ���?", audioClips[1])); break;
                case "time over"  : StartCoroutine(TextSpeechWarning("������� �غ���!", audioClips[2])); break;
            }
        }
    }

    IEnumerator TextSpeechWarning(string text,AudioClip audioClip )
    {
        m_bCoroutine = true; 
        m_textERROR.text = text;
        if (audioClip.length < 1f) yield return new WaitForSeconds(2.0f);
        else yield return new WaitForSeconds(audioClip.length);
        m_textERROR.text = "";
        m_bCoroutine = false;
    }
}
