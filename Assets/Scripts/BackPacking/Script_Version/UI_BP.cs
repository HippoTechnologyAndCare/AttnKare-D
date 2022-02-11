using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class UI_BP : MonoBehaviour
{
    [Header("CANVAS")]
    public CanvasGroup Camera_Stage; // Stage_Canvas
    public CanvasGroup Board_Start; //Start_Canvas
    public CanvasGroup Bag_Wrong; //Wrong Books
    public Transform PencilCase_Wrong; //WrongPencil
    public Transform Cap_Find; //Cap Canvas
    public Transform Button_Skip; //Button_Canvas
    public Transform Board_Finish; //Finish_Canvas
    public CanvasGroup Camera_Time; //5min_Canvas
    public CanvasGroup Camera_Finish; //FinCavas
    [Header("MEMO")]
    public Image Memo;
    public Sprite Stage2Memo;
    [Header("AUDIO SOUNDS")]
    public AudioSource Audio_Narration;
    public AudioClip[] Clips_Narration;
    public AudioSource Audio_Effect;
    public AudioClip[] Clips_Effect;

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
        yield return new WaitUntil(() => Audio_Narration.isPlaying);
        m_txtStartInfo.text = "< size = 1.2 >< b >< i > STAGE 1 :</ size ></ b ></ i >\n알림장을 보며 필기구를<i>필통</ i > 에 넣어줘!\n< size = 0.1 >\n</ size >< size = 1.2 >< b >< i > STAGE 2 :</ size ></ b ></ i >\n교과서와 알림장에 적힌 준비물을<i> 가방</ i > 에 넣어줘!\n< size = 0.1 >\n</ size > 제한시간은 < color = green > 2분 30초 </ color > 야.\n마음껏 돌아다니면서 가방을 챙겨봐!";
        NarrationSound(2);
        yield return new WaitUntil(() => Audio_Narration.isPlaying);
        bEndUI = true; //상위 스크립트가 이걸 읽은 뒤 그 다음 단계 실행 상위 스크립트에서 이걸 다시 false로 세팅해야함
    }

    void NarrationSound(int index)
    {
        Audio_Narration.clip = Clips_Narration[index];
        Audio_Narration.Play();
    }



    //==============When Wrong==================
   public IEnumerator WrongPencil() //Wrong in Pencilcase
    {
        PencilCase_Wrong.gameObject.SetActive(true);
        EffectSound("INCORRECT");
        yield return new WaitForSeconds(2.5f);
        PencilCase_Wrong.gameObject.SetActive(false);

    }

    public IEnumerator WrongBag()
    {
        Bag_Wrong.gameObject.SetActive(true);
        yield return null;
    }

    public IEnumerator StageNotification(int stage)
    {
        TextMeshProUGUI text;
        text = Camera_Stage.transform.Find("Num").GetComponent<TextMeshProUGUI>();
        text.text = stage.ToString();
        Camera_Stage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Camera_Stage.DOFade(0, 1.5f);
        yield return new WaitUntil(() => Camera_Stage.alpha == 0);
        Camera_Stage.gameObject.SetActive(false);
    }

    public void ChangeMemo()
    {
        Memo.sprite = Stage2Memo;
    }

    public void EffectSound(string when)
    {
        switch(when)
        {
            case "INCORRECT": Audio_Effect.clip = Clips_Effect[0];  break; //INCORRECT
            case "CORRECT": Audio_Effect.clip = Clips_Effect[1];  break; //CORRECT
            case "APPEAR": Audio_Effect.clip = Clips_Effect[2]; break; //Appear
            case "COMPLETE": Audio_Effect.clip = Clips_Effect[3]; break; //Appear
            case "STAR": Audio_Effect.clip = Clips_Effect[4]; break;
            case "BEEP": Audio_Effect.clip = Clips_Effect[5]; break; //Appear
            case "PENCILCASE": Audio_Effect.clip = Clips_Effect[6]; break; //Appear
        }
        Audio_Effect.Play();
    }

}
