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

    Camera mainCamera;      // ���� ������ ���� Camera  
    Ray ray;                // ������ ���� ���� ���� 
    RaycastHit hit;         // ������ �ε��� ������Ʈ ���� ���� 

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ���� ���� 
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //������ �ε����� ������Ʈ�� �����ؼ� hit�� ���� 
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //�ε��� ������Ʈ�� Transform ������ �Ű������� �̺�Ʈ ȣ�� 
                raycastEvent.Invoke(hit.transform);
            }
        }
    }

}
