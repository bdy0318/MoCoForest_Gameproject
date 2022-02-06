using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueSignal : MonoBehaviour
{
    public bool isPrologueFinish;
    public FadeInOut fade;
 
    void PrologueFinish()
    {
        isPrologueFinish = true;
        AudioManager.Instance.FadeOutMusic();
        fade.OnFade(FadeState.FadeOut);
    }

}
