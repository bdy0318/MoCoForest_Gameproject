using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
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
    public GameObject hammer;
    public GameObject handGun;
    public List<GameObject> decoObj;
    public List<int> decoList;

    public Text playerCoinTxt;

    public Vector3 playerPos; // ���ڽ� �� �÷��̾� ��ġ
    public Vector3 playerInitialPos; // �ʱ� �÷��̾� ��ġ
    public Vector3 questIconPos;    // ����Ʈ ������ ��ġ

    bool isEndNextTitle;
    void Start()
    {
        // ���� �� ���� ��� ���
        AudioManager.Instance.Play(0);
        AudioManager.Instance.FadeInMusic(0.3f);
        //����Ʈ ���� �Ѿ��
        player.coin = 5000;
        player.stone = 0;
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
            questIconPos = quest.QuestIcon.transform.position; // ����Ʈ ������ ��ġ ����

            // ���� ����
            if(quest.nowQuest == 0 && quest.isMapChanged)
            {
                quest.isMapChanged = false;
            }

            // ���ڷ��̼� �̼� ���� �ε� ��
            if (quest.nowQuest < 3 && SaveLoadManager.Instance.isLoad) // �ε� ��
            {
                while(decoObj.Count != 0)
                {
                    Destroy(decoObj[0]);
                    decoObj.RemoveAt(0);
                    decoList.RemoveAt(0);
                }
            }

            // ���ڷ��̼� �̼� ���� �ε� ��
            if (quest.nowQuest >= 3 && SaveLoadManager.Instance.isLoad) // �ε� ��
            {
                for (int i = 0; i < decoObj.Count; i++)
                {
                    if (decoObj[i] == null)
                    {
                        decoObj.RemoveAt(i);
                        decoList.RemoveAt(i);
                    }
                }
            }

            // �ε� ��
            if (SaveLoadManager.Instance.isLoad)
            {
                if (quest.nowQuest == 0 || quest.isComplete)
                    quest.QuestIcon.transform.position = quest.npc[quest.nowQuest].transform.position + new Vector3(0, 3, 0);
                else
                    quest.QuestIcon.transform.position = quest.npc[quest.nowQuest - 1].transform.position + new Vector3(0, 3, 0);
                quest.QuestIcon.gameObject.SetActive(true);

                // �� ȹ���� ���� �ı�
                if (player.hasWeapons.Length != 0)
                {
                    if (player.hasWeapons[0])
                        hammer.SetActive(false);
                    else
                        hammer.SetActive(true);

                    if (player.hasWeapons[1])
                        handGun.SetActive(false);
                    else
                        handGun.SetActive(true);
                }

                SaveLoadManager.Instance.isLoad = false;
            }

            // �δ��� ��� �� ����
            if (quest.nowQuest == 4 && !quest.isComplete && quest.isMapChanged)
            {
                AudioManager.Instance.Play(0);
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // ���ӿ��� �� ���
                if (!quest.isGameWin)
                    quest.FourthFailed();
                for (int i = 0; i < decoObj.Count; i++)
                {
                    decoObj[i].SetActive(true);
                    SaveLoadManager.Instance.deco[decoList[i]].isComplete = true;
                    SaveLoadManager.Instance.deco[decoList[i]].itemIndex = decoObj[i].GetComponent<Item>().value;
                }
                quest.QuestIcon.transform.position = quest.npc[3].transform.position + new Vector3(0, 3, 0);

                // �� ȹ���� ���� �ı�
                if (player.hasWeapons.Length != 0)
                {
                    if (player.hasWeapons[0])
                        hammer.SetActive(false);
                    else
                        hammer.SetActive(true);

                    if (player.hasWeapons[1])
                        handGun.SetActive(false);
                    else
                        handGun.SetActive(true);
                }
            }

            // īƮ���̽� �� ����
            if (quest.nowQuest == 5 && !quest.isComplete && quest.isMapChanged)
            {
                AudioManager.Instance.Play(0);
                AudioManager.Instance.FadeInMusic(0.3f);
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // ���ӿ��� �� ���
                if (!quest.isGameWin)
                    quest.FifthFailed();
                for (int i = 0; i < decoObj.Count; i++)
                {
                    decoObj[i].SetActive(true);
                    SaveLoadManager.Instance.deco[decoList[i]].isComplete = true;
                    SaveLoadManager.Instance.deco[decoList[i]].itemIndex = decoObj[i].GetComponent<Item>().value;
                }
                quest.QuestIcon.transform.position = quest.npc[4].transform.position + new Vector3(0, 3, 0);

                // �� ȹ���� ���� �ı�
                if (player.hasWeapons.Length != 0)
                {
                    if (player.hasWeapons[0])
                        hammer.SetActive(false);
                    else
                        hammer.SetActive(true);

                    if (player.hasWeapons[1])
                        handGun.SetActive(false);
                    else
                        handGun.SetActive(true);
                }
            }
            // ���� ���� �� �÷��̾� ��ġ �ʱ�ȭ
            else if(isEndNextTitle)
            {
                AudioManager.Instance.Play(0);
                AudioManager.Instance.FadeInMusic(0.3f);
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
            for (int i = 0; i < decoObj.Count; i++)
            {
                decoObj[i].SetActive(false);
            }
        }
        // īƮ���̽� ��
        else if (SceneManager.GetActiveScene().name == "CartRacing")
        {
            if(AudioManager.Instance.source.clip == AudioManager.Instance.clips[0] && AudioManager.Instance.flag)
                AudioManager.Instance.FadeOutMusic();
            else if(!AudioManager.Instance.flag && AudioManager.Instance.source.volume < 0.1f)
            {
                AudioManager.Instance.Play(1);
                AudioManager.Instance.FadeInMusic(0.6f);
            }

            player.gameObject.SetActive(false);
            quest.isMapChanged = true;
            for (int i = 0; i < decoObj.Count; i++)
            {
                decoObj[i].SetActive(false);
            }
        }
        // ���� �� ������Ʈ �ı�, ���� �ʱ�ȭ
        else if (SceneManager.GetActiveScene().name == "ObjectDestroy")
        {
            AudioManager.Instance.FadeOutMusic();
            playerPos = new Vector3(0, 999, 0);
            player.coin = 0;
            player.stone = 0;
            player.selectItem = null;
            if(player.equipWeapon != null)
                player.equipWeapon.gameObject.SetActive(false);
            player.equipWeapon = null;
            for (int i = 0; i < player.hasItem.Length; i++)
                player.hasItem[i] = 0;
            for (int i = 0; i < player.hasWeapons.Length; i++)
                player.hasWeapons[i] = false;

            quest.nowQuest = 0;
            quest.isComplete = false;
            quest.isGameWin = false;
            quest.isMapChanged = false;
            quest.thirdQuest = 0;
            for (int i = 0; i < quest.sixthQuest.Length; i++)
                quest.sixthQuest[i] = false;

            talkManager.talkData[1000] = new string[] { "�ȳ��! ���������� �� �� ȯ������.",
                                          "�ΰ� ģ���� �츮 ������ ���ٴ� �ű��ϴپ�" };
            talkManager.talkData[2000] = new string[] { "��, �ʳ�!", "�� �Գ�." };
            talkManager.talkData[3000] = new string[] { ".....", "�������� ���� ������ ���̴� �� ���� �ʾ�?" };
            talkManager.talkData[4000] = new string[] { "��!! �ȳ� �ݰ���!!", "�ʴ� �� �ű��ϰ� �����!!!" };
            talkManager.talkData[5000] = new string[] { "�װ� ������������ ��� �ʹٰ� �� �ΰ�����?",
                                          "���� ������ ���� �ڸ� �����Ѵٰ�!"};
            talkManager.talkData[6000] = new string[] { "������ ������ �ٰ����� ��. �� ��ĥ����?",
                                          "���� �ΰ��� �츮 ������ ���°� ������ �ȵ��."};

            while(decoObj.Count != 0)
            {
                Destroy(decoObj[0]);
                decoObj.RemoveAt(0);
                decoList.RemoveAt(0);
            }

            isEndNextTitle = true;
            quest.isMapChanged = true;
            SceneManager.LoadScene("StartScene"); // Ÿ��Ʋ�� �̵�
        }

        // ���ѷα� ����
        else if(SceneManager.GetActiveScene().name == "Prologue" && !player.gameObject.activeSelf)
        {
            player.gameObject.SetActive(true);
            player.prologue = FindObjectOfType<PlayableDirector>();
            TrackAsset temp = player.time.GetOutputTrack(6);
            player.prologue.SetGenericBinding(temp, player.GetComponent<Animator>());
        }
    }

    void LateUpdate()
    {
        if(coinPanel != null)
            playerCoinTxt.text = string.Format("{0:n0}", player.coin);
    }
}

