using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.Playables;

[TrackBindingType(typeof(TextMeshProUGUI))]
[TrackClipType(typeof(subtitleClip))]
public class subtitleTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<subtitleTrackMixer>.Create(graph, inputCount);
    }

}
