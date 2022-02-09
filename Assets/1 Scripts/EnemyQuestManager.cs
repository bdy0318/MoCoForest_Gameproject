using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyQuestManager : MonoBehaviour
{
    public Enemy enemyA;
    public Enemy enemyB;
    public Enemy enemyC;
    public Enemy enemyD;
    public Enemy enemyE;

    void Update()
    {
        // 6번 퀘스트 시작 시 적 리스폰
        if (GameManager.Instance.quest.nowQuest == 6 && !GameManager.Instance.quest.sixthQuest[0])
        {
            enemyA.gameObject.SetActive(true);
            enemyA.gameObject.layer = 3;
            enemyA.Invoke("ChaseStart", 2);
        }
        if (GameManager.Instance.quest.nowQuest == 6 && !GameManager.Instance.quest.sixthQuest[1])
        {
            enemyB.gameObject.SetActive(true);
            enemyB.gameObject.layer = 3;
            enemyB.Invoke("ChaseStart", 2);
        }
        if (GameManager.Instance.quest.nowQuest == 6 && !GameManager.Instance.quest.sixthQuest[2])
        {
            enemyC.gameObject.SetActive(true);
            enemyC.gameObject.layer = 3;
            enemyC.Invoke("ChaseStart", 2);
        }
        if (GameManager.Instance.quest.nowQuest == 6 && !GameManager.Instance.quest.sixthQuest[3])
        {
            enemyD.gameObject.SetActive(true);
            enemyD.gameObject.layer = 3;
            enemyD.Invoke("ChaseStart", 2);
        }
        if (GameManager.Instance.quest.nowQuest == 6 && !GameManager.Instance.quest.sixthQuest[4])
        {
            enemyE.gameObject.SetActive(true);
            enemyE.gameObject.layer = 3;
            enemyE.Invoke("ChaseStart", 2);
        }
    }
}
