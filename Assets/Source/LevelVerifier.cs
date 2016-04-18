using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Source;
using Miscellaneous;
using UnityEngine.UI;

public class LevelVerifier : MonoBehaviour
{
    [SerializeField]
    private NotePlayer player = null;
    [SerializeField]
    private TrackbarController trackbar = null;
    [SerializeField]
    private Text errorCountText = null;
    [SerializeField]
    private Image errorImage = null;
    [SerializeField]
    private Color errorColor = Color.red;
    [SerializeField]
    private Color correctColor = Color.green;

    [SerializeField]
    private GameObject succesButton = null;
    [SerializeField]
    private GameObject returnButton = null;

    [SerializeField]
    private bool InitializeFromEditorData = false;
    [SerializeField]
    [ConditionalHide("InitializeFromEditorData")]
    private SoundDiscMelody bleepsMelody = null;
    [SerializeField]
    [ConditionalHide("InitializeFromEditorData")]
    private SoundDiskInstrument[] bleepInstruments = new SoundDiskInstrument[0];
    [SerializeField]
    [ConditionalHide("InitializeFromEditorData")]
    private GooglyEyesController[] googlies = new GooglyEyesController[0];
    [SerializeField]
    [ConditionalHide("InitializeFromEditorData")]
    private Melody targetMelody = null;




    private int errorCount;

    private int melodyLength;
    private Melody bleepsMelodyNotes;

    private BeatEmitter beatEmitter;

    private bool[] googlyMoods; //true = happy, false = sad

    void Start()
    {
        if (InitializeFromEditorData)
            SetData(bleepsMelody, targetMelody, bleepInstruments, googlies);

        beatEmitter = gameObject.AddComponent<BeatEmitter>();
        beatEmitter.Initiliaze(player, 16); //sounddiskmelody uses 16th notes by definition

        beatEmitter.OnBeatChanged = new BeatEmitter.BeatEventHandler();
        beatEmitter.OnBeatChanged.AddListener(OnBeatChanged);
    }


    public void SetData(SoundDiscMelody bleep_melody, Melody target_melody, SoundDiskInstrument[] bleeps, GooglyEyesController[] googly_eyes)
    {
        bleepsMelody = bleep_melody;
        targetMelody = target_melody;

        bleepInstruments = bleeps;
        googlies = googly_eyes;

        bleepsMelodyNotes = bleepsMelody.melody;
        melodyLength = bleepsMelodyNotes.Length;

        googlyMoods = new bool[googlies.Length];
        for (int i = 0; i < googlyMoods.Length; i++)
        {
            googlyMoods[i] = true;
        }

        StartCoroutine("UpdateTotalErrorCountRoutine");
    }


    void Update()
    {
        if (bleepsMelody == null)
            return;

        if (trackbar.playBleeps)
        {
            for (int i = 0; i < googlyMoods.Length; i++)
            {
                googlies[i].GooglyMood = googlyMoods[i] ? GooglyEyesController.Mood.HAPPY : GooglyEyesController.Mood.SAD; // mood reflects faults
            }
        }
        else
        {
            for (int i = 0; i < googlyMoods.Length; i++)
            {
                googlies[i].GooglyMood = GooglyEyesController.Mood.SUPRISED; // bleeps are listening
            }
        }
    }

    private bool VerifyAtMelodyIndex(int index)
    {
        return bleepsMelodyNotes.Notes.ElementAt(index) == targetMelody.Notes.ElementAt(index);
    }

    private void OnBeatChanged(int beat)
    {
        if (bleepsMelody == null)
            return;

        var beat_16 = beat % 16;

        if (beat_16 == 0)
        {
            for (int i = 0; i < googlyMoods.Length; i++)
            {
                googlyMoods[i] = true;
            }
        }

        var correct = VerifyAtMelodyIndex(beat_16);
        errorImage.color = correct ? correctColor : errorColor;

        var googly_index = GetGooglyIndexForBeat(beat_16);
        if (googly_index >= 0)
            googlyMoods[googly_index] &= correct;
    }

    private int GetGooglyIndexForBeat(int beat_16)
    {
        var entry = bleepsMelody.noteControllers[beat_16];

        if (entry == null)
            return -1;

        for (int i = 0; i < bleepInstruments.Length; i++)
        {
            if (bleepInstruments[i].OwnsEntry(entry))
                return i;
        }

        return -1;
    }

    private IEnumerator UpdateTotalErrorCountRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            errorCount = 0;
            for (int i = 0; i < melodyLength; i++)
            {
                if (!VerifyAtMelodyIndex(i))
                    errorCount++;
            }

            errorCountText.text = errorCount > 0 ? "Something is still sounding off..." : "Perfect melody!";
            succesButton.SetActive(errorCount == 0);
            returnButton.SetActive(errorCount != 0);
        }
    }
}
