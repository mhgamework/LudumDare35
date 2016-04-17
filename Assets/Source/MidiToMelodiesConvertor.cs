using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Multimedia.Midi;
using UnityEngine;

namespace Assets.Source
{
    public class MidiToMelodiesConvertor : MonoBehaviour, IMidiMessageVisitor
    {
        [SerializeField] private NotePlayer player;
        [SerializeField]
        private Instrument funk;
        [SerializeField]
        private Instrument wurly;

        [SerializeField]
        private Instrument bass;

        [SerializeField]
        private Instrument kit;

        public void Start()
        {
            reader = new MidiFileReader(@"C:\_Speedy\LudumDare35\Assets\Audio\Soundtrack01\LD35\LD35.mid");


            trackNames = getTrackNames(reader);

            division = reader.Sequence.Division;
            Debug.Log("Division: " + division);
            foreach (var n in trackNames)
                Debug.Log(n);



            //print = true;

            StartCoroutine(play(extractMelody(getTrack("Wurly1")), wurly).GetEnumerator());
            StartCoroutine(play(extractMelody(getTrack("Wurly2")), wurly).GetEnumerator());
            StartCoroutine(play(extractMelody(getTrack("Kit")), kit).GetEnumerator());
            StartCoroutine(play(extractMelody(getTrack("Acoustic Bass")), bass).GetEnumerator());

            StartCoroutine(play(extractMelody(getTrack("FunkMaster")), funk).GetEnumerator());
        }

        private Multimedia.Midi.Track getTrack( string name)
        {
            return reader.Sequence[trackNames.ToList().IndexOf(name)];
        }

        private int[] extractMelody(Multimedia.Midi.Track funkTrack)
        {
            visitTrack(funkTrack);

            var max = noteOns.Keys.Max();

            var melody = Enumerable.Range(0, max + 1).Select(i =>
            {
                if (!noteOns.ContainsKey(i))
                    return 0;
                return noteOns[i];
            }).ToArray();
            return melody;
        }

        public void Awake()
        {

        }

        public void OnEnable()
        {
           

            
        }

        public IEnumerable<YieldInstruction> play(int[] melody, Instrument instrument1)
        {
            for (int i = 16*80; i < melody.Length; i++)
            {
                yield return new WaitForSeconds(60/tempo*(1/4f));
                var note = melody[i];
                if (note == 0) continue;
                player.PlayNote(instrument1.GetNote(melody[i]));

            }
        }

        private IEnumerable<string> getTrackNames(MidiFileReader reader)
        {
            for (int i = 0; i < reader.Sequence.Count; i++)
            {
                var track = reader.Sequence[i];
                trackName = "Unkown";

                visitTrack(track);
                yield return trackName;

            }
            yield break;
        }

        private void visitTrack(Multimedia.Midi.Track track)
        {
            //pressedSince.Clear();
            noteOns.Clear();
            ticks = 0;
            foreach (var e in track)
            {
                ticks += e.Ticks;
                e.Message.Accept(this);
            }
        }

        private string trackName;

        private bool print = false;
        private int ticks;
        private float tempo;
        private int division;

        private Dictionary<int, int> pressedSince = new Dictionary<int, int>();

        private Dictionary<int,int> noteOns = new Dictionary<int,int>();
        private IEnumerable<string> trackNames;
        private MidiFileReader reader;


        public void Visit(ChannelMessage message)
        {
            var sixtheents = (int)Mathf.Round( (ticks/(float)division) * 4);
            if (message.Command == ChannelCommand.NoteOn)
            {
                if (noteOns.ContainsKey(sixtheents) && noteOns[sixtheents] != message.Data1)
                    Debug.LogWarning("Input file has overlapping note at " + sixtheents + " kOld:" + noteOns[sixtheents] +
                                     " k:" + message.Data1);
                noteOns[sixtheents] = message.Data1;
                if (print)
                    Debug.Log("On:  t: "+ sixtheents + " k: "+ message.Data1 + " v:" + message.Data2 + " c:" +message.MidiChannel);
            }
            else if (message.Command == ChannelCommand.NoteOff)
            {
                if (print)
                    Debug.Log("Off: t: " + sixtheents + " k: " + message.Data1 + " v:" + message.Data2 + " c:" + message.MidiChannel);
            }
            else
            if (print)
            Debug.Log(message.Command);
        }

        public void Visit(MetaMessage message)
        {
            if (message.Type == MetaType.TrackName)
            {
                var buf = new byte[message.Length];
                for (int i = 0; i < buf.Length; i++)
                {
                    buf[i] = message[i];
                }
                trackName = Encoding.ASCII.GetString(buf);
            }
            if (message.Type == MetaType.Tempo)
            {
                tempo = 60/(MetaMessage.PackTempo(message) /1000f/1000f);
                Debug.Log("Tempo: " + tempo);
            }
            if (print)
                Debug.Log(message.Type);
        }

        public void Visit(SysCommonMessage message)
        {
        }

        public void Visit(SysExMessage message)
        {
        }

        public void Visit(SysRealtimeMessage message)
        {
        }
    }
}