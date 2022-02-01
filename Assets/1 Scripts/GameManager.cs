using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    // 게임 매니저 인스턴스 접근
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton
    public Player player;
    public Quest quest;
    public TalkManager talkManager;
    public GameObject coinPanel;

    public Text playerCoinTxt;

    public Vector3 playerPos; // 모코숲 내 플레이어 움직임
    void Start()
    {
        //퀘스트 쉽게 넘어가기
        player.coin = 50000;
        player.stone = 50;
        
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MocoForest")
        {
            playerPos = player.transform.position; // 플레이어 위치 저장

            // 두더지 잡기 후 설정
            if(quest.nowQuest == 4 && !quest.isComplete && quest.isMapChanged)
            {
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // 게임에서 진 경우
                if (!quest.isGameWin)
                    quest.FourthFailed();
            }

            // 카트레이싱 후 설정
            if (quest.nowQuest == 5 && !quest.isComplete && quest.isMapChanged)
            {
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // 게임에서 진 경우
                if(!quest.isGameWin)
                    quest.FifthFailed();
            }
        }
        //두더지 잡기 중
        else if(SceneManager.GetActiveScene().name == "Whack_A_Mole")
        {
            player.gameObject.SetActive(false);
            quest.isMapChanged = true;
        }
        // 카트레이싱 중
        else if (SceneManager.GetActiveScene().name == "CartRacing")
        {
            player.gameObject.SetActive(false);
            quest.isMapChanged = true;
        }
    }

    void LateUpdate()
    {
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
    }
}

