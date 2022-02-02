using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    [SerializeField]
    private MoleFSM[] moles;    //맵에 존재하는 두더지들
    [SerializeField]
    private float spawnTime;    //두더지 등장 주기

    void Start()
    {
        StartCoroutine("SpawnMole");
    }

    private IEnumerator SpawnMole()
    {
        while (true)
        {
            //0 ~ Moles.Length-1 중 임의의 숫자 선택
            int index = Random.Range(0, moles.Length);
            // index번째 두더지의 상태를 "MoveUp"으로 변경
            moles[index].ChangeState(MoleState.MoveUp);

            //spawnTime 시간동안 대기
            yield return new WaitForSeconds(spawnTime);
        }
    }

   
}
