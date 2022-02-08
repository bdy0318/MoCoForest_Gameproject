using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimeLine : MonoBehaviour
{
    private PlayableDirector director;
    public FadeInOut fade;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void StartPrologue()
    {
        AudioManager.Instance.Play(2);
        AudioManager.Instance.FadeInMusic(0.3f);
        director.Play();
    }


}
