using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInterection : MonoBehaviour
{

    //ī�޶��� �����忡�� ������ ��Ƽ� �����Ÿ����� ���𰡰� ������ �������ִ� ��ũ��


    public LayerMask whatIsTarget;
    private Camera playerCam;
    // ��ġ�� �����ϱ� ���ؼ��� �켱 ī�޶��� ��ġ�� �˰� �־����

    public float distance = 100f;
    // ī�޶�� ���� ������ �Ÿ�

    private Transform moveTarget;
    private float targetDistance;



    void Start()
    {
        playerCam = Camera.main;
        // ������ ���� �Ǹ� ���� Ȱ��ȭ�� ���� ī�޶� �����´�. 
    }


    void Update()
    {


        Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        // rayOrigin : ������ ���ư� ���߾���ġ 
        // ViewportToWolrdPoint : ī�޶��� �� ������ ��ġ�� ���ӻ󿡼�
        // ��� ��ġ(���� �߾�, ���� ��, ī�޶��� ����(0))�� ����
        Vector3 rayDir = playerCam.transform.forward;
        // ������ ���� = ī�޶��� ��


        Ray ray = new Ray(rayOrigin, rayDir);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);




        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit; // ����ĳ��Ʈ ������ ���� ��ü�� ������ ��� �����̳�


            if (Physics.Raycast(ray, out hit, distance, whatIsTarget))
            // ����ĳ��Ʈ�� ����ó�� �̹Ƿ� Phisics�� �Լ�
            // Raycast(���� ��������, ����, �Ÿ�)
            {

                GameObject hitTarget = hit.collider.gameObject;
                // hit�� �ε��� �ݶ��̴��� ���ӿ�����Ʈ�� �����´�.
                hitTarget.GetComponent<Renderer>().material.color = Color.red;

                moveTarget = hitTarget.transform;
                targetDistance = hit.distance;


                Debug.Log(hit.collider.gameObject.name);
                Debug.Log("�����ΰ� ������ �����Ǿ���.");
            }
        }


        if (Input.GetMouseButtonUp(0)) // ��Ŭ�� ������
        {
            if (moveTarget != null)
            {
                moveTarget.GetComponent<Renderer>().material.color = Color.white;
                // ���� ���� ������� ������. 
            }
            moveTarget = null;
            // 
        }

        if (moveTarget != null)
        {
            moveTarget.position = ray.origin + ray.direction * targetDistance;
            // ����Ÿ���� ��ġ�� ���� �߻� �������� ray �������� targetDistance ������ �����Ѵ�. 
        }
    }
}