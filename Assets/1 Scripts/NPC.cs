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
        // ���ı� ������Ʈ �Ҵ�
        quest = GameManager.Instance.quest;
        talkManager = GameManager.Instance.talkManager;
    }

    void Update()
    {
        //EŰ�� ���� npc�� ��ȭ
        if (Input.GetKeyDown(KeyCode.E) && nearNpc && !quest.endingManager.gameObject.activeSelf)
        {
            if (!quest.player.isTalking)
                quest.IsQuestNPC(id);
                nearNpc = true;     //���ϴ� ���߿��� ��� true. ��ȭ ���� ���� ����
            pressE.SetActive(false);
            talking();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            pressEText.text = "��ȭ�� �Ϸ��� [E]�� ��������";
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
        // ����Ʈ 4�� ���� �� ��ȭ ���� �� �δ��� ��� ����
        if (!isNpcTalking && !quest.isComplete && quest.nowQuest == 4 && id == 4000)
            quest.QuestScene();
        // ����Ʈ 5�� ���� �� ��ȭ ���� �� īƮ���̽� ����
        if (!isNpcTalking && !quest.isComplete && quest.nowQuest == 5 && id == 5000)
            quest.QuestScene();
        // ���� ����
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

        if(talkData == null) //�̾߱� ����
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
