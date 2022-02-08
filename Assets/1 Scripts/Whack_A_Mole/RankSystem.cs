using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankSystem : MonoBehaviour
{
    [SerializeField]
    int maxRankCount = 3;   //�ִ� ��ũ ǥ�� ����
    [SerializeField]
    GameObject textPrefab;  //��ũ ������ ����ϴ� Text UI ������
    [SerializeField]
    Transform panelRankInfo;    //Text�� ��ġ�Ǵ� �θ� Pannel Transfom
    [SerializeField]
    TextMeshProUGUI textResult;

    public GameController gameController;   //��ǥ ���� 
    public Quest quest;

    RankData[] rankDataArray;   //��ũ ������ �����ϴ� RankData Ÿ���� �迭
    int currentIndex = 0;

    private void Awake()
    {
        rankDataArray = new RankData[maxRankCount];

        //1. ������ ��ũ ���� �ҷ�����
        LoadRankData();
        //2. 1����� ���ʷ� ���� ������������ ȹ���� ������ ��
        CompareRank();
        //3. ��ũ ���� ���
        PrintRankData();
        //4. ���ο� ��ũ ���� ����
        SaveRankData();
    }

    void LoadRankData()
    {
        for(int i = 0; i< maxRankCount; ++i)
        {
            rankDataArray[i].score = PlayerPrefs.GetInt("RankScore" + i);
            rankDataArray[i].maxCombo = PlayerPrefs.GetInt("RankMaxCombo" + i);
            rankDataArray[i].normalMoleHitCount = PlayerPrefs.GetInt("RankNormalMoleHitCount" + i);
            rankDataArray[i].redMoleHitCount = PlayerPrefs.GetInt("RankRedMoleHitCount" + i);
            rankDataArray[i].dogMoleHitCount = PlayerPrefs.GetInt("RankDogMoleHitCount" + i);
        }
    }

    void CompareRank()
    {
        //���� ������������ �޼��� ����
        RankData currentData = new RankData();
        currentData.score = PlayerPrefs.GetInt("CurrentScore");
        currentData.maxCombo = PlayerPrefs.GetInt("CurrentMaxCombo");
        currentData.normalMoleHitCount = PlayerPrefs.GetInt("CurrentNormalMoleHitCount");
        currentData.redMoleHitCount = PlayerPrefs.GetInt("CurrentRedMoleHitCount");
        currentData.dogMoleHitCount = PlayerPrefs.GetInt("CurrentDogMoleHitCount");

        //1~3���� ������ ���� ������������ �޼��� ���� ��
        for(int i = 0; i < maxRankCount; ++i)
        {
            if(currentData.score > rankDataArray[i].score)
            {
                //��ũ�� �� �� �ִ� ������ �޼������� �ݺ��� ����
                currentIndex = i;
                break;
            }
        }

        //currentData�� ��� �Ʒ��� ������ ��ĭ�� �о ����
        for(int i = maxRankCount - 1; i > 0; --i)
        {
            rankDataArray[i] = rankDataArray[i - 1];

            if(currentIndex == i - 1)
            {
                break;
            }
        }

        //���ο� ������ ��ũ�� ����ֱ�
        rankDataArray[currentIndex] = currentData;
    }

    void PrintRankData()
    {
        Color color = Color.white;

        //���� - 5õ�� �̻� �޼�, ����
        if(PlayerPrefs.GetInt("CurrentScore") >= 5000)
        {
            textResult.text = "����";
            GameManager.Instance.quest.isGameWin = true;
        }
        else
        {
            textResult.text = "����";
            GameManager.Instance.quest.isGameWin = false;
        }

        for (int i = 0; i < maxRankCount; ++i)
        {
            //��� �÷����� ������ ��ũ�� ��ϵǸ� ������ ��������� ǥ��
            color = currentIndex != i ? Color.white : Color.yellow;

            //Text ���� �� ���ϴ� ������ ���
            SpawnText((i + 1).ToString(), color);
            SpawnText(rankDataArray[i].score.ToString(), color);
            SpawnText(rankDataArray[i].maxCombo.ToString(), color);
            SpawnText(rankDataArray[i].normalMoleHitCount.ToString(), color);
            SpawnText(rankDataArray[i].redMoleHitCount.ToString(), color);
            SpawnText(rankDataArray[i].dogMoleHitCount.ToString(), color);
        }
    }

    void SpawnText(string print, Color color)
    {
        GameObject clone = Instantiate(textPrefab);
        TextMeshProUGUI text = clone.GetComponent<TextMeshProUGUI>();

        clone.transform.SetParent(panelRankInfo);
        clone.transform.localScale = Vector3.one;

        text.text = print;
        text.color = color;
    }
    void SaveRankData()
    {
        for(int i = 0; i < maxRankCount; ++i)
        {
            PlayerPrefs.SetInt("RankScore"+i, rankDataArray[i].score);
            PlayerPrefs.SetInt("RankMaxCombo"+i, rankDataArray[i].maxCombo);
            PlayerPrefs.SetInt("RankNormalMoleHitCount"+i, rankDataArray[i].normalMoleHitCount);
            PlayerPrefs.SetInt("RankRedMoleHitCount"+i, rankDataArray[i].redMoleHitCount);
            PlayerPrefs.SetInt("RankDogMoleHitCount"+i, rankDataArray[i].dogMoleHitCount);
        }
    }
}

[System.Serializable]
public struct RankData
{
    public int score;
    public int maxCombo;
    public int normalMoleHitCount;
    public int redMoleHitCount;
    public int dogMoleHitCount;
}
