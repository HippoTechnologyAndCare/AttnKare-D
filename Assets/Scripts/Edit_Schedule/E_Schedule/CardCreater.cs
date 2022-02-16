using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scheduler
{
    public class CardCreater : MonoBehaviour
    {
        [SerializeField] GameObject originPos;
        [SerializeField] GameObject cardPrefab;
        public bool isStore;

        void Start()
        {
            originPos = this.gameObject;                     

            InstantiateCard(cardPrefab);
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
            if(other.name == this.name && !isStore)
            {
                isStore = true;

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name == this.name && isStore)
            {
                isStore = false;
            }
        }
    }
}

