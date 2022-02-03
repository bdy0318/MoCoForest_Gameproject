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
        // ���ı� ������Ʈ �Ҵ�
        player = GameManager.Instance.player;
        quest = GameManager.Instance.quest;
    }

    // �κ��丮 ��ư ���̱�, �κ��丮 �ݱ�
    public void ShowBtn()
    {
        // �������� ������
        if(isItem)
            itemList[index].SetActive(false);
        // �Ǹſ����� �� ��찡 �ƴϸ�
        if (!shop.isSell)
            btnInventory.SetActive(true);
        else
        {
            shop.CloseSellAnswer();
            if (isSelectedItem)
            {
                shop.ItemSellCount(player.hasItem[index]);
                shop.itemIndex = index; // ���� ������ �ε���
                shop.price = (int)(shop.itemPrice[index] * 0.8); // ���� ������ ����
            }
            else
            {
                shop.NotSellItem(); // �ƹ� �����۵� �Ǹ����� ����
            }
            player.isInventory = false;
        }
        panelInventroy.SetActive(false);
        isSelectedItem = false;
        if(Input.GetMouseButtonUp(0))
            player.isInventory = false;
    }
    // �κ��丮 ����, �κ��丮 ��ư �ݱ�
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
        // ���� �������� �ִ� ���
        if (isItem)
        {
            Button rb = rightBtn.GetComponent<Button>();
            rb.Select();
            leftBtn.GetComponent<Button>().interactable = true;
            rightBtn.GetComponent<Button>().interactable = true;
            selectButton.interactable = true;
            InventoryRight();
        }
        // ���� �������� ���� ���
        else
        {
            leftBtn.GetComponent<Button>().interactable = false;
            rightBtn.GetComponent<Button>().interactable = false;
            selectButton.interactable = false;
            itemName.text = "������ �ִ� �������� �����ϴ�!";
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
    // �κ��丮 ���� ��ư Ŭ��
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
    // �κ��丮 �� ��ư Ŭ��
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
    // �κ��丮 ������ ����
    public void SelectItem()
    {
        if (!shop.isSell)
            player.selectItem = itemList[index];
        isSelectedItem = true;

        ShowBtn();
    }
}
