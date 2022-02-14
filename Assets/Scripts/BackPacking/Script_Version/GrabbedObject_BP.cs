using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class GrabbedObject_BP : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Grabbed;
    bool check; //������ �� outline�� ���� �������������� outline�� Ű��
    Outlinable preChild;
    public float UnObject_Pick; // == bpUnpkT(�ʿ����� ���� ������ ���� �ð�)

    public bool stageCheck;
    public bool stageCheck2;

    public PlayMakerFSM UnnGrab;
    public GameObject Unn_timeCheck;
    public AddDelimiter delimiters;

    GameObject dataGameobject;

    bool nothing = true;//or when grabbing nothing
    bool disturbed = true; // for when grabbing unnecessary

    // Start is called before the first frame update
    // Update is called once per frame




    void Update()
    {
        Outlinable[] child_outline = gameObject.GetComponentsInChildren<Outlinable>();

        if (child_outline.Length > 0) //if grabber has child
        {
            if (!nothing) //Stop IDLE in data log
            {
                delimiters.addIDLE(nothing);
                nothing = true;
            }

            preChild = child_outline[0]; //���� ù��° child(but there's only one child anyway)
            Grabbed = preChild.gameObject; // send Grabbed to FSM
            check = true;
            UnnCheck(check, child_outline[0]);
            Grabbed.GetComponent<Collider>().isTrigger = true;

            if (preChild.tag == "Unnecessary")
            {
            //   Unn_timeCheck.SetActive(true);
                if (disturbed)
                {
                  //  UnnGrab.SendEvent("UnnecessaryObjectGrab");
                    delimiters.addDISTURB(disturbed);
                    disturbed = false;
                }

            }

            if (preChild.tag == "Necessary" || preChild.tag == "Necessary_Book")
            {
                if (stageCheck2 == true) //�ܺ� gamemanager_Fsm ���� stsge2�� ���۵Ǹ� set property�� stagecheck2 �� true�� ��������
                {
                    stageCheck = true; //Stage1to2_Manager���� �������� 2�� �ű�� �ʿ��� ������ ������ stagecheck�� true�� ������ "stage 1 ���� stage 2�� �����ϱ���� �ɸ��� �ð�"�� ����
                }

            }

        }
        else if (child_outline.Length == 0) //Grabber�Ʒ����� gameobject�� �ȵ���ִµ� 
        {
            if (nothing) //START IDLE in data log
            {
                delimiters.addIDLE(nothing);
                nothing = false;

            }
            if (!disturbed)
            {
                delimiters.addDISTURB(disturbed);
                disturbed = true;

            }



            if (preChild != null)// prechild���� ���� obj�� ����Ǿ����� ��
            {
                check = false;
                UnnCheck(check, preChild);
           //     Unn_timeCheck.SetActive(false);
                Grabbed.GetComponent<Collider>().isTrigger = false;

                Grabbed = null;
            }




        }



    }

    void UnnCheck(bool check, Outlinable child_outline) // ���ʿ��� ���� ���� �ð�
    {
        if (check) //
        {
            child_outline.enabled = false; //���� ���� ������Ʈ�� outline�� ������
        }

        else if (!check)
        {

            child_outline.enabled = true; //Grabber�� ���� ������ ������ٸ� outline�� ������
            preChild = null;

        }

    }

}
