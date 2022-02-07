using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public string sceneName = "MocoForest";

    public void ClickStart()
    {
        Debug.Log("시작로딩");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
