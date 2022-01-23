using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    // ���� �Ŵ��� �ν��Ͻ� ����
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

    public Vector3 playerPos; // ���ڽ� �� �÷��̾� ������
    void Start()
    {
        player.coin = 5000;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MocoForest")
        {
            playerPos = player.transform.position; // �÷��̾� ��ġ ����
            // īƮ���̽� �� ����
            if (quest.nowQuest == 5 && !quest.isComplete && quest.isMapChanged)
            {
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // ���ӿ��� �� ���
                if(!quest.isGameWin)
                    quest.FifthFailed();
            }
        }
        // īƮ���̽� ��
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

