using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectDetector : MonoBehaviour
{
    [System.Serializable]
    public class RaycastEvent : UnityEvent<Transform> { }

    [HideInInspector]
    public RaycastEvent raycastEvent = new RaycastEvent();

    Camera mainCamera;      // 광선 생성을 위한 Camera  
    Ray ray;                // 생성된 광선 정보 저장 
    RaycastHit hit;         // 광선에 부딪힌 오브젝트 정보 저장 

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성 
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //광선에 부딪히는 오브젝트를 검출해서 hit에 저장 
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //부딪힌 오브젝트의 Transform 정보를 매개변수로 이벤트 호출 
                raycastEvent.Invoke(hit.transform);
            }
        }
    }

}
