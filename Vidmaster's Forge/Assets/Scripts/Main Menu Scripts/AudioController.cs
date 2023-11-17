using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] private Text masterVolumeText = null;
    [SerializeField] Slider musicSlider;
    [SerializeField] private Text musicVolumeText = null;
    [SerializeField] Slider sfxSlider;
    [SerializeField] private Text sfxVolumeText = null;

    [SerializeField] private float defaultMaster = 1.0f;
    [SerializeField] private float defaultMusic = 1.0f;
    [SerializeField] private float defaultSFX = 1.0f;

    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private GameObject resetPrompt = null;

    private void Start() {
        if( PlayerPrefs.HasKey("MusicVolume")) {
            LoadVolume();
        }
        else {
            SetMasterVolume();
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMasterVolume() {
        float volume = masterSlider.value;
        mixer.SetFloat("Master", Mathf.Log10(volume)*20);
        masterVolumeText.text = volume.ToString("0.0");
    }
    
    public void SetMusicVolume() {
        float volume = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(volume)*20);
        musicVolumeText.text = volume.ToString("0.0");
    }

    public void SetSFXVolume() {
        float volume = sfxSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        sfxVolumeText.text = volume.ToString("0.0");
    }

    private void LoadVolume() {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
       
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }


    public void VolumeApply(int num) {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();

        StartCoroutine(Confirmation(num));
    }

    public void Reset(string MenuType) {
        if (MenuType == "Audio") {
            AudioListener.volume = defaultMaster;
            masterSlider.value = defaultMaster;
            masterVolumeText.text = defaultMaster.ToString("0.0");

            AudioListener.volume = defaultMusic;
            musicSlider.value = defaultMusic;
            musicVolumeText.text = defaultMusic.ToString("0.0");

            AudioListener.volume = defaultSFX;
            sfxSlider.value = defaultSFX;
            sfxVolumeText.text = defaultSFX.ToString("0.0");

            VolumeApply(1);
        }
    }


    public IEnumerator Confirmation(int type) {
        if ( type == 1 ) {
            resetPrompt.SetActive(true);
            yield return new WaitForSeconds(2);
            resetPrompt.SetActive(false);
        }
        else {
            confirmationPrompt.SetActive(true);
            yield return new WaitForSeconds(2);
            confirmationPrompt.SetActive(false);
        }
    }

}
