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
            Invoke("ResetPressCount", 3f);
            UIManager.ShowSkipCanvas();
        }
        else if (m_pressCount == 2 && !FactoryManager.m_gameData.IsDataSaved())
        {
            // Save Data
            FactoryManager.m_gameData.m_isSkipped = true;
            StageManager.ChangeGameState(GameState.GameEnd);

            // Show Second UI
            StartCoroutine(UIManager.CountSecondsOnCanvas());
        }
        else
            return;
    }

    void ClampButton()
    {
        if (Clicker.localPosition.z > m_buttonDownPos || Clicker.localPosition.z < m_buttonUpPos)
            Clicker.localPosition = new Vector3(Clicker.localPosition.x, Clicker.localPosition.y, Mathf.Clamp(Clicker.localPosition.z, m_buttonUpPos, m_buttonDownPos));
    }

    void ResetPressCount()
    {
        if (m_pressCount == 1)
        {
            m_pressCount = 0;
            UIManager.HideSkipCanvas();
        }
    }
}
