using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


    public class OpenManager : MonoBehaviour
    {
       
        public GameObject PlayerController;
        public GameObject Ghost;
        public GameObject Door;
        public Volume global;
        public Canvas Title;
        public AudioClip[] audioSource;
        Vector3 desPos;

        VolumeProfile globalVolume;




        ColorAdjustments _coloradjustment = null;
        public float speed;
        Transform SpeechBubble;
    Transform Logo;
        private float Timer = 0;
        private string state;
    bool fadeColor = false;
        float GhostRot;

    Vector3 testDes;




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
            Ghost.transform.position = new Vector3(-0.02f, -0.56f, 6.695f);
            _coloradjustment.saturation.value = -10f;
            StartCoroutine(startEnding());

        }





        }

    // Update is called once per frame
    void Update()
    {
        SpeechBubble = Ghost.transform.GetChild(1);
        Logo = Ghost.transform.GetChild(2);
        float GhostRot = Ghost.transform.eulerAngles.y;
        SpeechBubble.localEulerAngles = new Vector3(SpeechBubble.localEulerAngles.x, GhostRot, SpeechBubble.localEulerAngles.z);
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

            Logo.transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(1.5f,1.5f,1.5f), t);

            if (Logo.transform.localScale == new Vector3(1.5f, 1.5f, 1.5f))
            {
                
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
        Logo.gameObject.SetActive(true);

        
        yield return new WaitForSeconds(2.0f);
        Logo.gameObject.SetActive(false);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<color=red>안녕</color>"));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("여긴 ATTNKARE 세계야"));
        yield return new WaitForSeconds(3.0f);

        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("한번 출발해볼까"));


        desPos = new Vector3(0.031f, -0.664f, 2.823f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos,0.4f));
        yield return new WaitUntil(() => Ghost.transform.position == desPos);
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("가보자!!"));
        Ghost.GetComponent<Animator>().SetBool("isJump", true);
        yield return new WaitForSeconds(1.8f);
        Ghost.GetComponent<Animator>().SetBool("isJump", false);

        StartCoroutine(lerpDoor());
        desPos = new Vector3(0.031f, -0.519f, 6.885f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));


        yield return new WaitForSeconds(3.0f);
        PlayerPrefs.SetString("State", "END");
        SceneManager.LoadScene("Tutorial");




        yield return new WaitForSeconds(1.0f);




        }

    IEnumerator startEnding()
    {
        yield return new WaitForSeconds(1.5f);
        //밖으로 달려나감
        //bool go = true;
        StartCoroutine(lerpDoor());
        desPos = new Vector3(-0.02f,-0.56f,3.734f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.5f));
        yield return new WaitForSeconds(3.0f);


        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("<size=0.06>이제 헤어질 시간이야</size>"));
        yield return new WaitForSeconds(2.0f);

        desPos = new Vector3(-0.02f, -0.693f, 2.795f);
        StartCoroutine(Ghost.GetComponent<Actor>().MoveGhost(desPos, 0.2f));
        StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak("다음에 또 놀러와"));
        yield return new WaitForSeconds(3.0f);


        fadeColor = true;
        SpeechBubble.gameObject.SetActive(false);
        Ghost.GetComponent<Animator>().SetBool("isJump", true);

        yield return new WaitForSeconds(3.0f);
        PlayerPrefs.SetString("State", "OPEN");

        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("OPENEND");





    }


       



    }
