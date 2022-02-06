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
        // �޴� ���� �ݱ�
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

    // ����ϱ�
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

    // ���̺�
    public void Save()
    {
        saveLoadText.text = "���� ����!";
        SaveLoadManager.Instance.Save();
        saveLoadPanel.SetActive(true);
        saveButton.interactable = false;
        loadButton.interactable = false;
        isSaveLoad = true;
        StopAllCoroutines();
        StartCoroutine(WaitTime());
    }

    // �ε�
    public void Load()
    {
        saveLoadText.text = "�ε� ����!";
        SaveLoadManager.Instance.Load();
        saveLoadPanel.SetActive(true);
        saveButton.interactable = false;
        loadButton.interactable = false;
        isSaveLoad = true;
        StopAllCoroutines();
        StartCoroutine(WaitTime());
    }

    // Ÿ��Ʋ��
    public void Title()
    {
        Continue();
        player.gameObject.SetActive(false);
        AudioManager.Instance.FadeOutMusic();
        SceneManager.LoadScene("ObjectDestroy");
    }

    // �����ϱ�
    public void Quit()
    {
        Application.Quit();
    }

    // ���̺� �ε� ��
    IEnumerator WaitTime()
    {
        yield return new WaitForSecondsRealtime(1f);
        saveLoadPanel.SetActive(false);
        saveButton.interactable = true;
        loadButton.interactable = true;
        isSaveLoad = false;
    }
}
