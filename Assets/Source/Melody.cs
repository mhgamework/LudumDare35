using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source
{
    public class Melody
    {

        public Melody( int length)
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
    }
}