using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider UISlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioMixer am;

    private void Start()
    {
        //* Sets the volume for all sliders from the saved PlayerPrefs.
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        UISlider.value = PlayerPrefs.GetFloat("UIVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    
    public void ChangeMasterVolume()
    {
        //* Sets the volume for the master slider.
        float volume = masterSlider.value;
        am.SetFloat("MasterMixerVolume",volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void ChangeUISlider()
    {
        //* Sets the volume for the UI slider.
        float volume = UISlider.value;
        am.SetFloat("UIMixerVolume", volume);
        PlayerPrefs.SetFloat("UIVolume", volume);
    }
    
    public void ChangeSFXSlider()
    {
        //* Sets the volume for the SFX slider.
        float volume = SFXSlider.value;
        am.SetFloat("SFXMixerVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void ChangeMusicSlider()
    {
        //* Sets the volume for the music slider.
        float volume = musicSlider.value;
        am.SetFloat("MusicMixerVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
