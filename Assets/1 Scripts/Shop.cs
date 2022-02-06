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
    public GameObject sellChoosePanel;
    public GameObject sellCountPanel;
    public GameObject showKeyE;
    public Slider slider;
    public Text itemCount;
    public Text priceCount;
    // Item
    public GameObject[] itemObj;
    public string[] itemName;
    public int[] itemPrice;
    public string[] talkData;
    public bool isNext;
    public bool isClose;
    public bool isEnter;
    public bool isSell;

    public int price;
    public int itemIndex;
    // Camera
    public ShopCameraControl camControl;
    public GameObject mainCam;
    
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
        // 상점 판매 대화창의 경우
        else if (isSell)
        {
            camControl.shoppingCam.transform.position = camControl.closeupCam.transform.position;
            camControl.shoppingCam.transform.rotation = camControl.closeupCam.transform.rotation;
            camControl.target.transform.position = camControl.NPC.transform.position;
            camControl.closeupCam.SetActive(false);
            camControl.isSelling = false;
            camControl.isLerping = true;
            camControl.shoppingCam.SetActive(true);
            isSell = false;
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
    // 상점 아이템 판매 입장
    public void Sell()
    {
        isSell = true;
        //npc 대사, 카메라
        camControl.closeupCam.transform.position = camControl.shoppingCam.transform.position;
        camControl.closeupCam.transform.rotation = camControl.shoppingCam.transform.rotation;
        camControl.shoppingCam.SetActive(false);
        camControl.isSelling = true;
        camControl.isLerping = true;
        camControl.closeupCam.SetActive(true);
        NPCText.text = talkData[7];
        TalkUp();
        
        enterPlayer.isTalking = true;
        isNext = true;
    }
    // 아이템 판매 개수
    public void ItemSellCount(int max)
    {
        NPCText.text = talkData[8];
        sellCountPanel.SetActive(true);
        slider.value = 0;
        slider.minValue = 0;
        slider.maxValue = max;
        slider.Select();
        itemCount.text = "다음에 판매할래!";
        priceCount.text = "0";
    }
    // 슬라이더 아이템 판매 개수 변경
    public void ItemCountChanged()
    {
        if(slider.value == 0)
        {
            itemCount.text = "다음에 판매할래!";
        }
        else
            itemCount.text = slider.value.ToString();
        priceCount.text = (slider.value * price).ToString();
    }
    // 판매할 아이템 선택
    public void SellSelect()
    {
        // 0개 선택 시
        if (slider.value == 0)
        {
            NPCText.text = talkData[9];
        }
        else
        {
            // 돌맹이
            if (itemIndex == -1)
            {
                enterPlayer.stone -= (int)slider.value;
            }
            // 소지 아이템
            else
            {
                enterPlayer.hasItem[itemIndex] -= (int)slider.value;
            }
            NPCText.text = string.Format(talkData[10], (int)slider.value, (int)slider.value * price);
            enterPlayer.coin += (int)slider.value * price;
        }
        sellCountPanel.SetActive(false);
        if(Input.GetMouseButtonUp(0))
        {
            isNext = true;
            isClose = true;
        }
    }
    // 소지 아이템 판매 안 하는 경우
    public void NotSellItem()
    {
        NPCText.text = talkData[9];
        if (Input.GetMouseButtonUp(0))
        {
            isNext = true;
            isClose = true;
        }
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

    // 돌맹이 판매 선택 시
    public void AnswerSellStone()
    {
        itemIndex = -1;
        price = 100;
        ItemSellCount(enterPlayer.stone);
        if (Input.GetMouseButtonUp(0))
        {
            CloseSellAnswer();
        }
    }
    // 인벤토리 판매 선택 시
    public void AnswerSellInventory()
    {
        enterPlayer.inventory.ShowInventory();
        if (Input.GetMouseButtonUp(0))
        {
            CloseSellAnswer();
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
    // 판매 아이템 선택지 표시
    public void ShowSellAnswer()
    {
        isNext = false;
        Button[] btn = sellChoosePanel.GetComponentsInChildren<Button>();
        btn[0].Select();
        sellChoosePanel.SetActive(true);
    }

    // 선택지 버튼 닫기
    public void CloseAnswer()
    {
        answerPanel.SetActive(false);
    }
    // 판매 아이템 선택지 버튼 닫기
    public void CloseSellAnswer()
    {
        sellChoosePanel.SetActive(false);
    }

    // E key 위치 및 표시
    public void SetEPosition(Collider other)
    {
        // 상점 입장 전
        if(!camControl.shoppingCam.activeSelf && !camControl.closeupCam.activeSelf)
            showKeyE.transform.position = Camera.main.WorldToScreenPoint(other.transform.position + new Vector3(-1f, 1.5f, 0));
        // 상점 입장 후
        else if (camControl.shoppingCam.activeSelf)
        {
            showKeyE.transform.position = camControl.shoppingCam.GetComponent<Camera>().WorldToScreenPoint(other.transform.position + new Vector3(0, 1.7f, 0));
        }
        
        showKeyE.SetActive(true);
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
