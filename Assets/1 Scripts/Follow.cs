using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [SerializeField]
    Player targetPlayer;
    [SerializeField]
    Shop shop;

    public GameObject enterCamera;
    public GameObject shoppingCamera;
    //Vector3 EnterPosition = new Vector3(3.13f, 2.45f, 2.16f);
    //Vector3 ShoppingPosition = new Vector3();

    void Update()
    {
        if (targetPlayer.isShopping)
        {
            enterCamera.SetActive(true);
        }
        else if (shop.isEnter)
        {
            shoppingCamera.SetActive(true);
        }
        else
        {
            enterCamera.SetActive(false);
            shoppingCamera.SetActive(false);
            transform.position = target.position + offset; // 카메라 이동
        }

    }
}
