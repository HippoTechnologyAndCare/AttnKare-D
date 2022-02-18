using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    [SerializeField] FactoryManager m_factoryManager;

    [SerializeField] Transform Clicker;

    [SerializeField] float m_buttonUpPos;
    [SerializeField] float m_buttonDownPos;

    int m_pressCount = 0;

    private void Start()
    {
        m_buttonUpPos = Clicker.localPosition.z;
        m_buttonDownPos = Clicker.localPosition.z + .08f;
    }
    private void Update() { ClampButton(); }
    private void OnTriggerEnter(Collider other) { OnSkipButtonPressed(); }

    void OnSkipButtonPressed()
    {
        if (m_pressCount < 2) m_pressCount++;

        if (m_pressCount == 1)
        {
            // Show First UI
            Debug.Log("Press Count: 1");
            Invoke("ResetPressCount", 3f);
        }
        else if (m_pressCount == 2)
        {
            // Show Second UI and End Game
            FactoryManager.m_gameData.SetSkipped(true);
            m_factoryManager.SaveGameData();
            StageManager.ChangeGameState(GameState.GameEnd);
            Debug.Log("Press Count: 2");
        }
        else
            return;
    }

    void ClampButton()
    {
        if (Clicker.localPosition.z > m_buttonDownPos || Clicker.localPosition.z < m_buttonUpPos)
            Clicker.localPosition = new Vector3(Clicker.localPosition.x, Clicker.localPosition.y, Mathf.Clamp(Clicker.localPosition.z, m_buttonUpPos, m_buttonDownPos));
    }

    void ResetPressCount() { if(m_pressCount == 1) m_pressCount = 0; }
}
