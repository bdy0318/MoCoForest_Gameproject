using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCameraControl : MonoBehaviour
{
    // 카메라
    public GameObject closeupCam;
    public GameObject shoppingCam;

    public Vector3 clPos; // clsoeup 위치
    public Vector3 shPos; // shopping 위치
    public Transform NPC;
    public Transform target;
    public Vector3 targetPos;

    public bool isLerping;
    public bool isShopping;

    public void Update()
    {
        if (isLerping)
        {
            // Shopping Cam
            if(isShopping)
            {
                target.position = Vector3.MoveTowards(target.position, targetPos, Time.deltaTime);
                shoppingCam.transform.LookAt(target);
                shoppingCam.transform.position = Vector3.Lerp(shoppingCam.transform.position, shPos, Time.deltaTime);
            }
            // Closeup Cam
            else
            {
                closeupCam.transform.LookAt(NPC);
                closeupCam.transform.position = Vector3.Lerp(closeupCam.transform.position, clPos, Time.deltaTime);
            }
        }

        if (!isShopping & Vector3.Distance(clPos, closeupCam.transform.position) < 0.05f)
            isLerping = false;

        else if (isShopping & Vector3.Distance(shPos, shoppingCam.transform.position)  < 0.05f)
            isLerping = false;
    }
}
