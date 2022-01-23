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
    public Player player;

    void Update()
    {
        //E키를 눌러 npc와 대화
        if (Input.GetKeyDown(KeyCode.E) && nearNpc)
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
