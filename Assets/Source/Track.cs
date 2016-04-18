using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source;

public class Track
{

    public Melody melody;
    public float TrackStart = 0;

    public float TrackEnd = float.MaxValue;
    public bool Mute { get; set; }

    /// <summary>
    /// End point is excluded, start point is included
    /// Unit is in Beats, so start = 1 length = 1 gives all notes between beat 1 (inclusive) to beat 2 (exclusive)
    /// </summary>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public IEnumerable<Note> getNotesForInterval(float start, float end)
    {
        if (Mute)
            yield break;
        if (melody.Mute)
            yield break;

        // Apply offset
        start -= this.TrackStart;
        end -= this.TrackStart;

        start = Mathf.Max(0, start); // Trick: move start later or end earlier, so that we only have a valid interval while inside [trackstart,trackEnd)
        end = Mathf.Min(TrackEnd - TrackStart, end);

        // Melodies are now in 1/4 of a beat
        start *= 4;
        end *= 4;

        var firstIncludedBeat = (int)Mathf.Ceil(start);
        var lastIncludedBeat = (int)Mathf.Floor(end);

        if (lastIncludedBeat == end) lastIncludedBeat--; // Exclude end

        //Debug.Log(firstIncludedBeat + " - " + lastIncludedBeat);

        // Go over each beat
        for (int i = firstIncludedBeat; i <= lastIncludedBeat; i++)
        {
            var note = melody.GetNote(mod(i , melody.Length));
            if (note != null) yield return note;
        }
    }

    float nfmod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
