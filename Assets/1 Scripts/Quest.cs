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

    public string[] questList; // 퀘스트 목록
    public int nowQuest;
    public bool isComplete;
    public bool isMapChanged;
    public bool isGameWin;

    public int thirdQuest; // 세번째 퀘스트 마을 꾸미기 진행도 표시
    public GameObject Decoration; //세번째 퀘스트 시작할 때 꾸미기 기능 활성화
    public TextMesh QuestIcon; //물음표 아이콘

    // 퀘스트 목록 변경
    public void ChangeQuestList()
    {
        // 현재 받은 퀘스트가 없는 경우
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
    // 다음 퀘스트 부여
    public void NextQuest()
    {
        nowQuest++;
        isComplete = false;
        isGameWin = false;
        ChangeQuestList();
    }

    // 현재 NPC가 퀘스트를 주는 NPC인지 판단
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

    // 각 npc와 대화 시 실행
    public void FirstQuest()
    {
        // 퀘스트 완료
        if(nowQuest == 1 && player.stone >= 3 && !isComplete)
        {
            talkManager.talkData[1000] = npcTalk.completeTalk0;
            player.stone -= 3;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[1].transform.position + new Vector3(0, 3, 0);

        }
        // 퀘스트 받기 전
        else if (nowQuest == 0)
        {
            talkManager.talkData[1000] = npcTalk.questTalk0;
            NextQuest();
        }
    }

    public void SecondQuest()
    {
        // 퀘스트 완료 하기 쉽게 임시로 얘도 돌 3개로 바꿨음. --> 원래대로
        //if (nowQuest == 2 && player.stone >= 3 && !isComplete)
        if (nowQuest == 2 && !isComplete && player.selectItem)
        {
            talkManager.talkData[2000] = npcTalk.completeTalk1;
            player.hasItem[player.selectItem.GetComponent<Item>().value]--; // 선물한 아이템 인벤토리에서 제거
            //player.stone -= 3;
            player.selectItem = null;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[2].transform.position + new Vector3(0, 3, 0);

        }
        // 퀘스트 받기 전
        else if (nowQuest == 1 && isComplete)
        {
            talkManager.talkData[2000] = npcTalk.questTalk1;
            NextQuest();
        }

        //// 퀘스트 완료
        //if (nowQuest == 2 && !isComplete && player.selectItem) // player.selectedItem -> 선물하면
        //{
        //    talkManager.talkData[2000] = npcTalk.completeTalk1;
        //    player.hasItem[player.selectItem.GetComponent<Item>().value]--; // 선물한 아이템 인벤토리에서 제거
        //    player.selectItem = null;
        //    isComplete = true;
        //    ChangeQuestList();
        //}
        //// 퀘스트 받기 전
        //else if (nowQuest == 1 && isComplete) {
        //    talkManager.talkData[2000] = npcTalk.questTalk1;
        //    NextQuest();
        //}
    }

    public void ThirdQuest()
    {
        // 퀘스트 완료
        if (nowQuest == 3 && !isComplete && thirdQuest == 3)
        {
            talkManager.talkData[3000] = npcTalk.completeTalk2;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[3].transform.position + new Vector3(0, 3, 0);

        }
        // 퀘스트 받기 전
        else if (nowQuest == 2 && isComplete)
        {
            talkManager.talkData[3000] = npcTalk.questTalk2;
            NextQuest();
            thirdQuest = 0;
            Decoration.SetActive(true); //마을 꾸미기 기능 활성화
        }
    }

    public void FourthQuest()
    {
        // 퀘스트 완료
        if (nowQuest == 4 && !isComplete && true) // true -> 몇점 이상 달성하면
        {
            talkManager.talkData[4000] = npcTalk.completeTalk3;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[4].transform.position + new Vector3(0, 3, 0);

        }
        else if (nowQuest == 4 && !isComplete) // 게임 진행
        {
            // 게임으로 이동
        }
        // 퀘스트 받기 전
        else if (nowQuest == 3 && isComplete)
        {
            talkManager.talkData[4000] = npcTalk.questTalk3;
            NextQuest();
        }
    }

    public void FifthQuest()
    {
        // 퀘스트 완료
        if (nowQuest == 5 && !isComplete && isGameWin)
        {
            talkManager.talkData[5000] = npcTalk.completeTalk4;
            isComplete = true;
            ChangeQuestList();
            QuestIcon.transform.position = npc[5].transform.position + new Vector3(0, 3, 0);

        }
        // 퀘스트 진행 중
        else if (nowQuest == 5 && !isComplete)
        {
            talkManager.talkData[5000] = npcTalk.questTalk4;
        }
        // 퀘스트 받기 전
        else if (nowQuest == 4 && isComplete)
        {
            talkManager.talkData[5000] = npcTalk.questTalk4;
            NextQuest();
        }
    }

    public void SixthQuest()
    {
        // 퀘스트 완료
        if (nowQuest == 6 && !isComplete && true) // true -> 괴물 물리치면
        {
            talkManager.talkData[6000] = npcTalk.completeTalk5;
            isComplete = true;
        }
        else if (nowQuest == 6 && !isComplete)
        {
            talkManager.talkData[6000] = npcTalk.questTalk5;
        }
        // 퀘스트 받기 전
        else if (nowQuest == 5 && isComplete)
        {
            talkManager.talkData[6000] = npcTalk.questTalk5;
            NextQuest();
        }
    }

    // 퀘스트 장면 전환
    public void QuestScene()
    {
        // 5번 퀘스트 진행 시 카트레이싱
        if(nowQuest == 5)
            SceneManager.LoadScene("CartRacing");
    }

    // 5번 퀘스트 실패 시 대사 출력
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

    // 초 대기 후 대사 종료
    IEnumerator Second()
    {
        yield return new WaitForSeconds(2f);
        npc[4].NpcPannel.SetActive(false);
        player.isTalking = false;
    }
}
