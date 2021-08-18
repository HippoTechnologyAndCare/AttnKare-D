using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.Rendering;
using KetosGames.SceneTransition;


    public class OpenManager : MonoBehaviour
    {
       
        public GameObject PlayerController;
        public GameObject Ghost;
        public GameObject Door;
        public Volume global;
        public Transform Logo;
    public Transform TransitionIN;
    public Transform TransitionOut;

    public NetworkManager NetworkManager;



        public float speed;
        public Transform SpeechBubble;

        private float Timer = 0;
        private string state;
        bool fadeColor = false;
        float GhostRot;
        int audioIndex;
        Vector3 desPos;

        VolumeProfile globalVolume;

        ColorAdjustments _coloradjustment = null;





    private void Awake()
    {
        
        if(PlayerPrefs.HasKey("State"))
        {
            state = PlayerPrefs.GetString("State");

        }
        else
        {
            state = "OPEN";
        }
    }
    // Start is called before the first frame update
    void Start()
        {

        
        globalVolume = global.sharedProfile;
        globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

      

        if (state =="OPEN") 
        {

            Ghost.transform.position = new Vector3(3.1f, -1.337f, 3.09f);
          
            _coloradjustment.saturation.value = -100f;
            
            

            StartCoroutine(startOpening());


        }
        if(state =="END")
        {
            Ghost.transform.position = new Vector3(0.031f, -0.56f, 6.695f);
            _coloradjustment.saturation.value = -10f;
            StartCoroutine(startEnding());

        }





        }

    // Update is called once per frame
    void Update()
    {
        

        float GhostRot = Ghost.transform.eulerAngles.y;
        SpeechBubble.localPosition = new Vector3(Ghost.transform.localPosition.x + 0.482f, Ghost.transform.localPosition.y + 0.865f, Ghost.transform.localPosition.z) ;
        ///SpeechBubble.localEulerAngles = new Vector3(SpeechBubble.localEulerAngles.x, GhostRot, SpeechBubble.localEulerAngles.z);
        Logo.localEulerAngles = new Vector3(Logo.localEulerAngles.x, GhostRot, Logo.localEulerAngles.z);
        
        globalVolume = global.sharedProfile;
        globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);
        if (fadeColor)
        {
            if (state == "OPEN")
            {
                Timer += Time.deltaTime;
                if (Timer > 0.02f)
                {
                    Timer = 0;
                    _coloradjustment.saturation.value += 1f;

                }

            }
            if (state == "END")
            {
                Timer += Time.deltaTime;
                if (Timer > 0.02f)
                {
                    Timer = 0;
                    _coloradjustment.saturation.value -= 1f;


                }


                //synchronize rotation of ghost and speech bubbles
               




            }
        }
    }

        private void LateUpdate()
        {

            globalVolume = global.sharedProfile;
            globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

            if (_coloradjustment.saturation.value == -10f && state != "END")
            {
            state = "MID";
            }
        if (_coloradjustment.saturation.value == -100f && state != "OPEN")
        {
            state = "MID";
        }



    }


    
 

    IEnumerator lerpDoor()
    {
        float t = 0;
        while (true)
        {
            Door.transform.localEulerAngles = Vector3.Lerp(new Vector3(Door.transform.localEulerAngles.x, Door.transform.localEulerAngles.y, Door.transform.localEulerAngles.z), new Vector3(Door.transform.localEulerAngles.x, 106.108f, Door.transform.localEulerAngles.z), t);

            t += Time.deltaTime;

            if(Door.transform.localEulerAngles == new Vector3(Door.transform.localEulerAngles.x, 106.108f, Door.transform.localEulerAngles.z))
            {
                yield break;
            }

            yield return null;
        }
    }





    IEnumerator lerpLOGO()
    {


        Logo.gameObject.SetActive(true);
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime;

            Logo.transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(5050f,5050f,5050f), t);

            if (Logo.transform.localScale == new Vector3(5050f, 5050f, 5050f))
            {
                yield return new WaitForSeconds(3f);
                Logo.transform.localScale = Vector3.Lerp(new Vector3(5050,5050,5050), new Vector3(0,0,0), t);

                yield break;
            }

            yield return null;
        }

    }




        IEnumerator startOpening()
        {
        TransitionIN.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);


        TransitionIN.gameObject.SetActive(false);
        Ghost.transform.parent.gameObject.SetActive(true);
        Ghost.transform.localEulerAngles = new Vector3(Ghost.transform.localEulerAngles.x, Ghost.transform.localEulerAngles.y, 45 );
        yield return new WaitForSeconds(2.0f);
        Ghost.transform.localEulerAngles = new Vector3(Ghost.transform.localEulerAngles.x, Ghost.transform.localEulerAngles.y, 0 );
        yield return new WaitForSeconds(3.0f);
        
        //bool go = false;

        Vector3 desPos = new Vector3(0.031f, -1.337f, 1.596f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.3f));

        

        yield return new WaitUntil(() => Ghost.transform.position ==desPos); //wait until it sets position
        fadeColor = true;

        StartCoroutine(lerpLOGO());


        yield return new WaitUntil(() => Logo.localScale == new Vector3(0, 0, 0));

        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.08>�ȳ� \n<color=#2e86de>(^ v ^)~", audioIndex = 0));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.07>�̰���</size><size=0.09> <color=#EA2027>A</color><color=#EE5A24>T</color><color=#F79F1F>T</color><color=#009432>N</color><color=#0652DD>K</color><color=#1B1464>A</color><color=#B53471>R</color><color=#0984e3>E</color></size> <size=0.08>�����", audioIndex = 1));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>���� ���� \n���� ���̵��. \n �� ��Ź�� \n<color=#2e86de>(^ 0 ^)~", audioIndex = 2));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.053>�����ϱ� ����\n�����ϰ� �������ٰ�! \n<color=#2e86de>(^ 0 ^)~", audioIndex = 3));

        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>�ѹ� ����غ���?!\n<color=#2e86de><size=0.074>(o v o)/", audioIndex = 4));


        desPos = new Vector3(0.031f, -0.664f, 2.823f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos,0.4f));
        yield return new WaitUntil(() => Ghost.transform.position == desPos);
       // StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("����~!\n<size=0.07><color=#2e86de>(^ - ^)");
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
        yield return new WaitForSeconds(3.0f);
        Ghost.GetComponent<Animator>().SetBool("isJump", false);

        StartCoroutine(lerpDoor());
        desPos = new Vector3(0.031f, -0.519f, 6.885f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));


        yield return new WaitForSeconds(3.0f);
        PlayerPrefs.SetString("State", "END");
        SceneLoader.LoadScene("Tutorial");




        yield return new WaitForSeconds(1.0f);




        }

    IEnumerator startEnding()
    {
        NetworkManager.DoSendToFinishData();
        yield return new WaitForSeconds(1.5f);
        //������ �޷�����
        //bool go = true;
        StartCoroutine(lerpDoor());
        desPos = new Vector3(0.031f, -0.56f,3.734f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));
        yield return new WaitForSeconds(3.0f);


        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>�ƽ�����\n���� ����� \n�ð��̾�\n<color=#2e86de><size=0.09><b>(T ^ T)", audioIndex = 5));
        yield return new WaitForSeconds(2.5f);

        desPos = new Vector3(-0.02f, -0.693f, 2.795f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>���� �Բ��ؼ� \n�ʹ� ��ſ���\n<color=#2e86de><size=0.09><b>(^ - ^)", audioIndex = 6));
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>�ʵ� ��ſ��ٸ� \n���ڴ�!\n<color=#2e86de><size=0.09><b>(//^ ^//)", audioIndex = 7));
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.2f));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.055>�� �׻� \n�� ���� ������!\n������ \n�� ���\n<color=#2e86de><b>(^ 0 ^)/", audioIndex = 8));
        yield return new WaitForSeconds(2.0f);


        fadeColor = true;
        SpeechBubble.gameObject.SetActive(false);
        Ghost.GetComponent<Animator>().SetBool("isJump", true);

        yield return new WaitForSeconds(3.0f);
        
        TransitionOut.gameObject.SetActive(true);
        Ghost.transform.parent.gameObject.SetActive(false);
        PlayerPrefs.SetString("State", "OPEN");
        yield return new WaitForSeconds(1.2f);

        
        SceneLoader.LoadScene("OPENEND");





    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetString("State", "OPEN");

    }

  





}
