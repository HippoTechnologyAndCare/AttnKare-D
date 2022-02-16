﻿using System.Collections;
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

        public Vector3 StartPos;

        public float t;

        public GameObject activeSlot;

        [SerializeField] CardCreater cardCreater;

        [SerializeField] GameObject originPos_P;
        //[SerializeField] Transform originPos;
        [SerializeField] GameObject cardPrb;
        [SerializeField] GameObject intoSlot;
        [SerializeField] Transform currSlot;
        [SerializeField] Transform cube;

        [SerializeField] bool isHomeTW;
        //[SerializeField] bool isWorking;

        UIPointer uiPointer;
        Material mat;

        Vector3 vec2Pos;
        Vector3 zPos;

        //[SerializeField] bool working;
        bool NowClicked = false;
        bool PointerOnCube = false;


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
                    intoSlot = null;

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
            NowClicked = true;
            this.transform.SetParent(Canvas);
            scheduleManager.LockAllCollision(this.transform);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.transform.SetParent(Grp);
            scheduleManager.ReleaseAllCollision();
            scheduleManager.PlaySoundByTypes(ESoundType.PUT);

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

                    if(!isHomeTW && !cardCreater.isStore)
                    {
                        InstantiateCard(cardPrb);
                    }

                    activeSlot = intoSlot;
                    transform.localPosition = activeSlot.transform.localPosition;                    
                    activeSlot.GetComponent<PlanSlotController1>().passenger = this.gameObject;
                    activeSlot.GetComponent<PlanSlotController1>().isStore = true;
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
                    activeSlot.GetComponent<PlanSlotController1>().isStore = true;
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
                    transform.localPosition = StartPos;
                }
            }

            NowClicked = false;
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
                PointerOnCube = true;
                scheduleManager.PlaySoundByTypes(ESoundType.IN);
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
                    cube.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0, 0.545f, 0.7f);
                    if(!NowClicked && cube.GetComponent<MeshRenderer>().enabled == false)
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
                Debug.Log("exit");
                cardState = CARD_STATE.MOVE;
                if (Slots.Count == 0)
                {
                    intoSlot = null;
                }
            }

            if (collision.collider.tag == "POINTER")
            {
                PointerOnCube = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.name == this.name && !isHomeTW)
            {
                isHomeTW = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.name == this.name && isHomeTW)
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
            cardClone.transform.localPosition = StartPos;
            cardClone.transform.localScale = new Vector3(1, 1, 1);
        }

        public void resetPlanCube()
        {
            cardState = CARD_STATE.IDLE;
            activeSlot = null;
            intoSlot = null;
            transform.localPosition = StartPos;
        }
    }

}

