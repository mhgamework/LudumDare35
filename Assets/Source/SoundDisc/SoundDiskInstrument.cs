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

        public Color BleepColor = Color.blue;

        public NotePlayer player;

        private Dictionary<AFloatParameter, SoundDiscEntry> entryMap = new Dictionary<AFloatParameter, SoundDiscEntry>();
        private Note prevFeedbackedNote = null;

        private bool isInitialized;

        void Awake()
        {
            foreach (var soundDiscEntry in entries)
            {
                soundDiscEntry.SetInstrument(this);
                soundDiscEntry.controllingParameter.RegisterObserver(this);
                entryMap.Add(soundDiscEntry.controllingParameter, soundDiscEntry);
            }
        }

        void Start()
        {
            instrument.Initialize();
        }

        public void NotifyParameterChanged(AFloatParameter parameter, float value)
        {
            if (noteIndices.Length == 0) return;
            var note_index_index = (value / 100) * noteIndices.Length;
            var note_index = noteIndices[Mathf.FloorToInt(note_index_index)];

            SoundDiscEntry entry;
            if (!entryMap.TryGetValue(parameter, out entry))
                return;

            var note = GetNote(note_index);
            entry.SetNote(note);

            if (note == null)
                return;

            if (note != prevFeedbackedNote)
            {
                player.audioSource.PlaySample(note.clip); //play note instantly for user feedback
                prevFeedbackedNote = note;
            }
        }

        public bool OwnsEntry(SoundDiscEntry entry)
        {
            return entries.Contains(entry);
        }

        private Note GetNote(int index)
        {
            if (index < 0)
                return null;

            return instrument.GetNote(index);
        }

        public float GetParameterValueForNote(int note_index)
        {
            var note_index_index = -1;
            for (int i = 0; i < noteIndices.Length; i++)
            {
                if (note_index == noteIndices[i])
                {
                    note_index_index = i;
                    break;
                }
            }

            if (note_index_index < 0)
                return -1;

            var param_value = ((note_index_index + 0.5f) / (float)noteIndices.Length) * 100;
            return param_value;
        }
    }
}