using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoButton : MonoBehaviour
{
    // Please, name correctly your variables.
    // Yourself, in 3 months will be grateful
    GameObject m_rectAuto;
    Vector3 m_target;
    public float speed = 1.0f;
    public GameObject Face;
    public GameObject Finger;
    Guide_NumCheck guide;
    public string color = "#FF0000";
    TextMeshProUGUI m_tmrpoText;
    Color activatedColor;
    Color originalColor;
    bool m_bStart;
    void Start()
    {
        originalColor = Color.white;
        ColorUtility.TryParseHtmlString(color, out activatedColor);
        guide = GameObject.Find("Guide").GetComponent<Guide_NumCheck>();
    }

    void Update()
    {
        if (m_bStart)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            m_rectAuto.transform.localPosition= Vector3.MoveTowards(m_rectAuto.transform.localPosition, m_target, step);
        }
       
    }

    public void AutoMove()
    {
        guide.CannotGrab(null);
        StartCoroutine(SetPosition());
    }
    IEnumerator SetPosition()
    {
        Finger.SetActive(true);
        Face.SetActive(true);
        m_bStart = true;
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            m_rectAuto = Finger;
            int count = Random.Range(0, guide.arrPos.Count);
            m_target = guide.arrPos[count];
            yield return new WaitUntil(() => m_rectAuto.transform.localPosition == m_target);
            yield return new WaitForSeconds(1.2f);
        }
        m_target = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goNum.transform.localPosition;
        yield return new WaitUntil(() => m_rectAuto.transform.localPosition == m_target);
        m_tmrpoText = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goNum.GetComponentInChildren<TextMeshProUGUI>();
        m_tmrpoText.color = activatedColor;
        yield return new WaitForSeconds(1.0f);
        Finger.SetActive(false);
        m_rectAuto = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goNum;
        m_target = Guide_NumCheck.NCDB[Guide_NumCheck.Index].goTrig.transform.localPosition;
        yield return new WaitUntil(() => m_rectAuto.transform.localPosition == m_target);
        m_bStart = false;
        m_tmrpoText.color = originalColor;
        Face.SetActive(false);
        guide.CanGrab();
        Guide_NumCheck.Index++;
    }
    
    IEnumerator AutoMoveFinger()
    {
        yield return null;
    }

}
