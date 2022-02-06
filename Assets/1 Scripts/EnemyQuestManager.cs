using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyQuestManager : MonoBehaviour
{
    public Enemy enemyA;
    public Enemy enemyB;
    public Enemy enemyC;

    void Update()
    {
        // 6번 퀘스트 시작 시 적 리스폰
        if (GameManager.Instance.quest.nowQuest == 6 && !GameManager.Instance.quest.sixthQuest[0])
        {
            enemyA.gameObject.SetActive(true);
            enemyA.Invoke("ChaseStart", 2);
        }
        if (GameManager.Instance.quest.nowQuest == 6 && !GameManager.Instance.quest.sixthQuest[1])
        {
            enemyB.gameObject.SetActive(true);
            enemyB.Invoke("ChaseStart", 2);
        }
        if (GameManager.Instance.quest.nowQuest == 6 && !GameManager.Instance.quest.sixthQuest[2])
        {
            enemyC.gameObject.SetActive(true);
            enemyC.Invoke("ChaseStart", 2);
        }
    }
}
