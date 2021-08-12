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
        public float xrrigZ_minus;
        public Canvas Title;

        VolumeProfile globalVolume;
        public float xrrigZ_temp;
        Vector3 openDes;
        Vector3 endDes;
        Vector3 startDes;

        RectTransform Title_img;
        GameObject Title_vfx;
        ColorAdjustments _coloradjustment = null;
        public float speed;
        Transform SpeechBubble;
    Transform Logo;
        private float Timer = 0;
        private string state;
    bool fadeColor = false;
        float GhostRot;




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
        openDes = new Vector3(-0.032f,-0.559f, 1.734f);
        endDes = new Vector3(0.0312f, -0.289f, 4.648f);
        startDes = new Vector3(-0.394f, -.559f, 2.935f);
        SpeechBubble = Ghost.transform.GetChild(1);
        globalVolume = global.sharedProfile;
        globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

      

        if (state =="OPEN")
        {
          
            _coloradjustment.saturation.value = -100f;
            
            

            StartCoroutine(startOpening());


        }
        if(state =="END")
        {
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

        private void MoveGhost(Vector3 des, bool go)
        {
            
            if(go) //door open
            {
                SpeechBubble.gameObject.SetActive(false);
            StartCoroutine(lerpDoor());
            

            }

                Ghost.GetComponent<Actor>().SimpleMove(des);

             // StartCoroutine("LerpImage");
                Title_vfx.SetActive(true);




            
        }

    /*
        IEnumerator LerpImage()
        {
            float t = 0;
            Vector3 start_size = Title_img.GetComponent<RectTransform>().localScale;

            while (t < 2.1f)
            {
                Title_img.GetComponent<RectTransform>().localScale = Vector3.Lerp(start_size, new Vector3(2.5f, 2.5f, 0), t / .5f);

                t += Time.deltaTime;

                yield return null;
            }


            Title_img.GetComponent<RectTransform>().localScale = new Vector3(2.5f, 2.5f, 0);
        }
    */

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

            Logo.transform.localScale = Vector3.Lerp(new Vector3(0.3f, 0.3f, 0.3f), new Vector3(1, 1, 1), t);

            if (Logo.transform.localScale == new Vector3(1, 1, 1))
            {
                
                yield break;
            }

            yield return null;
        }

    }


        private void GhostSpeech(string input)
        {
        Ghost.GetComponent<AudioSource>().Play();
            SpeechBubble.gameObject.SetActive(true);
            SpeechBubble.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = input;


        }


        IEnumerator startOpening()
        {
        Ghost.transform.position = startDes;
        yield return new WaitForSeconds(1.5f);
        Ghost.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        fadeColor = true;
            bool go = false;
            MoveGhost(openDes, go);

        Logo.gameObject.SetActive(true);

        yield return new WaitUntil(() => Ghost.transform.position ==openDes); //wait until it sets position

        StartCoroutine(lerpLOGO());
        

        
            yield return new WaitForSeconds(2.0f);
        Logo.gameObject.SetActive(false);
        GhostSpeech("안녕");
            yield return new WaitForSeconds(3.0f);
            GhostSpeech("여긴 ATTNKARE 세계야");
            yield return new WaitForSeconds(3.0f);

            GhostSpeech("한번 출발해볼까");
            go = true;
            yield return new WaitForSeconds(3.0f);
            MoveGhost(endDes, go);
        yield return new WaitForSeconds(3.0f);
        PlayerPrefs.SetString("State", "END");
        SceneManager.LoadScene("OPENEND");




        yield return new WaitForSeconds(1.0f);




        }

    IEnumerator startEnding()
    {
        Ghost.transform.localPosition = endDes;
        Ghost.transform.parent.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(3.0f);
        bool go = true;
        MoveGhost(openDes, go);
        yield return new WaitForSeconds(2.0f);
        GhostSpeech("이제 헤어질 시간이야");
        yield return new WaitForSeconds(2.0f);
        GhostSpeech("다음에 또 놀러와!");
        yield return new WaitForSeconds(3.0f);
        fadeColor = true;
        SpeechBubble.gameObject.SetActive(false);
        Ghost.GetComponent<Actor>().enabled = false;
        float yRot = Ghost.transform.localEulerAngles.y;
        Ghost.transform.localEulerAngles = new Vector3(0, yRot, 0);
        Ghost.GetComponent<Animator>().SetBool("fin", true);


        yield return new WaitForSeconds(3.0f);
        PlayerPrefs.SetString("State", "OPEN");
        SceneManager.LoadScene("OPENEND");





    }


       



    }

