using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BNG;
using UserData;
using EPOOutline;

namespace Tutorial {
    public class Guide_Tutorial : MonoBehaviour
    {

        // Start is called before the first frame update
        [SerializeField] private Hud_Tutorial HUD;
        [SerializeField] private AutoVoiceRecording DataCollect;
        [Header("FADE IN/OUT")]
        [SerializeField] private GameObject prefabFadeIn;
        [SerializeField] private GameObject prefabFadeOut;
        [SerializeField] private Transform CenterEye;
        GameObject m_goFade;
        [Header("GHOST")]
        [SerializeField] private GameObject Ghost;

        [SerializeField] private GameObject SceneTransitionCircle;
        [SerializeField] private GameObject ButtonCircle;

        public GameObject Banana;
        private Transform m_tCol;
        private bool m_bCol;

        public SceneData_Send DataSend;
        DataManager dataManager;
        private InputBridge Controller;
        public GameObject XRRig;
        [SerializeField] private GameObject RightHandPointer;
        public GameObject Grabber;
        public GameObject Grabbed;
        public GameObject Note;
        public bool b_TriggerPressed;
        public bool b_GrabBanana = false;
        public bool b_NoteCheck;
        private bool b_NoteDone = true;
        private bool b_PutBanana = true;
        private bool b_Nothing = false;
        private bool b_ButtonPressed;
        private bool b_Trigger;

        public GameObject heldGrabbable;
        public int trigInt;

        private string gradeLH;
        public int buildIndex;
        Coroutine runningcoroutine = null;
        void Start()
        {
            StartCoroutine(StartTutorial());
            try
            {
                int grade = UserInfo_API.GetInstance().UserTotalInfo.user.grade;
                if (grade > 3) buildIndex = 5;// 고학년은 계획표로
                if (grade < 4) buildIndex = 1; //저학년은 책가방으로 }

            }
            catch (NullReferenceException ex)
            {
                Debug.Log("DEBUGGING MODE");
            }
        }
        private void Update()
        {
            if (Grabber.GetComponent<Grabber>().HeldGrabbable != null && b_GrabBanana)
            { heldGrabbable = Grabber.GetComponent<Grabber>().HeldGrabbable.transform.gameObject;
                if (heldGrabbable == Banana) { StartCoroutine(NoteCheck()); b_GrabBanana = false; }
            }
            switch (m_bCol)
            {
                case true: Enter(); break;
                case false: break;
            }
            Controller = XRRig.GetComponent<InputBridge>();
            if (b_TriggerPressed)
            {
                //TriggerDown Check
                if (Controller.RightTrigger > 0.7f && Controller.LeftTrigger > 0.7f) { StartCoroutine(GrabObject()); b_TriggerPressed = false; }
                if (Controller.LeftGrip > 0.7f && Controller.RightGrip > 0.7f) ghostSpeak(0);
            }

            if (b_NoteCheck)
            {
                if (Controller.LeftTrigger > 0.5)
                {
                    Note.SetActive(true);
                    if (b_NoteDone) { StartCoroutine(PutObject()); b_NoteDone = false; }
                }
                if (Controller.LeftTrigger <= 0.5) { b_Trigger = false; Note.SetActive(false); }
            }

            if (Controller.RightTrigger > 0.7f) b_Trigger = true;
            if (Controller.RightTrigger <= 0.8f) b_Trigger = false;
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject == Banana & m_tCol == false) {
                Debug.Log("INSIDE");
                m_bCol = true; m_tCol = collision.transform;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.transform == m_tCol) {
                Debug.Log("OUTSIDE");
                m_bCol = false; m_tCol = null;
                RunningCoroutione(BananaExit());
            }
        }

        void Enter()
        {
            if (b_Trigger) return; //wait until trigger is released
            if (!b_Trigger) { if (b_PutBanana) { RunningCoroutione(WaitForSecond()); b_PutBanana = false; } }//CheckObj(m_tCol); m_eState = Object_BP.STATE.EXIT; }
        }
        public void RunningCoroutione(IEnumerator coroutine)
        {
            if (runningcoroutine != null)
            {
                StopCoroutine(runningcoroutine);
            }
            runningcoroutine = StartCoroutine(coroutine);
        }
        IEnumerator WaitForSecond()
        {
            yield return new WaitForSeconds(1.7f);
            StartCoroutine(ButtonPressed());
        }

        IEnumerator BananaExit()
        {
            b_PutBanana = true;
            yield return null;
        }


        IEnumerator FadeIn()
        {
            m_goFade = Instantiate(prefabFadeIn, CenterEye.position, Quaternion.identity);
            m_goFade.transform.SetParent(CenterEye.transform);
            Debug.Log("wait");
            yield return new WaitForSeconds(1.2f);
        }
        IEnumerator FadeOut()
        {
            m_goFade = Instantiate(prefabFadeOut, CenterEye.transform.position, Quaternion.identity);
            m_goFade.transform.SetParent(CenterEye.transform);
            yield return new WaitForSeconds(3f);
            KetosGames.SceneTransition.SceneLoader.LoadScene(buildIndex);
        }

        IEnumerator StartTutorial()
        {
            StartCoroutine(FadeIn());
            Ghost.SetActive(true);
            HUD.RunCoroutine(HUD.CanvasStart());
            yield return new WaitUntil(() => HUD.bGuide == true);
            b_TriggerPressed = true;
        }

        IEnumerator GrabObject()
        {
            HUD.bGuide = false;
            b_TriggerPressed = false;
            HUD.RunCoroutine(HUD.GrabBanana());
            RightHandPointer.SetActive(true);
            RightHandPointer.GetComponent<LineRenderer>().enabled = true;
            yield return new WaitUntil(() => HUD.bGuide == true);
            b_GrabBanana = true;


        }
        IEnumerator NoteCheck()
        {
            HUD.bGuide = false;
            HUD.RunCoroutine(HUD.NoteCheck());
            yield return new WaitUntil(() => HUD.bGuide == true);
            b_NoteCheck = true;
        }
        IEnumerator PutObject()
        {
            HUD.bGuide = false;
            HUD.RunCoroutine(HUD.PutBanana());
            yield return new WaitUntil(() => HUD.bGuide == true);
            GetComponent<BoxCollider>().enabled = true;
        }

        IEnumerator ButtonPressed()
        {
            yield return new WaitForSeconds(2f);
            GetComponent<BoxCollider>().enabled = false;
            HUD.bGuide = false;
            HUD.RunCoroutine(HUD.PressButton());
            ButtonCircle.SetActive(true);
            yield return new WaitUntil(() => HUD.bGuide == true);
        }
        public void Press()
        {
            StartCoroutine(ButtonDone());
        }
        IEnumerator ButtonDone()
        {
            HUD.bGuide = false;
            ButtonCircle.SetActive(false);
            HUD.RunCoroutine(HUD.AllFinished());
            yield return new WaitUntil(() => HUD.bGuide == true);
            SceneTransitionCircle.SetActive(true);
        }
        public IEnumerator SceneChange(bool stay)
        {
            HUD.bGuide = false;
            yield return new WaitForSeconds(.8f);

            if (stay)
            {
                Debug.Log("STAY");
                HUD.RunCoroutine(HUD.NextScene());
                SceneTransitionCircle.GetComponent<SceneTransition_Tutorial>().enabled = false;
                SceneTransitionCircle.GetComponent<BoxCollider>().enabled = false;
                yield return new WaitUntil(() => HUD.bGuide == true);
                DataSend.GetSceneData();
                DataCollect.StopRecordingNBehavior();
                Password();
            }
            if (!stay) { HUD.RunCoroutine(HUD.StopNextScene()); Debug.Log("OUT"); }
        }

        void Password()
        {
            
            HUD.RunCoroutine(HUD.Password());
        }
        public IEnumerator PasswordEntered()
        {
            HUD.bGuide = false;
            DataSend.TutorialJson();
            HUD.RunCoroutine(HUD.PasswordDone());
            yield return new WaitUntil(() => HUD.bGuide == true);
            StartCoroutine(NextScene());
        }

        IEnumerator NextScene()
        {
            yield return new WaitForSeconds(1);
            Ghost.SetActive(false);
            StartCoroutine(FadeOut());

        }
        public void ghostSpeak(int strIndex)
        {
            int index = UnityEngine.Random.Range(0, 3);
            string[] Speechstr = { "그 버튼이 아니야\n<color=#2e86de>(O _ O)!", "바닥을 한번 살펴봐!", "뒤를 돌아볼래?", "직접 걸어서\n다가가야해!" };
            StartCoroutine(Ghost.GetComponent<Actor>().ghostSpeak(Speechstr[strIndex], index, 2.5f));

        }
    }
}
