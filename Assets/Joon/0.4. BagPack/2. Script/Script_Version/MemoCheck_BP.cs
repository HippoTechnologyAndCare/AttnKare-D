using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG {
    public class MemoCheck_BP : MonoBehaviour
    {
        public GameObject XRRig;
        public GameObject Note;
        [SerializeField]
        public float fNote;
        float lTrigger;
        public bool set = true;
        void Update()
        {
            lTrigger = XRRig.GetComponent<InputBridge>().LeftTrigger;
            if (lTrigger >= 0.5) { if (set) { StartCoroutine(ThreeSeconds()); } };
            if (lTrigger < 0.5) { StopAllCoroutines(); Note.SetActive(false); set = true; };
        }
        IEnumerator ThreeSeconds()
        {
            set = false;
            Note.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            fNote += 1;
            yield return new WaitForSeconds(1.7f);
            Note.SetActive(false);

        }
    }

}
