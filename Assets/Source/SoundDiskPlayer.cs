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
        public NotePlayer player;

        private Track track;
        private Note[] notes;

        [SerializeField] private Instrument instrument;

        public void Start()
        {
            notes = Enumerable.Range(0, 8)
                .Select(i => Resources.Load<AudioClip>("Audio/Piano/" + i.ToString()))
                .Select(c => new Note() { clip = c })
                .ToArray();

            if (track == null)
            {
                track = new Track();
                player.tracks.Add(track);
            }

            track.melody = new Melody(4);


        }

        public void OnValidate()
        {
            foreach (var val in values)
            {
                if (val == null) continue;
                Debug.Log(val);
                val.RegisterObserver(this);
            }
        }



        public void NotifyParameterChanged(AFloatParameter parameter, float value)
        {
            if (track == null) return;
            var note_index = (int) Mathf.Floor((value/100)*7);

            //track.melody.SetNote(values.IndexOf(parameter), notes[note_index]);

            track.melody.SetNote(values.IndexOf(parameter),  instrument.GetNote(note_index));
        }
    }
}