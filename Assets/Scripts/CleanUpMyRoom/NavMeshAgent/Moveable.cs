using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moveable : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    Vector3 lastPosition = Vector3.zero;

    [SerializeField]
    Transform[] targets = new Transform[4];

    [SerializeField]
    Transform parentTarget;

    [SerializeField]
    float speed;

    [SerializeField]
    int currentNum;

    [SerializeField]
    bool isGoing;  

    public float Speed { get => speed; set => speed = value; }

    private void Awake()
    {        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {        
        for(int i = 0; i < targets.Length; i++)
        {
           targets[i] = parentTarget.transform.GetChild(i).transform;
        }

        isGoing = false;
        currentNum = 0;
        StartCoroutine(GoToTarget());
    }

    void FixedUpdate()
    {
                
    }

    void Update()
    {
        Speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (Speed > 0.01f)
        {
            if (!isGoing)
            {
                isGoing = true;
                animator.SetInteger("Status", 1);
            }
        }

        else if (Speed < 0.01f)
        {
            animator.SetInteger("Status", 0);

            if (isGoing)
            {
                if (Speed == 0)
                {
                    isGoing = false;
                    StartCoroutine(GoToTarget());
                }
            }
        }
    }

    private void LateUpdate()
    {
        
    }
    IEnumerator GoToTarget()
    {       
        agent.SetDestination(targets[currentNum].position);
        AddCurrentNum();

        yield return new WaitForEndOfFrame();        
    }

    void AddCurrentNum()
    {        
        currentNum++;

        if (currentNum >= targets.Length)
        {
                currentNum = 0;
        }          
    }           
}
