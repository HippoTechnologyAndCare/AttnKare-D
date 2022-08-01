using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    /*[SerializeField] FactoryManager m_factoryManager;*/

    [SerializeField] BNG.CollectData m_collectData;

    [SerializeField] Material m_playerActive;
    [SerializeField] Material m_playerInactive;

    [SerializeField] float m_timeOutOfBounds = 0f;
    [SerializeField] int m_escapeCount = 0;

    Renderer m_playAreaColor;
    bool m_outOfBounds = false;
    
    BNG.InputBridge input;
    public float GetEscapeTime()  { return m_timeOutOfBounds; }
    public int   GetEscapeCount() { return m_escapeCount; }

    private void Awake() { input = BNG.InputBridge.Instance; }
    private void Start() { m_playAreaColor = GetComponent<Renderer>(); }

    private void Update()
    {
        if (m_outOfBounds) m_timeOutOfBounds += Time.deltaTime;
    }

    public void ChangeColor(bool inBounds)
    {
        if (inBounds)
        {
            m_playAreaColor.material = m_playerActive;
            /*m_factoryManager.RestartFactory();*/
        }
        else
        {
            m_playAreaColor.material = m_playerInactive;
            /*m_factoryManager.StopFactory();*/
            input.VibrateController(.3f, .1f, 5f, BNG.ControllerHand.Left);
            input.VibrateController(.3f, .1f, 5f, BNG.ControllerHand.Right);
        }
    }
    private void OnTriggerEnter(Collider other) { ChangeColor(true); UIManager.DestroyWarningCanvas(); ;
                                                    m_collectData.AddTimeStamp("ESCAPE END");   m_outOfBounds = false; }
    private void OnTriggerExit(Collider other)  { ChangeColor(false); UIManager.ShowWarningCanvas(); ;
                        m_collectData.AddTimeStamp("ESCAPE START"); m_outOfBounds = true;  m_escapeCount++; }
}
