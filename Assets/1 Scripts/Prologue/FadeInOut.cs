using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum FadeState { FadeIn = 0, FadeOut}


public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    float           fadeTime;     // fadeTime���� 10�̸� 1��(���� Ŭ���� ����)
    [SerializeField]
    AnimationCurve  fadeCurve;   //���̵� ȿ���� ����Ǵ� ���� ���� ��� ������ ����
    Image           image;
    FadeState       fadeState;

    public TimeLine timeline;
    public PrologueSignal prologueSignal;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        // ����� �־�� null ������ ���� �ʴ´�!
        OnFade(FadeState.FadeIn);
    }

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
                timeline.StartPrologue();
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));
                break;
        }
    }

    IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
            // fadeTime���� ����� fadeTime �ð�����
            // percent ���� 0���� 1�� �����ϵ��� ��
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // ���İ��� start���� end���� fadeTime �ð� ���� ��ȭ��Ų��
            Color color = image.color;
            color.a     = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }


        if (prologueSignal.isPrologueFinish)        // ���ѷα� ��
        {
            SceneManager.LoadScene("MocoForest");
        }        
            








    }
}
