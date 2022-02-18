using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BNG;

namespace Scheduler
{
    public class PlanCubeController1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        public ScheduleManager1 scheduleManager;

        public enum CARD_STATE { IDLE, MOVE, ENTER, DONE };
        
        public CARD_STATE cardState;

        public Transform HandCursor;
        public HandController handController;

        public Transform Canvas;
        public Transform Grp;

        public GameObject activeSlot;
        public Vector3 startPos;

        public float t;
        public bool isHomeTW;

        [SerializeField] CardCreater cardCreater;

        [SerializeField] GameObject originPos_P;
        //[SerializeField] Transform originPos;
        [SerializeField] GameObject cardPrb;
        [SerializeField] GameObject intoSlot;        
        [SerializeField] Transform cube;
        
        //[SerializeField] bool isWorking;

        UIPointer uiPointer;
        Material mat;

        Vector3 vec2Pos;
        Vector3 zPos;

        //[SerializeField] bool working;
        bool nowClicked = false;
        bool pointerOnCube = false;


        //test
        [SerializeField] List<GameObject> Slots;
        void Start()
        {
            FindOriginPos();

            intoSlot = null;
            cube = null;
            cardState =  CARD_STATE.IDLE;
            cardPrb = this.gameObject;          
            Slots = new List<GameObject>();
            zPos.z = 2.21874f;
            startPos = this.transform.localPosition;
            uiPointer = HandCursor.GetComponent<BNG.UIPointer>();
        }

        void Update()
        {
        }
        void FixedUpdate()
        {
            if (nowClicked)
            {
                if (HandCursor.GetComponent<LineRenderer>().enabled == true)
                {
                    MoveCard();
                }
                else
                {
                    Debug.Log("???");
                }

                if (!pointerOnCube && handController.PointAmount == 1)
                {
                    this.transform.SetParent(Grp);
                    scheduleManager.ReleaseAllCollision();
                    scheduleManager.PlaySoundByTypes(ESoundType.Put);

                    nowClicked = false;
                    intoSlot = null;

                    if (activeSlot != null)
                    {
                        transform.localPosition = activeSlot.transform.localPosition;
                    }
                    else
                    {
                        transform.localPosition = startPos;
                    }
                }
            }
        }

        void FindOriginPos()
        {
            originPos_P = GameObject.Find("Origin Pos");
            string myName = this.name.Replace("(Clone)", "");
            cardCreater = originPos_P.transform.Find(myName).GetComponent<CardCreater>();            
        }

        void MoveCard()
        {
            // LaserEnd의 포지션을 따라간다
            cardState = CARD_STATE.MOVE;
            Vector2 a = this.transform.position;
            Vector2 b = uiPointer._cursor.transform.position;
            vec2Pos = Vector2.Lerp(a, b, t);
            vec2Pos.z = zPos.z;
            this.transform.position = vec2Pos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            nowClicked = true;
            this.transform.SetParent(Canvas);
            scheduleManager.LockAllCollision(this.transform);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.transform.SetParent(Grp);
            scheduleManager.ReleaseAllCollision();
            scheduleManager.PlaySoundByTypes(ESoundType.Put);

            if (intoSlot != null) //슬롯에 들어온 경우
            {
                if (intoSlot == activeSlot)
                {
                    transform.localPosition = activeSlot.transform.localPosition;
                    return;
                }

                // 슬롯에 큐브가 들어가는 조건 탐색
                if (intoSlot.GetComponent<PlanSlotController1>().passenger == null) //새로운 슬롯이 비어있는 경우
                {
                    if (activeSlot != null)
                    {
                        activeSlot.GetComponent<PlanSlotController1>().passenger = null;
                    }

                    if (!isHomeTW && !cardCreater.isStored)
                    {
                        if(!scheduleManager.isReset)
                        {
                            InstantiateCard(cardPrb);
                        }                        
                    }

                    activeSlot = intoSlot;
                    transform.localPosition = activeSlot.transform.localPosition;                    
                    activeSlot.GetComponent<PlanSlotController1>().passenger = this.gameObject;
                    activeSlot.GetComponent<PlanSlotController1>().inSlot = true;
                    MeshRendererOff(activeSlot);
                    cardState = CARD_STATE.DONE;
                }
                else
                {
                    //새로운 슬롯에 이미 계획이 있는 경우

                    if (activeSlot == null) //현재 할당된 슬롯이 없는 경우
                    {
                        intoSlot.GetComponent<PlanSlotController1>().passenger.GetComponent<PlanCubeController1>().resetPlanCube();
                    }
                    else
                    {
                        //현재 할당된 슬롯이 있어서 바꿔치기 하는 경우
                        GameObject tempObj = intoSlot.GetComponent<PlanSlotController1>().passenger;

                        tempObj.transform.localPosition = activeSlot.transform.localPosition;
                        tempObj.GetComponent<PlanCubeController1>().activeSlot = activeSlot;
                        activeSlot.GetComponent<PlanSlotController1>().passenger = tempObj;
                    }

                    cardState = CARD_STATE.DONE;
                    activeSlot = intoSlot;
                    transform.localPosition = activeSlot.transform.localPosition;
                    activeSlot.GetComponent<PlanSlotController1>().passenger = this.gameObject;
                    activeSlot.GetComponent<PlanSlotController1>().inSlot = true;
                    MeshRendererOff(activeSlot);
                }

                scheduleManager.CheckMovingCnt();
                scheduleManager.CheckAllScheduleOnSlot();
            }
            else
            {
                if (activeSlot != null)
                {
                    cardState = CARD_STATE.DONE;
                    transform.localPosition = activeSlot.transform.localPosition;
                }
                else
                {
                    cardState = CARD_STATE.IDLE;
                    transform.localPosition = startPos;
                }
            }

            nowClicked = false;
            intoSlot = null;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "SLOT")
            {
                intoSlot = collision.gameObject;
                Slots.Add(intoSlot);
            }

            if (collision.collider.tag == "POINTER")
            {
                pointerOnCube = true;
                scheduleManager.PlaySoundByTypes(ESoundType.In);
            }
        }

        void OnCollisionStay(Collision collision)
        {
            if (collision.collider.tag == "SLOT")
            {
                if (Slots.Count == 1)
                {
                    //isWorking = true;
                    intoSlot = collision.collider.gameObject;                    
                    cardState = CARD_STATE.ENTER;
                    cube = intoSlot.gameObject.transform.Find("Cube");
                    if(activeSlot == null)
                    {
                        cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.7f);
                    }
                    
                    if(!nowClicked && cube.GetComponent<MeshRenderer>().enabled == false)
                    {
                        cardState = CARD_STATE.DONE;
                    }
                }
            }

        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.collider.tag == "SLOT")
            {
                //isWorking = false;
                intoSlot = collision.gameObject;
                Slots.Remove(intoSlot);
                cube = intoSlot.gameObject.transform.Find("Cube");
                cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.12f);               
                cardState = CARD_STATE.MOVE;
                if (Slots.Count == 0)
                {
                    intoSlot = null;
                }
            }

            if (collision.collider.tag == "POINTER")
            {
                pointerOnCube = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Birthplace" && !isHomeTW)
            {
                isHomeTW = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Birthplace" && !isHomeTW)
            {
                isHomeTW = false;
            }
        }

        Transform MeshRendererOff(GameObject actSlot)
        {
            cube = intoSlot.gameObject.transform.Find("Cube");
            cube.GetComponent<MeshRenderer>().enabled = false;
            return cube;
        }

        void InstantiateCard(GameObject thisG)
        {

            GameObject cardClone = (Instantiate(thisG));
            cardClone.transform.SetParent(Grp);
            cardClone.transform.localPosition = startPos;
            cardClone.transform.localScale = new Vector3(1, 1, 1);
        }

        public IEnumerator resetPlanCube()
        {
            cardState = CARD_STATE.IDLE;
            activeSlot = null;
            intoSlot = null;
            yield return new WaitForSeconds(0.07f);
            transform.localPosition = startPos;
        }
    }

}

