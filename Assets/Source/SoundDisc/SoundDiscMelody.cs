using System;
using UnityEngine;
using System.Collections;
using Assets.Source;

/// <summary>
/// Represents a melody of which the notes are controlled by the entries of one (or multiple) sounddiscs.
/// </summary>
public class SoundDiscMelody : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Every entry in this array is a controller of a 16th note. Leave null values if nothing must be played.")]
    private SoundDiscEntry[] noteControllers = new SoundDiscEntry[0];

    [SerializeField]
    private NotePlayer player = null;
    private Track track;

    private Melody melody;

    private BeatEmitter beatEmitter;

    void Start()
    {
        melody = new Melody(noteControllers.Length);

        track = new Track();
        track.melody = melody;

        player.tracks.Add(track);

        beatEmitter = gameObject.AddComponent<BeatEmitter>();
        beatEmitter.Initiliaze(player, 16); //sounddiskmelody uses 16th notes by definition

        beatEmitter.OnBeatChanged = new BeatEmitter.BeatEventHandler();
        beatEmitter.OnBeatChanged.AddListener(OnBeatChanged);
    }

    private void OnBeatChanged(int beat_number)
    {
        var index = beat_number % 16;
        if (noteControllers.Length < index || noteControllers[index] == null)
            return;

        noteControllers[index].OnPlayed();
    }

    void Update()
    {
        for (int i = 0; i < noteControllers.Length; i++)
        {
            melody.SetNote(i, noteControllers[i].GetNote());
        }
    }
}
