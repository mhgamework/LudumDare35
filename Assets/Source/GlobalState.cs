using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Source
{
    public class GlobalState : Singleton<GlobalState>
    {
        public bool StartLevelSolved = false;
        public string LastLevelName = null;
        public bool LastLevelSolved = false;
        public List<MelodyData> CompletedMelodies = new List<MelodyData>();


        public void MarkLevelCompleted(string levelName, MelodyData melody)
        {
            LastLevelName = levelName;
            LastLevelSolved = true;
            CompletedMelodies.Add(melody);
        }

        public void MarkLevelFailed(string levelName, MelodyData melody)
        {
            //Nothing as of now
        }

        public void Start()
        { 
        }


    }
}