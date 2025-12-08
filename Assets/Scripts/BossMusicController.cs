using UnityEngine;

public class BossMusicController : MonoBehaviour
{
    public AudioSource musicSource;

    [Header("Music per phase (optional)")]
    public AudioClip phase1Music;
    public AudioClip phase2Music;
    public AudioClip phase3Music;

    public void PlayPhaseMusic(int phase)
    {
        if (musicSource == null)
            return;

        AudioClip clip = null;
        switch (phase)
        {
            case 1: clip = phase1Music; break;
            case 2: clip = phase2Music; break;
            case 3: clip = phase3Music; break;
        }

        if (clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        // Do not force looping here; respect whatever is set on the AudioSource
        musicSource.Play();
    }
}
