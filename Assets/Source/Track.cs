using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source;

public class Track : ScriptableObject
{

    public Melody melody;

    /// <summary>
    /// End point is excluded, start point is included
    /// Unit is in Beats, so start = 1 length = 1 gives all notes between beat 1 (inclusive) to beat 2 (exclusive)
    /// </summary>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public IEnumerable<Note> getNotesForInterval(float start, float end)
    {
        var firstIncludedBeat = (int)Mathf.Ceil(start);
        var lastIncludedBeat = (int)Mathf.Floor(end); 

        if (lastIncludedBeat == end) lastIncludedBeat --; // Exclude end

        //Debug.Log(firstIncludedBeat + " - " + lastIncludedBeat);

        // Go over each beat
        for (int i = firstIncludedBeat; i <= lastIncludedBeat; i++)
        {
            var note = melody.GetNote(i%melody.Length);
            if (note != null) yield return note;
        }
    }
}
