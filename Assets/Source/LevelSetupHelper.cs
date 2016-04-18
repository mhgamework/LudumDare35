using UnityEngine;
using System.Collections;
using Assets.Source;

public class LevelSetupHelper : MonoBehaviour
{
    [SerializeField]
    private MelodyData targetMelodyData = null;
    [SerializeField]
    private Instrument targetInstrument = null;
    [SerializeField]
    private NotePlayer player = null;

    [SerializeField]
    private MelodyData initialMelodyData = null;
    [SerializeField]
    private SoundDiscMelody bleepsMelody = null;

    [SerializeField]
    private SoundDiskInstrument[] bleepsInstruments = new SoundDiskInstrument[0];
    [SerializeField]
    private GooglyEyesController[] bleepsGooglyEyes = new GooglyEyesController[0];

    [SerializeField]
    private TrackbarController trackbar = null;
    [SerializeField]
    private LevelVerifier verifier = null;
    [SerializeField]
    private LevelController levelController = null;

    private Melody targetMelody;
    private Track targetTrack;

    void Start()
    {
        StartCoroutine("SetupRoutine");
    }

    private IEnumerator SetupRoutine()
    {
        yield return null;
        yield return null;
        yield return null;
        //wait a few frames for object initialization to complete

        foreach (var soundDiskInstrument in bleepsInstruments)
        {
            soundDiskInstrument.EnablePlayerSoundFeedback = false; //disable sound feedback while setting initial bleeps state
        }

        targetMelody = gameObject.AddComponent<Melody>();
        Melody.LoadsNotesToMelody(ref targetMelody, targetMelodyData, targetInstrument);

        targetTrack = new Track { melody = targetMelody };
        player.tracks.Add(targetTrack);

        bleepsMelody.LoadData(initialMelodyData);

        trackbar.SetGooglies(bleepsGooglyEyes);
        trackbar.LoadData(bleepsMelody, targetMelody);

        verifier.SetData(bleepsMelody, targetMelody, bleepsInstruments, bleepsGooglyEyes);

        levelController.MelodyData = targetMelodyData;

        foreach (var soundDiskInstrument in bleepsInstruments)
        {
            soundDiskInstrument.EnablePlayerSoundFeedback = true;
        }
    }

}
