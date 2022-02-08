using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankSystem : MonoBehaviour
{
    [SerializeField]
    int maxRankCount = 3;   //최대 랭크 표시 개수
    [SerializeField]
    GameObject textPrefab;  //랭크 정보를 출력하는 Text UI 프리팹
    [SerializeField]
    Transform panelRankInfo;    //Text가 배치되는 부모 Pannel Transfom
    [SerializeField]
    TextMeshProUGUI textResult;

    public GameController gameController;   //목표 점수 
    public Quest quest;

    RankData[] rankDataArray;   //랭크 정보를 저장하는 RankData 타입의 배열
    int currentIndex = 0;

    private void Awake()
    {
        rankDataArray = new RankData[maxRankCount];

        //1. 기존의 랭크 정보 불러오기
        LoadRankData();
        //2. 1등부터 차례로 현재 스테이지에서 획득한 점수와 비교
        CompareRank();
        //3. 랭크 정보 출력
        PrintRankData();
        //4. 새로운 랭크 정보 저장
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
        //현재 스테이지에서 달성한 정보
        RankData currentData = new RankData();
        currentData.score = PlayerPrefs.GetInt("CurrentScore");
        currentData.maxCombo = PlayerPrefs.GetInt("CurrentMaxCombo");
        currentData.normalMoleHitCount = PlayerPrefs.GetInt("CurrentNormalMoleHitCount");
        currentData.redMoleHitCount = PlayerPrefs.GetInt("CurrentRedMoleHitCount");
        currentData.dogMoleHitCount = PlayerPrefs.GetInt("CurrentDogMoleHitCount");

        //1~3등의 점수와 현재 스테이지에서 달성한 점수 비교
        for(int i = 0; i < maxRankCount; ++i)
        {
            if(currentData.score > rankDataArray[i].score)
            {
                //랭크에 들어갈 수 있는 점수를 달성했으면 반복문 중지
                currentIndex = i;
                break;
            }
        }

        //currentData의 등수 아래로 점수를 한칸씩 밀어서 저장
        for(int i = maxRankCount - 1; i > 0; --i)
        {
            rankDataArray[i] = rankDataArray[i - 1];

            if(currentIndex == i - 1)
            {
                break;
            }
        }

        //새로운 점수를 랭크에 집어넣기
        rankDataArray[currentIndex] = currentData;
    }

    void PrintRankData()
    {
        Color color = Color.white;

        //성공 - 5천점 이상 달성, 실패
        if(PlayerPrefs.GetInt("CurrentScore") >= 5000)
        {
            textResult.text = "성공";
            GameManager.Instance.quest.isGameWin = true;
        }
        else
        {
            textResult.text = "실패";
            GameManager.Instance.quest.isGameWin = false;
        }

        for (int i = 0; i < maxRankCount; ++i)
        {
            //방금 플레이의 점수가 랭크에 등록되면 색상을 노란색으로 표시
            color = currentIndex != i ? Color.white : Color.yellow;

            //Text 생성 및 원하는 데이터 출력
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
