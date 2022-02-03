using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class Object_BP : MonoBehaviour
{
    /*
     * 고학년 버전 필요한 오브젝트
     * 1단계
     * 연필 3개, 빨간볼펜, 초록 볼펜, 지우개 (필요 없는 학용품 : 검정볼펜, 파랑볼펜)
     * 필통 뚜껑
     * 
     * 2단계
     * 필통
     * 교과서 : 국어, 과학, 미술, 영어 (필요 없는 책 : 사회, 수학, 음악, 체육, 도덕)
     * 딱풀
     * 
     * 방해물
     *  장난감 기차, 장난감 차, 폴더, 물감, 노트
     */
    public enum OBJ_BP { DISTURB, PENCIL, PEN, ERASER, TXTBOOK, GLUE, PCAP, PCASE}
    public enum TAG_BP {NECESSARY, UNNECESSARY, NECESSARY_PENCIL, NECESSARY_BOOK}
    public enum KIND_BP { NONE, GREEN, RED, BLUE, PURLPLE, BLACK, KOREAN, SCIENCE, ART, ENGLISH, SOCIALS, MATH, MUSIC, GYM, ETHICS }


    public struct BP_INFO
    {
        public OBJ_BP eObj;
        public KIND_BP eKind;
        public TAG_BP eTag;

        public BP_INFO(OBJ_BP eObj, KIND_BP eKind, TAG_BP eTag)
        {
            this.eObj = eObj;
            this.eKind = eKind;
            this.eTag = eTag;
        } 
    }
    public static List<BP_INFO> listObj = new List<BP_INFO>();


    //PACK_DISTRACTION
    // Start is called before the first frame update


    public InputBridge XrRig;
    public static bool bGrabbed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (XrRig.RightTrigger > 0.8f) bGrabbed = true;
        if (XrRig.RightTrigger < 0.2f) bGrabbed = false;
    }
}
