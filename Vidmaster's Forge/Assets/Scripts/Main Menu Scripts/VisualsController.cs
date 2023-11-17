using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VisualsController : MonoBehaviour
{
    [SerializeField] Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public Toggle isFullScreen;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    public Dropdown qualityDropdown;

    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private GameObject resetPrompt = null;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    void Start() {

        qualityDropdown.value = PlayerPrefs.GetInt("optionvalue", 3);

        resolutions = Screen.resolutions;
        isFullScreen.isOn = PlayerPrefs.GetInt("fullscreen") == 0;

        bool setDefault = false;

        if ( PlayerPrefs.GetInt("set default resolution") == 0 ) {
            setDefault = true;
            PlayerPrefs.GetInt("set default resolution", 1);
        }

        for ( int i = 0; i < resolutions.Length; i++ ) {
            string resolutionString = resolutions[i].width.ToString() + " x " + resolutions[i].height.ToString();
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolutionString));

            if( setDefault && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height ) {
                resolutionDropdown.value = i;
            }
        }
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution selection");

        qualityDropdown.onValueChanged.AddListener(new UnityAction<int>(index => {
            PlayerPrefs.SetInt("optionvalue", qualityDropdown.value);
            PlayerPrefs.Save();
        }));
    }

    public void ChangeResolution() {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, isFullScreen.isOn);
        PlayerPrefs.SetInt("resolution selection", resolutionDropdown.value);
    }

    public void ChangeFullscreen(){
        Screen.SetResolution(resolutions[resolutionDropdown.value].width,resolutions[resolutionDropdown.value].height,isFullScreen.isOn);
        if( isFullScreen.isOn ) {
            PlayerPrefs.SetInt("fullscreen",0);
        }
        else {
            PlayerPrefs.SetInt("fullscreen",1);
        }
    }

    public void setQuality( int quality ) {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetBrightness(float brightness) {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }


    public void VisualsApply(int num) {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);

       // PlayerPrefs.SetInt("masterQuality",)



        StartCoroutine(Confirmation(num));
    }


    public void Reset(string MenuType) {
        if (MenuType == "Visuals") {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            // qualityDropdown.value = 1;
            // QualitySettings.SetQualityLevel(1);

            // isFullScreen.isOn = false;
            // Screen.fullscreen = false;

            // Resolution currentResolution = Screen.currentResolution;
            // Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullscreen);
            // resolutionDropdown.value = resolutions.Length;

            VisualsApply(1);
        }
    }

    public IEnumerator Confirmation(int type) {
        if ( type == 1 ) {
            resetPrompt.SetActive(true);
            yield return new WaitForSeconds(1);
            resetPrompt.SetActive(false);
        }
        else {
            confirmationPrompt.SetActive(true);
            yield return new WaitForSeconds(1);
            confirmationPrompt.SetActive(false);
        }
    }
}
