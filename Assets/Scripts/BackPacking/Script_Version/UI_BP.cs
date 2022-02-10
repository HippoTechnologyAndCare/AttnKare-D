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

        m_txtStartInfo.text = "���߾�!\n< b >< i > ������ ì�ܾ��� �� �𸦶� �˸����� ���� ���� ��! </ b ></ i >";
        yield return new WaitUntil(() => m_audio.isPlaying);
        m_txtStartInfo.text = "< size = 1.2 >< b >< i > STAGE 1 :</ size ></ b ></ i >\n�˸����� ���� �ʱⱸ��<i>����</ i > �� �־���!\n< size = 0.1 >\n</ size >< size = 1.2 >< b >< i > STAGE 2 :</ size ></ b ></ i >\n�������� �˸��忡 ���� �غ���<i> ����</ i > �� �־���!\n< size = 0.1 >\n</ size > ���ѽð��� < color = green > 2�� 30�� </ color > ��.\n������ ���ƴٴϸ鼭 ������ ì�ܺ�!";
        AudioPlay(2);
        yield return new WaitUntil(() => m_audio.isPlaying);
        bEndUI = true; //���� ��ũ��Ʈ�� �̰� ���� �� �� ���� �ܰ� ���� ���� ��ũ��Ʈ���� �̰� �ٽ� false�� �����ؾ���
    }

    void AudioPlay(int index)
    {
        m_audio.clip = Audio_Narration[index];
        m_audio.Play();
    }

}
