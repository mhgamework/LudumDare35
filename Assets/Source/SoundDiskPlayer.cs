using System.Collections.Generic;
using System.Linq;
using Miscellaneous.ParameterBoxing.FloatParameter;
using UnityEngine;

namespace Assets.Source
{
    public class SoundDiskPlayer : MonoBehaviour, IFloatParameterObserver
    {
        [SerializeField]
        private List<AFloatParameter> values;
        [SerializeField]
        [Tooltip("Indices of the notes, ordered low to high (in the order they appear on the sounddisc). Negative values are muted.")]
        private int[] noteIndices = new int[0];

        public NotePlayer player;
        private Track track;

        [SerializeField]
        private Instrument instrument;

        public void Start()
        {
            instrument.Initialize();

            if (track == null)
            {
                track = new Track();
                player.tracks.Add(track);
            }

            track.melody = new Melody(values.Count);

            foreach (var val in values)
            {
                val.RegisterObserver(this);
            }
        }

        public void NotifyParameterChanged(AFloatParameter parameter, float value)
        {
            if (track == null || noteIndices.Length == 0) return;
            var note_index = noteIndices[Mathf.FloorToInt((value / 100) * noteIndices.Length)];

            if (note_index < 0)
            {
                track.melody.SetNote(values.IndexOf(parameter), null);
                return;
            }

            track.melody.SetNote(values.IndexOf(parameter), instrument.GetNote(note_index));
        }
    }
}