using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject dialogue;
    public GameObject scanObject;

    public Text talkText;
    public bool isAction;
    public bool iDown;
    public int talkIndex;

    public void Action(GameObject scanObj)
    {
        if (isAction) //Exit Action 
        {
            isAction = false;
        }
        else //Enter Action
        {
            isAction = true;
            scanObject = scanObj;
            ObjData objData = scanObject.GetComponent<ObjData>();
            Talk(objData.id, objData.isNpc);
        }
        dialogue.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        talkText.text = talkData;
    }
}
