using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public Inventory inventory;
    public Player player;
    public string[] questList;
    public int nowQuest;
    public int thirdQuest; // ����° ����Ʈ ���� �ٹ̱� ���൵ ǥ��
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

    // �� npc�� ��ȭ �� ����
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
        if(nowQuest == 2 && !isComplete && true) //true -> ���� �ָ�
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
        if (nowQuest == 4 && !isComplete && true) // true -> ���� �̻� �޼��ϸ�
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
        if (nowQuest == 5 && !isComplete && true) // true -> ���� �̻� �޼��ϸ�
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
        if (nowQuest == 6 && !isComplete && true) // true -> ���� ����ġ��
        {
            isComplete = true;
            // �������� ����
        }
        else if (nowQuest == 5 && isComplete)
        {
            NextQuest();
        }
    }
}
