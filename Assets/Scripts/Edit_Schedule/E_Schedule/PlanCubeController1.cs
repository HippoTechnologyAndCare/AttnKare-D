using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BNG;

public class PlanCubeController1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    
    public ScheduleManager1 scheduleManager;

    public Transform HandCursor;
    public HandController handController;

    public Transform Canvas;
    public Transform Grp;

    public Vector2 StartPos;

    public float t;

    public GameObject activeSlot;

    GameObject IntoSlot;
    UIPointer uiPointer;

    Vector3 vec2Pos;
    Vector3 zPos;

    bool NowClicked = false;
    bool PointerOnCube = false;

    void Start()
    {
        zPos.z = 2.21874f;
        StartPos = this.transform.localPosition;
        uiPointer = HandCursor.GetComponent<BNG.UIPointer>();        
    }

    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (NowClicked)
        {
            if (HandCursor.GetComponent<LineRenderer>().enabled == true)
            {
                MoveCard();
            }
            else
            {
                Debug.Log("???");
            }

            if (!PointerOnCube && handController.PointAmount == 1)
            {
                this.transform.SetParent(Grp);
                scheduleManager.ReleaseAllCollision();
                scheduleManager.PlaySoundByTypes(ESoundType.PUT);

                NowClicked = false;
                IntoSlot = null;

                if (activeSlot != null)
                {
                    transform.localPosition = activeSlot.transform.localPosition;
                }
                else
                {
                    transform.localPosition = StartPos;
                }
            }
        }
    }
    void MoveCard()
    {
        // LaserEnd의 포지션을 따라간다
        Vector2 a = this.transform.position;
        Vector2 b = uiPointer._cursor.transform.position;
        vec2Pos = Vector2.Lerp(a, b, t);
        vec2Pos.z = zPos.z;
        this.transform.position = vec2Pos;        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        NowClicked = true;
        this.transform.SetParent(Canvas);
        scheduleManager.LockAllCollision(this.transform);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        this.transform.SetParent(Grp);
        scheduleManager.ReleaseAllCollision();
        scheduleManager.PlaySoundByTypes(ESoundType.PUT);

        if (IntoSlot != null) //슬롯에 들어온 경우
        {
            if (IntoSlot == activeSlot)
            {
                transform.localPosition = activeSlot.transform.localPosition;
                return;
            }

            if (IntoSlot.GetComponent<PlanSlotController1>().passenger == null) //새로운 슬롯이 비어있는 경우
            {
                if (activeSlot != null)
                {
                    activeSlot.GetComponent<PlanSlotController1>().passenger = null;
                }

                activeSlot = IntoSlot;
                transform.localPosition = activeSlot.transform.localPosition;
                activeSlot.GetComponent<PlanSlotController1>().passenger = this.gameObject;
            }
            else
            {
                //새로운 슬롯에 이미 계획이 있는 경우

                if (activeSlot == null) //현재 할당된 슬롯이 없는 경우
                {
                    IntoSlot.GetComponent<PlanSlotController1>().passenger.GetComponent<PlanCubeController1>().resetPlanCube();
                }
                else
                {
                    //현재 할당된 슬롯이 있어서 바꿔치기 하는 경우
                    GameObject tempObj = IntoSlot.GetComponent<PlanSlotController1>().passenger;

                    tempObj.transform.localPosition = activeSlot.transform.localPosition;
                    tempObj.GetComponent<PlanCubeController1>().activeSlot = activeSlot;
                    activeSlot.GetComponent<PlanSlotController1>().passenger = tempObj;
                }

                activeSlot = IntoSlot;
                transform.localPosition = activeSlot.transform.localPosition;
                activeSlot.GetComponent<PlanSlotController1>().passenger = this.gameObject;
            }

            scheduleManager.CheckMovingCnt();
            scheduleManager.CheckAllScheduleOnSlot();
        }
        else
        {
            if (activeSlot != null)
            {
                transform.localPosition = activeSlot.transform.localPosition;
            }
            else
            {
                transform.localPosition = StartPos;
            }
        }

        NowClicked = false;
        IntoSlot = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "SLOT")
        {
            IntoSlot = collision.collider.gameObject;
        }

        if (collision.collider.tag == "POINTER")
        {
            PointerOnCube = true;
            scheduleManager.PlaySoundByTypes(ESoundType.IN);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "SLOT")
        {
            IntoSlot = null;
        }

        if (collision.collider.tag == "POINTER")
        {
            PointerOnCube = false;
        }
    }

    public void resetPlanCube()
    {
        activeSlot = null;
        IntoSlot = null;
        transform.localPosition = StartPos;
    }
}
