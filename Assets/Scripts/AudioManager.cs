using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;

    public AudioClip bossMusic;

    private void Start()
    {
        musicSource.clip = bossMusic;
        musicSource.Play();
    }
}