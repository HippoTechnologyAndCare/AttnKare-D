using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource m_audioSource;

    [SerializeField] List<AudioClip> m_introAudio;

    [SerializeField] AudioClip m_intro;
    [SerializeField] AudioClip m_stage1Audio;
    [SerializeField] AudioClip m_stage2Audio;
    [SerializeField] AudioClip m_stage3Audio;

    // Initialized as Something Random (Not GameState.Waiting)
    GameState prev = GameState.Stage3End;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateChanged())
        {
            if (StageManager.currentGameState == GameState.Waiting)   { PlayStageAudio(0); }
            if (StageManager.currentGameState == GameState.Stage1End) { PlayStageAudio(1); }
            if (StageManager.currentGameState == GameState.Stage2End) { PlayStageAudio(2); }
            if (StageManager.currentGameState == GameState.Stage3End) { PlayStageAudio(3); }
        }
        
        /*if (m_audioSource.isPlaying) Debug.Log("Playing Audio");
        else Debug.Log("Muted Audio");*/
    }

    bool GameStateChanged()
    {
        bool isChanged = false;

        if (StageManager.currentGameState != prev) isChanged = true;
        prev = StageManager.currentGameState;

        return isChanged;
    }
    public void PlayStageAudio(int index)
    {
        // Play Audio
        switch(index)
        {
            case 0: m_audioSource.clip = m_intro;       m_audioSource.Play(); break;
            case 1: m_audioSource.clip = m_stage1Audio; m_audioSource.Play(); break;
            case 2: m_audioSource.clip = m_stage2Audio; m_audioSource.Play(); break;
            case 3: m_audioSource.clip = m_stage3Audio; m_audioSource.Play(); break;
        }

        // Wait for Audio to End
        StartCoroutine(WaitForAudio(index));
    }

    public void PlayIntroAudio()
    {

    }

    IEnumerator WaitForAudio(int index)
    {
        // while Audio is playing, wait 1 sec
        while (m_audioSource.isPlaying) yield return new WaitForSeconds(1);

        // on break, change game state to GameState.StageNStart
        switch (index)
        {
            case 0: StageManager.currentGameState = GameState.Stage1Start; break;
            case 1: StageManager.currentGameState = GameState.Stage2Start; break;
            case 2: StageManager.currentGameState = GameState.Stage3Start; break;
            case 3: StageManager.currentGameState = GameState.GameEnd;     break;
        }
    }
}
