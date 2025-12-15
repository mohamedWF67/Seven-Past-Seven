using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider UISlider;
    
    [SerializeField]TextMeshProUGUI masterText;
    [SerializeField]TextMeshProUGUI musicText;
    [SerializeField]TextMeshProUGUI SFXText;
    [SerializeField]TextMeshProUGUI UIText;
    
    [SerializeField] private AudioMixer am;
    

    private void Start()
    {
        //* Sets the volume for all sliders from the saved PlayerPrefs.
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        UISlider.value = PlayerPrefs.GetFloat("UIVolume");
        
        masterText.text = (((masterSlider.value + 80f) / 80) * 100).ToString("000");
        musicText.text = (((musicSlider.value + 80f) / 80)* 100).ToString("000");
        SFXText.text = (((SFXSlider.value + 80f) / 80)* 100).ToString("000");
        UIText.text = (((UISlider.value + 80f) / 80)* 100).ToString("000");
    }
    
    public void ChangeMasterVolume()
    {
        //* Sets the volume for the master slider.
        float volume = masterSlider.value;
        am.SetFloat("MasterMixerVolume",volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        masterText.text = (((masterSlider.value + 80f) / 80)* 100).ToString("000");
    }    
    public void ChangeMusicSlider()         
    {
        //* Sets the volume for the music slider.
        float volume = musicSlider.value;
        am.SetFloat("MusicMixerVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        musicText.text = (((musicSlider.value + 80f) / 80)* 100).ToString("000");
    }
    public void ChangeSFXSlider()
    {
        //* Sets the volume for the SFX slider.
        float volume = SFXSlider.value;
        am.SetFloat("SFXMixerVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        SFXText.text = (((SFXSlider.value + 80f) / 80)* 100).ToString("000");
    }
    public void ChangeUISlider()
    {
        //* Sets the volume for the UI slider.
        float volume = UISlider.value;
        am.SetFloat("UIMixerVolume", volume);
        PlayerPrefs.SetFloat("UIVolume", volume);
        UIText.text = (((UISlider.value + 80f) / 80)* 100).ToString("000");
    }
    
}
