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
    public GameObject[] saveLoadManagerList;

    Player player;
    Quest quest;

    private void Start()
    {
        // 파괴 오브젝트 할당
        // 플레이어
        player = GameManager.Instance.player;
        player.shop = playerList[0].GetComponent<Shop>();
        player.inventory = playerList[1].GetComponent<Inventory>();

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
        GameManager.Instance.coinPanel = gameManagerList[0];
        GameManager.Instance.playerCoinTxt = gameManagerList[1].GetComponent<Text>();
        GameManager.Instance.hammer = gameManagerList[2];
        GameManager.Instance.handGun = gameManagerList[3];

        // 세이브 로드 매니저
        for (int i = 0; i < saveLoadManagerList.Length; i++)
            SaveLoadManager.Instance.deco[i] = saveLoadManagerList[i].GetComponent<Deco>();
    }
}
