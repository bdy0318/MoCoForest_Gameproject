using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject btnHelp;
    public GameObject pannelHelp;
    public Player player;
    public Button closeButton;

    public void Start()
    {
        player = GameManager.Instance.player;
    }

    // 조작방법 열기
    public void ShowBtnHelp()
    {
        btnHelp.SetActive(true);
        pannelHelp.SetActive(false);
        if (Input.GetMouseButtonUp(0))
            player.isInventory = false;
    }

    //조작방법 닫기
    public void ShowHelp()
    {
        btnHelp.SetActive(false);
        pannelHelp.SetActive(true);
        player.isInventory = true;
    }
    
}
