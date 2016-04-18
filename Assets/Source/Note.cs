using UnityEngine;

namespace Assets.Source
{
    public class Note
    {
        private AudioClip internal_clip;
        public AudioClip clip
        {
            get
            {
                if (internal_clip == null)
                    internal_clip = Instrument.extractSample(instrument.Samples, key*0.5f, 0.5f, 90);
                   return internal_clip;
            }
        }

        public Instrument instrument;
        public int key;
    }
}