using System;
using System.Linq;
using UnityEngine;

namespace Assets.Source
{
    public class Instrument : MonoBehaviour
    {
        [Obsolete]
        public string FolderName;

        public AudioClip Samples;


        private Note[] notes;


        private int lowestNoteNum = 0;//60 - 1 * 12;
        private int numOctaves = 7;

        public Instrument()
        {

        }

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (notes != null) return;

            notes = Enumerable.Range(0, 12 * numOctaves).Select(i => extractSample(Samples, i * 0.5f, 0.5f, 90))
                .Select(c => new Note() { clip = c })
                .ToArray();
        }

        private AudioClip extractSample(AudioClip instrumentSamples, float startBeat, float lengthBeat, float bpm)
        {
            var secondsOffset = startBeat / bpm * 60;
            var secondsLength = lengthBeat / bpm * 60;

            var partLength = (int)(secondsLength * instrumentSamples.frequency);

            var clip = AudioClip.Create("subpart", partLength, instrumentSamples.channels, instrumentSamples.frequency, false);

            float[] smp1 = new float[(partLength) * instrumentSamples.channels];
            instrumentSamples.GetData(smp1, (int)(secondsOffset * instrumentSamples.frequency));
            clip.SetData(smp1, 0);

            return clip;
        }

        public Note GetNote(int code)
        {
            Initialize();

            code -= lowestNoteNum;
            if (code < 0 || code >= notes.Length)
            {
                Debug.Log("Note out of range! " + (code + lowestNoteNum));
                return notes[0];
            }
            

            return notes[code ];
        }


    }
}