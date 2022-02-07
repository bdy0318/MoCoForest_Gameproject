using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pausePanel;
    public Button saveButton;
    public Button loadButton;
    public Button continueButton;
    public GameObject saveLoadPanel;
    public Text saveLoadText;
    public Player player;
    public bool isEnd;
    bool isSaveLoad;
    bool mDown;
    #region Singleton
    public static Menu instance;
    public static Menu Instance
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
    void Update()
    {
        mDown = Input.GetButtonDown("Menu");
        // 메뉴 열고 닫기
        if (mDown && SceneManager.GetActiveScene().name == "MocoForest" && !player.isTalking && !player.isInventory && !player.isShopping)
        {
            if(!pausePanel.activeSelf)
            {
                continueButton.Select();
                pausePanel.SetActive(true);
                player.isMenu = true;
                isEnd = false;
                Time.timeScale = 0.0f;
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1.0f;
                player.isMenu = false;
            }
        }
        if (SaveLoadManager.Instance.IsSave() && !isSaveLoad)
        {
            loadButton.interactable = true;
        }
        else
            loadButton.interactable = false;
    }

    // 계속하기
    public void Continue()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        isEnd = true;
        if(Input.GetMouseButtonUp(0))
        {
            player.isMenu = false;
            isEnd = false;
        }   
    }

    // 세이브
    public void Save()
    {
        saveLoadText.text = "저장 성공!";
        SaveLoadManager.Instance.Save();
        saveLoadPanel.SetActive(true);
        saveButton.interactable = false;
        loadButton.interactable = false;
        isSaveLoad = true;
        StopAllCoroutines();
        StartCoroutine(WaitTime());
    }

    // 로드
    public void Load()
    {
        saveLoadText.text = "로드 성공!";
        SaveLoadManager.Instance.Load();
        saveLoadPanel.SetActive(true);
        saveButton.interactable = false;
        loadButton.interactable = false;
        isSaveLoad = true;
        StopAllCoroutines();
        StartCoroutine(WaitTime());
    }

    // 타이틀로
    public void Title()
    {
        Continue();
        player.gameObject.SetActive(false);
        AudioManager.Instance.FadeOutMusic();
        SceneManager.LoadScene("ObjectDestroy");
    }

    // 종료하기
    public void Quit()
    {
        Application.Quit();
    }

    // 세이브 로드 텀
    IEnumerator WaitTime()
    {
        yield return new WaitForSecondsRealtime(1f);
        saveLoadPanel.SetActive(false);
        saveButton.interactable = true;
        loadButton.interactable = true;
        isSaveLoad = false;
    }
}
