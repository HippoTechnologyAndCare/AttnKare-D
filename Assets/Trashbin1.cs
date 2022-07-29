using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbin1 : MonoBehaviour
{
    public GameObject warningNotTrash;
    // Start is called before the first frame update
    public bool m_isDuckinTrash = false;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unnecessary")
        {
            m_isDuckinTrash = true;
            warningNotTrash.SetActive(true);
        }
            
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Unnecessary")
            m_isDuckinTrash = false;
            warningNotTrash.SetActive(false);

    }
}
