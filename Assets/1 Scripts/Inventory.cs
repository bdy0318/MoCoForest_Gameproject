using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject btnInventory;
    public GameObject panelInventroy;
    public Player player;
    public Quest quest;
    public Shop shop;
    // UI
    public Text Stone;
    public Text Coin;
    public Button closeButton;
    public GameObject leftBtn;
    public GameObject rightBtn;
    public Button selectButton;
    public Text itemName;
    public Text itemCount;
    public Text questText;
    public GameObject[] itemList;

    bool isItem;
    public bool isSelectedItem;
    int index;
    int length;

    private void Start()
    {
        // 비파괴 오브젝트 할당
        player = GameManager.Instance.player;
        quest = GameManager.Instance.quest;
    }

    // 인벤토리 버튼 보이기, 인벤토리 닫기
    public void ShowBtn()
    {
        // 아이템이 있으면
        if(isItem)
            itemList[index].SetActive(false);
        // 판매용으로 연 경우가 아니면
        if (!shop.isSell)
            btnInventory.SetActive(true);
        else
        {
            shop.CloseSellAnswer();
            if (isSelectedItem)
            {
                shop.ItemSellCount(player.hasItem[index]);
                shop.itemIndex = index; // 선택 아이템 인덱스
                shop.price = (int)(shop.itemPrice[index] * 0.8); // 선택 아이템 가격
            }
            else
            {
                shop.NotSellItem(); // 아무 아이템도 판매하지 않음
            }
            player.isInventory = false;
        }
        panelInventroy.SetActive(false);
        isSelectedItem = false;
        if(Input.GetMouseButtonUp(0))
            player.isInventory = false;
    }
    // 인벤토리 열기, 인벤토리 버튼 닫기
    public void ShowInventory()
    {
        length = player.hasItem.Length;
        index = -1;
        isItem = false;
        for (int i = 0; i < player.hasItem.Length; i++)
        {
            if (player.hasItem[i] != 0)
            {
                isItem = true;
                break;
            }
        }
        // 소지 아이템이 있는 경우
        if (isItem)
        {
            Button rb = rightBtn.GetComponent<Button>();
            rb.Select();
            leftBtn.GetComponent<Button>().interactable = true;
            rightBtn.GetComponent<Button>().interactable = true;
            selectButton.interactable = true;
            InventoryRight();
        }
        // 소지 아이템이 없는 경우
        else
        {
            leftBtn.GetComponent<Button>().interactable = false;
            rightBtn.GetComponent<Button>().interactable = false;
            selectButton.interactable = false;
            itemName.text = "가지고 있는 아이템이 없습니다!";
            itemCount.text = "0";
            closeButton.Select();
        }

        Stone.text = player.stone.ToString();
        Coin.text = player.coin.ToString();
        btnInventory.SetActive(false);
        panelInventroy.SetActive(true);
        player.isInventory = true;
        quest.ChangeQuestList();
    }
    // 인벤토리 오른 버튼 클릭
    public void InventoryRight()
    {
        if(index != -1)
        {
            itemList[index].SetActive(false);
        }

        index = (index+1)%length;
        if(player.hasItem[index] != 0)
        {
            itemList[index].SetActive(true);
            itemName.text = itemList[index].GetComponent<Item>().itemName;
            itemCount.text = player.hasItem[index].ToString();
        }
        else
        {
            InventoryRight();
        }
    }
    // 인벤토리 왼 버튼 클릭
    public void InventoryLeft()
    {
        itemList[index].SetActive(false);
        if (index == 0)
            index = length;
        index--;
        if (player.hasItem[index] != 0)
        {
            itemList[index].SetActive(true);
            itemName.text = itemList[index].GetComponent<Item>().itemName;
            itemCount.text = player.hasItem[index].ToString();
        }
        else
        {
            InventoryLeft();
        }
    }
    // 인벤토리 아이템 선택
    public void SelectItem()
    {
        if (!shop.isSell)
            player.selectItem = itemList[index];
        isSelectedItem = true;

        ShowBtn();
    }
}
