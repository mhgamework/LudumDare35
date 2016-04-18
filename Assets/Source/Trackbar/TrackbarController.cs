using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Source;
using Miscellaneous.ObjectPooling;
using UI;
using UnityEngine.UI;

public class TrackbarController : MonoBehaviour
{
    [SerializeField]
    private ObjectPoolComponent trackbarPartPool = null;
    [SerializeField]
    private RectTransform trackbarPartContainer = null;
    [SerializeField]
    private Transform scrubberTransform = null;
    [SerializeField]
    private InputCaptor inputCaptor = null;
    [SerializeField]
    private NotePlayer player = null;

    [SerializeField]
    private Toggle muteToggle = null;

    [SerializeField]
    private SoundDiscMelody debugMelody = null;

    private List<PooledObject> currentPooledObjects = new List<PooledObject>();

    [SerializeField]
    private float displayedBeats;
    private SoundDiscMelody bleepsMelody;
    private Melody targetMelody;

    private GooglyEyesController[] googlies;
    public bool playBleeps { get; private set; }
    private bool playTarget;


    void Start()
    {
        if (debugMelody != null)
        {
            VisualizeMelody(debugMelody);
            bleepsMelody = debugMelody;
        }
    }

    public void PlayBleeps(bool play_bleeps)
    {
        playBleeps = play_bleeps;

        UpdatePause();
        UpdateMute();
    }

    public void PlayTarget(bool play_target)
    {
        playTarget = play_target;

        UpdatePause();
        UpdateMute();
    }

    private void UpdatePause()
    {
        if (!playTarget && !playBleeps)
        {
            player.StopPlaying();
        }
        else
        {
            player.StartPlaying(player.EstimateCurrentBeat());
        }
    }

    private void UpdateMute()
    {
        if (bleepsMelody != null)
            bleepsMelody.Mute(!playBleeps);
        if (targetMelody != null)
            targetMelody.Mute = !playTarget;
    }

    void Update()
    {
        if (displayedBeats <= 0)
            return;

        if (Input.GetMouseButton(0) && inputCaptor.HasFocus())
        {
            var mouse_pos = Input.mousePosition;
            var normalized_mouse_pos = Mathf.Clamp01(mouse_pos.x / Screen.width);
            var scrub_beat = displayedBeats * normalized_mouse_pos;
            player.Scrub(scrub_beat);
        }

        var current_scaling = GetComponent<Transform>().localScale.x;

        var scrubber_min_pos = -Screen.width * 0.5f / current_scaling;
        var scrubber_max_pos = Screen.width * 0.5f / current_scaling;

        scrubberTransform.localPosition = new Vector3(scrubber_min_pos + GetNormalizedScrubberPosition() * (scrubber_max_pos - scrubber_min_pos), scrubberTransform.localPosition.y, scrubberTransform.localPosition.z);


    }

    public void VisualizeMelody(SoundDiscMelody melody)
    {
        foreach (var pooledObject in currentPooledObjects)
        {
            pooledObject.GameObject.SetActive(false);
            pooledObject.Release();
        }
        currentPooledObjects.Clear();

        foreach (var color in melody.GetTrackColors())
        {
            var new_object = trackbarPartPool.FetchPooledObject();
            new_object.GetComponent<ColorSetter>().SetColor(color);
            new_object.GetComponent<Transform>().SetParent(trackbarPartContainer);
            new_object.GameObject.SetActive(true);

            currentPooledObjects.Add(new_object);
        }

        displayedBeats = currentPooledObjects.Count * 0.25f;
    }

    public float GetNormalizedScrubberPosition()
    {
        //the trackbar displays beats 0 to displayedBeats
        var current_player_beat = player.EstimateCurrentBeat();

        //Debug.Log(current_player_beat);

        var normalized = Mathf.Clamp01(current_player_beat / displayedBeats);

        //Debug.Log(normalized);

        return normalized;
    }

    public void LoadData(SoundDiscMelody bleeps_melody, Melody target_melody)
    {
        bleepsMelody = bleeps_melody;
        VisualizeMelody(bleeps_melody);

        targetMelody = target_melody;

        UpdateMute();
    }

    public void SetGooglies(GooglyEyesController[] bleepsGooglyEyes)
    {
        googlies = bleepsGooglyEyes;
    }
}
