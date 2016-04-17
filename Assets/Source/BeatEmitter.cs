using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BeatEmitter : MonoBehaviour
{
    [SerializeField]
    private NotePlayer player = null;
    [SerializeField]
    [Tooltip("The value of one beat (1 = 1 full note, 4 = quater note, ...)")]
    public int BeatValue = 1;
    [Serializable]
    public class BeatEventHandler : UnityEvent<int> { }
    public BeatEventHandler OnBeatChanged;

    [SerializeField]
    private bool initializeFromEditorAtStart = false;

    private bool isInitialized;
    private int prevBeatValue = -1;

    void Start()
    {
        if (initializeFromEditorAtStart)
            Initiliaze(player, BeatValue);
    }

    public void Initiliaze(NotePlayer note_player, int beat_value)
    {
        if (isInitialized)
            throw new InvalidOperationException("Can only initialize once!");
        isInitialized = true;

        player = note_player;
        BeatValue = beat_value;
    }

    void Update()
    {
        if (!isInitialized)
            return;

        var current_beat = player.EstimateCurrentBeat() * BeatValue * 0.25f;
        var current_beat_value = Mathf.FloorToInt(current_beat);// - (current_beat % (float)BeatValue));

        //Debug.Log(current_beat_value);

        if (current_beat_value != prevBeatValue)
        {
            prevBeatValue = current_beat_value; //keep in mind that beat not necessarily increments (we may play backwards for instance, or do some scrubbing in the player)
            if (OnBeatChanged != null)
                OnBeatChanged.Invoke(current_beat_value);
        }
    }

}
