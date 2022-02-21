using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Scheduler
{
    public class OriginPosController : MonoBehaviour
    {
        [SerializeField] private GameObject originPos;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private GameObject storedCard;
        [SerializeField] private ScheduleManager1 schManager;


        [SerializeField] private string otherName;
        public bool isStored;

        private string word;

        private void Start()
        {
            schManager = FindObjectOfType<ScheduleManager1>();
            originPos = gameObject;
            word = "(Clone)";
        }
        
        public void CardDestroyer(GameObject cardB)
        {
            if (storedCard == null)
            {
                storedCard = cardB;
                cardB.transform.localPosition = originPos.transform.localPosition;
                Debug.Log("originsPos가 비어서 cardB를 옮겨옴");
            }
            
            else if (storedCard != null)
            {
                if (!RemoveWord.EndsWithWord(cardB.name, word))
                {
                    cardB.transform.localPosition = originPos.transform.localPosition;
                    Destroy(storedCard);
                    storedCard = cardB;
                    Debug.Log("cardB가 원본이라 origin pos로 옮겨오고기존자리 카드는 삭제함");
                }
                
                else if (!RemoveWord.EndsWithWord(storedCard.name, word))
                {
                    Destroy(cardB);
                    Debug.Log("origin pos에 있는 카드가 원본이라 cardB는 삭제함");
                }
            }

            else
            {
                Debug.Log("아무 조건문도 안거침");
            }
            storedCard.GetComponent<PlanCubeController1>().activeSlot = null;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("PLAN")) return;
            other.GetComponent<PlanCubeController1>().cardState = PlanCubeController1.CardState.Idle;
            otherName = other.name.Replace("(Clone)", "");

            if (name == otherName && !isStored)
            {                    
                other.GetComponent<PlanCubeController1>().isHomeTW = true;
                isStored = true;
                storedCard = other.gameObject;
            }  
                
            // 리셋 버튼을 눌러 전체 카드 리셋을 하려고 하는데 Origin Pos안에 카드가 들어 있을 경우 해당 카드를 삭제
            else if(storedCard != null && schManager.isReset)
            {
                if(!RemoveWord.EndsWithWord(storedCard.name, word)) return;
                Destroy(storedCard);
                isStored = false;
                storedCard = null;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("PLAN")) return;
            otherName = other.name.Replace("(Clone)", "");
                
            if (name != otherName || !isStored) return;
            other.GetComponent<PlanCubeController1>().isHomeTW = false;
            isStored = false;
            storedCard = null;
        }
    }
}

