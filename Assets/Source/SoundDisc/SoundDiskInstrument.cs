using System.Collections.Generic;
using System.Linq;
using Miscellaneous.ParameterBoxing.FloatParameter;
using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Sets the notes of each sounddisk entry based on the user input and the current instrument.
    /// </summary>
    public class SoundDiskInstrument : MonoBehaviour, IFloatParameterObserver
    {
        [SerializeField]
        private List<SoundDiscEntry> entries;
        [SerializeField]
        [Tooltip("Indices of the notes, ordered low to high (in the order they appear on the sounddisc). Negative values are muted.")]
        private int[] noteIndices = new int[0];
        [SerializeField]
        private Instrument instrument;

        public NotePlayer player;

        private Dictionary<AFloatParameter, SoundDiscEntry> entryMap = new Dictionary<AFloatParameter, SoundDiscEntry>();
        private Note prevFeedbackedNote = null;

        public void Start()
        {
            instrument.Initialize();
            foreach (var entry in entries)
            {
                entry.controllingParameter.RegisterObserver(this);
                entryMap.Add(entry.controllingParameter, entry);
            }
        }

        public void NotifyParameterChanged(AFloatParameter parameter, float value)
        {
            if (noteIndices.Length == 0) return;
            var note_index = noteIndices[Mathf.FloorToInt((value / 100) * noteIndices.Length)];

            SoundDiscEntry entry;
            if (!entryMap.TryGetValue(parameter, out entry))
                return;

            if (note_index < 0)
            {
                entry.SetNote(null);
                return;
            }

            var note = instrument.GetNote(note_index);
            if (note != prevFeedbackedNote)
            {
                player.audioSource.PlaySample(note.clip); //play note instantly for user feedback
                prevFeedbackedNote = note;
            }

            entry.SetNote(note);
        }
    }
}