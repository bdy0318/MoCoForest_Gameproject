using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pressKey_Q : MonoBehaviour
{
    //Q키 나타나기
    public GameObject pressQ;

    // 무기 테이블에 가까이 다가갔을 때 Q키를 누르세요 보임
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
