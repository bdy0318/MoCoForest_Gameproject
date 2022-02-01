using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoleHitTextViewer : MonoBehaviour
{
    [SerializeField]
    float               moveSpeed =  30.0f;    //이동 속도
    Vector2             defaultPosition;    //이동 애니메이션이 있어서 초기 위치 저장
    TextMeshProUGUI     textHit;
    RectTransform       rectHit;

    private void Awake()
    {
        textHit = GetComponent<TextMeshProUGUI>();
        rectHit = GetComponent<RectTransform>();
        defaultPosition = rectHit.anchoredPosition;

        gameObject.SetActive(false);
    }

    public void OnHit(string hitData, Color color)
    {
        //오브젝트를 화면에 보이도록 설정
        gameObject.SetActive(true);
        textHit.text = hitData;

        //텍스트가 위로 이동하면서 점점 사라지는 코루틴 실행
        StopCoroutine("OnAnimation");
        StartCoroutine("OnAnimation", color);
    }

    IEnumerator OnAnimation(Color color)
    {
        //위치 리셋
        rectHit.anchoredPosition = defaultPosition;

        while(color.a > 0)
        {
            //Vector 2.up 방향으로 이동
            rectHit.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
            //투명도 1 -> 0으로 감소
            color.a -= Time.deltaTime;
            textHit.color = color;

            yield return null;
        }

        gameObject.SetActive(false);
    }


}
