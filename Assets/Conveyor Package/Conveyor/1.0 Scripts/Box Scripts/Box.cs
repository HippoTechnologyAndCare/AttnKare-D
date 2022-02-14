using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BoxType
{
    PlainBox,
    OpenBox,
    ClosedBox
}

public class Box : MonoBehaviour
{
    [SerializeField] BoxType m_boxType;

    [SerializeField] FactoryManager m_factoryManager;                           // Factory Manager Object
    [SerializeField] StageManager   m_stageManager;                             // Stage Manager Object

    List<int> m_toysToFill;
    List<int> m_toysInBox;                                                      // Information to Send to Factory Manager
    List<int> m_scoreDiff;                                                      // Score Difference
    protected Collider m_boxCollider;

    [SerializeField] List<BNG.Toy> m_toyObjectsInBox;

    BoxScore m_boxScore;

    // Start is called before the first frame update
    void Start()
    {
        m_toyObjectsInBox = new List<BNG.Toy>();
        m_boxScore   = new BoxScore();
        m_toysToFill = new List<int>();
        m_toysInBox  = new List<int>();
        m_scoreDiff  = new List<int>();
        for (int i = 0; i < 3; i++) { m_toysToFill.Add(0); m_toysInBox.Add(0); m_scoreDiff.Add(0); }
        m_boxCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy Box and Send Score to Factory Manager
        if (other.gameObject.name == "Collider (Open Box In)") CalculateScore();
    }

    public void AddToy(BNG.Toy toy)    { if (!m_toyObjectsInBox.Contains(toy))   m_toyObjectsInBox.Add(toy); }
    public void RemoveToy(BNG.Toy toy) { if (m_toyObjectsInBox.Contains(toy))    m_toyObjectsInBox.Remove(toy); }

    // For Debugging
    void CountToy (BNG.Toy toyCmp)
    {
        if (toyCmp.GetToyType() == BNG.Toy.ToyType.Yellow) m_toysInBox[0]++;
        if (toyCmp.GetToyType() == BNG.Toy.ToyType.Blue)   m_toysInBox[1]++;
        if (toyCmp.GetToyType() == BNG.Toy.ToyType.Green)  m_toysInBox[2]++;
    }

    // Calculates and Debugs Individual Box Scores
    void CalculateScore()
    {
        m_boxScore.PackData(m_toyObjectsInBox);

        // Destroy Toys in Box and Log to Console
        foreach (BNG.Toy toy in m_toyObjectsInBox) 
        { 
            CountToy(toy);
            FactoryManager.RemoveFromGrabbedList(toy.gameObject);
            Destroy(toy.gameObject);
        }

        // Debug Tool
        FactoryManager.CheckMissing();

        // Tell Factory Manager to Spawn Next Box
        m_factoryManager.BoxIn(m_boxType);

        // Destroy Box
        Destroy(gameObject);

        // Send m_toysInBox to Factory Manager
        SendScore(m_boxScore.GetDataPacket());

        m_toyObjectsInBox.Clear();
    }

    void SendScore(List<int> scores)
    {
        // Send Score to Factory Manager
        if (StageManager.currentGameState == GameState.Stage1)  m_factoryManager.GetScore(scores, 1);
        if (StageManager.currentGameState == GameState.Stage2)  m_factoryManager.GetScore(scores, 2);
        if (StageManager.currentGameState == GameState.Stage3)  m_factoryManager.GetScore(scores, 3);
    }
}

class BoxScore
{
    int m_isSuccessful = 0;       // Is the first toy the right color?
    int m_excess = 0;             // Number of Excess Toys with Correct Color
    int m_wrongColor = 0;         // Number of Excess Toys with Wrong Color
    int m_total = 0;

    List<int> m_dataPacket;// 0: m_isSuccessful, 1: m_excess, 2: m_wrongColor

    public BoxScore() 
    {
        m_dataPacket = new List<int>();
        for (int i = 0; i < 4; i++) m_dataPacket.Add(-1);
    }

    public List<int> GetDataPacket() { return m_dataPacket; }
    public int GetSuccessful() { return m_isSuccessful; }
    public int GetExcess() { return m_excess; }
    public int GetWrongColor() { return m_wrongColor; }

    public void PackData(List<BNG.Toy> toyList)
    {
        CheckSuccessful(toyList);
        CheckWrong(toyList);
    }
    public void CheckSuccessful(List<BNG.Toy> toyList)
    {
        m_isSuccessful = 0;
        m_total = toyList.Count;

        if (toyList.Count == 0)
        {
            m_dataPacket[0] = 0;
            m_dataPacket[3] = m_total;
            return;
        }

        if (toyList[0].GetToyType() == StageManager.m_currentColor && toyList.Count == 1) m_isSuccessful = 1;

        m_dataPacket[0] = m_isSuccessful;
        m_dataPacket[3] = m_total;
    }

    public void CheckWrong(List<BNG.Toy> toyList)
    {
        foreach(BNG.Toy toy in toyList)
        {
            if (toy.GetToyType() == StageManager.m_currentColor) m_excess++;
            else m_wrongColor++;
        }

        if (m_isSuccessful == 1 && m_excess > 0) m_excess--;
        else if(m_isSuccessful == 0 && m_wrongColor > 0) m_wrongColor--;

        m_dataPacket[1] = m_excess;
        m_dataPacket[2] = m_wrongColor;
    }
}
