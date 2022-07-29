using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTouch : MonoBehaviour
{
    // Start is called before the first frame update
    public CheckData_NumCheck data;
    bool m_bhit = false;
    private float m_fspeed;
    private GameObject Hand;
    // Start is called before the first frame updat

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Grabber")
        {
            Hand = other.gameObject;
            m_bhit = true;
            data.hitCharacter++;
        }
        // get the direction from player to collider
       
    }
    private void Update()
    {
        if (m_bhit) m_fspeed += Time.deltaTime;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Hand)
        {
            m_bhit = false;
            data.hitSpeed.Add(m_fspeed);
            m_fspeed = 0;
        }
    }
}
