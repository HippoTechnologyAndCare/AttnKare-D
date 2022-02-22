using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxProp : MonoBehaviour
{
    [SerializeField] BoxType m_boxType;

    [SerializeField] FactoryManager m_factoryManager;
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.name == "Collider (Plain Box In)")
        {
            //Destroy and call factorymanager.boxin()
            Destroy(gameObject);
            m_factoryManager.BoxIn(m_boxType);
        }
        if (other.gameObject.name == "Collider (Closed Box In)") Destroy(gameObject); 
    }
    private void OnTriggerExit(Collider other) { if (other.gameObject.name == "Collider (Box Out)") Debug.Log("Open Shutter"); }
}
