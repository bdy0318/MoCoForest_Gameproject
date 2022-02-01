using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Quest : MonoBehaviour
{
    #region Singleton
    public static Quest instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton
    public Inventory inventory;
    public Player player;
    public TalkManager talkManager;
    public Ending endingManager;
    public QuestTalk npcTalk;
    public List<NPC> npc;

    public string[] questList; // ����Ʈ ���
    public int nowQuest;
    public bool isComplete;
    public bool isMapChanged;
    public bool isGameWin;

    public int thirdQuest; // ����° ����Ʈ ���� �ٹ̱� ���൵ ǥ��
    public GameObject Decoration; //����° ����Ʈ ������ �� �ٹ̱� ��� Ȱ��ȭ
    public TextMesh QuestIcon; //����ǥ ������

    // ����Ʈ ��� ����
    public void ChangeQuestList()
    {
        // ���� ���� ����Ʈ�� ���� ���
        if (isComplete || nowQuest == 0)
            inventory.questText.text = "";
        else
        {
            if (nowQuest == 3)
            {
                inventory.questText.text = string.Format(questList[nowQuest - 1], thirdQuest);
            }
            else
                inventory.questText.text = questList[nowQuest - 1];
        }

    }
    // ���� ����Ʈ �ο�
    public void NextQuest()
    {
        nowQuest++;
        isComplete = false;
        isGameWin = false;
        ChangeQuestList();
    }

    // ���� NPC�� ����Ʈ�� �ִ� NPC���� �Ǵ�
    public void IsQuestNPC(int id)
    {
        player.isTalking = true;
        switch (id) {
            case 1000: FirstQuest(); break;
            case 2000: SecondQuest(); break;
            case 3000: ThirdQuest(); break;
            case 4000: FourthQuest(); break;
            case 5000: FifthQuest(); break;
            case 6000: SixthQuest(); break;
        }
    } 

    // �� npc�� ��ȭ �� ����
    public void FirstQuest()
    {
        // ����Ʈ �Ϸ�
        if(nowQuest == 1 && player.stone >= 3 && !isComplete)
        {
            talkManager.talkData[1000] = npcTalk.completeTalk0;
            player.stone -= 3;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[1].transform.position + new Vector3(0, 3, 0);

        }
        // ����Ʈ �ޱ� ��
        else if (nowQuest == 0)
        {
            talkManager.talkData[1000] = npcTalk.questTalk0;
            NextQuest();
        }
    }

    public void SecondQuest()
    {
        // ����Ʈ �Ϸ� �ϱ� ���� �ӽ÷� �굵 �� 3���� �ٲ���. --> �������
        //if (nowQuest == 2 && player.stone >= 3 && !isComplete)
        if (nowQuest == 2 && !isComplete && player.selectItem)
        {
            talkManager.talkData[2000] = npcTalk.completeTalk1;
            player.hasItem[player.selectItem.GetComponent<Item>().value]--; // ������ ������ �κ��丮���� ����
            //player.stone -= 3;
            player.selectItem = null;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[2].transform.position + new Vector3(0, 3, 0);

        }
        // ����Ʈ �ޱ� ��
        else if (nowQuest == 1 && isComplete)
        {
            talkManager.talkData[2000] = npcTalk.questTalk1;
            NextQuest();
        }

        //// ����Ʈ �Ϸ�
        //if (nowQuest == 2 && !isComplete && player.selectItem) // player.selectedItem -> �����ϸ�
        //{
        //    talkManager.talkData[2000] = npcTalk.completeTalk1;
        //    player.hasItem[player.selectItem.GetComponent<Item>().value]--; // ������ ������ �κ��丮���� ����
        //    player.selectItem = null;
        //    isComplete = true;
        //    ChangeQuestList();
        //}
        //// ����Ʈ �ޱ� ��
        //else if (nowQuest == 1 && isComplete) {
        //    talkManager.talkData[2000] = npcTalk.questTalk1;
        //    NextQuest();
        //}
    }

    public void ThirdQuest()
    {
        // ����Ʈ �Ϸ�
        if (nowQuest == 3 && !isComplete && thirdQuest == 3)
        {
            talkManager.talkData[3000] = npcTalk.completeTalk2;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[3].transform.position + new Vector3(0, 3, 0);

        }
        // ����Ʈ �ޱ� ��
        else if (nowQuest == 2 && isComplete)
        {
            talkManager.talkData[3000] = npcTalk.questTalk2;
            NextQuest();
            thirdQuest = 0;
            Decoration.SetActive(true); //���� �ٹ̱� ��� Ȱ��ȭ
        }
    }

    public void FourthQuest()
    {
        // ����Ʈ �Ϸ�
        if (nowQuest == 4 && !isComplete && true) // true -> ���� �̻� �޼��ϸ�
        {
            talkManager.talkData[4000] = npcTalk.completeTalk3;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[4].transform.position + new Vector3(0, 3, 0);

        }
        else if (nowQuest == 4 && !isComplete) // ���� ����
        {
            // �������� �̵�
        }
        // ����Ʈ �ޱ� ��
        else if (nowQuest == 3 && isComplete)
        {
            talkManager.talkData[4000] = npcTalk.questTalk3;
            NextQuest();
        }
    }

    public void FifthQuest()
    {
        // ����Ʈ �Ϸ�
        if (nowQuest == 5 && !isComplete && isGameWin)
        {
            talkManager.talkData[5000] = npcTalk.completeTalk4;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[5].transform.position + new Vector3(0, 3, 0);

        }
        // ����Ʈ ���� ��
        else if (nowQuest == 5 && !isComplete)
        {
            talkManager.talkData[5000] = npcTalk.questTalk4;
        }
        // ����Ʈ �ޱ� ��
        else if (nowQuest == 4 && isComplete)
        {
            talkManager.talkData[5000] = npcTalk.questTalk4;
            NextQuest();
        }
    }

    public void SixthQuest()
    {
        // ����Ʈ �Ϸ�
        if (nowQuest == 6 && !isComplete && true) // true -> ���� ����ġ��
        {
            talkManager.talkData[6000] = npcTalk.completeTalk5;
            isComplete = true;
        }
        else if (nowQuest == 6 && !isComplete)
        {
            talkManager.talkData[6000] = npcTalk.questTalk5;
        }
        // ����Ʈ �ޱ� ��
        else if (nowQuest == 5 && isComplete)
        {
            talkManager.talkData[6000] = npcTalk.questTalk5;
            NextQuest();
        }
    }

    // ����Ʈ ��� ��ȯ
    public void QuestScene()
    {
        // 5�� ����Ʈ ���� �� īƮ���̽�
        if(nowQuest == 5)
            SceneManager.LoadScene("CartRacing");
    }

    // 5�� ����Ʈ ���� �� ��� ���
    public void FifthFailed()
    {
        player.isTalking = true;
        npc[4].pressE.SetActive(false);
        talkManager.talkData[5000] = npcTalk.FailedTalk4;
        npc[4].nameText.text = npc[4].Name;
        npc[4].pressE.SetActive(false);
        npc[4].DialogueText.text = talkManager.talkData[5000][0];
        npc[4].NpcPannel.SetActive(true);
        StartCoroutine(Second());
    }

    // �� ��� �� ��� ����
    IEnumerator Second()
    {
        yield return new WaitForSeconds(2f);
        npc[4].NpcPannel.SetActive(false);
        player.isTalking = false;
    }
}
