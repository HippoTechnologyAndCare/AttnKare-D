using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scheduler
{
    public class PlanSlotController1 : MonoBehaviour
    {
        public GameObject passenger;

        public bool inSlot;

        [SerializeField] Transform cube;

        MeshRenderer mesh;


        void Start()
        {
            inSlot = false;
            cube = this.gameObject.transform.Find("Cube");
            mesh = cube.GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            // 슬롯안에 stored된 카드를 빼는 상황일때
            if (passenger == null && inSlot)
            {
                inSlot = false;
                mesh.enabled = true;
                
            }
        }        

        public void resetPlanSlot()
        {
            
            if(passenger != null)
            {
                string keyword = "(Clone)";
                if (RemoveWord.EndsWithWord(passenger.name, keyword))
                {
                    Destroy(passenger);
                }
                passenger = null;
            }
                        
        }        
    }
}




