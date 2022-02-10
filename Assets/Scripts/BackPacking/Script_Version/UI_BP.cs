using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class UI_BP : MonoBehaviour
{
    public CanvasGroup Camera_Stage; // Stage_Canvas
    public CanvasGroup Board_Start; //Start_Canvas
    public CanvasGroup Bag_Wrong; //Wrong Books
    public Transform Cap_Find; //Cap Canvas
    public Transform Button_Skip; //Button_Canvas
    public Transform Board_Finish; //Finish_Canvas
    public CanvasGroup Camera_Time; //5min_Canvas
    public CanvasGroup Camera_Finish; //FinCavas

    public AudioClip[] Audio_Narration;
    public AudioClip[] Audio_Effect;

    AudioSource m_audio;
    TextMeshProUGUI m_txtStartInfo;

    public bool bEndUI;
    // Start is called before the first frame update
    void Start()
    {
        m_txtStartInfo = Board_Start.GetComponentInChildren<TextMeshProUGUI>();
        bEndUI = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CanvasStart()
    {
        yield return new WaitForSeconds(0.8f);

        m_txtStartInfo.text = "잘했어!\n< b >< i > 무엇을 챙겨야할 지 모를땐 알림장을 열어 보면 돼! </ b ></ i >";
        yield return new WaitUntil(() => m_audio.isPlaying);
        m_txtStartInfo.text = "< size = 1.2 >< b >< i > STAGE 1 :</ size ></ b ></ i >\n알림장을 보며 필기구를<i>필통</ i > 에 넣어줘!\n< size = 0.1 >\n</ size >< size = 1.2 >< b >< i > STAGE 2 :</ size ></ b ></ i >\n교과서와 알림장에 적힌 준비물을<i> 가방</ i > 에 넣어줘!\n< size = 0.1 >\n</ size > 제한시간은 < color = green > 2분 30초 </ color > 야.\n마음껏 돌아다니면서 가방을 챙겨봐!";
        AudioPlay(2);
        yield return new WaitUntil(() => m_audio.isPlaying);
        bEndUI = true; //상위 스크립트가 이걸 읽은 뒤 그 다음 단계 실행 상위 스크립트에서 이걸 다시 false로 세팅해야함
    }

    void AudioPlay(int index)
    {
        m_audio.clip = Audio_Narration[index];
        m_audio.Play();
    }

}
