using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    //�̱��� ó���� ���� instance ���� ����
    static ShakeCamera instance;
    //�ܺο��� Get ���ٸ� �����ϵ��� Instance ������Ƽ ����
    public static ShakeCamera Instance => instance;

    float shakeTime;
    float shakeIntensity;

    public ShakeCamera()
    {
        //�ڱ� �ڽſ� ���� ������ static ������ instance ������ �����ؼ�
        //�ܺο��� ���� ������ �� �ֵ��� ��
        instance = this;
    }

    public void OnShakeCamera(float shakeTime=1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }

    private IEnumerator ShakeByPosition()
    {
        //��鸮�� ������ ���� ��ġ(��鸲 ���� �� ���ƿ� ��ġ)
        Vector3 startPosition = transform.position;

        while(shakeTime > 0.0f)
        {
            //�ʱ� ��ġ�κ��� �� ���� * shakeIntensity �ȿ��� ī�޶� ��ġ ����
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            //�ð� ����
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
    }
   
}
