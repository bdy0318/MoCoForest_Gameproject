using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public Text NPCText;
    public GameObject answerPanel;

    public GameObject[] itemObj;
    public string[] itemName;
    public int[] itemPrice;
    public string[] talkData;
    public bool isNext;
    public bool isClose;
    public bool isEnter;

    int price;

    Player enterPlayer;

    public void Enter(Player player)
    {
        StopAllCoroutines();
        enterPlayer = player;
        isEnter = true;
        TalkUp();
        TalkHello();
    }

    public void Exit()
    {
        StopAllCoroutines();
        TalkUp();
        isNext = false;
        enterPlayer.isShopping = false;
        StartCoroutine(TalkBye());
    }

    public void Close()
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
        enterPlayer.isTalking = false;
        enterPlayer.isShopping = true;
        isNext = false;
        isClose = false;
        isEnter = false;
    }

    public void Buy(int index)
    {
        price = itemPrice[index];
        string name = itemName[index];

        TalkBuy(name, price);
    }

    public void AnswerYes()
    {
        if (price > enterPlayer.coin)
        {
            TalkCoin();
        }
        else
        {
            CloseAnswer();
            enterPlayer.coin -= price;
            NPCText.text = talkData[6];
            isNext = true;
            isClose = true;
        }
    }

    public void AnswerNo()
    {
        CloseAnswer();
        NPCText.text = talkData[4];
        enterPlayer.isTalking = true;
        isNext = true;
        isClose = true;
    }

    public void TalkUp()
    {
        uiGroup.anchoredPosition = Vector3.down * 330;
    }
    public void ShowAnswer()
    {
        answerPanel.SetActive(true);
    }

    public void CloseAnswer()
    {
        answerPanel.SetActive(false);
    }

    public void TalkHello()
    {
        NPCText.text = talkData[0];
        enterPlayer.isTalking = true;
        isNext = true;
    }
    public void TalkCoin()
    {
        CloseAnswer();
        NPCText.text = talkData[1];
        enterPlayer.isTalking = true;
        isNext = true;
        isClose = true;
    }

    public void TalkBuy(string name, int price)
    {
        if(name == "단지" || name == "수상한 솥단지")
            NPCText.text = string.Format(talkData[3], name, price);
        else
            NPCText.text = string.Format(talkData[2], name, price);
        TalkUp();
        enterPlayer.isTalking = true;
        isNext = true;
    }

    IEnumerator TalkBye()
    {
        NPCText.text = talkData[5];
        yield return new WaitForSeconds(1.5f);
        Close();
        enterPlayer.isShopping = false;
    }
}
