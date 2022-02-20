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

        public enum CardState { Idle, Move, Enter, Done }
        
        public CardState cardState;

        [FormerlySerializedAs("HandCursor")] public Transform handCursor;
        public HandController handController;

        [FormerlySerializedAs("Canvas")] public Transform canvas;
        [FormerlySerializedAs("Grp")] public Transform grp;

        public GameObject activeSlot;
        public GameObject intoSlot;
        public GameObject workingSlot;
        public Vector3 startPos;

        public float t;
        public bool isHomeTW;

        [SerializeField] CardCreator cardCreator;

        [SerializeField] GameObject originPos_P;


        //[SerializeField] Transform originPos;
        [SerializeField] private GameObject cardPrb;
        [SerializeField] private GameObject prevSlot;
        [SerializeField] Transform cube;
        
        //[SerializeField] bool isWorking;

        private UIPointer uiPointer;

        private Vector3 vec2Pos;
        private Vector3 zPos;

        //[SerializeField] bool working;
        private bool nowClicked = false;
        private bool pointerOnCube = false;
        
        //test
        [SerializeField] private List<GameObject> slots;

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
            //아래 조건문에 activeSlot아니고 intoSlot일 수도 있음
            if(activeSlot == null) return;
            MeshRendererOn(activeSlot);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.SetParent(grp);
            scheduleManager.ReleaseAllCollision();
            scheduleManager.PlaySoundByTypes(ESoundType.Put);

            if (intoSlot != null) //슬롯에 들어온 경우
            {
                if (intoSlot == null)
                {
                    if(activeSlot == null) return;
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
                    //activeSlot.GetComponent<PlanSlotController1>().inSlot = true;
                    MeshRendererOff(activeSlot);
                    cardState = CardState.Done;
                }
                else
                {
                    //새로운 슬롯에 이미 계획이 있는 경우
                    if (activeSlot == null) //현재 할당된 슬롯이 없는 경우
                    {
                        intoSlot.GetComponent<PlanSlotController1>().passenger.GetComponent<PlanCubeController1>().resetPlanCube(0.07f);
                        activeSlot = intoSlot;
                    }
                    
                    //현재 할당된 슬롯이 있어서 바꿔치기 하는 경우 // A큐브가 -> B슬롯으로 옮김, B큐브가 -> A슬롯으로 옮겨짐
                    else if (activeSlot.GetComponent<PlanSlotController1>().passenger != intoSlot.GetComponent<PlanSlotController1>().passenger)
                    {
                        // B슬롯에 있는 passenger(B큐브)를 tempCard에 임시로 복사
                        GameObject tempCard = intoSlot.GetComponent<PlanSlotController1>().passenger;
                        // B큐브에 있는 activeSlot(B슬롯)을 tempSlot에 임시로 복사
                        GameObject tempSlot = tempCard.GetComponent<PlanCubeController1>().activeSlot;
                        //이전 슬롯인 prevSlot의 포지션으로 B큐브를 옮김
                        tempCard.transform.localPosition = prevSlot.transform.localPosition;
                        // A가 원래 가지고 있던 activeSlot을 B에게 복사 밀어넣음 (내 activeSlot을 B로 아직 바꾸지 않음)
                        tempCard.GetComponent<PlanCubeController1>().activeSlot = activeSlot;
                        //B오브젝트를 내가 가지고 있는 컴포넌트 속 passenger에 넣음으로 
                        //activeSlot.GetComponent<PlanSlotController1>().passenger = tempCard;
                        activeSlot = tempSlot;
                    }
                    
                    cardState = CardState.Done;
                    //activeSlot = intoSlot;
                    transform.localPosition = activeSlot.transform.localPosition;
                    activeSlot.GetComponent<PlanSlotController1>().passenger = gameObject;
                    //activeSlot.GetComponent<PlanSlotController1>().inSlot = true;
                    MeshRendererOff(activeSlot);
                }

                scheduleManager.CheckMovingCnt();
                scheduleManager.CheckAllScheduleOnSlot();
            }
            else
            {
                if (activeSlot != null)
                {
                    MeshRendererOff(activeSlot);
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
            // 슬롯에 들어왔고 들어온 슬롯 갯수가 딱 1개만인지 확인
            if (collision.collider.tag != "SLOT") return;
            if (slots.Count != 1) return;
            //isWorking = true;
            
            if(activeSlot == null || prevSlot != null)
            {
                intoSlot = collision.collider.gameObject;                    
                cardState = CardState.Enter;
                cube = intoSlot.gameObject.transform.Find("Cube");
                cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.7f);
            }
                    
            if(!nowClicked && cube.GetComponent<MeshRenderer>().enabled == false)
            {
                cardState = CardState.Done;
            }

        }

        private void OnCollisionExit(Collision collision)
        {
            switch (collision.collider.tag)
            {
                case "SLOT":
                {
                    //isWorking = false;
                    prevSlot = collision.gameObject;
                    slots.Remove(prevSlot);
                    cube = prevSlot.gameObject.transform.Find("Cube");
                    cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.12f);               
                    cardState = CardState.Move;
                    if (slots.Count == 0)
                    {
                        intoSlot = null;
                    }

                    break;
                }
                case "POINTER":
                    pointerOnCube = false;
                    break;
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

        private void MeshRendererOn(GameObject actSlot)
        {
            cube = actSlot.gameObject.transform.Find("Cube");
            cube.GetComponent<MeshRenderer>().enabled = true;
        }
        
        private void MeshRendererOff(GameObject actSlot)
        {
            cube = actSlot.gameObject.transform.Find("Cube");
            cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.12f);
            cube.GetComponent<MeshRenderer>().enabled = false;
        }

        private void InstantiateCard(GameObject thisG)
        {
            var cardClone = Instantiate(thisG);
            cardClone.transform.SetParent(grp);
            cardClone.transform.localPosition = startPos;
            cardClone.transform.localScale = new Vector3(1, 1, 1);
        }

        public IEnumerator resetPlanCube(float wait)
        {
            cardState = CardState.Idle;
            activeSlot = null;
            intoSlot = null;
            yield return new WaitForSeconds(wait);
            transform.localPosition = startPos;
        }
    }

}

