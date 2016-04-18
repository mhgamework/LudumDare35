using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source
{
    public class GlobalState : Singleton<GlobalState>
    {
        public bool StartLevelSolved = false;
        public string LastLevelName = null;
        public bool LastLevelSolved = false;
        public List<MelodyData> CompletedMelodies = new List<MelodyData>();


        public void Start()
        { 
        }


    }
}