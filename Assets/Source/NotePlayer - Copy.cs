using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MelodyPlayer : MonoBehaviour
{
    
    [SerializeField]
    private List<AudioSource> sources;

    private AudioClip[] clips;

    public float bpm = 60;

    // Use this for initialization
    void Start()
    {
        clips = Enumerable.Range(1, 8).Select(i => Resources.Load<AudioClip>("Audio/Piano/" + i.ToString())).ToArray();
        foreach (var audioSource in sources)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
        

        StartCoroutine(playNotes().GetEnumerator());
    }

    private IEnumerable<YieldInstruction> playNotes()
    {
        var current = 0;
        for (;;)
        {
            PlayNote(current);
            current = (current + 1)%8;
            yield return new WaitForSeconds(1 / (bpm / 60));
        }

    }



    // Update is called once per frame
    void Update()
    {

    }

    public void PlayNote(int num)
    {
        if (sources.Count == 0) return;
        AudioSource bestToKill = sources[0];
        var factor = float.MaxValue;
        foreach (var src in sources)
        {
            if (!src.isPlaying)
            {
                src.clip = clips[num];
                src.Play();
                return;
            }

            var timeLeft = src.clip.length - src.time;
            if (timeLeft < factor)
            {
                factor = timeLeft;
                bestToKill = src;
            }
        }


        bestToKill.Stop();
        bestToKill.clip = clips[num];
        bestToKill.Play();



    }
}
