using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BNG;

public class PlanCubeController1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    
    public ScheduleManager1 scheduleManager;

    public Transform HandCursor;
    public Transform HandController;

    public Transform Canvas;
    public Transform Grp;

    public Vector2 StartPos;

    public GameObject slot_Now;
    GameObject slot_New;

    bool NowClicked = false;
    bool PointerOnCube = false;

    void Start()
    {
        StartPos = this.transform.localPosition;
    }

    void Update()
    {
        if (NowClicked)
        {
            if (HandCursor.GetComponent<LineRenderer>().enabled == true)
            {
                transform.position = HandCursor.GetComponent<BNG.UIPointer>()._cursor.transform.position;                
            }
            else
            {
                Debug.Log("???");
            }


            if (!PointerOnCube && HandController.GetComponent<BNG.HandController>().PointAmount == 1)
            {
                this.transform.SetParent(Grp);
                scheduleManager.ReleaseAllCollision();
                scheduleManager.PlaySoundByTypes("PUT");                

                NowClicked = false;
                slot_New = null;

                if (slot_Now != null)
                {
                    transform.localPosition = slot_Now.transform.localPosition;
                }
                else
                {
                    transform.localPosition = StartPos;
                }
            }
        }
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
        scheduleManager.PlaySoundByTypes("PUT");

        if (slot_New != null) //슬롯에 들어온 경우
        {
            if (slot_New == slot_Now)
            {
                transform.localPosition = slot_Now.transform.localPosition;
                return;
            }

            if (slot_New.GetComponent<PlanSlotController1>().passenger == null) //새로운 슬롯이 비어있는 경우
            {
                if (slot_Now != null)
                {
                    slot_Now.GetComponent<PlanSlotController1>().passenger = null;
                }

                slot_Now = slot_New;
                transform.localPosition = slot_Now.transform.localPosition;
                slot_Now.GetComponent<PlanSlotController1>().passenger = this.gameObject;
            }
            else
            {
                //새로운 슬롯에 이미 계획이 있는 경우

                if (slot_Now == null) //현재 할당된 슬롯이 없는 경우
                {
                    slot_New.GetComponent<PlanSlotController1>().passenger.GetComponent<PlanCubeController1>().resetPlanCube();
                }
                else
                {
                    //현재 할당된 슬롯이 있어서 바꿔치기 하는 경우
                    GameObject tempObj = slot_New.GetComponent<PlanSlotController1>().passenger;

                    tempObj.transform.localPosition = slot_Now.transform.localPosition;
                    tempObj.GetComponent<PlanCubeController1>().slot_Now = slot_Now;
                    slot_Now.GetComponent<PlanSlotController1>().passenger = tempObj;
                }

                slot_Now = slot_New;
                transform.localPosition = slot_Now.transform.localPosition;
                slot_Now.GetComponent<PlanSlotController1>().passenger = this.gameObject;
            }

            scheduleManager.CheckMovingCnt();
            scheduleManager.CheckAllScheduleOnSlot();
        }
        else
        {
            if (slot_Now != null)
            {
                transform.localPosition = slot_Now.transform.localPosition;
            }
            else
            {
                transform.localPosition = StartPos;
            }
        }

        NowClicked = false;
        slot_New = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "SLOT")
        {
            slot_New = collision.collider.gameObject;
        }

        if (collision.collider.tag == "POINTER")
        {
            PointerOnCube = true;
            scheduleManager.PlaySoundByTypes("IN");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "SLOT")
        {
            slot_New = null;
        }

        if (collision.collider.tag == "POINTER")
        {
            PointerOnCube = false;
        }
    }

    public void resetPlanCube()
    {
        slot_Now = null;
        slot_New = null;
        transform.localPosition = StartPos;
    }
}
