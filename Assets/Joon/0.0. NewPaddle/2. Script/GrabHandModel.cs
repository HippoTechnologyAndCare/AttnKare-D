using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandModel : MonoBehaviour
{
    [Header("HandModel Change")]
    public GameObject OriginalModel;
    public GameObject NewModel;
  
    public void ChageHand(bool grab)
    {
        if (grab) { OriginalModel.SetActive(false); NewModel.SetActive(true); }
        if (!grab) { OriginalModel.SetActive(true); NewModel.SetActive(false); }
    }
}
