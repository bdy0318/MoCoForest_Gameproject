using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyObjectList : MonoBehaviour
{
    // �ı� ����Ʈ
    public GameObject[] playerList;
    public GameObject[] questList;
    public GameObject[] gameManagerList;

    Player player;
    Quest quest;

    private void Start()
    {
        // �ı� ������Ʈ �Ҵ�
        // �÷��̾�
        player = GameManager.Instance.player;
        player.shop = playerList[0].GetComponent<Shop>();
        player.inventory = playerList[1].GetComponent<Inventory>();
        player.helpButton = playerList[2].GetComponent<HelpButton>();

        // ����Ʈ
        quest = GameManager.Instance.quest;
        quest.inventory = questList[0].GetComponent<Inventory>();
        for (int i = 0; i < 6; i++)
        {
            quest.npc[i] = questList[i + 1].GetComponent<NPC>();
        }
        quest.endingManager = questList[7].GetComponent<Ending>();
        quest.Decoration = questList[8];
        quest.QuestIcon = questList[9].GetComponent<TextMesh>();

        // ���� �Ŵ���
        if (GameManager.Instance.playerPos != new Vector3(0, 999, 0))
            player.transform.position = GameManager.Instance.playerPos;
        if (GameManager.Instance.questIconPos != new Vector3(999, 0, 0))
            quest.QuestIcon.transform.position = GameManager.Instance.questIconPos;
        GameManager.Instance.coinPanel = gameManagerList[0];
        GameManager.Instance.playerCoinTxt = gameManagerList[1].GetComponent<Text>();
    }
}
