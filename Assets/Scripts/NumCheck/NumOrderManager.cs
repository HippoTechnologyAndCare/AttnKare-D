using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;
using UnityEngine.UI;
using HutongGames.PlayMaker;
using KetosGames.SceneTransition;

public class NumOrderManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentButton;
    public GameObject Ghost;
    public AutoVoiceRecording DataCollection;
    public InputBridge XrRig;
    public bool active= false;
    public Sprite[] DistracImage;
    public GameObject ImagePrefab;
    public MoveButton[] arrBtn;
    public GameObject[] arrTrig;
    public AudioClip[] arrNarr;
    public CheckData_NumCheck dataCheck;
    public PlayMakerFSM dataFin;
    public Transform GameDataMG;
    public TextMeshProUGUI Text;
    public GameObject speechBubble;
    public TextandSpeech narration;
    [HideInInspector]
    public bool turn = true;
    public string btnNum;
    int sprite = 0;
    bool coroutine=false;
    Vector3 vecAnswer = new Vector3(1, 1, 1); // Vector3(prevCard, trigger, card)
    AudioSource audioPlay;
    MoveButton crntCard = null;

    public bool b_narStart;

    void Start()
    {
        string[] arrRandom = new string[arrBtn.Length];
        for (int i = 0; i < arrBtn.Length; i++)
            arrRandom[i] = (i + 1).ToString();  
        ShuffleNum(arrRandom); //shuffle numbers
        for (int count = 0; count < arrBtn.Length; count++){
            string num_s = arrRandom[count];
            int num = int.Parse(num_s);
            arrBtn[count].btnNum = num_s;
            arrBtn[count].SetBtnNum();
            if (num > arrBtn.Length - DistracImage.Length) // 이미지 추가
                SetSprite(arrBtn[count]);
        }
        StartCoroutine(HighlightTrigger());
        
    }
    public string[] ShuffleNum(string[] arrNum) //shuffle button numbers
    {
        for (int i = 0; i < arrNum.Length; i++){
            int rnd_n = Random.Range(0, arrNum.Length);
            string temp = arrNum[i];
            arrNum[i] = arrNum[rnd_n];
            arrNum[rnd_n] = temp;
        }
        return arrNum;
    }
    private void SetSprite(MoveButton btn) //특정 버튼에 Distraction Image를 추가
    {
        btn.distraction = true;
        btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
        Image btnImage = btn.transform.GetChild(0).GetComponent<Image>();
        btnImage.sprite = DistracImage[sprite];
        var tempColor = btnImage.color;
        tempColor.a = 1f;
        btnImage.color = tempColor;
        sprite++;
    }
public void CannotGrab(MoveButton num) //한 버튼 만졌을 때 다른 버튼 MoveButton OFF
    {
        for(int i =0;i<arrBtn.Length;i++){
            if(arrBtn[i] != num)
                arrBtn[i].enabled = false;
        }
    }

    public void CanGrab() //버튼 놓은 후 다시 MoveButton On
    {
        for (int i = 0; i < arrBtn.Length; i++)
            arrBtn[i].enabled = true;
    }
    
    public void CardInTrigger(MoveButton card, TriggerButton trigger) //카드가 트리거 안에 들어갔을 때 데이터 체크
    {
        crntCard = card;
        float cardNum = float.Parse(card.btnNum);
        float trigNum = float.Parse(trigger.trigNum);
        Vector3 myVector = new Vector3(vecAnswer.x, trigNum, cardNum);
        if (myVector == vecAnswer)
        {
            card.SetButton();
            active = false;
            if(cardNum == arrTrig.Length)
            {
                GameClear();
                return;
            }
            vecAnswer = new Vector3(cardNum + 1, cardNum + 1, cardNum + 1);
            return;
        }
        if (myVector.x != myVector.z)
        {
            if (!coroutine)
            {
          //      NarrPlay(arrNarr[4]);
                StartCoroutine(Warning("순서대로 다시 놓아볼까?"));
            }
            dataCheck.wrongorder++;
        }
        if (myVector.y != myVector.z)
        {
            if (!coroutine)
            {
        //        NarrPlay(arrNarr[5]);
                StartCoroutine(Warning("올바른 빈칸인지 다시 확인해봐!"));
            }
            dataCheck.wrongTrigger++;
        }
        crntCard.ResetButton();
    }
    IEnumerator HighlightTrigger() //Highlight Trigger as introduction
    {
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(narration.Introduction());
        for (int i =0; i <3; i++){
            foreach(GameObject trigger in arrTrig)
            {
                trigger.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                trigger.SetActive(true);
            }
        }
        foreach (MoveButton button in arrBtn) { button.enabled = true; }
        dataCheck.start = true; //data check playtime
    }

    IEnumerator Warning(string text)
    {
        coroutine = true;
        string originalText = Text.text;
        Text.text = text;
        yield return new WaitForSeconds(2.0f);
        Text.text = originalText;
        coroutine = false;
    }

    IEnumerator AllClear()
    {
        DataCollection.StopRecordingNBehavior();
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
        dataCheck.start = false;
    //    NarrPlay(arrNarr[6]);
        Text.text = "숫자를 1부터 8까지 정리했어! 잘했어~!";
        yield return new WaitWhile(() => audioPlay.isPlaying);
        dataFin.SendEvent("AllDone");
        GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();
        Text.text = "다음으로 넘어가는 중...";
        yield return new WaitForSeconds(2.0f);
        KetosGames.SceneTransition.SceneLoader.LoadScene(13); //load play paddle scene
    }
    private void GameClear()
    {
        StopAllCoroutines();
        StartCoroutine(AllClear());
    }

  
}
