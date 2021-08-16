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
        public Transform LogoParent;







        
        public float speed;
        Transform SpeechBubble;
        Transform Logo;
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

        SpeechBubble = Ghost.transform.GetChild(1);
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
        SpeechBubble = Ghost.transform.GetChild(1);
        Logo =  LogoParent.GetChild(0);
        float GhostRot = Ghost.transform.eulerAngles.y;
        SpeechBubble.localEulerAngles = new Vector3(SpeechBubble.localEulerAngles.x, GhostRot, SpeechBubble.localEulerAngles.z);
        LogoParent.localEulerAngles = new Vector3(LogoParent.localEulerAngles.x, GhostRot, LogoParent.localEulerAngles.z);

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


        LogoParent.gameObject.SetActive(true);
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime;

            Logo.transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(12f,12f,12f), t);

            if (Logo.transform.localScale == new Vector3(12f, 12f, 12f))
            {
                yield return new WaitForSeconds(3f);
                Logo.transform.localScale = Vector3.Lerp(new Vector3(12,12,12), new Vector3(0,0,0), t);

                yield break;
            }

            yield return null;
        }

    }




        IEnumerator startOpening()
        {

        yield return new WaitForSeconds(1.5f);
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

        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.1>안녕 \n<color=#2e86de>(^ v ^)~", audioIndex = 0));
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.08>여긴</size><size=0.1> <color=#EA2027>A</color><color=#EE5A24>T</color><color=#F79F1F>T</color><color=#009432>N</color><color=#0652DD>K</color><color=#1B1464>A</color><color=#B53471>R</color><color=#0984e3>E</color></size> <size=0.08>세계야", audioIndex = 1));
        yield return new WaitForSeconds(1.8f);

        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>한번 출발해볼까?!\n<color=#2e86de><size=0.074>(o v o)/", audioIndex = 2));


        desPos = new Vector3(0.031f, -0.664f, 2.823f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos,0.4f));
        yield return new WaitUntil(() => Ghost.transform.position == desPos);
       // StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("들어간다~!\n<size=0.07><color=#2e86de>(^ - ^)");
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
        yield return new WaitForSeconds(1.8f);
        Ghost.GetComponent<Animator>().SetBool("isJump", false);

        StartCoroutine(lerpDoor());
        desPos = new Vector3(0.031f, -0.519f, 6.885f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));


        yield return new WaitForSeconds(1.8f);
        PlayerPrefs.SetString("State", "END");
        SceneLoader.LoadScene("Tutorial");




        yield return new WaitForSeconds(1.0f);




        }

    IEnumerator startEnding()
    {
        yield return new WaitForSeconds(1.5f);
        //밖으로 달려나감
        //bool go = true;
        StartCoroutine(lerpDoor());
        desPos = new Vector3(0.031f, -0.56f,3.734f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));
        yield return new WaitForSeconds(3.0f);


        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.07>이제 헤어질 \n시간이야\n<color=#2e86de><size=0.06>(T ^ T)", audioIndex = 3));
        yield return new WaitForSeconds(2.0f);

        desPos = new Vector3(-0.02f, -0.693f, 2.795f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.2f));
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.07>다음에 또 놀러와\n<color=#2e86de>(^ 0 ^)/", audioIndex = 4));
        yield return new WaitForSeconds(2.0f);


        fadeColor = true;
        SpeechBubble.gameObject.SetActive(false);
        Ghost.GetComponent<Animator>().SetBool("isJump", true);

        yield return new WaitForSeconds(5.0f);
        PlayerPrefs.SetString("State", "OPEN");

        yield return new WaitForSeconds(2.0f);
        SceneLoader.LoadScene("OPENEND");





    }


       



    }
