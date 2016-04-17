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
            Initialize();
        }

        public void Initialize()
        {
            if (notes != null) return;
            notes = Enumerable.Range(0, 8)
                .Select(i => Resources.Load<AudioClip>("Audio/" + FolderName + "/" + i.ToString()))
                .Select(c => new Note() {clip = c})
                .ToArray();
        }

        public Note GetNote(int code)
        {
            Initialize();
                return notes[code];
        }


    }
}