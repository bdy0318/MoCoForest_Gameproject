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
        //퀘스트 쉽게 넘어가기
        player.coin = 50000;
        player.stone = 50;
        
    }

    void LateUpdate()
    {
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
    }


}

