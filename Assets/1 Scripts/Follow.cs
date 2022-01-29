using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
        // Ÿ�� �÷��̾� ���� �Ŵ������� �Ҵ�
        target = GameManager.Instance.player.transform;
    }

    void Update()
    {
        transform.position = target.position + offset; // ī�޶� �̵�
    }
}
