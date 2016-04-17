using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class MultiVoiceAudioSource: MonoBehaviour
{
    //TODO: add default audio source and make multiple sources by copying

    [SerializeField]
    private List<AudioSource> sources;


    // Use this for initialization
    void Start()
    {
        foreach (var audioSource in sources)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    private float lastStutterWarning = 0;
    private float timeBetweenStutterWarning = 0.25f;

    public void PlaySample(AudioClip clip)
    {
        PlaySample(clip, 0, clip.length);
      


    }

    public void PlaySample(AudioClip clip, float start, float end)
    {
        if (sources.Count == 0) return;
        AudioSource bestToKill = sources[0];
        var factor = float.MaxValue;
        foreach (var src in sources)
        {
            if (!src.isPlaying)
            {
                src.clip = clip;
                
                src.Play();
                return;
            }

            var timeLeft = src.clip.length - src.time;
            if (timeLeft < factor)
            {
                factor = timeLeft;
                bestToKill = src;
            }
        }


        bestToKill.Stop();
        bestToKill.clip = clip;
        bestToKill.Play();
        if (Time.realtimeSinceStartup > lastStutterWarning + timeBetweenStutterWarning)
        {
            Debug.Log("Stuttering! Seconds left: " + factor);
            lastStutterWarning = Time.realtimeSinceStartup;
        }

    }
}
