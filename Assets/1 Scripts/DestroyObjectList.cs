using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyObjectList : MonoBehaviour
{
    // 파괴 리스트
    public GameObject[] playerList;
    public GameObject[] questList;
    public GameObject[] gameManagerList;

    Player player;
    Quest quest;

    private void Start()
    {
        // 파괴 오브젝트 할당
        // 플레이어
        player = GameManager.Instance.player;
        player.shop = playerList[0].GetComponent<Shop>();
        player.inventory = playerList[1].GetComponent<Inventory>();
        player.helpButton = playerList[2].GetComponent<HelpButton>();

        // 퀘스트
        quest = GameManager.Instance.quest;
        quest.inventory = questList[0].GetComponent<Inventory>();
        for (int i = 0; i < 6; i++)
        {
            quest.npc[i] = questList[i + 1].GetComponent<NPC>();
        }
        quest.endingManager = questList[7].GetComponent<Ending>();
        quest.Decoration = questList[8];
        quest.QuestIcon = questList[9].GetComponent<TextMesh>();

        // 게임 매니저
        if (GameManager.Instance.playerPos != new Vector3(0, 999, 0))
            player.transform.position = GameManager.Instance.playerPos;
        if (GameManager.Instance.questIconPos != new Vector3(999, 0, 0))
            quest.QuestIcon.transform.position = GameManager.Instance.questIconPos;
        GameManager.Instance.coinPanel = gameManagerList[0];
        GameManager.Instance.playerCoinTxt = gameManagerList[1].GetComponent<Text>();
    }
}
