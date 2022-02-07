using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public string sceneName = "MocoForest";

    public void ClickStart()
    {
        Debug.Log("���۷ε�");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickExit()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }
}
