using System;
using UnityEngine;

public class SoundFXManagerScript : MonoBehaviour
{
    public static SoundFXManagerScript instance;

    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundSFXObject;
    [SerializeField] private AudioSource soundUIObject;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        backgroundMusicSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.Play();
    }

    public void PlaySFXSound(AudioClip clip, Transform transform, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(soundSFXObject, transform.position, Quaternion.identity);
        
        audioSource.clip = clip;
        
        audioSource.volume = volume;
        
        audioSource.Play();
        
        float audioLength = audioSource.clip.length;
        
        Destroy(audioSource.gameObject, audioLength);
    }
    
    public void PlayUISound(AudioClip clip, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(soundUIObject, transform.position, Quaternion.identity);
        
        audioSource.clip = clip;
        
        audioSource.volume = volume;
        
        audioSource.Play();
        
        float audioLength = audioSource.clip.length;
        
        Destroy(audioSource.gameObject, audioLength);
    }
}
