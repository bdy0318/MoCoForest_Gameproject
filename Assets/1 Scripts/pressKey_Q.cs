using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pressKey_Q : MonoBehaviour
{
    //QŰ ��Ÿ����
    public GameObject pressQ;

    // ���� ���̺� ������ �ٰ����� �� QŰ�� �������� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(showQ());
        }
    }

    IEnumerator showQ()
    {
        pressQ.SetActive(true);
        yield return new WaitForSeconds(10);
        pressQ.SetActive(false);
    }

}
