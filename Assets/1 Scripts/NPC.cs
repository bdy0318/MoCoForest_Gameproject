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
    public Text DialogueText;
    public Text nameText;

    public bool nearNpc;
    public bool isNpcTalking;

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
        if (Input.GetKeyDown(KeyCode.E) && nearNpc == true)
        {
            if(!quest.player.isTalking)
                quest.IsQuestNPC(id);
            pressE.SetActive(false);
            talking();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            nearNpc = true;
            pressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            nearNpc = false;
            pressE.SetActive(false);
        }
    }

    public void talking()
    {
        Talk(id);
        NpcPannel.SetActive(isNpcTalking);
        // ����Ʈ 5�� ���� �� ��ȭ ���� �� īƮ���̽� ����
        if (!isNpcTalking && !quest.isComplete && quest.nowQuest == 5 && id == 5000)
            quest.QuestScene();
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
