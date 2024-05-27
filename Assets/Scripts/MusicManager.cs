using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip normalMusic;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = normalMusic;
    }
    

    public void PlayHighPitchMusic()
    {
        if (!audioSource.isPlaying) // Eğer ses çalmıyorsa
        {
            audioSource.pitch += 0.1f; // Pitch'i artır
            audioSource.Play(); // Müziği çal
        }
    }

    public void PlayNormalMusic()
    {
        if (!audioSource.isPlaying) // Eğer ses çalmıyorsa
        {
            audioSource.pitch = 1f; // Normal pitch'e geri dön
            audioSource.Play(); // Müziği çal
        }
    }
}