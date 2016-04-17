using UnityEngine;
using System.Collections;
using Assets.Source;
using Miscellaneous.ParameterBoxing.FloatParameter;

/// <summary>
/// Represents one controllable entry of a sounddisk (eg an 8-sounddisk has 8 entries)..
/// </summary>
public class SoundDiscEntry : MonoBehaviour
{
    public AFloatParameter controllingParameter = null;

    [SerializeField]
    private AEntryVisualizer[] visualizers = new AEntryVisualizer[0];

    private Note currentNote;

    public void OnPlayed()
    {
        foreach (var visualizer in visualizers)
        {
            if (visualizer != null)
                visualizer.Highlight();
        }
    }

    public void SetNote(Note note)
    {
        currentNote = note;
    }

    public Note GetNote()
    {
        return currentNote;
    }

}
