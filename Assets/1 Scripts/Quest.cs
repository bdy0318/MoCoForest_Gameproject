using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public Inventory inventory;
    public Player player;
    public string[] questList;
    public int nowQuest;
    public int thirdQuest; // 세번째 퀘스트 마을 꾸미기 진행도 표시
    public bool isComplete;

    public void ChangeQuestList()
    {
        if(nowQuest == 3)
        {
            inventory.questText.text = string.Format(questList[nowQuest-1].ToString(), thirdQuest);
        }

        inventory.questText.text = questList[nowQuest-1].ToString();
    }

    public void NextQuest()
    {
        nowQuest++;
        isComplete = false;
        ChangeQuestList();
    }

    // 각 npc와 대화 시 실행
    public void FirstQuest()
    {
        if(nowQuest == 1 && player.stone >= 3 && !isComplete)
        {
            player.stone -= 3;
            isComplete = true;
        }
        else if (nowQuest == 0 && isComplete)
        {
            NextQuest();
        }
    }

    public void SecondQuest()
    {
        if(nowQuest == 2 && !isComplete && true) //true -> 선물 주면
        {
            player.hasItem[player.selectItem.GetComponent<Item>().value]--;
            isComplete = true;
        }
        else if(nowQuest == 1 && isComplete) {
            NextQuest();
        }
    }

    public void ThirdQuest()
    {
        if (nowQuest == 3 && !isComplete && thirdQuest >= 3)
        {
            isComplete = true;
        }
        else if (nowQuest == 2 && isComplete)
        {
            NextQuest();
        }
    }

    public void FourthQuest()
    {
        if (nowQuest == 4 && !isComplete && true) // true -> 몇점 이상 달성하면
        {
            isComplete = true;
        }
        else if (nowQuest == 3 && isComplete)
        {
            NextQuest();
        }
    }

    public void FifthQuest()
    {
        if (nowQuest == 5 && !isComplete && true) // true -> 몇점 이상 달성하면
        {
            isComplete = true;
        }
        else if (nowQuest == 4 && isComplete)
        {
            NextQuest();
        }
    }

    public void SixthQuest()
    {
        if (nowQuest == 6 && !isComplete && true) // true -> 괴물 물리치면
        {
            isComplete = true;
            // 엔딩으로 연결
        }
        else if (nowQuest == 5 && isComplete)
        {
            NextQuest();
        }
    }
}
