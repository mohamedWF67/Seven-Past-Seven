using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundFXManagerScript : MonoBehaviour
{
    public static SoundFXManagerScript instance;

    [SerializeField] private List<AudioScene> audioScenes;
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
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //* Starts playing the background music.
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return null;
        
        PlayBackgroundMusic();
    }
    
    public void PlayBackgroundMusic()
    {
        ManageBackgroundMusic();
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
    
    public void ManageBackgroundMusic()
    {
        int currentScene = GameManagerScript.instance.currentSceneIndex;
        if (audioScenes[currentScene].audioClip.Equals(backgroundMusic))
        {
            Debug.Log("Same audio clip");
            return;
        }
        Debug.Log("Different audio clip");
        backgroundMusicSource.clip = audioScenes[currentScene].audioClip;
        backgroundMusicSource.Play();
    }
}
