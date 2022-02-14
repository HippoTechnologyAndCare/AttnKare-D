using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteLOD_BP : MonoBehaviour
{
    //Change alpha value of sprite as user gets close
    //Create LOD effect
    public float closeDistance; //가까이 왔다고 판단되는 위치
    public Image Image;

    Color m_cImage;
    float m_falpha; // alpha value chages as distance decrease
    float m_fdistance; //distance between object and user
    float m_fFarAlpha; //initial alpha value
    // Start is called before the first frame update
    void Start()
    {
        m_cImage = Image.color;
        m_fFarAlpha = m_cImage.a;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "HeadCollision")
        {
            m_fdistance = Vector3.Distance(this.transform.position, other.transform.position);
            Debug.Log(m_fdistance);
            m_falpha = 1 / m_fdistance / closeDistance;
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, m_falpha);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "HeadCollision")
        {
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, m_fFarAlpha);
        }
    }
}
