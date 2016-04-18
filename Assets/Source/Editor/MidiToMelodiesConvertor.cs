using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Multimedia.Midi;
using UnityEditor;
using UnityEngine;
using Track = Multimedia.Midi.Track;

namespace Assets.Source
{
    public class MidiToMelodiesConvertor : IMidiMessageVisitor
    {
        [SerializeField]
        private NotePlayer player;
        [SerializeField]
        private Instrument funk;
        [SerializeField]
        private Instrument wurly;

        [SerializeField]
        private Instrument bass;

        [SerializeField]
        private Instrument kit;

        public bool generate = true;

        public void Start()
        {
            Debug.Log("Reading midi");
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayProgressBar("Generating Melodies", "Reading midi file", 0);
            reader = new MidiFileReader(@"C:\_Speedy\LudumDare35\Assets\Audio\Soundtrack01\LD35\LD35.mid");


            EditorUtility.DisplayProgressBar("Generating Melodies", "Extracting track names", 0.2f);
            trackNames = getTrackNames(reader);
            division = reader.Sequence.Division;
            Debug.Log("Division: " + division);
            foreach (var n in trackNames)
                Debug.Log(n);


            EditorUtility.DisplayProgressBar("Generating Melodies", "Reading tracks", 0.5f);
            var kitTrack = writeTrack(extractMelody(getTrack("Kit")),"Kit");
            var wurly1Track = writeTrack(extractMelody(getTrack("Wurly1")),"Wurly1");
            var wurly2Track = writeTrack(extractMelody(getTrack("Wurly2")), "Wurly2");
            var bassTrack = writeTrack(extractMelody(getTrack("FunkMaster")), "FunkMaster");

            EditorUtility.DisplayProgressBar("Generating Melodies", "Writing melody assets", 0.7f);


            saveMelody(bassTrack, 92, 16, "BassA");
            saveMelody(bassTrack, 96, 16, "BassB");

            saveMelody(kitTrack, 92, 8, "RithmAC");
            saveMelody(kitTrack, 96, 8, "RithmB");
            saveMelody(kitTrack, 100, 8, "RithmD");
            saveMelody(kitTrack, 104, 16, "HiHat");
            saveMelody(kitTrack, 108, 32, "CrashAndCo");

            

            saveMelody(wurly1Track, 92, 16, "Melody1A");
            saveMelody(wurly1Track, 96, 16, "Melody1B");

            saveMelody(wurly2Track, 92, 16, "Melody2A");
            saveMelody(wurly2Track, 96, 16, "Melody2B");


            saveMelody(wurly2Track, 92, 16, "Melody2A");
            saveMelody(wurly2Track, 96, 16, "Melody2B");

            EditorUtility.ClearProgressBar();


            //foreach(var m in GetBleepMelodies(extractMelody(getTrack("Kit")),kit,() => gameObject.AddComponent<Melody>()))
            //{
            //    Debug.Log("Melody for kit");
            //}


            ////print = true;

            //StartCoroutine(play(extractMelody(getTrack("Wurly1")), wurly).GetEnumerator());
            //StartCoroutine(play(extractMelody(getTrack("Wurly2")), wurly).GetEnumerator());
            //StartCoroutine(play(extractMelody(getTrack("Kit")), kit).GetEnumerator());
            //StartCoroutine(play(extractMelody(getTrack("Acoustic Bass")), bass).GetEnumerator());

            //StartCoroutine(play(extractMelody(getTrack("FunkMaster")), funk).GetEnumerator());
        }

        private int[] writeTrack(int[] track,string name)
        {
            var dir = Directory.CreateDirectory(@"C:\_Speedy\LudumDare35\Assets\Audio\Soundtrack01\LD35\MIDI");
            File.WriteAllText(dir.FullName + @"\" + name + ".txt",track.Select((val,i) => i + " - " + val).Aggregate((el,acc) => (el + "\n" + acc)));

            return track;

        }

        private void saveMelody(int[] track, int startMeasure, int length, string name)
        {
            var m = ScriptableObject.CreateInstance<MelodyData>();
            m.notes = new int[length];
            for (int i = 0; i < length; i++)
            {
                var trackIndex = startMeasure * 16 + i;

                int val = -1;
                if (trackIndex < track.Length)
                    val = track[trackIndex];

                m.notes[i] = val;
            }
            if (!generate)
                return;

            var path = "Assets/Generated/MelodyData";

            //var class_name = typeof(T).ToString().Split('.').Last();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".asset");

            AssetDatabase.CreateAsset(m, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            EditorUtility.SetDirty(m);
            Selection.activeObject = m;

        }


        public IEnumerable<Melody> GetBleepMelodies(int[] melody, Instrument instr, Func<Melody> createMelody)
        {
            int current = 92;

            for (;;)
            {
                if (melody.Skip(current * 16).Take(16).All(i => i == 0))
                    yield break;

                var m = createMelody();
                m.SetLength(16);
                for (int i = current * 16; i < (current + 1) * 16; i++)
                {
                    m.SetNote(i - (current * 16), melody[i] == 0 ? null : instr.GetNote(melody[i]));
                }

                yield return m;
                current += 4;
            }
        }

        private Multimedia.Midi.Track getTrack(string name)
        {
            var indexOf = trackNames.ToList().IndexOf(name);
            if (indexOf == -1)
                throw new Exception("Track not found: " + name);
            return reader.Sequence[indexOf];
        }

        private int[] extractMelody(Multimedia.Midi.Track funkTrack)
        {
            visitTrack(funkTrack);

            var max = noteOns.Keys.Max();

            var melody = Enumerable.Range(0, max + 1).Select(i =>
            {
                if (!noteOns.ContainsKey(i))
                    return -1;
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
            for (int i = 16 * 80; i < melody.Length; i++)
            {
                yield return new WaitForSeconds(60 / tempo * (1 / 4f));
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

        private Dictionary<int, int> noteOns = new Dictionary<int, int>();
        private IEnumerable<string> trackNames;
        private MidiFileReader reader;


        public void Visit(ChannelMessage message)
        {
            var sixtheents = (int)Mathf.Round((ticks / (float)division) * 4);
            if (message.Command == ChannelCommand.NoteOn)
            {
                if (noteOns.ContainsKey(sixtheents) && noteOns[sixtheents] != message.Data1)
                    Debug.LogWarning("Input file has overlapping note at " + sixtheents + " kOld:" + noteOns[sixtheents] +
                                     " k:" + message.Data1);
                noteOns[sixtheents] = message.Data1;
                if (print)
                    Debug.Log("On:  t: " + sixtheents + " k: " + message.Data1 + " v:" + message.Data2 + " c:" + message.MidiChannel);
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
                tempo = 60 / (MetaMessage.PackTempo(message) / 1000f / 1000f);
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