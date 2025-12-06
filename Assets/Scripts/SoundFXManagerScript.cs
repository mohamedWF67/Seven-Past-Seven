using System;
using UnityEngine;

public class SoundFXManagerScript : MonoBehaviour
{
    public static SoundFXManagerScript instance;
    
    [SerializeField] private AudioSource SoundSFXObject;
    [SerializeField] private AudioSource SoundUIObject;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFXSound(AudioClip clip, Transform transform, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(SoundSFXObject, transform.position, Quaternion.identity);
        
        audioSource.clip = clip;
        
        audioSource.volume = volume;
        
        audioSource.Play();
        
        float audioLength = audioSource.clip.length;
        
        Destroy(audioSource.gameObject, audioLength);
    }
    
    public void PlayUISound(AudioClip clip, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(SoundUIObject, transform.position, Quaternion.identity);
        
        audioSource.clip = clip;
        
        audioSource.volume = volume;
        
        audioSource.Play();
        
        float audioLength = audioSource.clip.length;
        
        Destroy(audioSource.gameObject, audioLength);
    }
}
