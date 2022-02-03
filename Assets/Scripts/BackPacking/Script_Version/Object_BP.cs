using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class Object_BP : MonoBehaviour
{
    /*
     * ���г� ���� �ʿ��� ������Ʈ
     * 1�ܰ�
     * ���� 3��, ��������, �ʷ� ����, ���찳 (�ʿ� ���� �п�ǰ : ��������, �Ķ�����)
     * ���� �Ѳ�
     * 
     * 2�ܰ�
     * ����
     * ������ : ����, ����, �̼�, ���� (�ʿ� ���� å : ��ȸ, ����, ����, ü��, ����)
     * ��Ǯ
     * 
     * ���ع�
     *  �峭�� ����, �峭�� ��, ����, ����, ��Ʈ
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
