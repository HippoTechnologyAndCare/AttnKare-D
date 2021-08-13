using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace dw.game.doorlock
{
    public class BtnDoorLock : MonoBehaviour
    {
        [System.Serializable]
        public class ButtonEvent : UnityEvent { }

        public float pressLength;
        public bool pressed;
        public ButtonEvent downEvent;
        Vector3 startPos;

        void Start()
        {
            startPos = transform.position;
        }

        void Update()
        {
            float distance = Mathf.Abs(transform.position.y - startPos.y);
            if (distance >= pressLength)
            {
                transform.position = new Vector3(transform.position.x, startPos.y - pressLength, transform.position.z);
                if (!pressed)
                {
                    pressed = true;
                    downEvent?.Invoke();
                }
            }
            else
            {
                pressed = false;
            }
            if (transform.position.y > startPos.y)
            {
                transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
            }
        }

        public void OnClick() 
        {
            Debug.Log("OnClick :" + name);
        }
    }
}
