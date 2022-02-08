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
    float           fadeTime;     // fadeTime값이 10이면 1초(값이 클수록 빠름)
    [SerializeField]
    AnimationCurve  fadeCurve;   //페이드 효과가 적용되는 알파 값을 곡선의 값으로 설정
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
        // 여기다 넣어야 null 오류가 나지 않는다!
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
            // fadeTime으로 나누어서 fadeTime 시간동안
            // percent 값이 0에서 1로 증가하도록 함
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // 알파값을 start부터 end까지 fadeTime 시간 동안 변화시킨다
            Color color = image.color;
            color.a     = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }


        if (prologueSignal.isPrologueFinish)        // 프롤로그 끝
        {
            SceneManager.LoadScene("MocoForest");
        }        
            








    }
}
