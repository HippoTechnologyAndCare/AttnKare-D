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

    [SerializeField] GameObject IntoSlot;
    [SerializeField] Transform currSlot;
    [SerializeField] Transform cube;
    UIPointer uiPointer;
    Material mat;

    Vector3 vec2Pos;
    Vector3 zPos;

    [SerializeField] bool working;
    bool NowClicked = false;
    bool PointerOnCube = false;


    //test
    [SerializeField] List<GameObject> Slots;
    void Start()
    {
        working = false;
        Slots = new List<GameObject>();
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "SLOT" /*&& !PlanCubeController1.working*/)
        {
            IntoSlot = collision.gameObject;
            Slots.Add(IntoSlot);
            if(IntoSlot == Slots[0])
            {
                                                           
            }
            //foreach(GameObject s in currSlot)
            //{                
            //    if(s == Slots[0])
            //    {
            //        IntoSlot = collision.collider.gameObject;
            //        currSlot.GetComponent<Material>().color = new Color(0.67f, 0, 0.545f, 0.12f);
            //    }
            //}
            //PlanCubeController1.working = true;            
        }

        if (collision.collider.tag == "POINTER")
        {
            PointerOnCube = true;
            scheduleManager.PlaySoundByTypes(ESoundType.IN);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "SLOT")
        {
            if (Slots.Count == 1 && !working)
            {
                IntoSlot = collision.collider.gameObject;
                Debug.Log("new stay");
                working = true;
                cube = IntoSlot.gameObject.transform.Find("Cube");
                cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.7f);
                //mat.color = new Color(0.67f, 0, 0.545f, 0.7f);
            }
        }
        
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "SLOT")
        {
            IntoSlot = collision.gameObject;
            Slots.Remove(IntoSlot);
            cube = IntoSlot.gameObject.transform.Find("Cube");
            cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.12f);
            Debug.Log("exit");
            working = false;
            if (Slots.Count == 0)
            {
                
                //mat.color = new Color(0.67f, 0, 0.545f, 0.12f);
                IntoSlot = null;
            }
                
            //foreach (GameObject s in currSlot)
            //{
            //    if(s == currSlot)
            //    {
            //        
            //    }
            //}
            
            //PlanCubeController1.working = false;
        }

        if (collision.collider.tag == "POINTER")
        {
            PointerOnCube = false;
        }
    }

    public void resetPlanCube()
    {
        working = false;
        activeSlot = null;
        IntoSlot = null;
        transform.localPosition = StartPos;
    }
}
