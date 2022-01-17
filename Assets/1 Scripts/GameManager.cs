using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public Player player;

    public GameObject coinPanel;

    public Text playerCoinTxt;

    void Start()
    {
        player.coin = 5000;
    }

    void LateUpdate()
    {
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
    }


}

