using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source;
using Miscellaneous;

public class NotePlayer : MonoBehaviour
{
    [SerializeField]
    public MultiVoiceAudioSource audioSource;

    [SerializeField]
    public bool Loop = false;
    [SerializeField]
    [ConditionalHide("Loop")]
    private float LoopBeats = 0f;

    public float bpm = 120;

    public List<Track> tracks = new List<Track>();

    private bool isPlaying = true;

    public void StopPlaying()
    {
        isPlaying = false;
    }

    public void StartPlaying(float beat)
    {
        isPlaying = true;
        Scrub(beat);
    }

    // Use this for initialization
    void Start()
    {

    }
    //private float currentTime; //s

    private float playOffset = 0;

    private float notesSentUntil = -1000; // Excluding this exact timestamp

    private float maxSendInterval = 0.3f;

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
            return;

        //AudioSettings.dspTime

        var currentTime = Time.realtimeSinceStartup + playOffset;

        if (notesSentUntil < -999) // Init
        {
            Scrub(0);
            return;
        }
        if (currentTime - notesSentUntil > maxSendInterval)
        {
            Debug.Log("Frame to slow or game was paused, skipping notes");
            notesSentUntil = currentTime - maxSendInterval;
        }

        var timeToBeats = bpm / 60;


        //TODO: maybe use playscheduled

        foreach (var track in tracks)
        {
            foreach (var note in track.getNotesForInterval(notesSentUntil * timeToBeats, currentTime * timeToBeats))
            {
                if (note.clip == null)
                {
                    // pause/scrub to now/ replay
                    Scrub(notesSentUntil*bpm/60); //This does the time warp
                    return;
                }
                PlayNote(note);
                //Debug.Log("Play Note");
            }
        }

        notesSentUntil = currentTime;

        //Debug.Log(EstimateCurrentBeat());

        var notesSentUntilInBeats = notesSentUntil * bpm / 60;

        if (Loop && notesSentUntilInBeats + 0.08f > LoopBeats)
            Scrub(notesSentUntilInBeats - LoopBeats);
    }

    public void Scrub(float beat)
    {
        playOffset = -Time.realtimeSinceStartup + beat / bpm * 60;

        notesSentUntil = Time.realtimeSinceStartup + playOffset;
    }

    public void PlayNote(Note note)
    {
        audioSource.PlaySample(note.clip);
    }

    public int GetBeatsPerMeasure()
    {
        return 4;

    }

    //private float MagicNumberBLeepBLeep = 4;

    public float EstimateCurrentBeat()
    {
        return Math.Max(0, notesSentUntil * bpm / 60 );
    }


}
