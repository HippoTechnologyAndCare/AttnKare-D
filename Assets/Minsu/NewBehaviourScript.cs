using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInterection : MonoBehaviour
{

    //카메라의 정중장에서 광선을 쏘아서 일정거리내에 무언가가 있으면 감지해주는 스크립


    public LayerMask whatIsTarget;
    private Camera playerCam;
    // 위치를 감지하기 위해서는 우선 카메라의 위치를 알고 있어야함

    public float distance = 100f;
    // 카메라로 부터 감지할 거리

    private Transform moveTarget;
    private float targetDistance;



    void Start()
    {
        playerCam = Camera.main;
        // 게임이 시작 되면 현재 활성화된 메인 카메라를 가져온다. 
    }


    void Update()
    {


        Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        // rayOrigin : 광선이 날아갈 정중앙위치 
        // ViewportToWolrdPoint : 카메라의 한 지점의 위치가 게임상에서
        // 어느 위치(가로 중앙, 세로 중, 카메라의 깊이(0))를 리턴
        Vector3 rayDir = playerCam.transform.forward;
        // 광선의 방향 = 카메라의 앞


        Ray ray = new Ray(rayOrigin, rayDir);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);




        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit; // 레이캐스트 광선에 맞은 객체의 정보르 담는 컨테이너


            if (Physics.Raycast(ray, out hit, distance, whatIsTarget))
            // 레이캐스트는 물리처리 이므로 Phisics의 함수
            // Raycast(광선 시작지점, 방향, 거리)
            {

                GameObject hitTarget = hit.collider.gameObject;
                // hit에 부딪힌 콜라이더의 게임오브젝트를 가져온다.
                hitTarget.GetComponent<Renderer>().material.color = Color.red;

                moveTarget = hitTarget.transform;
                targetDistance = hit.distance;


                Debug.Log(hit.collider.gameObject.name);
                Debug.Log("무엇인가 광선에 감지되었다.");
            }
        }


        if (Input.GetMouseButtonUp(0)) // 좌클릭 해제시
        {
            if (moveTarget != null)
            {
                moveTarget.GetComponent<Renderer>().material.color = Color.white;
                // 원래 색인 흰색으로 돌린다. 
            }
            moveTarget = null;
            // 
        }

        if (moveTarget != null)
        {
            moveTarget.position = ray.origin + ray.direction * targetDistance;
            // 무브타겟의 위치는 광선 발사 지점에서 ray 방향으로 targetDistance 앞으로 설정한다. 
        }
    }
}