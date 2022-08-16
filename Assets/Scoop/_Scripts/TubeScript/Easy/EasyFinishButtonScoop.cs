using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KetosGames.SceneTransition;
using TMPro;

public class EasyFinishButtonScoop : MonoBehaviour
{
    bool bFin = false;
    bool bActive = false;
    public CanvasGroup FinishCanvas;
    private Coroutine coroutine = null;
    string debugstring;
    Transform Fin1;
    Transform Fin2;
    public int buildindex = 3;
    [SerializeField] Transform scoreboard;

    // Start is called before the first frame update
    void Start()
    {
        Fin1 = FinishCanvas.transform.GetChild(0);
        Fin2 = FinishCanvas.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        bActive = false;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.25f);
        foreach(Collider hit in hitColliders)
        {
            if(hit.name == "RaycastCollider") 
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

        if (bFin) //if it is second press
        {
            

            if (bActive) // if hand is what touching button
            {
                coroutine = StartCoroutine(NextScene());
            }

        }
        if(!bFin)
        {

            coroutine = StartCoroutine(PressedFirst());

        }
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

        Debug.Log("NEXT SCENE SEND");
        GetComponent<BNG.Button>().enabled = false;
        FinishCanvas.alpha = 1f;
        Fin1.gameObject.SetActive(false);
        Fin2.gameObject.SetActive(true);

        StartCoroutine( scoreboard.GetComponent<EasyTubeScoreboard>().GoToLobby(true));
        yield return null;




    }
  



}
