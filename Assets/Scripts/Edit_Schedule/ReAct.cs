using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BNG;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

// �� ��ũ��Ʈ�� Child ������Ʈ�� �ٿ��� ����ؾ� �մϴ�
public class ReAct : MonoBehaviour
{
    [Header("Auto Insert")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject frame;
    [SerializeField] GameObject getParents;

    [Header("Find a reference and put it in")]
    [SerializeField] GrabbablesInTrigger grInTrigger;

    [SerializeField] private GameObject btnG;

    Rigidbody p_Rigidbody;
    NavMeshAgent p_NavMeshAgent;
    //bool grab;

    void Start()
    {
        player = GameObject.Find("PlayerController");
        frame = transform.Find("Frame").gameObject;
        getParents = transform.parent.gameObject;
        p_Rigidbody = getParents.GetComponent<Rigidbody>();
        p_NavMeshAgent = getParents.GetComponent<NavMeshAgent>();
        //grab = false;
    }

    void Update()
    {
        // Y�ุ ȸ������ target�� �ٶ󺸰� �Ѵ�
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y,
                                                player.transform.position.z);
        transform.LookAt(targetPosition);
        btnG.transform.LookAt(targetPosition);

        // Nav Mesh Agent �۵� ������ RigidbodyConstraints.FreezeAll �Ǿ� �ִ� ����
        // �׷��Ҷ� RigidbodyConstraints.None ���ִ� �ڵ� / None�� �ƴϸ� �׷��Ұ�
        if (grInTrigger.ClosestRemoteGrabbable != null && grInTrigger.ClosestRemoteGrabbable.tag == "Unnecessary") 
        {            
            RemoteConstraints02();
        }       
    }

    public void StartReAct()
    {
        StartCoroutine(ReAct01());
    }
    
    private IEnumerator ReAct01()
    {
        frame.SetActive(true);
        yield return new WaitForSeconds(2f);
        frame.SetActive(false);
    }    

    IEnumerator RemoteConstraints()
    {       
        if (InputBridge.Instance.RightTriggerDown == true)
        {            
            p_Rigidbody.constraints = RigidbodyConstraints.None;
        }

        if (InputBridge.Instance.RightTriggerDown == false)
        {
            p_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        
        yield return null;
    }

    void RemoteConstraints02()
    {
        if (InputBridge.Instance.RightTriggerDown == true)
        {
            p_NavMeshAgent.enabled = false;
            p_Rigidbody.constraints = RigidbodyConstraints.None;
        }                
    }    
}
