using Hertzole.ScriptableValues;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject Audio, Visuals, Gameplay;
    public ScriptableFloatEvent GUIChange;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeText = null;
    [SerializeField] private TextMeshProUGUI musicVolumeText = null;
    [SerializeField] private TextMeshProUGUI sfxVolumeText = null;
    [SerializeField] private float defaultMaster = 1.0f;
    [SerializeField] private float defaultMusic = 1.0f;
    [SerializeField] private float defaultSFX = 1.0f;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    #region Scene Swapping
    private void OnAudioClicked()
    {
        Audio.SetActive(true);
        Visuals.SetActive(false);
        Gameplay.SetActive(false);
    }

    private void OnVisualsClicked()
    {
        Audio.SetActive(false);
        Visuals.SetActive(true);
        Gameplay.SetActive(false);
    }

    private void OnGameplayClicked()
    {
        Audio.SetActive(false);
        Visuals.SetActive(false);
        Gameplay.SetActive(true);
    }
    #endregion

    #region Audio Settings

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        masterVolumeText.text = volume.ToString("0.0");
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        musicVolumeText.text = volume.ToString("0.0");
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        sfxVolumeText.text = volume.ToString("0.0");
    }

    private void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }


    public void VolumeApply(int num)
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void Reset(string MenuType)
    {
        if (MenuType == "Audio")
        {
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

    #endregion


    #region Visual Settings




    #endregion


}
