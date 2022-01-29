using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;
    // ����� �Ŵ��� �ν��Ͻ� ����
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
    public AudioClip[] clips; // ����� ���� �ҽ�
    public AudioSource source;
    public bool flag;
    
    private WaitForSeconds waitTime = new WaitForSeconds(0.007f);

    // ���� ���
    public void Play(int track)
    {
        source.clip = clips[track];
        source.Play();
    }

    // ���� ��� ����
    public void Stop()
    {
        source.Stop();
    }

    // ���̵� �ƿ�
    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    // ���̵� ��
    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
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

    IEnumerator FadeIn()
    {
        flag = true;
        for (float i = source.volume; i < 0.3f; i+= 0.001f)
        {
            source.volume = i;
            yield return waitTime;
        }
        source.volume = 0.3f;
    }
}
