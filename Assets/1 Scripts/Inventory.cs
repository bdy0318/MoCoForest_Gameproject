using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject btnInventory;
    public GameObject panelInventroy;
    public Player player;

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
    int index;
    int length;

    public void ShowBtn()
    {
        if(isItem)
            itemList[index].SetActive(false);
        btnInventory.SetActive(true);
        panelInventroy.SetActive(false);
        if(Input.GetMouseButtonUp(0))
            player.isInventory = false;
    }

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

        if (isItem)
        {
            Button rb = rightBtn.GetComponent<Button>();
            rb.Select();
            leftBtn.GetComponent<Button>().interactable = true;
            rightBtn.GetComponent<Button>().interactable = true;
            selectButton.interactable = true;
            InventoryRight();
        }
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
    }

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

    public void SelectItem()
    {
        player.selectItem = itemList[index];
        ShowBtn();
    }
}
