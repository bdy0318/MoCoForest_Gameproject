using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pausePanel;
    public Button loadButton;
    public Player player;
    public bool isEnd;
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
        if (mDown && SceneManager.GetActiveScene().name == "MocoForest" && !player.isTalking && !player.isInventory && !player.isShopping)
        {
            if(!pausePanel.activeSelf)
            {
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
        if(SaveLoadManager.Instance.IsSave())
        {
            loadButton.interactable = true;
        }
    }

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

    public void Save()
    {
        SaveLoadManager.Instance.Save();
    }

    public void Load()
    {
        SaveLoadManager.Instance.Load();
    }

    public void Title()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
