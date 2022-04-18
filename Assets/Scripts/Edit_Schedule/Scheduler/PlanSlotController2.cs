﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scheduler
{
    public class PlanSlotController2 : MonoBehaviour
    {
        [SerializeField] ScheduleManager2 schManager;
        
        public GameObject passenger;

        //public bool inSlot;

        [SerializeField] Transform cube;

        private MeshRenderer mesh;
        //private GameObject otherCard;


        private void Start()
        {
            passenger = null;
            schManager = FindObjectOfType<ScheduleManager2>();
            cube = gameObject.transform.Find("Cube");
            mesh = cube.GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            // if (passenger == null )
            // {
            //     mesh.enabled = true;
            // }

            
            // 슬롯안에 stored된 카드를 빼는 상황일때
        }
                
        // 리셋 버튼을 눌렀을때 Slot안에 있는 클론된 큐브들은 소멸
        public void ResetPlanSlot()
        {
            if (passenger == null) return;
            const string keyword = "(Clone)";
            if (RemoveWord.EndsWithWord(passenger.name, keyword))
            {
                schManager.grpList.Remove(passenger.transform);
                Destroy(passenger);
            }
            passenger = null;

        }

        public IEnumerator ResetSlotMesh(float wait)
        {
            yield return new WaitForSeconds(wait);
            mesh.material.color = new Color(1f, 0.9568627f, 0.9019608f, 1f);
            mesh.enabled = true;
        }
    }
}




