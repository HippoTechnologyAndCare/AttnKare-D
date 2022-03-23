using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KetosGames.SceneTransition;
using TMPro;
using UnityEngine.Events;

public class SkipForPaddle : MonoBehaviour
{
    bool bFin = false;
    bool bActive = false;
    public CanvasGroup FinishCanvas;
    private Coroutine coroutine = null;
    string debugstring;
    Transform Fin1;
    Transform Fin2;
    public UnityEvent NextEvent;


    void Start()
    {
        Fin1 = FinishCanvas.transform.GetChild(0);
        Fin2 = FinishCanvas.transform.GetChild(1);
    }

    void Update()
    {
        bActive = false;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.25f);
        foreach(Collider hit in hitColliders)
        {
            if(hit.name == "RaycastCollider") //have to hit with hand
            {
                bActive = true;
            }
        }
    }

    public void OnButtonDown()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = bFin ? bActive ? StartCoroutine(NextScene()) : null : StartCoroutine(PressedFirst());
    }

    IEnumerator PressedFirst()
    {
        float lerpTime = 0f;
        while (FinishCanvas.alpha < 1.0f) //fade in
        {
            lerpTime += Time.deltaTime;
            FinishCanvas.alpha = Mathf.Lerp(0, 1, lerpTime / .8f);

        }
        yield return new WaitForSeconds(0.8f);

        bFin = true;
        yield return new WaitForSeconds(2.5f);

        lerpTime = 0f;
        while (FinishCanvas.alpha > 0f) //fade out
        {
            lerpTime += Time.deltaTime;
            FinishCanvas.alpha = Mathf.Lerp(1, 0, lerpTime / 1.2f);

        }

        yield return new WaitForSeconds(7.0f);

        bFin = false; //if not pressed for 7 seconds turn off 
    }


    IEnumerator NextScene()
    {
        if (NextEvent != null)
        {
            NextEvent.Invoke();
        }
        yield return null;
    }
}
