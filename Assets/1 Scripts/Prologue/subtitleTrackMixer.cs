using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;


public class subtitleTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI text = playerData as TextMeshProUGUI;
        string currentText = "";

        if (!text) { return; }

        int inputCount = playable.GetInputCount();
        for(int i =0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);

            if(inputWeight > 0f)
            {
                ScriptPlayable<subtitleBehaviour> inputPlayable = (ScriptPlayable<subtitleBehaviour>)playable.GetInput(i);

                subtitleBehaviour input = inputPlayable.GetBehaviour();
                currentText = input.subtitleText;
            }
        }

        text.text = currentText;


    }
}
