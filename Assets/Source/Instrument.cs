using System.Linq;
using UnityEngine;

namespace Assets.Source
{
    public class Instrument : MonoBehaviour
    {
        public string FolderName;

        private Note[] notes;


        public Instrument()
        {
           
        }

        public void Start()
        {
            initialize();
        }

        private void initialize()
        {
            if (notes != null) return;
            notes = Enumerable.Range(0, 8)
                .Select(i => Resources.Load<AudioClip>("Audio/" + FolderName + "/" + i.ToString()))
                .Select(c => new Note() {clip = c})
                .ToArray();
        }

        public Note GetNote(int code)
        {
            initialize();
            return notes[code];
        }


    }
}