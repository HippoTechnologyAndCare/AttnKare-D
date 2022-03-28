using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource m_audioSource;

    [SerializeField] List<AudioClip> m_introAudio;

    [SerializeField] AudioClip m_stage1Audio;
    [SerializeField] AudioClip m_stage2Audio;
    [SerializeField] AudioClip m_stage3Audio;
    [SerializeField] AudioClip m_stageClearEffect;

    float[] m_waitLength;                               // 0: intro, 1: stage1clear, 2: stage2clear, 3: stage3clear

    // Initialized as Something Random (Not GameState.Waiting)
    GameState prev = GameState.Stage3End;

    [SerializeField] PlayArea m_playArea;

    // Enable this after Guide
    [SerializeField] GameObject m_playerTracker;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_waitLength = new float[4];
        for (int i = 0; i < m_waitLength.Length; i++) m_waitLength[i] = 0;
        for (int i = 0; i < m_introAudio.Count; i++)
        {
            m_waitLength[0] += m_introAudio[i].length;
        }

        m_waitLength[1] = m_stage1Audio.length + m_stageClearEffect.length;
        m_waitLength[2] = m_stage2Audio.length + m_stageClearEffect.length;
        m_waitLength[3] = m_stage3Audio.length + m_stageClearEffect.length;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateChanged())
        {
            if (StageManager.currentGameState == GameState.Waiting)   { PlayAudio(0); }
            if (StageManager.currentGameState == GameState.Stage1End) { PlayAudio(1); }
            if (StageManager.currentGameState == GameState.Stage2End) { PlayAudio(2); }
            if (StageManager.currentGameState == GameState.Stage3End) { PlayAudio(3); }
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
    public void PlayAudio(int index)
    {
        // Play Audio
        switch(index)
        {
            case 0: StartCoroutine(PlayIntroAudio());  StartCoroutine(AudioEnd(0)); break;
            case 1: StartCoroutine(PlayStageAudio(1)); StartCoroutine(AudioEnd(1)); break;
            case 2: StartCoroutine(PlayStageAudio(2)); StartCoroutine(AudioEnd(2)); break;
            case 3: StartCoroutine(PlayStageAudio(3)); StartCoroutine(AudioEnd(3)); break;
        }
    }

    public IEnumerator PlayIntroAudio()
    {
        for (int i = 0; i < m_introAudio.Count; i++)
        {
            m_audioSource.clip = m_introAudio[i];
            UIManager.SetUIText(UIManager.s_MainUIText, UIManager.s_AudioLine[i]);

            if (i == 1 || i == 3) InvokeGuide(i);
            if (i == 2) CancelInvoke("InvokeBlink");
            if (i == 4) { m_playArea.ChangeColor(true); m_playerTracker.SetActive(true); }

            m_audioSource.Play();

            while(m_audioSource.isPlaying)
            {
                yield return null;
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    public IEnumerator PlayStageAudio(int stage)
    {
        m_audioSource.clip = m_stageClearEffect;

        m_audioSource.Play();

        while(m_audioSource.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        
        switch(stage)
        {
            case 1: m_audioSource.clip = m_stage1Audio; m_audioSource.Play(); break;
            case 2: m_audioSource.clip = m_stage2Audio; m_audioSource.Play(); break;
            case 3: m_audioSource.clip = m_stage3Audio; m_audioSource.Play(); break;
        }
    }
    
    // Call StageManager.AudioEnd() after audio finishes playing
    public IEnumerator AudioEnd(int stage)
    {
        yield return new WaitForSeconds(m_waitLength[stage]);

        StageManager.AudioEnd(stage);

        if (stage == 0)
        {
            yield return new WaitForSeconds(2f);
            UIManager.s_MainUIText.enabled = false;
            UIManager.ScaleUI();
        }

        yield break;
    }
    void InvokeGuide(int index)
    {
        // Blink UI
        if(index == 1)
        {
            InvokeRepeating("InvokeBlink", 1f, .5f);
        }

        // Show Play Area Rules
        if(index == 3)
        {
            Invoke("InvokePlayArea", 1.8f);
        }
    }

    void InvokeBlink() { UIManager.BlinkImage(); }
    void InvokePlayArea() { m_playArea.ChangeColor(false); }
}
