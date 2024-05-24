using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip normalMusic;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = normalMusic;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayHighPitchMusic();
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            PlayNormalMusic();
        }
    }

    void PlayHighPitchMusic()
    {
        if (!audioSource.isPlaying) // Eğer ses çalmıyorsa
        {
            audioSource.pitch += 0.1f; // Pitch'i artır
            audioSource.Play(); // Müziği çal
        }
    }

    void PlayNormalMusic()
    {
        if (!audioSource.isPlaying) // Eğer ses çalmıyorsa
        {
            audioSource.pitch = 1f; // Normal pitch'e geri dön
            audioSource.Play(); // Müziği çal
        }
    }
}