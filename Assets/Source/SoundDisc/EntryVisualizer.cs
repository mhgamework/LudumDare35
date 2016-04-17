using UnityEngine;
using System.Collections;

public abstract class AEntryVisualizer : MonoBehaviour
{
    /// <summary>
    /// Highlight the current entry (for a short time)
    /// </summary>
    public abstract void Highlight();
}

public class EntryVisualizer : AEntryVisualizer
{
    public override void Highlight()
    {
        throw new System.NotImplementedException();
    }
}
