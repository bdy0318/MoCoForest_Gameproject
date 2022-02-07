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
    public GameObject hammer;
    public GameObject handGun;
    public List<GameObject> decoObj;
    public List<int> decoList;

    public Text playerCoinTxt;

    public Vector3 playerPos; // 모코숲 내 플레이어 위치
    public Vector3 playerInitialPos; // 초기 플레이어 위치
    public Vector3 questIconPos;    // 퀘스트 아이콘 위치

    bool isEndNextTitle;
    void Start()
    {
        // 시작 시 마을 브금 재생
        AudioManager.Instance.Play(0);
        AudioManager.Instance.FadeInMusic(0.3f);
        //퀘스트 쉽게 넘어가기
        player.coin = 5000;
        player.stone = 0;
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
            questIconPos = quest.QuestIcon.transform.position; // 퀘스트 아이콘 위치 저장

            // 게임 시작
            if(quest.nowQuest == 0 && quest.isMapChanged)
            {
                quest.isMapChanged = false;
            }

            // 데코레이션 미션 이전 로드 후
            if (quest.nowQuest < 3 && SaveLoadManager.Instance.isLoad) // 로드 후
            {
                while(decoObj.Count != 0)
                {
                    Destroy(decoObj[0]);
                    decoObj.RemoveAt(0);
                    decoList.RemoveAt(0);
                }
            }

            // 데코레이션 미션 이후 로드 후
            if (quest.nowQuest >= 3 && SaveLoadManager.Instance.isLoad) // 로드 후
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

            // 로드 시
            if (SaveLoadManager.Instance.isLoad)
            {
                if (quest.nowQuest == 0 || quest.isComplete)
                    quest.QuestIcon.transform.position = quest.npc[quest.nowQuest].transform.position + new Vector3(0, 3, 0);
                else
                    quest.QuestIcon.transform.position = quest.npc[quest.nowQuest - 1].transform.position + new Vector3(0, 3, 0);
                quest.QuestIcon.gameObject.SetActive(true);

                // 맵 획득한 무기 파괴
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

            // 두더지 잡기 후 설정
            if (quest.nowQuest == 4 && !quest.isComplete && quest.isMapChanged)
            {
                AudioManager.Instance.Play(0);
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // 게임에서 진 경우
                if (!quest.isGameWin)
                    quest.FourthFailed();
                for (int i = 0; i < decoObj.Count; i++)
                {
                    decoObj[i].SetActive(true);
                    SaveLoadManager.Instance.deco[decoList[i]].isComplete = true;
                    SaveLoadManager.Instance.deco[decoList[i]].itemIndex = decoObj[i].GetComponent<Item>().value;
                }
                quest.QuestIcon.transform.position = quest.npc[3].transform.position + new Vector3(0, 3, 0);

                // 맵 획득한 무기 파괴
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

            // 카트레이싱 후 설정
            if (quest.nowQuest == 5 && !quest.isComplete && quest.isMapChanged)
            {
                AudioManager.Instance.Play(0);
                AudioManager.Instance.FadeInMusic(0.3f);
                quest.isMapChanged = false;
                player.gameObject.SetActive(true);
                // 게임에서 진 경우
                if (!quest.isGameWin)
                    quest.FifthFailed();
                for (int i = 0; i < decoObj.Count; i++)
                {
                    decoObj[i].SetActive(true);
                    SaveLoadManager.Instance.deco[decoList[i]].isComplete = true;
                    SaveLoadManager.Instance.deco[decoList[i]].itemIndex = decoObj[i].GetComponent<Item>().value;
                }
                quest.QuestIcon.transform.position = quest.npc[4].transform.position + new Vector3(0, 3, 0);

                // 맵 획득한 무기 파괴
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
            // 엔딩 종료 후 플레이어 위치 초기화
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
        //두더지 잡기 중
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
        // 카트레이싱 중
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
        // 엔딩 후 오브젝트 파괴, 설정 초기화
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

            talkManager.talkData[1000] = new string[] { "안녀엉! 동물마을에 온 걸 환여엉해.",
                                          "인간 친구가 우리 마을에 오다니 신기하다아" };
            talkManager.talkData[2000] = new string[] { "앗, 너냐!", "왜 왔냐." };
            talkManager.talkData[3000] = new string[] { ".....", "…마을이 뭔가 허전해 보이는 것 같지 않아?" };
            talkManager.talkData[4000] = new string[] { "와!! 안녕 반가워!!", "너는 참 신기하게 생겼다!!!" };
            talkManager.talkData[5000] = new string[] { "네가 동물마을에서 살고 싶다고 한 인간이지?",
                                          "나는 나보다 강한 자만 인정한다고!"};
            talkManager.talkData[6000] = new string[] { "나한테 가까이 다가오지 마. 날 해칠꺼지?",
                                          "나는 인간이 우리 마을에 오는게 마음에 안들어."};

            while(decoObj.Count != 0)
            {
                Destroy(decoObj[0]);
                decoObj.RemoveAt(0);
                decoList.RemoveAt(0);
            }

            isEndNextTitle = true;
            quest.isMapChanged = true;
            SceneManager.LoadScene("StartScene"); // 타이틀로 이동
        }

        // 프롤로그 시작
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

