using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
        // 타겟 플레이어 게임 매니저에서 할당
        target = GameManager.Instance.player.transform;
    }

    void Update()
    {
        transform.position = target.position + offset; // 카메라 이동
    }
}
