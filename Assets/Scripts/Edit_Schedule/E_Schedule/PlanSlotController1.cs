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
            if (passenger == null && inSlot)
            {
                inSlot = false;
                mesh.enabled = true;
            }
        }


        public void resetPlanSlot()
        {
            passenger = null;
        }
    }
}




