using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source;

/// <summary>
/// Represents a melody of which the notes are controlled by the entries of one (or multiple) sounddiscs.
/// </summary>
public class SoundDiscMelody : MonoBehaviour
{
    [SerializeField]
    private MelodyData startData = null;

    [SerializeField]
    [Tooltip("Every entry in this array is a controller of a 16th note. Leave null values if nothing must be played.")]
    public SoundDiscEntry[] noteControllers = new SoundDiscEntry[0];

    [SerializeField]
    private SoundDiskInstrument[] soundDiscsInstruments = new SoundDiskInstrument[0];

    [SerializeField]
    private NotePlayer player = null;
    private Track track;

    public Melody melody { get; private set; }

    private BeatEmitter beatEmitter;

    void Start()
    {
        TryInitialize();
    }

    private void TryInitialize()
    {
        if (melody != null)
            return;

        melody = gameObject.AddComponent<Melody>();
        melody.SetLength(noteControllers.Length);

        track = new Track();
        track.melody = melody;

        player.tracks.Add(track);

        beatEmitter = gameObject.AddComponent<BeatEmitter>();
        beatEmitter.Initiliaze(player, 16); //sounddiskmelody uses 16th notes by definition

        beatEmitter.OnBeatChanged = new BeatEmitter.BeatEventHandler();
        beatEmitter.OnBeatChanged.AddListener(OnBeatChanged);

        if (startData != null)
        {
            LoadData(startData);
        }
    }

    /// <summary>
    /// Returns a color for each 16th of the melody.
    /// </summary>
    /// <returns></returns>
    public List<Color> GetTrackColors()
    {
        TryInitialize();

        var color_list = new List<Color>();

        var last_color = Color.magenta;
        foreach (var noteController in noteControllers)
        {
            foreach (var instrument in soundDiscsInstruments)
            {
                if (instrument.OwnsEntry(noteController))
                {
                    last_color = instrument.BleepColor;
                    break;
                }
            }

            color_list.Add(last_color); //fill empty spaces with last used color
        }

        return color_list;
    }

    public void Mute(bool mute)
    {
        TryInitialize();
        melody.Mute = mute;
    }

    private void OnBeatChanged(int beat_number)
    {
        var index = beat_number % 16;
        if (noteControllers.Length < index || noteControllers[index] == null)
            return;

        //Debug.Log(index);

        noteControllers[index].OnPlayed();
    }

    void Update()
    {
        for (int i = 0; i < noteControllers.Length; i++)
        {
            melody.SetNote(i, noteControllers[i].GetNote());
        }
    }

    public void LoadData(MelodyData data)
    {
        var data_notes = data.notes;

        if (data_notes.Length != noteControllers.Length)
            throw new InvalidOperationException("Melody length not compatible!");

        for (int i = 0; i < data_notes.Length; i++)
        {
            var contoller = noteControllers[i];
            if (contoller == null)
                continue;

            contoller.SetNoteByIndex(data_notes[i]);
        }
    }

}
