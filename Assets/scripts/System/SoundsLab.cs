using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsLab : MonoBehaviour
{
    public AudioSource click;
    public AudioSource pababa;
    public Dictionary<string, AudioSource> sources = new();
    public static SoundsLab Instance { get; private set;}
    void Start()
    {
        Instance = this;

        foreach (Transform child in transform) {
            AudioSource source = child.GetComponent<AudioSource>();
            if (source != null) {
                sources[child.name] = source;
            }
        }
    }

    public void Play(string audio, float startTime = 0, bool alreadyPlayed = false) {
        AudioSource cs = null;
        if (sources.TryGetValue(audio, out cs)) {
            if (!alreadyPlayed) {
                cs.Stop();
                cs.Play();
            }
            cs.time = startTime;
        }
    }

    public void Stop(string audio) {
        AudioSource cs = null;
        if (sources.TryGetValue(audio, out cs)) {
            cs.Stop();
        }
    }
}
