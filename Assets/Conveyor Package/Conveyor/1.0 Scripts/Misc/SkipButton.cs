using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    /*[SerializeField] Transform Clicker;*/

    [SerializeField] float m_buttonUpPos;
    [SerializeField] float m_buttonDownPos;

    int m_pressCount = 0;

    private void Start()
    {
        m_buttonUpPos = transform.localPosition.z;
        m_buttonDownPos = transform.localPosition.z + .08f;
    }
    private void Update() { ClampButton(); }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pressed by " + other.gameObject.name);

        OnSkipButtonPressed();
    }

    void OnSkipButtonPressed()
    {
        if (m_pressCount < 2) m_pressCount++;

        if (m_pressCount == 1)
        {
            // Show First UI
        }
        else if (m_pressCount == 2)
        {
            // Show Second UI and End Game
            FactoryManager.m_gameData.SetSkipped(true);
        }
        else
            return;
    }

    void ClampButton()
    {
        /*if(Clicker.localPosition.y < m_downLimit)
        {
            Clicker.localPosition = new Vector3(Clicker.localPosition.x, m_downLimit, Clicker.localPosition.z);
        }    
        if(Clicker.localPosition.y > m_upLimit)
        {
            Clicker.localPosition = new Vector3(Clicker.localPosition.x, m_upLimit, Clicker.localPosition.z);
        }*/
        if (transform.localPosition.z > m_buttonDownPos || transform.localPosition.z < m_buttonUpPos)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, m_buttonUpPos, m_buttonDownPos));
    }
}
