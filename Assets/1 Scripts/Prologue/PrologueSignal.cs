using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueSignal : MonoBehaviour
{
    bool isPrologueFinish;
    public FadeInOut fade;

    //public void PrologueStart()
    //{
    //    isPrologueFinish = false;
    //    fade.OnFade(FadeState.FadeOut);

    //}
    public void PrologueFinish()
    {
        isPrologueFinish = true;
        fade.OnFade(FadeState.FadeIn);

    }


}
