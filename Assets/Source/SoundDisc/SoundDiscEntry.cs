using UnityEngine;
using System.Collections;
using Assets.Source;
using Miscellaneous.ParameterBoxing.FloatParameter;
using UI;

/// <summary>
/// Represents one controllable entry of a sounddisk (eg an 8-sounddisk has 8 entries)..
/// </summary>
public class SoundDiscEntry : MonoBehaviour
{
    public AFloatParameter controllingParameter = null;

    [SerializeField]
    public SliderController sliderController = null;

    [SerializeField]
    private AEntryVisualizer[] visualizers = new AEntryVisualizer[0];

    private Note currentNote;

    private SoundDiskInstrument instrument;

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

    public void SetNoteByIndex(int note_index)
    {
        var param_value = instrument.GetParameterValueForNote(note_index);
        if (param_value > 0)
            AlterParameter(param_value);
    }

    public Note GetNote()
    {
        return currentNote;
    }

    public void SetInstrument(SoundDiskInstrument instrument)
    {
        this.instrument = instrument;
    }

    private void AlterParameter(float new_value)
    {
        controllingParameter.SetValue(new_value);
    }

    public void SetSnapValue(float value)
    {
        sliderController.SetSnapValue(value);
    }

}
