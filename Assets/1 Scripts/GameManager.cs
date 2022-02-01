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

    public Vector3 playerPos; // 모코숲 내 플레이어 위치
    public Vector3 playerInitialPos; // 초기 플레이어 위치

    bool isEndNextTitle;
    void Start()
    {
        // 시작 시 마을 브금 재생
        AudioManager.Instance.Play(0);
        AudioManager.Instance.FadeInMusic();
        //퀘스트 쉽게 넘어가기
        player.coin = 50000;
        player.stone = 50;
        
    }

    private void Update()
    {
        // 대화 시 골드 표시 숨김
        if(coinPanel != null && player.isTalking)
        {
            coinPanel.SetActive(false);
        }
        // 대화 종료 시 골드 표시
        else if(!player.isTalking && coinPanel != null)
        {
            coinPanel.SetActive(true);
        }
        if (SceneManager.GetActiveScene().name == "MocoForest")
        {
            playerPos = player.transform.position; // 플레이어 위치 저장

            // 두더지 잡기 후 설정
            if(quest.nowQuest == 4 && !quest.isComplete && quest.isMapChanged)
            {
                AudioManager.Instance.Play(0);
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // 게임에서 진 경우
                if (!quest.isGameWin)
                    quest.FourthFailed();
            }

            // 카트레이싱 후 설정
            if (quest.nowQuest == 5 && !quest.isComplete && quest.isMapChanged)
            {
                AudioManager.Instance.Play(0);
                AudioManager.Instance.FadeInMusic();
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // 게임에서 진 경우
                if (!quest.isGameWin)
                    quest.FifthFailed();
            }
            // 엔딩 종료 후 플레이어 위치 초기화
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
        //두더지 잡기 중
        else if(SceneManager.GetActiveScene().name == "Whack_A_Mole")
        {
            AudioManager.Instance.Stop();
            player.gameObject.SetActive(false);
            quest.isMapChanged = true;
        }
        // 카트레이싱 중
        else if (SceneManager.GetActiveScene().name == "CartRacing")
        {
            if(AudioManager.Instance.source.clip == AudioManager.Instance.clips[0] && AudioManager.Instance.flag)
                AudioManager.Instance.FadeOutMusic();
            player.gameObject.SetActive(false);
            quest.isMapChanged = true;
        }
        // 엔딩 후 오브젝트 파괴, 설정 초기화
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

            talkManager.talkData[1000] = new string[] { "안녀엉! 동물마을에 온 걸 환여엉해.",
                                          "인간 친구가 우리 마을에 오다니 신기하다아" };
            talkManager.talkData[2000] = new string[] { "앗, 너냐!", "왜 왔냐." };
            talkManager.talkData[3000] = new string[] { ".....", "…마을이 뭔가 허전해 보이는 것 같지 않아?" };
            talkManager.talkData[4000] = new string[] { "와!! 안녕 반가워!!", "너는 참 신기하게 생겼다!!!" };
            talkManager.talkData[5000] = new string[] { "네가 동물마을에서 살고 싶다고 한 인간이지?",
                                          "나는 나보다 강한 자만 인정한다고!"};
            talkManager.talkData[6000] = new string[] { "나한테 가까이 다가오지 마. 날 해칠꺼지?",
                                          "나는 인간이 우리 마을에 오는게 마음에 안들어."};

            isEndNextTitle = true;
            SceneManager.LoadScene("MocoForest"); // 마을 씬으로 이동
        }
    }

    void LateUpdate()
    {
        if(coinPanel != null)
            playerCoinTxt.text = string.Format("{0:n0}", player.coin);
    }
}

