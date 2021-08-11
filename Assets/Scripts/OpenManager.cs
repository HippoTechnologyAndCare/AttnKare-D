using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.Rendering;


    public class OpenManager : MonoBehaviour
    {
       
        public GameObject PlayerController;
        public GameObject Ghost;
        public Volume global;
        public float xrrigZ_minus;
        public Canvas Title;
        VolumeProfile globalVolume;
        public float xrrigZ_temp;
        Vector3 des;
        Vector3 startdes;
        public bool start;
        RectTransform Title_img;
        GameObject Title_vfx;
        ColorAdjustments _coloradjustment = null;
        public float speed;
        Transform SpeechBubble;
        private float Timer = 0;

        float GhostRot;




        // Start is called before the first frame update
        void Start()
        {

            des = new Vector3(0.03f, -0.821f, 1.53f);
            startdes = Ghost.transform.position;
            SpeechBubble = Ghost.transform.GetChild(1);


            globalVolume = global.sharedProfile;
            globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

            _coloradjustment.saturation.value = -100f;

            Title_img = Title.transform.GetChild(0).GetComponent<RectTransform>();
            Title_vfx = Title.transform.GetChild(1).gameObject;

            StartCoroutine(startOpening());





        }

        // Update is called once per frame
        void Update()
        {



            globalVolume = global.sharedProfile;
            globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

            if (start)
            {
                Timer += Time.deltaTime;
                if (Timer > 0.02f)
                {
                    Timer = 0;
                    _coloradjustment.saturation.value += 1f;

                }

            }


            //synchronize rotation of ghost and speech bubbles
            SpeechBubble = Ghost.transform.GetChild(1);
            float GhostRot = Ghost.transform.eulerAngles.y;
            SpeechBubble.localEulerAngles = new Vector3(SpeechBubble.localEulerAngles.x, GhostRot, SpeechBubble.localEulerAngles.z);




        }

        private void LateUpdate()
        {

            globalVolume = global.sharedProfile;
            globalVolume.TryGet<ColorAdjustments>(out _coloradjustment);

            if (_coloradjustment.saturation.value == -10f)
            {
                start = false;
            }



        }

        private void MoveGhost()
        {
            if (start)
            {

                Ghost.GetComponent<Actor>().SimpleMove(des);

                StartCoroutine("LerpImage");
                Title_vfx.SetActive(true);


            }


            if (!start)
            {
                Ghost.GetComponent<Actor>().SimpleMove(startdes);

                //itle_vfx.SetActive(false);

            }
        }


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


        private void GhostSpeech(string input)
        {

            SpeechBubble.gameObject.SetActive(true);
            SpeechBubble.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = input;


        }


        IEnumerator startOpening()
        {

            yield return new WaitForSeconds(3.0f);
            start = true;
            MoveGhost();
            yield return new WaitForSeconds(3.0f);
            GhostSpeech("안녕");
            yield return new WaitForSeconds(3.0f);
            GhostSpeech("여긴 ATTNKARE 세계야");

            yield return new WaitForSeconds(1.0f);
            Debug.Log(GhostRot);




        }

       



    }

