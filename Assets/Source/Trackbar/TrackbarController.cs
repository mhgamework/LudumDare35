﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Miscellaneous.ObjectPooling;
using UI;

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
    private SoundDiscMelody debugMelody = null;

    private List<PooledObject> currentPooledObjects = new List<PooledObject>();

    private float displayedBeats;

    void Start()
    {
        if (debugMelody != null)
            VisualizeMelody(debugMelody);
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

        var scrubber_min_pos = -Screen.width * 0.5f;
        var scrubber_max_pos = Screen.width * 0.5f;

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


}
