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

    // 상점 입장
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
    // 상점 종료
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
    // npc 대화창 닫기
    public void Close()
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
        enterPlayer.isTalking = false;
        enterPlayer.isShopping = true;
        isNext = false;
        isClose = false;
        
        // 상점 입장 대화창의 경우
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
    // 상점 아이템 상호 작용 시
    public void Buy(int index)
    {
        itemIndex = index;
        price = itemPrice[itemIndex];
        string name = itemName[itemIndex];

        TalkBuy(name, price);
    }
    // 구매 확인 선택 시
    public void AnswerYes()
    {
        if (price > enterPlayer.coin)
        {
            TalkCoin();
        }
        else
        {
            // 골드 차감
            enterPlayer.coin -= price;
            // 소지 아이템 증가(아이템 value 순 { Column, Light, Book, Bottle, Candle, Jug, Pot })
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
    // 구매 취소 선택 시
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
    // 대화창 표시
    public void TalkUp()
    {
        uiGroup.anchoredPosition = Vector3.down * 330;
    }
    // 아이템 살지 여부 선택지 버튼 표시
    public void ShowAnswer()
    {
        isNext = false;
        Button[] btn = answerPanel.GetComponentsInChildren<Button>();
        btn[0].Select();
        answerPanel.SetActive(true);
    }
    // 선택지 버튼 닫기
    public void CloseAnswer()
    {
        answerPanel.SetActive(false);
    }
    // 상점 입장시 인사
    public void TalkHello()
    {
        NPCText.text = talkData[0];
        enterPlayer.isTalking = true;
        isNext = true;
    }
    // 소지 금액 부족한 경우
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
    // 상점 구매 진행시 npc 대사 출력
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
    // 상점 퇴장 시 npc 대사 출력
    IEnumerator TalkBye()
    {
        NPCText.text = talkData[5];
        yield return new WaitForSeconds(1.5f);
        Close();
        enterPlayer.isShopping = false;
    }
}
