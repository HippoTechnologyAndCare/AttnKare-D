using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BNG;
using TMPro;
using UnityEngine.UI;
using HutongGames.PlayMaker;
using KetosGames.SceneTransition;
using TooltipAttribute = UnityEngine.TooltipAttribute;

public class NumOrderManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("CREATE PREFAB")]
    [Tooltip("Prefab for Number")]
    public GameObject prefab_Button;
    public InputBridge XrRig;
    public UIPointer RighthandPointer;
    public int int_buttonN;
    public Transform parent;
    private List<Vector3> arrPos = new List<Vector3>();
    private MoveButton[] arrBtn;
    public GameObject[] arrTrig;
    [Tooltip("Images for disctraction")]
    public Sprite[] DistracImage;

    [Header("DATA COLLECTION")]
    public AutoVoiceRecording DataCollection;
    public CheckData_NumCheck dataCheck;
    public PlayMakerFSM dataFin;
    public Transform GameDataMG;

    public bool active= false;
    public GameObject currentButton;
    public GameObject Ghost;
    public TextandSpeech narration;
    [HideInInspector]
    public bool turn = true;
    int sprite = 0;
    Vector3 vecAnswer = new Vector3(1, 1, 1); // Vector3(prevCard, trigger, card)
    private MoveButton crntCard = null;

    void Start()
    {
        SetPosition();
        CreateButton();
        StartCoroutine(GameStart());
    }

    private void SetPosition() //creat list of position of number buttons on board(total 30)
    {
        for (float i = -1.1f; i <= 1.15f; i += 0.25f)
            for (float j = 0.3f; j >= -0.2f; j -= 0.25f)
                arrPos.Add(new Vector3(i, j, 0));
        Shuffle.ShuffleList(arrPos);
    }
    private void CreateButton()
    {
        arrBtn = new MoveButton[int_buttonN];
        for (int i = 0; i < int_buttonN; i++)
        {
            GameObject go = Instantiate(prefab_Button, new Vector3(0, 0, 0), Quaternion.identity, parent);
            go.transform.localPosition = arrPos[i]; go.transform.SetSiblingIndex(i); //Set button's position and index in Hiearchy(if not above trigger, pointer cannot detect button)
            go.GetComponent<MoveButton>().btnNum = (i + 1).ToString(); go.GetComponent<MoveButton>().SetBtnNum(); //Set button Number
            arrBtn[i] = go.GetComponent<MoveButton>();
            arrBtn[i].XrRig = this.XrRig;
            arrBtn[i].RighthandPointer = this.RighthandPointer;
            arrBtn[i].Manager = this;
            if (i > arrBtn.Length - DistracImage.Length) SetSprite(arrBtn[i]);
        }
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
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(0)); //wrong order warning and narration
            dataCheck.wrongorder++;
        }
        if (myVector.y != myVector.z)
        {
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(1)); //wrong trigger warning text and narration
            dataCheck.wrongTrigger++;
        }
        crntCard.ResetButton();
    }
    private IEnumerator GameStart() //Highlight Trigger as introduction
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
    private IEnumerator ClearCoroutine()
    {
        DataCollection.StopRecordingNBehavior();
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
        dataCheck.start = false;
        yield return StartCoroutine(narration.BoardUI(2)); //Game clear narration
        dataFin.SendEvent("AllDone");
        GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();
        yield return StartCoroutine(narration.BoardUI(3));
        yield return new WaitForSeconds(2.0f);
        KetosGames.SceneTransition.SceneLoader.LoadScene(13); //load play paddle scene
    }
    private void GameClear()
    {
        StopAllCoroutines();
        StartCoroutine(ClearCoroutine());
    }

   
}
