using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scheduler
{
    public class CardCreator : MonoBehaviour
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

        private void InstantiateCard(GameObject cardP)
        {
            var cardClone = Instantiate(cardP, cardPrefab.GetComponent<PlanCubeController1>().grp, true);
            cardClone.transform.localPosition = originPos.transform.localPosition;
            cardClone.transform.localScale = new Vector3(1, 1, 1);
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

