using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    [SerializeField] Material m_playerActive;
    [SerializeField] Material m_playerInactive;

    Renderer m_playAreaColor;
    bool m_outOfBounds = false;

    [SerializeField] float m_timeOutOfBounds = 0f;
    [SerializeField] int m_escapeCount = 0;

    public float GetEscapeTime()  { return m_timeOutOfBounds; }
    public int   GetEscapeCount() { return m_escapeCount; }
    
    private void Start() { m_playAreaColor = GetComponent<Renderer>(); }

    private void Update()
    {
        if (m_outOfBounds) m_timeOutOfBounds += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) { m_playAreaColor.material = m_playerActive;    m_outOfBounds = false; }
    private void OnTriggerExit(Collider other)  { m_playAreaColor.material = m_playerInactive;  m_outOfBounds = true;  m_escapeCount++; }
}
