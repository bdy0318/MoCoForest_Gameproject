using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;
    // 오디오 매니저 인스턴스 접근
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton
    public AudioClip[] clips; // 사용할 음악 소스
    public AudioSource source;
    public bool flag;
    
    private WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.007f);

    // 음악 재생
    public void Play(int track)
    {
        source.clip = clips[track];
        source.Play();
    }

    // 음악 재생 중지
    public void Stop()
    {
        source.Stop();
    }

    // 페이드 아웃
    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    // 페이드 인
    public void FadeInMusic(float volume)
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn(volume));
    }

    IEnumerator FadeOut()
    {
        flag = false;
        for (float i = source.volume; i>=0f; i-= 0.001f)
        {
            source.volume = i;
            yield return waitTime;
        }
        source.volume = 0;
    }

    IEnumerator FadeIn(float volume)
    {
        flag = true;
        for (float i = source.volume; i < volume; i+= 0.001f)
        {
            source.volume = i;
            yield return waitTime;
        }
        source.volume = volume;
    }
}
