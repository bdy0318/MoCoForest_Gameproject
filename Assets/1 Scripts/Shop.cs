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
        // ���� �Ǹ� ��ȭâ�� ���
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
    // ���� ������ ��ȣ �ۿ� ��
    public void Buy(int index)
    {
        itemIndex = index;
        price = itemPrice[itemIndex];
        string name = itemName[itemIndex];

        TalkBuy(name, price);
    }
    // ���� ������ �Ǹ� ����
    public void Sell()
    {
        isSell = true;
        //npc ���, ī�޶�
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
    // ������ �Ǹ� ����
    public void ItemSellCount(int max)
    {
        NPCText.text = talkData[8];
        sellCountPanel.SetActive(true);
        slider.value = 0;
        slider.minValue = 0;
        slider.maxValue = max;
        slider.Select();
        itemCount.text = "������ �Ǹ��ҷ�!";
        priceCount.text = "0";
    }
    // �����̴� ������ �Ǹ� ���� ����
    public void ItemCountChanged()
    {
        if(slider.value == 0)
        {
            itemCount.text = "������ �Ǹ��ҷ�!";
        }
        else
            itemCount.text = slider.value.ToString();
        priceCount.text = (slider.value * price).ToString();
    }
    // �Ǹ��� ������ ����
    public void SellSelect()
    {
        // 0�� ���� ��
        if (slider.value == 0)
        {
            NPCText.text = talkData[9];
        }
        else
        {
            // ������
            if (itemIndex == -1)
            {
                enterPlayer.stone -= (int)slider.value;
            }
            // ���� ������
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
    // ���� ������ �Ǹ� �� �ϴ� ���
    public void NotSellItem()
    {
        NPCText.text = talkData[9];
        if (Input.GetMouseButtonUp(0))
        {
            isNext = true;
            isClose = true;
        }
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

    // ������ �Ǹ� ���� ��
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
    // �κ��丮 �Ǹ� ���� ��
    public void AnswerSellInventory()
    {
        enterPlayer.inventory.ShowInventory();
        if (Input.GetMouseButtonUp(0))
        {
            CloseSellAnswer();
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
    // �Ǹ� ������ ������ ǥ��
    public void ShowSellAnswer()
    {
        isNext = false;
        Button[] btn = sellChoosePanel.GetComponentsInChildren<Button>();
        btn[0].Select();
        sellChoosePanel.SetActive(true);
    }

    // ������ ��ư �ݱ�
    public void CloseAnswer()
    {
        answerPanel.SetActive(false);
    }
    // �Ǹ� ������ ������ ��ư �ݱ�
    public void CloseSellAnswer()
    {
        sellChoosePanel.SetActive(false);
    }

    // E key ��ġ �� ǥ��
    public void SetEPosition(Collider other)
    {
        // ���� ���� ��
        if(!camControl.shoppingCam.activeSelf && !camControl.closeupCam.activeSelf)
            showKeyE.transform.position = Camera.main.WorldToScreenPoint(other.transform.position + new Vector3(-1f, 1.5f, 0));
        // ���� ���� ��
        else if (camControl.shoppingCam.activeSelf)
        {
            showKeyE.transform.position = camControl.shoppingCam.GetComponent<Camera>().WorldToScreenPoint(other.transform.position + new Vector3(0, 1.7f, 0));
        }
        
        showKeyE.SetActive(true);
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
