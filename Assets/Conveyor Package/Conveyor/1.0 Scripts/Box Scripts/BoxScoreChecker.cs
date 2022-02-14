using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScoreChecker : MonoBehaviour
{
    [SerializeField] Box m_box;

    private void OnTriggerEnter(Collider other)
    {
        if (CheckToyComponent(other.gameObject))
        {
            BNG.Toy toyCmp = other.GetComponent<BNG.Toy>();
            m_box.AddToy(toyCmp);
        }
    }

    private void OnTriggerStay(Collider other)  { if(CheckToyComponent(other.gameObject)) other.GetComponent<BNG.Toy>().DisableGrab(); }

    private void OnTriggerExit(Collider other)  { if (CheckToyComponent(other.gameObject)) m_box.RemoveToy(other.GetComponent<BNG.Toy>()); }

    bool CheckToyComponent(GameObject toy)
    {
        bool ret = false;
        if (toy.GetComponent<BNG.Toy>() != null) ret = true;
        return ret;
    }
}
