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
    public Transform Bag_Wrong; //Wrong Books
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

    int a;
    string line;
    Object_BP Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("GameFlow_Manager").GetComponent<Object_BP>();
        bEndUI = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator CanvasStart()
    {
        yield return new WaitForSeconds(2.0f);
        var child = Board_Start.transform.GetChild(0);
        m_txtStartInfo = child.transform.Find("Info1").GetComponent<TextMeshProUGUI>();
        m_txtStartInfo.text = "<size=1.4>å������ ì����! </size>\n\n<size=1.2><b><i> STAGE 1 :</size></b></i>\n�˸����� ���� �ʱⱸ��<i>����</i> �� �־���!\n<size=0.1>\n</size><size=1.2><b><i> STAGE 2 :</size></b></i>\n�������� �˸��忡 ���� �غ���<i> ����</i> �� �־���!\n<size=0.1>\n</size>";
        NarrationSound(0);
        yield return new WaitUntil(() => !Audio_Narration.isPlaying);
        m_txtStartInfo.text = "<size=1.1>�˸���� �ð�ǥ�� �� ���� �ٿ��׾�\n������ �ٰ����� ���̴� �����!\n���ѽð��� <color=green> 2�� 30�� </color>��.\n������ ���ƴٴϸ鼭 ������ ì�ܺ�!";
        NarrationSound(1);
        yield return new WaitUntil(() => !Audio_Narration.isPlaying);
        Board_Start.DOFade(0, 3);
        yield return new WaitUntil(() => Board_Start.alpha == 0);
        Manager.Stage1();
        
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

    public IEnumerator WrongBag(int index)
    {
        TextMeshProUGUI tmproText = Bag_Wrong.GetComponentInChildren<TextMeshProUGUI>();
        switch (index)
        {
            case 1: line = "������ ���뿡 �ִ°� ���� ��!";  break; //pencil
            case 2: line = "�ð�ǥ�� �ٽ� Ȯ���غ�!";  break; //textbook
            case 3: line = "�˸����� �ٽ� Ȯ���غ�!";  break; //memo
            case 4: line = "������ ���뿡 �ʱⱸ�� �־�� ��"; break; //wrong stage
        }
        tmproText.text = line;
        Bag_Wrong.gameObject.SetActive(true);
        EffectSound("INCORRECT");
        yield return new WaitForSeconds(3.0f);
        Bag_Wrong.gameObject.SetActive(false);
    }

    public IEnumerator StageNotification(int stage)
    {
        TextMeshProUGUI text;
        var child = Camera_Stage.transform.GetChild(0);
        text = child.Find("Num").GetComponent<TextMeshProUGUI>();
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
    //===========Time Check=======

    public IEnumerator TimeCheck(string strStamp)
    {
        TextMeshProUGUI tmproTime = Camera_Time.GetComponentInChildren<TextMeshProUGUI>();
        int index = 0;
        switch (strStamp)
        {
            case "TIME LIMIT": tmproTime.text = "���ݸ� ���ѷ�����?"; index = 2; break;
            case "TIME OUT": tmproTime.text = "������� �غ���!"; index = 3; break;
            default: break;
        }
        Camera_Time.DOFade(1, 0.8f);
        NarrationSound(index);
        yield return new WaitUntil(() => !Audio_Narration.isPlaying);
        Camera_Time.DOFade(0, 0.8f);
        yield return new WaitUntil(() => Camera_Time.alpha == 0);
        bEndUI = true;
    }

    public IEnumerator GameFinish()
    {
        GameObject Fin2 = Camera_Finish.transform.Find("Fin_2").gameObject;
        GameObject Fin1 = Camera_Finish.transform.Find("Fin_1").gameObject;
        Fin1.SetActive(false);
        Fin2.SetActive(true);
        Camera_Finish.DOFade(1, 0.4f);
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1.0f);
        Fin2.GetComponentInChildren<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1.0f);
        Manager.NextScene();
        
    }

    public void BagAllPacked()
    {
        Board_Finish.gameObject.SetActive(true);
        EffectSound("STAR");
    }
}
