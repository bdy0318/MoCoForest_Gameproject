using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoleHitTextViewer : MonoBehaviour
{
    [SerializeField]
    float               moveSpeed =  30.0f;    //�̵� �ӵ�
    Vector2             defaultPosition;    //�̵� �ִϸ��̼��� �־ �ʱ� ��ġ ����
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
        //������Ʈ�� ȭ�鿡 ���̵��� ����
        gameObject.SetActive(true);
        textHit.text = hitData;

        //�ؽ�Ʈ�� ���� �̵��ϸ鼭 ���� ������� �ڷ�ƾ ����
        StopCoroutine("OnAnimation");
        StartCoroutine("OnAnimation", color);
    }

    IEnumerator OnAnimation(Color color)
    {
        //��ġ ����
        rectHit.anchoredPosition = defaultPosition;

        while(color.a > 0)
        {
            //Vector 2.up �������� �̵�
            rectHit.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
            //���� 1 -> 0���� ����
            color.a -= Time.deltaTime;
            textHit.color = color;

            yield return null;
        }

        gameObject.SetActive(false);
    }


}
