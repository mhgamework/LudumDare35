using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Source
{
    public class MainMenuController : MonoBehaviour
    {
        public List<InstrumentTrack> tracks;
        public NotePlayer player;

        private Dictionary<MelodyData, List<Track>> tracksForMelody = new Dictionary<MelodyData, List<Track>>();

        public void Start()
        {

            addSongToPlayer();


            readLevelResult();




        }

        private void addSongToPlayer()
        {
            foreach (var t in tracks)
            {
                for (int iMelody = 0; iMelody < t.melodies.Count; iMelody++)
                {
                    var m = t.melodies[iMelody];
                    if (m == null) continue;

                    var track = new Track();
                    track.melody = Melody.FromNoteMelody2(m, t.instrument);
                    track.TrackStart = iMelody * 2;
                    track.TrackEnd = track.TrackStart + track.melody.Length / 4f;

                    player.tracks.Add(track);

                    track.Mute = true;

                    List<Track> list;
                    if (!tracksForMelody.TryGetValue(m, out list))
                    {
                        list = new List<Track>();
                        tracksForMelody[m] = list;
                    }
                    list.Add(track);

                }
            }

        }



        private void readLevelResult()
        {
            if (GlobalState.Instance.LastLevelName == null) return;

            foreach (var l in FindObjectsOfType<Text>().Where(t => t.text == GlobalState.Instance.LastLevelName))
            {
                l.text = "done";

                foreach (var t in tracksForMelody.Values.SelectMany(t => t))
                    t.Mute = true;

                foreach (var m in GlobalState.Instance.CompletedMelodies)
                {
                    if (tracksForMelody.ContainsKey(m))
                        foreach (var t in tracksForMelody[m])
                            t.Mute = false;
                }

             

            }
        }


        public void OpenLevel(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }



        [Serializable]
        public class InstrumentTrack
        {
            public Instrument instrument;
            public List<MelodyData> melodies;
        }


    }
}