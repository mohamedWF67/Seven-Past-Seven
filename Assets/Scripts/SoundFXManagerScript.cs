using System;
using UnityEngine;

public class SoundFXManagerScript : MonoBehaviour
{
    public static SoundFXManagerScript instance;

    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource backgroundMusicSource;
    [SerializeField] private GameObject soundSFXObject;
    [SerializeField] private GameObject soundUIObject;

    private void Awake()
    {
        //* Creates a singleton.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        //* Gets the audio source's reference.
        backgroundMusicSource = GetComponent<AudioSource>();
        //* Starts playing the background music.
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        //* Set's the clip to the background music's audio clip.
        backgroundMusicSource.clip = backgroundMusic;
        //* Plays the audio source with the background music.
        backgroundMusicSource.Play();
    }

    public void PlaySFXSound(AudioClip clip, Transform transform, float volume = 1f)
    {
        PlaySound(soundSFXObject, clip, volume,transform);
    }
    
    public void PlayUISound(AudioClip clip, float volume = 1f)
    {
        PlaySound(soundUIObject, clip, volume,transform);
    }

    void PlaySound(GameObject audioObject, AudioClip clip, float volume,Transform transform)
    {
        //* Creates a sound object and assigns it as an audio source.
        AudioSource audioSource = Instantiate(audioObject, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        //* Sets the instantiated audio source to the clip from the parameter.
        audioSource.clip = clip;
        //* Sets the instantiated audio source's volume from the parameter.
        audioSource.volume = volume;
        //* Plays the instantiated audio source with the inserted audio clip.
        audioSource.Play();
        //* Gets the audio clip length and destroys the object after it finishes playing.
        float audioLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, audioLength);
    }
}
