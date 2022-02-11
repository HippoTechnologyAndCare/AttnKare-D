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

public class Guide_NumCheck : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("CREATE PREFAB")]
    [Tooltip("Prefab for Number")]
    public GameObject prefab_Button;
    public InputBridge XrRig;
    public UIPointer RighthandPointer;
    public int int_buttonN;
    public Transform parent;
    public List<Vector3> arrPos = new List<Vector3>();
    public List<MoveButton> arrBtn;
    public GameObject[] arrTrig;
    [Tooltip("Images for disctraction")]
    public Sprite[] DistracImage;

    [Header("DATA COLLECTION")]
    public AutoVoiceRecording DataCollection;
    public CheckData_NumCheck dataCheck;
    public PlayMakerFSM dataFin;
    public Transform GameDataMG;

    public bool active= false;
    public GameObject Ghost;
    public TextandSpeech narration;
    [HideInInspector]
    public bool turn = true;
    int sprite = 0;

    public static int Index = 0;
    AutoButton auto;

    public enum INDEX { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN }
    public struct NUMCHECK_LIST
    {
        public INDEX eIndex;
        public int nNum;//����
        public GameObject goNum;
        public GameObject goTrig; //trigger
        public int nBtn;
        public bool bIN;
        public NUMCHECK_LIST(INDEX eIndex, int nNum, GameObject goNum, GameObject goTrig, int nBtn, bool bIN)
        {
            this.eIndex = eIndex;
            this.nNum = nNum;
            this.goNum = goNum;
            this.goTrig = goTrig;
            this.nBtn = nBtn;
            this.bIN = bIN;
        }
    }

    public static NUMCHECK_LIST[] NCDB = new NUMCHECK_LIST[]  {
      new NUMCHECK_LIST( INDEX.ONE ,1,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.TWO ,2,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.THREE ,3,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.FOUR ,4,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.FIVE,5,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.SIX ,6,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.SEVEN ,7,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.EIGHT ,8,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.NINE ,9,  null, null, 0, false),
      new NUMCHECK_LIST(INDEX.TEN ,10, null, null, 0, false)
    };
    void Awake()
    {
        SetPosition();
        CreateButton();

    }
    void Start()
    {
        SetTrigger();
        auto = GetComponentInChildren<AutoButton>();
        StartCoroutine(GameStart());
    }

    private void SetPosition() //creat list of position of number buttons on board(total 30)
    {
        for (float i = -0.8f; i <= 0.95f; i += 0.25f)
            for (float j = 0.3f; j >= -0.2f; j -= 0.25f)
            {
                float n = i;
                float m = j;
                n += Random.Range(-0.07f, 0.07f);   //��ġ�� �����ϰ� �ϱ� ���ؼ�
                m -= Random.Range(-0.08f, 0.08f);   //��ġ�� �����ϱ� �ϱ� ���ؼ�
                arrPos.Add(new Vector3(n, m, 0)); 
            }
        Shuffle.ShuffleList(arrPos);
    }
    private void CreateButton()
    {
        arrBtn = new List<MoveButton>();
        for (int i = 0; i < int_buttonN; i++)
        {
            GameObject go = Instantiate(prefab_Button, new Vector3(0, 0, 0), Quaternion.identity, parent);
            MoveButton goTemp = go.GetComponent<MoveButton>();
            go.transform.localPosition = arrPos[i]; go.transform.SetSiblingIndex(i); //Set button's position and index in Hiearchy(if not above trigger, pointer cannot detect button)
            goTemp.btnNum = (i + 1).ToString(); go.GetComponent<MoveButton>().SetBtnNum(); //Set button Number
            goTemp.XrRig = this.XrRig;
            goTemp.RighthandPointer = this.RighthandPointer;
            if (i < NCDB.Length) { NCDB[i].goNum = goTemp.gameObject; goTemp.m_eIndex = NCDB[i].eIndex; }
            arrBtn.Add(goTemp);
            if (i > int_buttonN - DistracImage.Length) SetSprite(goTemp);
        }
    }

    void SetTrigger()
    {
        for (int i = 0; i < arrTrig.Length; i++) NCDB[i].goTrig = arrTrig[i];
    }
    private void SetSprite(MoveButton btn) //Ư�� ��ư�� Distraction Image�� �߰�
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
    public void CannotGrab(MoveButton num) //�� ��ư ������ �� �ٸ� ��ư MoveButton OFF
    {
        for(int i =0;i<arrBtn.Count;i++){
            if(arrBtn[i] != num) arrBtn[i].enabled = false;
        }
    }
    public void CanGrab() //��ư ���� �� �ٽ� MoveButton On
    {
        for (int i = 0; i < arrBtn.Count; i++) arrBtn[i].enabled = true;
    }
    public void NumInTrigger(MoveButton num, GameObject trigger) //ī�尡 Ʈ���� �ȿ� ���� �� ������ üũ
    {
        int cardNum = int.Parse(num.btnNum);
        NUMCHECK_LIST m_current = NCDB[Index];
        m_current.nBtn = cardNum;
        if(NCDB[(int)num.m_eIndex].nNum == NCDB[Index].nNum && NCDB[(int)num.m_eIndex].goTrig == trigger.gameObject)
        {
            num.SetButton();
            active = false;
            if (cardNum >= arrTrig.Length)
            {
                GameClear();
                return;
            }
            arrBtn.Remove(num);
            Destroy(num);
            Index++;
            StartCoroutine(narration.BoardUI(4));
            auto.AutoMove();
            return;
        }
        if(NCDB[(int)num.m_eIndex].nNum != m_current.nNum) //���� ��ư�� ������ ���� ����
        {
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(0)); //wrong order warning and narration
            dataCheck.wrongorder++;
        }
        if(NCDB[(int)num.m_eIndex].goTrig != trigger.gameObject) //���� ��ư�� �ùٸ� ĭ�� ���� ����
        {
            if (!narration.coroutine) StartCoroutine(narration.BoardUI(1)); //wrong trigger warning text and narration
            dataCheck.wrongTrigger++;
        }
        CanGrab();
        num.ResetButton();

    }
    private IEnumerator GameStart() //Highlight Trigger as introduction
    {
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(narration.Introduction());
        for (int i =0; i <1; i++){
            foreach(GameObject trigger in arrTrig)
            {
                trigger.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                trigger.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.8f);
        foreach (MoveButton button in arrBtn) { button.enabled = true; }
        dataCheck.start = true; //data check playtime
        auto.AutoMove();

    }
    private IEnumerator ClearCoroutine()
    {
        DataCollection.StopRecordingNBehavior();
        GameDataMG.GetComponent<GameDataManager>().SaveCurrentData();
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
        dataCheck.start = false;
        yield return StartCoroutine(narration.BoardUI(3)); //Game clear narration
        dataFin.SendEvent("AllDone");
        yield return new WaitForSeconds(2.5f);
        narration.EndUI("�������� �Ѿ�� ��....");
        yield return new WaitForSeconds(2.0f);
        KetosGames.SceneTransition.SceneLoader.LoadScene(13); //load play paddle scene
    }
    private void GameClear()
    {
        StopAllCoroutines();
        StartCoroutine(ClearCoroutine());
    }

   
}
