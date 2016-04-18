using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source
{
    public class Melody : MonoBehaviour
    {
        public bool Mute;

        public void SetLength(int length)
        {
            Length = length;
            notes = new Note[length];
        }

        public int Length { get; set; }

        private Note[] notes;

        public void SetNote(int num, Note note)
        {
            notes[num] = note;
        }

        public Note GetNote(int num)
        {
            return notes[num];
        }

        public IEnumerable<Note> Notes { get { return notes; } }

        public static void LoadsNotesToMelody(ref Melody to_melody, MelodyData melodyData, Instrument instrument)
        {
            to_melody.SetLength(melodyData.notes.Length);
            for (int i = 0; i < melodyData.notes.Length; i++)
            {
                Note note = null;
                var key = melodyData.notes[i];
                if (key != -1)
                    note = instrument.GetNote(key);
                to_melody.SetNote(i, note);
            }
        }
    }
}