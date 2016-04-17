using Multimedia.Midi;
using UnityEngine;

namespace Assets.Source
{
    public class MidiToMelodiesConvertor : MonoBehaviour
    {
        public void Awake()
        {

        }

        public void OnEnable()
        {
            var reader = new MidiFileReader(@"C:\_Speedy\LudumDare35\Assets\Audio\Soundtrack01\LD35\LD35.mid");


            int a = 5;

        }
    }
}