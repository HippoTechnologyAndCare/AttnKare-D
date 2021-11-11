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
    Transform target;
    [SerializeField]
    float speed;

    public float Speed { get => speed; set => speed = value; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (Speed >= 0.05f)
        {
            animator.SetInteger("Status", 1);
        }

        else if (Speed < 0.05f)
        {
            animator.SetInteger("Status", 0);
        }
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }
        agent.SetDestination(target.position);
    }
}
