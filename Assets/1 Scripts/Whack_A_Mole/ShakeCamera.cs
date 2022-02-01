using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    //싱글톤 처리를 위한 instance 변수 선언
    static ShakeCamera instance;
    //외부에서 Get 접근만 가능하도록 Instance 프로퍼티 선언
    public static ShakeCamera Instance => instance;

    float shakeTime;
    float shakeIntensity;

    public ShakeCamera()
    {
        //자기 자신에 대한 정보를 static 형태의 instance 변수에 저장해서
        //외부에서 쉽게 접근할 수 있도록 함
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
        //흔들리기 직전의 시작 위치(흔들림 종료 후 돌아올 위치)
        Vector3 startPosition = transform.position;

        while(shakeTime > 0.0f)
        {
            //초기 위치로부터 구 범위 * shakeIntensity 안에서 카메라 위치 변동
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            //시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
    }
   
}
