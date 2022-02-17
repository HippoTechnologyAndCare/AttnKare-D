using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scheduler
{
    public class CardCreater : MonoBehaviour
    {
        [SerializeField] GameObject originPos;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] GameObject storedCard;
        [SerializeField] ScheduleManager1 schManager;


        [SerializeField] string otherName;
        public bool isStored;

        string word;

        void Start()
        {
            schManager = GameObject.FindObjectOfType<ScheduleManager1>();
            originPos = this.gameObject;
            word = "(Clone)";
        }

        void Update()
        {
            
        }

        void InstantiateCard(GameObject cardP)
        {
            GameObject cardClone = (Instantiate(cardP));
            cardClone.transform.SetParent(cardPrefab.GetComponent<PlanCubeController1>().Grp);
            cardClone.transform.localPosition = originPos.transform.localPosition;
            cardClone.transform.localScale = new Vector3(1, 1, 1);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "PLAN")
            {
                other.GetComponent<PlanCubeController1>().cardState = PlanCubeController1.CARD_STATE.IDLE;
                otherName = other.name.Replace("(Clone)", "");

                if (this.name == otherName && !isStored)
                {                    
                    other.GetComponent<PlanCubeController1>().isHomeTW = true;
                    isStored = true;
                    storedCard = other.gameObject;
                }  
                
                else if(storedCard != null && schManager.isReset)
                {
                    if(RemoveWord.EndsWithWord(storedCard.name, word))
                    {
                        Destroy(storedCard);
                        isStored = false;
                        storedCard = null;
                    }
                }
            }                        
        }

        private void OnTriggerExit(Collider other)
        {

            if (other.tag == "PLAN")
            {
                otherName = other.name.Replace("(Clone)", "");
                if (this.name == otherName && isStored)
                {
                    other.GetComponent<PlanCubeController1>().isHomeTW = false;
                    isStored = false;
                    storedCard = null;
                }
            }
        }
    }
}

