using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source
{
    public class NotePlayerTest : MonoBehaviour
    {
        public NotePlayer player;
        private Note[] notes;

        // Use this for initialization
        void Start()
        {
            notes = Enumerable.Range(1, 8)
                .Select(i => Resources.Load<AudioClip>("Audio/Piano/" + i.ToString()))
                .Select(c => new Note() { clip = c})
                .ToArray();


            var m = new Melody(8);
            var ip = 0;
            foreach (var n in notes)
            {
                m.SetNote(ip, n);
                ip++;
            }


            m = new Melody(8);

            m.SetNote(0, notes[5]);
            m.SetNote(2, notes[3]);
            m.SetNote(4, notes[4]);
            m.SetNote(6, notes[2]);

            //player.tracks.Add(new Track() {melody = m});
        }


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
                player.audioSource.PlayNote(notes[0].clip);
            if (Input.GetKeyDown(KeyCode.Keypad2))
                player.audioSource.PlayNote(notes[1].clip);
            if (Input.GetKeyDown(KeyCode.Keypad3))
                player.audioSource.PlayNote(notes[2].clip);
            if (Input.GetKeyDown(KeyCode.Keypad4))
                player.audioSource.PlayNote(notes[3].clip);
            if (Input.GetKeyDown(KeyCode.Keypad5))
                player.audioSource.PlayNote(notes[4].clip);
            if (Input.GetKeyDown(KeyCode.Keypad6))
                player.audioSource.PlayNote(notes[5].clip);
            if (Input.GetKeyDown(KeyCode.Keypad7))
                player.audioSource.PlayNote(notes[6].clip);
            if (Input.GetKeyDown(KeyCode.Keypad8))
                player.audioSource.PlayNote(notes[7].clip);
        }

    }
}