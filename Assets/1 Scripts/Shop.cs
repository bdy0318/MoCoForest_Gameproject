using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // UI
    public RectTransform uiGroup;
    public Text NPCText;
    public GameObject answerPanel;
    // Item
    public GameObject[] itemObj;
    public string[] itemName;
    public int[] itemPrice;
    public string[] talkData;
    public bool isNext;
    public bool isClose;
    public bool isEnter;
    // Camera
    public ShopCameraControl camControl;
    public GameObject mainCam;

    int price;
    int itemIndex;

    Player enterPlayer;

    // ���� ����
    public void Enter(Player player)
    {
        StopAllCoroutines();
        enterPlayer = player;
        
        camControl.closeupCam.transform.position = mainCam.transform.position;
        camControl.closeupCam.transform.rotation = mainCam.transform.rotation;
        camControl.closeupCam.SetActive(true);
        camControl.isLerping = true;
        isEnter = true;
        TalkUp();
        TalkHello();
    }
    // ���� ����
    public void Exit()
    {
        StopAllCoroutines();
        TalkUp();
        isNext = false;
        enterPlayer.isShopping = false;
        camControl.isShopping = false;
        camControl.shoppingCam.SetActive(false);
        StartCoroutine(TalkBye());
    }
    // npc ��ȭâ �ݱ�
    public void Close()
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
        enterPlayer.isTalking = false;
        enterPlayer.isShopping = true;
        isNext = false;
        isClose = false;
        
        // ���� ���� ��ȭâ�� ���
        if(isEnter)
        {
            camControl.shoppingCam.transform.position = camControl.closeupCam.transform.position;
            camControl.shoppingCam.transform.rotation = camControl.closeupCam.transform.rotation;
            camControl.target.transform.position = camControl.NPC.transform.position;
            camControl.closeupCam.SetActive(false);
            camControl.isShopping = true;
            camControl.isLerping = true;
            camControl.shoppingCam.SetActive(true);
            isEnter = false;
        }
    }
    // ���� ������ ��ȣ �ۿ� ��
    public void Buy(int index)
    {
        itemIndex = index;
        price = itemPrice[itemIndex];
        string name = itemName[itemIndex];

        TalkBuy(name, price);
    }
    // ���� Ȯ�� ���� ��
    public void AnswerYes()
    {
        if (price > enterPlayer.coin)
        {
            TalkCoin();
        }
        else
        {
            // ��� ����
            enterPlayer.coin -= price;
            // ���� ������ ����(������ value �� { Column, Light, Book, Bottle, Candle, Jug, Pot })
            enterPlayer.hasItem[itemIndex] += 1;

            NPCText.text = talkData[6];
            isNext = true;
            if(Input.GetMouseButtonUp(0))
            {
                CloseAnswer();
                isClose = true;
            }  
        }
    }
    // ���� ��� ���� ��
    public void AnswerNo()
    {
        NPCText.text = talkData[4];
        enterPlayer.isTalking = true;
        isNext = true;
        if (Input.GetMouseButtonUp(0))
        {
            CloseAnswer();
            isClose = true;
        }
    }
    // ��ȭâ ǥ��
    public void TalkUp()
    {
        uiGroup.anchoredPosition = Vector3.down * 330;
    }
    // ������ ���� ���� ������ ��ư ǥ��
    public void ShowAnswer()
    {
        isNext = false;
        Button[] btn = answerPanel.GetComponentsInChildren<Button>();
        btn[0].Select();
        answerPanel.SetActive(true);
    }
    // ������ ��ư �ݱ�
    public void CloseAnswer()
    {
        answerPanel.SetActive(false);
    }
    // ���� ����� �λ�
    public void TalkHello()
    {
        NPCText.text = talkData[0];
        enterPlayer.isTalking = true;
        isNext = true;
    }
    // ���� �ݾ� ������ ���
    public void TalkCoin()
    {
        NPCText.text = talkData[1];
        enterPlayer.isTalking = true;
        isNext = true;
        if (Input.GetMouseButtonUp(0))
        {
            CloseAnswer();
            isClose = true;
        }
    }
    // ���� ���� ����� npc ��� ���
    public void TalkBuy(string name, int price)
    {
        if(name == "����" || name == "������ �ܴ���")
            NPCText.text = string.Format(talkData[3], name, price);
        else
            NPCText.text = string.Format(talkData[2], name, price);
        TalkUp();
        enterPlayer.isTalking = true;
        isNext = true;
    }
    // ���� ���� �� npc ��� ���
    IEnumerator TalkBye()
    {
        NPCText.text = talkData[5];
        yield return new WaitForSeconds(1.5f);
        Close();
        enterPlayer.isShopping = false;
    }
}
