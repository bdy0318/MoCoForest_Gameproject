using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPC : MonoBehaviour
{
    public int id;
    public string Name;
    public int talkIndex;

    public GameObject NpcPannel;
    public GameObject pressE;
    public Text pressEText;
    public Text DialogueText;
    public Text nameText;

    public bool nearNpc;
    public bool isNpcTalking;
    bool finishTalk;

    public TalkManager talkManager;
    public Quest quest;

    private void Start()
    {
        // 비파괴 오브젝트 할당
        quest = GameManager.Instance.quest;
        talkManager = GameManager.Instance.talkManager;
    }

    void Update()
    {
        //E키를 눌러 npc와 대화
        if (Input.GetKeyDown(KeyCode.E) && nearNpc && !quest.endingManager.gameObject.activeSelf)
        {
            if (!quest.player.isTalking)
                quest.IsQuestNPC(id);
                nearNpc = true;     //말하는 도중에는 계속 true. 대화 도중 오류 방지
            pressE.SetActive(false);
            talking();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            pressEText.text = "대화를 하려면 [E]를 누르세요";
            pressE.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            nearNpc = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pressE.SetActive(false);
            nearNpc = false;

        }
    }
    
    public void talking()
    {
        Talk(id);
        NpcPannel.SetActive(isNpcTalking);
        // 퀘스트 4번 진행 시 대화 종료 후 두더지 잡기 시작
        if (!isNpcTalking && !quest.isComplete && quest.nowQuest == 4 && id == 4000)
            quest.QuestScene();
        // 퀘스트 5번 진행 시 대화 종료 후 카트레이싱 시작
        if (!isNpcTalking && !quest.isComplete && quest.nowQuest == 5 && id == 5000)
            quest.QuestScene();
        // 엔딩 진행
        else if(!isNpcTalking && quest.isComplete && quest.nowQuest == 6 && id == 6000)
        {
            quest.endingManager.gameObject.SetActive(true);
            quest.player.isTalking = true;
        }
        nameText.text = Name;
    }

    void Talk(int id)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if(talkData == null) //이야기 끝남
        {
            isNpcTalking = false;
            talkIndex = 0;
            quest.player.isTalking = false;
            return;
        }

        DialogueText.text = talkData;

        isNpcTalking=true;
        talkIndex++;
    }








}
