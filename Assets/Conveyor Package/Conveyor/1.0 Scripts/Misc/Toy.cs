using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{
    public class Toy : MonoBehaviour, IPooledObject
    {
        public enum ToyType { Yellow, Blue, Green}

        [SerializeField] ToyType m_toyType;

        Rigidbody m_rigidbody;
        Grabbable m_grabbable;
        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_grabbable = GetComponent<Grabbable>();
        }

        private void Update()
        {
            if (m_grabbable.BeingHeld)
            {
                m_rigidbody.constraints = RigidbodyConstraints.None;
                m_rigidbody.mass = .01f;

                FactoryManager.AddToGrabbedList(gameObject);

                // Change to Layer that Ignores Collision
                // Robots Don't Collide with each other
                for(int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.layer = 12;
                }
            }
            // Change Back to Original Layer
            else
            {
                /*m_rigidbody.mass = 1f;*/

                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.layer = 10;
                }
            }
        }

        public ToyType GetToyType() { return m_toyType; }

        public bool IsBeingHeld() { return m_grabbable.BeingHeld; }

        public void DisableGrab() { if(!m_grabbable.BeingHeld) m_grabbable.enabled = false; }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.name == "Destroy Toy")
            {
                Destroy(gameObject);

                if(m_toyType == StageManager.m_currentColor)
                {
                    FactoryManager.m_destroyCount++;
                }
            }
        }

        public void OnObjectSpawn() { }
    }
}

