using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source;

public class NotePlayer : MonoBehaviour
{

    [SerializeField]
    public MultiVoiceAudioSource audioSource;

    public float bpm = 120;

    public List<Track> tracks = new List<Track>();

    // Use this for initialization
    void Start()
    {

    }


    private float notesSentUntil = -1; // Excluding this exact timestamp

    private float maxSendInterval = 0.3f;

    // Update is called once per frame
    void Update()
    {

        var time = Time.realtimeSinceStartup;
        //AudioSettings.dspTime

        if (notesSentUntil < 0)
        {
            notesSentUntil = time;
            return;
        }
        if (time - notesSentUntil > maxSendInterval)
        {
            Debug.Log("Frame to slow or game was paused, skipping notes");
            notesSentUntil = time - maxSendInterval;
        }

        var timeToBeats = bpm / 60;


        //TODO: maybe use playscheduled

        foreach (var track in tracks)
        {
            foreach (var note in track.getNotesForInterval(notesSentUntil * timeToBeats, time * timeToBeats))
            {
                PlayNote(note);
                //Debug.Log("Play Note");
            }

        }

        notesSentUntil = time;

    }

    public void PlayNote(Note note)
    {
        audioSource.PlaySample(note.clip);
    }

    public int GetBeatsPerMeasure()
    {
        return 4;

    }

    public float EstimateCurrentBeat()
    {
        return notesSentUntil * bpm / 60;
    }
    
}
