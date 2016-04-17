using UnityEngine;
using System.Collections;
using Assets.Source;

public class Enemy : MonoBehaviour
{
    public NotePlayer player;

    public Instrument instrument;

    private Track track;

	// Use this for initialization
	void Start () {
	    if (track == null)
	    {
	        track = new Track();
	        track.melody = new Melody(8);
	        player.tracks.Add(track);

	        setRandom();
	    }


	}

    private void setRandom()
    {
        
        track.melody.SetNote(6 - 2, instrument.GetNote(r.Next(0,7)));
        track.melody.SetNote(7 - 2, instrument.GetNote(r.Next(0, 7)));
        track.melody.SetNote(8 - 2, instrument.GetNote(r.Next(0, 7)));
        track.melody.SetNote(9 - 2, instrument.GetNote(r.Next(0, 7)));
    }

    private System.Random r = new System.Random();
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.R))
	    {
	        setRandom();
	    }
	
	}


}
