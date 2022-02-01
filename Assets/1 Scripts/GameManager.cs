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

    public Vector3 playerPos; // ���ڽ� �� �÷��̾� ��ġ
    public Vector3 playerInitialPos; // �ʱ� �÷��̾� ��ġ

    bool isEndNextTitle;
    void Start()
    {
        // ���� �� ���� ��� ���
        AudioManager.Instance.Play(0);
        AudioManager.Instance.FadeInMusic();
        //����Ʈ ���� �Ѿ��
        player.coin = 50000;
        player.stone = 50;
        
    }

    private void Update()
    {
        // ��ȭ �� ��� ǥ�� ����
        if(coinPanel != null && player.isTalking)
        {
            coinPanel.SetActive(false);
        }
        // ��ȭ ���� �� ��� ǥ��
        else if(!player.isTalking && coinPanel != null)
        {
            coinPanel.SetActive(true);
        }
        if (SceneManager.GetActiveScene().name == "MocoForest")
        {
            playerPos = player.transform.position; // �÷��̾� ��ġ ����

            // �δ��� ��� �� ����
            if(quest.nowQuest == 4 && !quest.isComplete && quest.isMapChanged)
            {
                AudioManager.Instance.Play(0);
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // ���ӿ��� �� ���
                if (!quest.isGameWin)
                    quest.FourthFailed();
            }

            // īƮ���̽� �� ����
            if (quest.nowQuest == 5 && !quest.isComplete && quest.isMapChanged)
            {
                AudioManager.Instance.Play(0);
                AudioManager.Instance.FadeInMusic();
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // ���ӿ��� �� ���
                if (!quest.isGameWin)
                    quest.FifthFailed();
            }
            // ���� ���� �� �÷��̾� ��ġ �ʱ�ȭ
            else if(isEndNextTitle)
            {
                AudioManager.Instance.Play(0);
                AudioManager.Instance.FadeInMusic();
                player.transform.position = playerInitialPos;
                player.transform.eulerAngles = new Vector3(0, 0, 0);
                player.gameObject.SetActive(true);
                player.isTalking = false;
                isEndNextTitle = false;
            }
        }
        //�δ��� ��� ��
        else if(SceneManager.GetActiveScene().name == "Whack_A_Mole")
        {
            AudioManager.Instance.Stop();
            player.gameObject.SetActive(false);
            quest.isMapChanged = true;
        }
        // īƮ���̽� ��
        else if (SceneManager.GetActiveScene().name == "CartRacing")
        {
            if(AudioManager.Instance.source.clip == AudioManager.Instance.clips[0] && AudioManager.Instance.flag)
                AudioManager.Instance.FadeOutMusic();
            player.gameObject.SetActive(false);
            quest.isMapChanged = true;
        }
        // ���� �� ������Ʈ �ı�, ���� �ʱ�ȭ
        else if (SceneManager.GetActiveScene().name == "ObjectDestroy")
        {
            AudioManager.Instance.FadeOutMusic();
            playerPos = new Vector3(0, 999, 0);
            player.coin = 0;
            player.stone = 0;
            player.selectItem = null;
            for (int i = 0; i < player.hasItem.Length; i++)
                player.hasItem[i] = 0;

            quest.nowQuest = 0;
            quest.isComplete = false;
            quest.isGameWin = false;
            quest.isMapChanged = false;
            quest.thirdQuest = 0;

            talkManager.talkData[1000] = new string[] { "�ȳ��! ���������� �� �� ȯ������.",
                                          "�ΰ� ģ���� �츮 ������ ���ٴ� �ű��ϴپ�" };
            talkManager.talkData[2000] = new string[] { "��, �ʳ�!", "�� �Գ�." };
            talkManager.talkData[3000] = new string[] { ".....", "�������� ���� ������ ���̴� �� ���� �ʾ�?" };
            talkManager.talkData[4000] = new string[] { "��!! �ȳ� �ݰ���!!", "�ʴ� �� �ű��ϰ� �����!!!" };
            talkManager.talkData[5000] = new string[] { "�װ� ������������ ��� �ʹٰ� �� �ΰ�����?",
                                          "���� ������ ���� �ڸ� �����Ѵٰ�!"};
            talkManager.talkData[6000] = new string[] { "������ ������ �ٰ����� ��. �� ��ĥ����?",
                                          "���� �ΰ��� �츮 ������ ���°� ������ �ȵ��."};

            isEndNextTitle = true;
            SceneManager.LoadScene("MocoForest"); // ���� ������ �̵�
        }
    }

    void LateUpdate()
    {
        if(coinPanel != null)
            playerCoinTxt.text = string.Format("{0:n0}", player.coin);
    }
}

