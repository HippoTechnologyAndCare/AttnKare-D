using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hud_Paddle : MonoBehaviour
{

    public TextMeshProUGUI m_textDISTANCE;
    public TextMeshProUGUI m_textERROR;
    int m_nDISTANCE  = 0;
    int m_nPERCENT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDistance(int nStage)
    { 
        m_nPERCENT = Manager_Paddle.SDB[nStage].intPercent;
        m_nDISTANCE += m_nPERCENT;
        m_textDISTANCE.text = m_nDISTANCE.ToString();
    }

    public void ErrorMessage()
    {

    }
}
