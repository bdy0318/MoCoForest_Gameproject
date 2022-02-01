using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueSignal : MonoBehaviour
{
    bool isPrologueFinish;

    public void PrologueStart()
    {
        isPrologueFinish = false;

    }
    public void PrologueFinish()
    {
        isPrologueFinish = true;
    }
    
}
