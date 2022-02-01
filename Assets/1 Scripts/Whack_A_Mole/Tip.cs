using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
   
    public GameObject tipImage;
    public GameController gameController;
    public void OnClickButton()
    {
        tipImage.SetActive(false);
        gameController.StartGame();
    }
}
