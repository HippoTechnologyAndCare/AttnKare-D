using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BNG;
using UnityEngine.Serialization;

namespace Scheduler
{
    public class PlanCubeController1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        public ScheduleManager1 scheduleManager;

        public enum CardState { Idle, Move, Enter, Done };
        
        public CardState cardState;

        [FormerlySerializedAs("HandCursor")] public Transform handCursor;
        public HandController handController;

        [FormerlySerializedAs("Canvas")] public Transform canvas;
        [FormerlySerializedAs("Grp")] public Transform grp;

        public GameObject activeSlot;
        public Vector3 startPos;

        public float t;
        public bool isHomeTW;

        [FormerlySerializedAs("cardCreater")] [SerializeField] CardCreator cardCreator;

        [SerializeField] GameObject originPos_P;
        //[SerializeField] Transform originPos;
        [SerializeField] GameObject cardPrb;
        [SerializeField] GameObject intoSlot;        
        [SerializeField] Transform cube;
        
        //[SerializeField] bool isWorking;

        private UIPointer uiPointer;

        private Vector3 vec2Pos;
        private Vector3 zPos;

        //[SerializeField] bool working;
        private bool nowClicked = false;
        private bool pointerOnCube = false;


        //test
        [FormerlySerializedAs("Slots")] [SerializeField] List<GameObject> slots;

        private void Start()
        {
            FindOriginPos();

            slots = new List<GameObject>();
            
            intoSlot = null;
            cube = null;
            cardState =  CardState.Idle;
            cardPrb = gameObject;
            zPos.z = 2.21874f;
            startPos = transform.localPosition;
            uiPointer = handCursor.GetComponent<BNG.UIPointer>();
        }

        private void FixedUpdate()
        {
            if (!nowClicked) return;
            if (handCursor.GetComponent<LineRenderer>().enabled)
            {
                MoveCard();
            }
            else
            {
                Debug.Log("???");
            }

            if (pointerOnCube || handController.PointAmount != 1) return;
            transform.SetParent(grp);
            scheduleManager.ReleaseAllCollision();
            scheduleManager.PlaySoundByTypes(ESoundType.Put);

            nowClicked = false;
            intoSlot = null;

            transform.localPosition = activeSlot == null ? startPos : activeSlot.transform.localPosition;
        }

        private void FindOriginPos()
        {
            originPos_P = GameObject.Find("Origin Pos");
            var myName = name.Replace("(Clone)", "");
            cardCreator = originPos_P.transform.Find(myName).GetComponent<CardCreator>();            
        }

        private void MoveCard()
        {
            // LaserEnd의 포지션을 따라간다
            cardState = CardState.Move;
            Vector2 a = transform.position;
            Vector2 b = uiPointer._cursor.transform.position;
            vec2Pos = Vector2.Lerp(a, b, t);
            vec2Pos.z = zPos.z;
            transform.position = vec2Pos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            nowClicked = true;
            transform.SetParent(canvas);
            scheduleManager.LockAllCollision(transform);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.SetParent(grp);
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

                    if (!isHomeTW && !cardCreator.isStored)
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
                    cardState = CardState.Done;
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

                    cardState = CardState.Done;
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
                    cardState = CardState.Done;
                    transform.localPosition = activeSlot.transform.localPosition;
                }
                else
                {
                    cardState = CardState.Idle;
                    transform.localPosition = startPos;
                }
            }

            nowClicked = false;
            intoSlot = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "SLOT")
            {
                intoSlot = collision.gameObject;
                slots.Add(intoSlot);
            }

            if (collision.collider.tag == "POINTER")
            {
                pointerOnCube = true;
                scheduleManager.PlaySoundByTypes(ESoundType.In);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider.tag != "SLOT") return;
            if (slots.Count != 1) return;
            //isWorking = true;
            intoSlot = collision.collider.gameObject;                    
            cardState = CardState.Enter;
            cube = intoSlot.gameObject.transform.Find("Cube");
            if(activeSlot == null)
            {
                cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.7f);
            }
                    
            if(!nowClicked && cube.GetComponent<MeshRenderer>().enabled == false)
            {
                cardState = CardState.Done;
            }

        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.tag == "SLOT")
            {
                //isWorking = false;
                intoSlot = collision.gameObject;
                slots.Remove(intoSlot);
                cube = intoSlot.gameObject.transform.Find("Cube");
                cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.12f);               
                cardState = CardState.Move;
                if (slots.Count == 0)
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

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Birthplace" && !isHomeTW)
            {
                isHomeTW = false;
            }
        }

        private void MeshRendererOff(GameObject actSlot)
        {
            cube = actSlot.gameObject.transform.Find("Cube");
            cube.GetComponent<MeshRenderer>().enabled = false;
        }

        private void InstantiateCard(GameObject thisG)
        {
            var cardClone = Instantiate(thisG);
            cardClone.transform.SetParent(grp);
            cardClone.transform.localPosition = startPos;
            cardClone.transform.localScale = new Vector3(1, 1, 1);
        }

        public IEnumerator resetPlanCube()
        {
            cardState = CardState.Idle;
            activeSlot = null;
            intoSlot = null;
            yield return new WaitForSeconds(0.07f);
            transform.localPosition = startPos;
        }
    }

}

