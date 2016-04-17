using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source;

/// <summary>
/// Test for the scene which tests the improved audio playing mechanics, eg playing samples for a multi-sample audio file
/// </summary>
public class ImprovedAudioPlayerTest : MonoBehaviour
{

    [SerializeField]
    private NotePlayer player;

    [SerializeField]
    private Instrument instrument;

    [SerializeField] private AudioClip instrumentClip;
    [SerializeField]
    private MultiVoiceAudioSource audioSource;

    private AudioClip clip;

    // Use this for initialization
    void Start ()
    {

        // 12 => 6 beats => 90 bpm => 1.5 bps => 0.75 s/beat => 0.75*6 sec => 4.5 sec => 4.5s * freq (sampl/s) = samples

        var bpm = 90f;
        var numBeats = 6f;

        var secondsLength = numBeats/bpm*60;

        var partLength = (int)(secondsLength * instrumentClip.frequency);

        clip = AudioClip.Create("subpart", partLength, instrumentClip.channels, instrumentClip.frequency,false);

        float[] smp1 = new float[(partLength) * instrumentClip.channels];
        instrumentClip.GetData(smp1, 0);
        clip.SetData(smp1,0);

        //len1 = sourceClip.samples / 2;
        //len2 = sourceClip.samples - len1;
        //overlapSamples = (int)(overlap * sourceClip.frequency);
        //cutClip1 = AudioClip.Create("cut1", len1 + overlapSamples, sourceClip.channels, sourceClip.frequency, false, false);
        //cutClip2 = AudioClip.Create("cut2", len2 + overlapSamples, sourceClip.channels, sourceClip.frequency, false, false);
        //float[] smp1 = new float[(len1 + overlapSamples) * sourceClip.channels];
        //float[] smp2 = new float[(len2 + overlapSamples) * sourceClip.channels];
        //sourceClip.GetData(smp1, 0);
        //sourceClip.GetData(smp2, len1 - overlapSamples);
        //cutClip1.SetData(smp1, 0);
        //cutClip2.SetData(smp2, 0);

    }


	// Update is called once per frame
	void Update () {
	
        foreach (var key in Enumerable.Range(0,9))
        {
            var code = (KeyCode)Enum.Parse(typeof (KeyCode), "Keypad" + key);
            if (Input.GetKeyDown(code))
                player.PlayNote(instrument.GetNote(60+key));
        }

	    if (Input.GetKeyDown(KeyCode.UpArrow))
	        audioSource.PlaySample(clip);




    }
}
