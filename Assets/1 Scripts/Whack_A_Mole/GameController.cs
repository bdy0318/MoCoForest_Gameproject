using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CountDown countDown;
    [SerializeField]
    private MoleSpawner moleSpawner;
    int score;
    int combo;
    float currentTime;
    public Tip tip;

    public int Score
    {
        set => score = Mathf.Max(0, value);     
        get => score;
    }

    public int Combo
    {
        set
        {
            combo = Mathf.Max(0, value);    
            //70이상일땐 MaxSpawnMole이 5로 고정되기 때문에 70까지만 체크
            if(combo <= 70)
            {
                //콤보에 따라 생성되는 최대 두더지 숫자
                moleSpawner.MaxSpawnMole = 1 + (combo + 10) / 20;
            }
            //최대 콤보 저장
            if(combo > MaxCombo)
            {
                MaxCombo = combo;
            }
        }
        get => combo;
    }
    public int MaxCombo { private set; get; }
    public int NormalMoleHitCount { set; get; }
    public int RedMoleHitCount { set; get; }
    public int DogMoleHitCount { set; get; }


    [field: SerializeField] //자동 구현 프로퍼티를 inspector view에 보이게 할 때 사용
    public float MaxTime { private set; get; }
    public float CurrentTime { private set; get; }

    public void StartGame()
    {
        // 맨 처음 도움말 버튼 누르면 게임 시작
        countDown.StartCountDown(GameStart);    //GameStart: endOfCountDown.Invoke();에서 실행됨 - action 메소드
    }

    private void GameStart()
    {
        moleSpawner.Setup();

        StartCoroutine("OnTimeCount");
    }

    private IEnumerator OnTimeCount()
    {
        CurrentTime = MaxTime;

        while(CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;

            yield return null;  
        }

        //CurrentTime이 0이 되면 GameOver
        GameOver();
    }

    void GameOver()
    {
        //현재 스테이지에서 획득한 여러 정보 저장
        PlayerPrefs.SetInt("CurrentScore", Score);
        PlayerPrefs.SetInt("CurrentMaxCombo", MaxCombo);
        PlayerPrefs.SetInt("CurrentNormalMoleHitCount", NormalMoleHitCount);
        PlayerPrefs.SetInt("CurrentRedMoleHitCount", RedMoleHitCount);
        PlayerPrefs.SetInt("CurrentDogMoleHitCount", DogMoleHitCount);

        //GameOver 씬으로 이동
        SceneManager.LoadScene("Mole_GameOver");
    }
}
