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
            //70�̻��϶� MaxSpawnMole�� 5�� �����Ǳ� ������ 70������ üũ
            if(combo <= 70)
            {
                //�޺��� ���� �����Ǵ� �ִ� �δ��� ����
                moleSpawner.MaxSpawnMole = 1 + (combo + 10) / 20;
            }
            //�ִ� �޺� ����
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


    [field: SerializeField] //�ڵ� ���� ������Ƽ�� inspector view�� ���̰� �� �� ���
    public float MaxTime { private set; get; }
    public float CurrentTime { private set; get; }

    public void StartGame()
    {
        // �� ó�� ���� ��ư ������ ���� ����
        countDown.StartCountDown(GameStart);    //GameStart: endOfCountDown.Invoke();���� ����� - action �޼ҵ�
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

        //CurrentTime�� 0�� �Ǹ� GameOver
        GameOver();
    }

    void GameOver()
    {
        //���� ������������ ȹ���� ���� ���� ����
        PlayerPrefs.SetInt("CurrentScore", Score);
        PlayerPrefs.SetInt("CurrentMaxCombo", MaxCombo);
        PlayerPrefs.SetInt("CurrentNormalMoleHitCount", NormalMoleHitCount);
        PlayerPrefs.SetInt("CurrentRedMoleHitCount", RedMoleHitCount);
        PlayerPrefs.SetInt("CurrentDogMoleHitCount", DogMoleHitCount);

        //GameOver ������ �̵�
        SceneManager.LoadScene("Mole_GameOver");
    }
}
